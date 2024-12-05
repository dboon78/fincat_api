using Microsoft.EntityFrameworkCore;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using api.Extensions;
using api.Mappers;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private IConfiguration _configuration { get; set; }
        private readonly IEmailSender _emailSender;
        public readonly IFreeCurrencyService _freeCurrencyService;
        public AccountController(UserManager<AppUser> userManager,
                                ITokenService tokenService, 
                                SignInManager<AppUser> signInManager,
                                IPortfolioRepository portfolioRepository,
                                IConfiguration configuration,
                                IEmailSender emailSender,
                                IFreeCurrencyService freeCurrencyService
                            ){
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _portfolioRepository = portfolioRepository;
            _configuration=configuration;
            _emailSender=emailSender;
            _freeCurrencyService=freeCurrencyService;
        }
        /// <summary>
/// Registration
/// </summary>
/// <param name="registerDto">{"username": "Test","email": "test@example.com","password": "Password1234!"}</param>
/// <returns>NewUserDto</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(NewUserDto),200)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto){
            try{
                if(!ModelState.IsValid)return BadRequest(ModelState);
                var appUser=new AppUser{
                    UserName = registerDto.Username,
                    Email=registerDto.Email,
                    CurrencyCode="USD"
                };
                var createdUser=await _userManager.CreateAsync(appUser,registerDto.Password);
                if(createdUser.Succeeded){
                    var roleResult=await _userManager.AddToRoleAsync(appUser,"User");
                    if(roleResult.Succeeded){
                        await _portfolioRepository.CreateAsync(new Portfolio(){
                            PortfolioName="Default",
                            AppUserId=appUser.Id
                        });
                        if(appUser.UserCurrency==null)appUser.UserCurrency=await _freeCurrencyService.GetCurrency(appUser.CurrencyCode);

                        return Ok(
                            new NewUserDto{
                                UserName=appUser.UserName,
                                Email=appUser.Email,
                                Token=_tokenService.CreateToken(appUser),
                                UserCurrency=appUser.UserCurrency.ToCurrencyDto()
                            }
                        );
                    }
                    else{
                        return StatusCode(500,roleResult.Errors);
                    }
                }else{
                    return StatusCode(500,createdUser.Errors);
                }

            }catch(Exception err){
                return StatusCode(500,err);
            }
        }
        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="loginDto">Contains username and password</param>
        /// <returns>NewUserDto</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto){
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var user = await _userManager.Users.Include(u=>u.UserCurrency).FirstOrDefaultAsync(x=>x.UserName==loginDto.UserName.ToLower());
            if(user==null){
                return Unauthorized("Invalid Username!");
            }
            var result =  await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded){
                return Unauthorized("Username not found and/or password incorrect");
            }
            if(user.CurrencyCode==null){
                user.CurrencyCode="USD";
                await _userManager.UpdateAsync(user);
            }
            if(user.UserCurrency==null)user.UserCurrency=await _freeCurrencyService.GetCurrency(user.CurrencyCode);
            return Ok(new NewUserDto{
                UserName = user.UserName,
                Email= user.Email,
                Token=_tokenService.CreateToken(user),
                UserCurrency=user.UserCurrency.ToCurrencyDto(),
            });
        }

/// <summary>
/// The user has forgotten their password
/// </summary>
/// <param name="forgotPasswordDto"></param>
/// <returns>If an account exists for that email, a password reset link has been sent.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Find user by email
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist for security purposes
                return Ok("If an account exists for that email, a password reset link has been sent.");
            }

            // Generate password reset token
            var resetToken =  WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));

            // Create a URL with the reset token (Assume a frontend URL structure here)
            var resetUrl = $"{_configuration["Website:Url"]}/reset-password?token={ resetToken}&email={Uri.EscapeDataString(forgotPasswordDto.Email)}";

            // Send email (assuming a mail service is set up)
            await _emailSender.SendEmailAsync(forgotPasswordDto.Email,"Reset Password", $"Reset your password here: {resetUrl}");

            return Ok("If an account exists for that email, a password reset link has been sent.");
        }
        /// <summary>
        /// The user has received a reset link in their email and is submitting a new password
        /// </summary>
        /// <param name="resetPasswordDto"></param>
        /// <returns>NewUserDto</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            // Find user by email
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid token or email.");
            }
            var decodedToken = resetPasswordDto.Token;
            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDto.NewPassword);

            if (result.Succeeded)   
            { 
                if(user.CurrencyCode==null)user.CurrencyCode="USD";
                if(user.UserCurrency==null)user.UserCurrency=await _freeCurrencyService.GetCurrency(user.CurrencyCode);
           
                return Ok(
                    new NewUserDto{
                        UserName=user.UserName,
                        Email=user.Email,
                        Token=_tokenService.CreateToken(user),
                        UserCurrency=user.UserCurrency.ToCurrencyDto()
                    }
                );
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> SelfDelete(){
            var username=User.GetUsername();
            
            var appUser=await _userManager.FindByNameAsync(username);
            await _userManager.DeleteAsync(appUser);
            return Ok("Deleted");
        }

        [HttpPost("settings")]
        [Authorize]
        public async Task<IActionResult> SettingsPost(SettingsPostDto settingsPostDto){
            var username=User.GetUsername();
            
            var appUser=await _userManager.FindByNameAsync(username);
            if(appUser==null){ return BadRequest("User not found");}
            Console.WriteLine($"SettingsPost for {username} setting currencyCode:{settingsPostDto.CurrencyCode}");
            appUser.CurrencyCode=settingsPostDto.CurrencyCode;
            appUser.UserCurrency=await _freeCurrencyService.GetCurrency(appUser.CurrencyCode);
             return Ok(
                    new NewUserDto{
                        UserName=appUser.UserName,
                        Email=appUser.Email,
                        Token=_tokenService.CreateToken(appUser),
                        UserCurrency=appUser.UserCurrency.ToCurrencyDto()
                    }
                );


        }
        [HttpGet("currency-list")]
        public async Task<IActionResult> currencyList(){
            var currencyList= await _freeCurrencyService.GetCurrencyList();
            if(currencyList==null){ return BadRequest("Currency list not found");}
            return Ok(currencyList.Select(currency=>currency.Code));
        }

    }
}
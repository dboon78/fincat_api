using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using api.Mappers;
namespace api.Controllers
{
    [Route("api/google-auth")]
    public class GoogleProtectedController  : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IConfiguration _configuration;
        public readonly IFreeCurrencyService _freeCurrencyService;
        public GoogleProtectedController(IConfiguration configuration,
                                IFreeCurrencyService freeCurrencyService,UserManager<AppUser> userManager,ITokenService tokenService, SignInManager<AppUser> signInManager,IPortfolioRepository portfolioRepository){
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _portfolioRepository = portfolioRepository;
            _freeCurrencyService=freeCurrencyService;
        }

        public static string ConvertGivenNameToUsername(string givenName)
        {
            if (string.IsNullOrWhiteSpace(givenName))
            {
                throw new ArgumentException("Given name cannot be null or empty.");
            }

            // Trim leading and trailing whitespaces
            givenName = givenName.Trim();

            // Convert to lowercase for consistency
            givenName = givenName.ToLowerInvariant();

            // Replace spaces and special characters with underscores or hyphens
            // This regex keeps only letters, numbers, underscores, and hyphens
            givenName = Regex.Replace(givenName, @"[^a-z0-9-_]", "_");

            // Limit username length if needed (optional, assuming max 20 characters)
            int maxLength = 20;
            if (givenName.Length > maxLength)
            {
                givenName = givenName.Substring(0, maxLength);
            }

            // Ensure username is unique, if necessary (handle this elsewhere with a DB check)

            return givenName;
        }
/// <summary>
/// A google login has occured on the client, sync to our db
/// </summary>
/// <param name="request"></param>
/// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthRequest request)
        {
            try
            {
                Console.WriteLine("GoogleLogin token:"+request.Token);  
                // Verify the Google token
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
                
                Console.WriteLine($"GoogleLogin payload audience,clientid\n{payload.Audience}\n{ _configuration["Authentication:Google:ClientId"]}");  
                if (payload.Audience.ToString() != _configuration["Authentication:Google:ClientId"])
                {   
                    return Unauthorized("Invalid Google token");
                }
                Console.WriteLine($"Google token is good\n");
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.GoogleId == payload.Subject);
                
                Console.WriteLine($"GoogleLogin user({(user!=null)}) from subject/googleId {payload.Subject} ");  
                if (user== null)
                {
                    user = await _userManager.FindByEmailAsync(payload.Email);
                    if(user!=null){
                        Console.WriteLine($"GoogleLogin  user({(user!=null)}) from email {payload.Email}");  
                        user!.GoogleId=payload.Subject;
                        var updateUserResult = await _userManager.UpdateAsync(user);
                    }
                }

                if(user==null){
       
                    
                    user = new AppUser
                    {
                        UserName =ConvertGivenNameToUsername( payload.Name),
                        Email = payload.Email,
                        GoogleId=payload.Subject,
                        CurrencyCode="USD"
                    };
                    user.GeneratePasswordHash();
           
                    Console.WriteLine($"GoogleLogin new login {user.UserName}  email:{user.Email}  googleId:{user.GoogleId}");  

                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        return StatusCode(500, createUserResult.Errors);
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleResult.Succeeded)
                    {
                        return StatusCode(500, roleResult.Errors);
                    }

                    // Create a default portfolio for the new user
                    await _portfolioRepository.CreateAsync(new Portfolio
                    {
                        PortfolioName = "Default",
                        AppUserId = user.Id
                    });
                }

                // Create a JWT token for the user
                var token = _tokenService.CreateToken(user);

                if(user.CurrencyCode==null)user.CurrencyCode="USD";
                await _userManager.UpdateAsync(user);
                if(user.UserCurrency==null)user.UserCurrency=await _freeCurrencyService.GetCurrency(user.CurrencyCode);
                // Return the authenticated user data and token
                return Ok(new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token,
                    UserCurrency=user.UserCurrency.ToCurrencyDto()
                });
            }
            catch (InvalidJwtException)
            {
                return Unauthorized("Invalid Google token");
            }
        }

    }
}
public class GoogleAuthRequest
{
    public string Token { get; set; }
}
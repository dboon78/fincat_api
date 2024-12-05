using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Portfolio;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager,
                                    IStockRepository stockRepo,
                                    IPortfolioRepository portfolioRepo,
                                    IFMPService fmpService){
            _userManager=userManager;
            // _stockRepository=stockRepo;
            _portfolioRepository=portfolioRepo;
            _fmpService=fmpService;
        }       
        /// <summary>
        /// Get the user's portfolio, which will include holdings and children
        /// </summary>
        /// <returns></returns>
        [HttpGet]   
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio(){
            Console.WriteLine("GetUserPorfolio");
            var username=User.GetUsername();
            Console.WriteLine("GetUserPorfolio username:"+username);
            
            var appUser=await _userManager.FindByNameAsync(username);
            
            Console.WriteLine("GetUserPorfolio appUser:"+appUser.Id);
            var userPortfolio = await _portfolioRepository.GetPortfolios(appUser);
            Console.WriteLine("GetUserPorfolio"+userPortfolio.Count());
            return Ok(userPortfolio.Select(x=>x.ToDto()));
        }
        /// <summary>
        /// Delete portfolio and it's holdings by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePorfolio(int id){
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var userPortfolio=await _portfolioRepository.GetPortfolios(appUser);
            if(userPortfolio==null){
                return BadRequest("Portfolio not found");
            }
            
            var filterStock=userPortfolio.Where(s=>s.PortfolioId==id).ToList();
            if(filterStock.Count==1){
                await _portfolioRepository.DeletePortfolio(appUser,id);
            }else{
                return BadRequest("Stock is not in portfolio");
            }
            return NoContent();
        }
        /// <summary>
        /// Add a portfolio, which only takes the name property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string name){
            var username=User.GetUsername();
            // var stock=await _stockRepository.GetBySymbolAsync(symbol,_fmpService);
            // if(stock==null){
            //     return BadRequest("Stock not found");
            // }
            var appUser=await _userManager.FindByNameAsync(username);
            // var stock= await _stockRepository.GetBySymbolAsync(symbol);
            // if(stock==null){
            //     return BadRequest("Stock not found");
            // }
            var userPortfolio=await _portfolioRepository.GetPortfolios(appUser);
            if(userPortfolio.Any(s=>s.PortfolioName==name)){
                return BadRequest("Name already in use");
            }
            var portfolioModel= new Portfolio{
                AppUserId=appUser.Id,
                PortfolioName=name
            };
            portfolioModel=await _portfolioRepository.CreateAsync(portfolioModel);

            if(portfolioModel==null){
                return StatusCode(500,"Could not create");
            }else{
                return Ok(portfolioModel.ToDto());
            }
        }
        /// <summary>
        /// Changes the portfolio's info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutPortfolio([FromBody]UpdatePortfolioRequest request){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
                   
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var model=await _portfolioRepository.UpdatePortfolio(appUser,request);
            return Ok(model.ToDto());

        }

        [HttpGet]
        [Route("price-list/")]
        [Authorize]
        public async Task<IActionResult> GetPriceList(){

            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            List<PriceDto> priceList=await _portfolioRepository.PriceList(appUser);
            return Ok(JsonConvert.SerializeObject(priceList));
        }
    }
}
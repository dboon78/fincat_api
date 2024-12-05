using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Holding;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/holding")]
    [ApiController]
    public class HoldingController:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fmpService;
        private readonly IHoldingRepository _holdingRepository;
        public HoldingController(UserManager<AppUser> userManager,
                                  IStockRepository stockRepository,
                                  IPortfolioRepository portfolioRepository,
                                  IFMPService fMPService,
                                  IHoldingRepository holdingRepository  ){

            _userManager=userManager;
            _stockRepository=stockRepository;
            _portfolioRepository=portfolioRepository;
            _fmpService=fMPService;
            _holdingRepository=holdingRepository;
            
        }
/// <summary>
/// Get all holdings for this user
/// </summary>
/// <returns>holdings dto</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(){
            
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            List<Holding> holdings=await _holdingRepository.GetHoldingsAsync(appUser);
            if(holdings==null)return BadRequest("No Holdings Found");
            return Ok(holdings.Select(x=>x.ToDto()));
        }
/// <summary>
/// Creates a new holding.
/// </summary>
/// <param name="createHolding"></param>
/// <returns>holding dto</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddHolding([FromBody]CreateHoldingRequest createHolding){
            
            Console.WriteLine($"CreateHoldingRequest symbol:{createHolding.Symbol} portfolioId:{createHolding.PortfolioId} isCrypto:{createHolding.IsCrypto}");

            if(!ModelState.IsValid)return BadRequest(ModelState);
            Stock? stock= await _stockRepository.GetBySymbolAsync(createHolding.Symbol,createHolding.IsCrypto);
            if(stock==null)return BadRequest("Stock not found");
            Holding holding = await _holdingRepository.CreateHolding(stock.Id,createHolding.PortfolioId);
            
            return Ok(holding.ToDto());
        }
        /// <summary>
        /// Delete a holding
        /// </summary>
        /// <param name="holdingId"></param>
        /// <returns>SUCCESS</returns>
        [HttpDelete]
        [Route("{holdingId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteHolding(int holdingId){
            await _holdingRepository.DeleteHolding(holdingId);
            return Ok("SUCCESS");
        }
        /// <summary>
        /// Updates info for units and book cost
        /// </summary>
        /// <param name="holdingDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateHolding([FromBody]UpdateHoldingRequest holdingDto){
            var holding= await _holdingRepository.UpdateHolding(holdingDto);
            return Ok(holding.ToDto());
        }   
    }
}
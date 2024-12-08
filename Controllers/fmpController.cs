
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using FincatTester;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    /// <summary>
    /// Holds endpoints for interacting with Financial Modeling Prep
    /// </summary>
    [Route("api/fmp")]
    [ApiController]
    public class fmpController:ControllerBase
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public fmpController(UserManager<AppUser> userManager,
                            IFMPService fmpService){
            _userManager = userManager;
            _fmpService = fmpService;
        }
        /// <summary>
        /// Direct relay to fmp
        /// </summary>
        /// <param name="endpoint">url encoded enpoint for fmp</param>
        /// <returns>string</returns>
        [HttpGet]
        [Route("relay")]
        [Authorize  ]
        public async Task<IActionResult> Get([FromQuery(Name ="endpoint")] string endpoint){
            Console.WriteLine("endpoint  "+endpoint);
            string rtn=await _fmpService.GetRelay(endpoint); 
            return Ok(rtn);
        }

        /// <summary>
        /// The primary fmp function because we want to search stocks and cryptos
        /// </summary>
        /// <param name="query">stock/crypto symbol</param>
        /// <returns>FMPSearch[]</returns>
        [HttpGet]
        [Route("search")]
        [Authorize  ]
        public async Task<IActionResult> GetSearch([FromQuery(Name ="query")] string query){
            Console.WriteLine("search query  "+query);
            List<FMPSearch> rtn=await _fmpService.GetSearch(query); 
            return Ok(rtn);
        }
        [HttpGet("balance-sheet-statement/{symbol}")]
        [Authorize]
        public async Task<IActionResult> GetBalanceSheets(string symbol){
            FMPBalanceSheet[] sheets= await _fmpService.GetBalanceSheetsAsync(symbol);
            return Ok(sheets);
        }

        [HttpGet("cash-flow-statement/{symbol}")]
        [Authorize]
        public async Task<IActionResult> GetCashFlow(string symbol){
            FMPCashFlow[] sheets= await _fmpService.GetCashFlowsAsync(symbol);
            return Ok(sheets);
        }

        [HttpGet("ratios/{symbol}")]
        [Authorize]
        public async Task<IActionResult> GetKeyRatios(string symbol){
            FMPKeyRatios[] sheets= await _fmpService.GetKeyRatiosAsync(symbol);
            return Ok(sheets);
        }

        [HttpGet("profile/{symbol}")]
        [Authorize]
        public async Task<IActionResult> GetProfiles(string symbol){
            FMPProfile[] sheets= await _fmpService.GetProfilesAsync(symbol);
            return Ok(sheets);
        }
    }
}   
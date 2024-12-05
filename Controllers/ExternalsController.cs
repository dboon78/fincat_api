using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [Route("api/externals")]
    [ApiController]
    public class ExternalsController : ControllerBase
    {
        public readonly ICoinPaprika _coinPaprika;
        public readonly IFinnhubService _finnhubService;
        public readonly IFreeCurrencyService _freeCurrencyService;
        private readonly ApplicationDBContext _context;
        public ExternalsController(ApplicationDBContext applicationDBContext,ICoinPaprika coinPaprika, IFinnhubService finnhubService,IFreeCurrencyService freeCurrencyService){
            _coinPaprika = coinPaprika;
            _context=applicationDBContext;
            _finnhubService = finnhubService;
            _freeCurrencyService = freeCurrencyService;
        }

        [HttpGet]
        [Route("allcoins")]
        public async Task<IActionResult> GetPaprikaCoins(){
            if(_context.CoinPaprikaCoins.Count()>0)return Ok(_context.CoinPaprikaCoins.ToList());
            List<CoinPaprikaCoin> coins=await _coinPaprika.GetCoins();
            return Ok(coins.Count);
        }

        [HttpGet]
        [Route("paprikaget")]
        public async Task<IActionResult> GetPaprikaCoin([FromQuery(Name = "symbol")] string symbol)
        {
            try
            {
                CoinPaprikaDto coin = await _coinPaprika.GetCoinData(symbol);
                if (coin == null) return BadRequest($"{symbol} not found");
                Console.WriteLine($"GetPaprikaCoin {coin.name} {coin.close}");
                return Ok(coin);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPaprikaCoin: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet]
        [Route("stockprofile")]
        public async Task<IActionResult> GetFinnProfile([FromQuery(Name ="symbol")]string symbol){
            FinnhubProfile profile=await _finnhubService.GetProfile(symbol);
            if(profile==null)return BadRequest($"{symbol} not found");
            return Ok(profile);

        }
        [HttpGet]
        [Route("stockssearch")]
        public async Task<IActionResult> GetFinnStocks([FromQuery(Name ="symbol")]string symbol){
            List<FinnhubSearch> search=await _finnhubService.GetSearch(symbol);
            if(search==null)return BadRequest($"{symbol} not found");
            return Ok(search);
        }
        
        [HttpGet]
        [Route("currencies")]
        public async Task<IActionResult> GetCurrencies(){
            if(_context.Currencies.Count()>0)return Ok(_context.Currencies.ToList());
            List<Currency> currencies=await _freeCurrencyService.GetCurrencyList();
            return Ok(currencies);
        }
    }
}
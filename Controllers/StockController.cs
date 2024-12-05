using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController:ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly ApplicationDBContext _context;
        private readonly IFMPService _fmpService;
        private readonly ITwelveDataService _tdService;
        private readonly ICoinApiService _coinApiService;
        public StockController(ApplicationDBContext applicationDBContext,ICoinApiService coinApiService,IStockRepository stockRepo,IFMPService fmpService, ITwelveDataService tdService)
        {
            _stockRepo=stockRepo;
            _context=applicationDBContext;
            _fmpService=fmpService;
            _tdService=tdService;
            _coinApiService=coinApiService;
        }
        /// <summary>
        /// Gets all of the user's stocks 
        /// </summary>
        /// <param name="query">Symbol, CompanyName, SortBy, IsDescending,PageNumber,PageSize</param>
        /// <returns></returns>
        [HttpGet]   
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);//.Stocks.ToListAsync();
            
            var stockDto=stocks.Select(s=>s.ToStockDto()).ToList();
            foreach(var stock in stocks.Where(s=>s.IsExpired)){
                _stockRepo.GetByIdAsync(stock.Id);
            }
            return Ok(stockDto);
        }
        /// <summary>
        /// Get a stock by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var stock = await _stockRepo.GetByIdAsync(id);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        /// <summary>
        /// Load and update the exchange hours
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("exchanges")]
        [Authorize]
        public async Task<IActionResult> LoadExchanges(){
            if(_context.StockExchanges.Count()>0)return Ok(_context.StockExchanges.ToList());
            List<StockExchange> stockExchanges=await _fmpService.UpdateExchanges();
            return Ok(stockExchanges.Select(x=>x.ToExchangeDto()).ToList());
        }

        /// <summary>
        /// Returns an array of daily values up to 5 years
        /// </summary>
        /// <param name="id"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history/{id:int}")]
        // [Authorize]
        public async Task<IActionResult> HistoricalData([FromRoute] int id,[FromQuery(Name ="period")]string period="1Y")
        {
            Stock stock=await _context.Stocks.Include(s=>s.Exchange ).FirstOrDefaultAsync(x => x.Id==id);
            if(stock==null){
                Console.WriteLine($"UpdatedHistoricData no stock with id:{id}");   
            }else{
                Console.WriteLine($"Stock found: {stock.CompanyName}");
            }
            List<HistoricPrice> historicPrices=null;
            Console.WriteLine($"HistoricData stock:{stock.Symbol} exchange:{stock.Exchange.ExchangeName}");
            if(stock.Exchange.ExchangeName=="CCC"){
                //crypto
                string symbol=stock.Symbol.Replace("USD","");
                historicPrices=await _coinApiService.GetCoinApiHistoryAsync(id, symbol);
                historicPrices=historicPrices.Take(90).ToList();
            }else{
                historicPrices=await _tdService.UpdatedHistoricData(id,stock.Symbol,period);    
                historicPrices=historicPrices.Take(90).ToList();
            }
            
            Console.WriteLine($"HistoricalData records returning:{historicPrices.Count()}");
            return Ok(historicPrices.Select(x=>x.Close));
        }


/// <summary>
        /// Returns an array of daily values up to 5 years
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        // [HttpGet]
        // [Route("history/{symbol:string}")]
        // // [Authorize]
        // public async Task<IActionResult> HistoricalData([FromRoute] string symbol,[FromQuery(Name ="period")]string period="1Y",[FromQuery(Name ="crypto")]string isCrypto="false")
        // {
        //     Stock stock=await _context.Stocks.Include(s=>s.Exchange ).FirstOrDefaultAsync(x => x.Symbol==symbol);
        //     if(stock==null){
        //         Console.WriteLine($"UpdatedHistoricData no stock with symbol:{symbol}, creating new record");

        //     }else{
        //         Console.WriteLine($"Stock found: {stock.CompanyName}");
        //     }
        //     List<HistoricPrice> historicPrices=null;
        //     Console.WriteLine($"HistoricData stock:{stock.Symbol} exchange:{stock.Exchange.ExchangeName}");
        //     if(stock.Exchange.ExchangeName=="CCC"){
        //         //crypto
        //         symbol=stock.Symbol.Replace("USD","");
        //         historicPrices=await _coinApiService.GetCoinApiHistoryAsync(stock.Id, symbol);
        //         historicPrices=historicPrices.Take(90).ToList();
        //     }else{
        //         historicPrices=await _tdService.UpdatedHistoricData(stock.Id,stock.Symbol,period);    
        //         historicPrices=historicPrices.Take(90).ToList();
        //     }
            
        //     Console.WriteLine($"HistoricalData records returning:{historicPrices.Count()}");
        //     return Ok(historicPrices.Select(x=>x.Close));
        // }


        [HttpGet]
        [Route("quote/{id:int}")]
        public async Task<IActionResult> FastQuote([FromRoute] int id){
            float rtnVal=await _stockRepo.Quote(id);
            return Ok(rtnVal);
        }
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery(Name ="query")]string query){
            Console.WriteLine($"Search {query}");
            List<FMPSearch> search=await _stockRepo.Search(query);
            if(search==null){
                return BadRequest("Unable to search");
            }
            return Ok(search);
        }


        // [HttpPost]
        // public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            
        //     if(!ModelState.IsValid)return BadRequest(ModelState);
        //     var stockModel=stockDto.ToStockFromCreateDTO();
        //     await _stockRepo.CreateAsync(stockModel);
        //     return CreatedAtAction(nameof(GetById),new {id=stockModel.Id},stockModel.ToStockDto());
        // }
        // [HttpPut]
        // [Route("{id:int}")]
        // public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
            
        //     if(!ModelState.IsValid)return BadRequest(ModelState);
        //     var stockModel=await _stockRepo.UpdateAsync(id,updateDto);
        //     if(stockModel==null){
        //         return NotFound();
        //     }
        //     return Ok(stockModel.ToStockDto());
        // }
        // [HttpDelete]
        // [Route("{id:int}")]
        // public async Task<IActionResult> Delete([FromRoute] int id){
            
        //     if(!ModelState.IsValid)return BadRequest(ModelState);
        //     var stockModel=await _stockRepo.DeleteAsync(id);
        //     if(stockModel==null){
        //         return NotFound();
        //     }
        //     return NoContent();
        // }

    }
}

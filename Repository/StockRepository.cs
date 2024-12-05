using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IFMPService _fmpService; 
        private readonly ApplicationDBContext _context;
        private readonly IFinnhubService _finnhubService;
        public readonly ICoinPaprika _coinPaprika;
        private readonly ICryptoPrices _cryptoPrices;

        public StockRepository(ApplicationDBContext context,ICoinPaprika coinPaprika, IFMPService fmpService,IFinnhubService finnhubService, ICryptoPrices cryptoPriceService){
            _context=context;
            _fmpService=fmpService;
            _finnhubService=finnhubService;
            _cryptoPrices=cryptoPriceService;
            _coinPaprika=coinPaprika;
        }

        public async Task<Stock?> CreateAsync(Stock stockModel)
        {
            Console.WriteLine($"CreateAsync {JsonSerializer.Serialize(stockModel)}");
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel=await _context.Stocks.FirstOrDefaultAsync(x=>x.Id==id);
            
            if(stockModel==null)
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {

            var stocks= _context.Stocks.Include(c=>c.Comments).ThenInclude(a=>a.AppUser).Include(c=>c.PriceChange).AsQueryable();//.ToListAsync();
            if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks=stocks.Where(x=>x.CompanyName.ToLower().Contains(query.CompanyName.ToLower()));
            }
            if(!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks=stocks.Where(x=>x.Symbol.ToLower().Contains(query.Symbol.ToLower()));
            }
            if(!string.IsNullOrWhiteSpace(query.SortBy)){
                if(query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase)){
                    stocks=query.IsDescending?stocks.OrderByDescending(x=>x.Symbol):stocks.OrderBy(x=>x.Symbol);
                }
            }
            var skipNumber=(query.PageNumber-1)*query.PageSize;


            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            Stock? stock=await _context.Stocks.Include(x=>x.Comments).Include(e=>e.Exchange).FirstOrDefaultAsync(x=>x.Id==(id));
            if(stock.IsExpired){
                stock=await _fmpService.FindStockBySymbolAsync(stock.Symbol);
                if(stock==null){
                    return null;
                }else{
                    await UpdateAsync(id,stock.ToUpdateDto());
                }
            }
            return stock;
        }
        private async Task<Stock?> GetCryptoBySymbolAsync(string symbol) {
            Stock stock= await _fmpService.FindCryptoBySymbolAsync(symbol);
            if(stock==null){
                stock = await _coinPaprika.FindCryptoBySymbolAsync(symbol);
            }
            return stock;
        }
        private async Task<Stock?> GetStockBySymbolAsync(string symbol) {
            Stock stock= await _fmpService.FindStockBySymbolAsync(symbol);
            if(stock==null){
                stock = await _finnhubService.FindStockBySymbolAsync(symbol);
            }
            return stock;
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol,bool isCrypto)
        {
            Stock? stock=await _context.Stocks.FirstOrDefaultAsync(x=>x.Symbol.Equals(symbol));
            if(stock==null||stock.IsExpired){
                int stockId=stock!=null?stock.Id:-1;    
                Console.WriteLine($"GetBySymbolAsync symbol:{symbol}  isCrypto:{isCrypto}");
                stock=isCrypto?await GetCryptoBySymbolAsync(symbol): await GetStockBySymbolAsync(symbol);
                stock.LastUpdated=DateTime.UtcNow;
                if(stock==null){
                    Console.WriteLine($"GetBySymbolAsync() stock was null");
                    return null;
                }else if(stockId==-1){
                    Console.WriteLine($"Create stock for {symbol}");
                    await CreateAsync(stock);
                }else{                    
                    Console.WriteLine($"Updating existing stock info {stockId},{stock.Symbol}");
                    await UpdateAsync(stockId,stock.ToUpdateDto());
                }

            }

            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x=>x.Id==id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existing=await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id);
            if(existing==null)
            {return null;}  
            _context.Entry(existing).State = EntityState.Modified; // Ensure the entity is tracked
            existing.Purchase=stockDto.Purchase;
            existing.LastDiv=stockDto.LastDiv;
            existing.MarketCap=stockDto.MarketCap;
            existing.LastUpdated=DateTime.UtcNow;
            try { 
                await _context.SaveChangesAsync(); 
            } catch (DbUpdateException ex) { 
                // Log and handle the exception 
                Console.WriteLine(ex.InnerException?.Message); 
                throw;
            } 
            return existing;

        }

        public async Task<float> Quote(int id){
            var stock=await _context.Stocks.Include(s=>s.Exchange).SingleOrDefaultAsync(x=>x.Id==id);
            if(stock==null){
                Console.WriteLine($"Stock {id} not found");
                return 0;
            }
            float returnVal=0;
            if(stock.Exchange.ExchangeName=="CCC"){
                returnVal=await _cryptoPrices.GetPriceAsync(stock.Symbol);
            }else {
            
                returnVal= await _finnhubService.GetCurrentPrice(stock.Symbol);   
            }
            return returnVal;
        }
        public async Task<List<FMPSearch>> Search(string query){
            List<FMPSearch> search=await _fmpService.GetSearch(query);
            if(search!=null&&search.Count>0){
                return search;
            }
            List<FinnhubSearch> finnSearch=await _finnhubService.GetSearch(query);
            if(finnSearch!=null){
                search=finnSearch.Select(x=>x.ToFmpSearch()).ToList();
            }
                // List<CoinPaprikaCoin> coins=await _coinPaprika.GetCoins();
                // coins.RemoveAll(x=>x.symbol.ToLower().Contains(query));
                // search.AddRange(coins.Select(x=>x.ToFmpSearch()).ToList());
            return search;
        }
    }
}
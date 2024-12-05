using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Service
{
    public class CoinPaprikaService : ICoinPaprika
    {
         private readonly ApplicationDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string url=>_config["Authentication:CoinPaprika:Url"];
        private string apiurl(string endpoint) =>$"{url}{endpoint}";
        
         public CoinPaprikaService(HttpClient httpClient,IConfiguration config,ApplicationDBContext context){          
            _httpClient=httpClient;
            _config=config;
            _context=context;
        }
        public async Task<Stock> FindCryptoBySymbolAsync(string symbol){
            CoinPaprikaDto coinDto=await GetCoinData(symbol);
            Stock existing=await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol.ToLower() == symbol.ToLower());
            Stock newCrypto=coinDto.ToStock();

            if(existing==null){
                existing=newCrypto;
                StockExchange exchange=_context.StockExchanges.FirstOrDefault(x=>x.ExchangeName=="CCC");
                existing.ExchangeId=exchange.ExchangeId;
                await _context.Stocks.AddAsync(existing);
            }else{
                existing.Purchase=newCrypto.Purchase;
                existing.MarketCap=newCrypto.MarketCap;                
            }
            existing.LastUpdated=DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<CoinPaprikaDto> GetCoinData(string symbol)
        {
            symbol=symbol.ToLower().Replace("usd","");
            Console.WriteLine($"GetCoinData({symbol})");
            CoinPaprikaCoin coin=await _context.CoinPaprikaCoins.FirstOrDefaultAsync(x=>x.symbol.ToLower()==symbol);
            if(coin==null){
                Console.WriteLine($"No paprika coin found for {symbol}");
                return null;
            }
            using (var httpClient = new HttpClient()) {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"),apiurl(coin.id+"/ohlcv/latest")))
                {
                    var response = await _httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode){
                        var content=await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"GetCoinData content:{content}");
                        CoinPaprikaLatest[] latest=JsonConvert.DeserializeObject<CoinPaprikaLatest[]>(content);
                        if(latest==null){
                            return null;
                        }
                        CoinPaprikaDto coinDto=latest[0].ToDto(coin);
                        Console.WriteLine($"GetCoinData for {coinDto.name} deserialized price{coinDto.close}");
                        return coinDto;
                    }
                }
            }
            return null;
        }

        public async Task<List<CoinPaprikaCoin>> GetCoins()
        { 
            using (var httpClient = new HttpClient()) {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"),apiurl("")))
                {
                    var response = await _httpClient.SendAsync(request);
                    
                    if(response.IsSuccessStatusCode){

                        var content=await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"GetCoins content:{content.Length}");
                        CoinPaprikaCoin[] coins=JsonConvert.DeserializeObject<CoinPaprikaCoin[]>(content);
      
                        // Console.WriteLine($"Deserialized CoinPaprika Length:{coins.Length}");
                        List<CoinPaprikaCoin> currentList=await _context.CoinPaprikaCoins.ToListAsync();

                        if(coins==null||currentList.Count==coins.Length){
                            //list is the same size, exit
                            // Console.WriteLine($"existing same length returning existing");
                            return currentList;
                        }
                        List<CoinPaprikaCoin> newCoins= coins.ToList().FindAll(coin=>!currentList.Any(cur=>cur.id==coin.id)&&coin.is_active);
                        int newCount=newCoins.Count;
                        
                        // Console.WriteLine($"newCount is {newCount} coins");
                        if(newCoins.Count>0){
                            currentList.AddRange(newCoins);
                            await _context.CoinPaprikaCoins.AddRangeAsync(newCoins);

                            await _context.SaveChangesAsync();
                        }
                        // Console.WriteLine($"Saving exchanges complete, returning to controller");
                        return currentList;
                    }
                    
                }
            }
            return null;
        }
    }
}
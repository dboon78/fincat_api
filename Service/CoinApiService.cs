using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
    public class CoinApiService : ICoinApiService
    {

        private readonly ApplicationDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string key=>_config["Authentication:CoinAPI:Key"];
        private string url=>_config["Authentication:CoinAPI:Url"];
        private string apiurl(string endpoint) =>endpoint.Contains("?")?$"{url}{endpoint}":$"{url}{endpoint}";

        public CoinApiService(HttpClient httpClient, IConfiguration config, ApplicationDBContext context){
            _httpClient = httpClient;

            _config = config;
            _context = context;
        }
        private async Task<List<CoinApiHistory>> FetchCoinApiHistorics(string symbol){
            try{
                // _httpClient.DefaultRequestHeaders.Accept.Clear();
                // _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                // _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("X-CoinAPI-Key",key);
                _httpClient.DefaultRequestHeaders.Add("Accept", "text/plain");
                _httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key",key);
                var result=await _httpClient.GetAsync(apiurl($"ohlcv/BIGONE_SPOT_{symbol}_USDT/history?period_id=1DAY&limit=100&time_start={DateTime.UtcNow.AddDays(-100).ToString("o")}"));
                
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    //Console.WriteLine("FetchCoinApiHistorics: "+content);
                    CoinApiHistory[] coinApiHistory=JsonConvert.DeserializeObject<CoinApiHistory[]>(content);
                    if(coinApiHistory==null){
                      //  Console.WriteLine("coinApiHistory is NULL");
                        return null;
                    }
                    //Console.WriteLine($"coinApiHistory count:{coinApiHistory.Length}");
                    return coinApiHistory.ToList();
                }
            }catch(Exception err){     
                Console.WriteLine("Error: "+err.Message);           
            }            
            return null;
        }
        //
        public async Task<List<HistoricPrice>> GetCoinApiHistoryAsync(int stockId, string symbol)
        {
            Console.WriteLine($"GetCoinApiHistoryAsync({stockId},{symbol})");
            List<HistoricPrice> historicPrices=await _context.HistoricPrices.Where(x=>x.StockId==stockId).OrderByDescending(hp=>hp.Date).ToListAsync();
            if(historicPrices.Count>0&&historicPrices[0].Date==DateTime.Today){
                Console.WriteLine($"Saved historic prices for {symbol} is up to date.");
                return historicPrices;
            }
            Console.WriteLine("historic prices:"+historicPrices.Count);
            var coinApiList=await FetchCoinApiHistorics(symbol);
            if (coinApiList == null) {
                Console.WriteLine($"GetCoinApiHistoryAsync no response found for {symbol}");
                return historicPrices;
            }
            coinApiList.RemoveAll(x=>historicPrices.Any(hp=>hp.Date.Date==x.time_close.Date.Date));
            historicPrices.AddRange(coinApiList.Select(x=>x.ToHistoricPrice(stockId)));
            await _context.HistoricPrices.AddRangeAsync(coinApiList.Select(x=>x.ToHistoricPrice(stockId)));
            await _context.SaveChangesAsync();
            
            return historicPrices.OrderByDescending(x=>x.Date).ToList();        
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Data;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TwelveDataSharp;
using TwelveDataSharp.Library.ResponseModels;
using static api.Dtos.Stock.TwelveData;

namespace api.Service
{
    public class TwelveDataService : ITwelveDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string key=>_config["Authentication:TwelveData:Key"];
        private string url=>_config["Authentication:TwelveData:Url"];
        private string apiurl(string endpoint) =>endpoint.Contains("?")?$"{url}{endpoint}&apikey={key}":$"{url}{endpoint}?apikey={key}";
        
        private readonly ApplicationDBContext _context;
        public TwelveDataService(HttpClient httpClient,IConfiguration config,ApplicationDBContext context){          
            _httpClient=httpClient;
            _config=config;
            _context=context;
        }
        public async Task<List<HistoricPrice>> FetchHistoric(string symbol, int stockId){
            try{
                var result=await _httpClient.GetAsync(apiurl($"time_series?interval=1day&symbol={symbol}&outputsize=100"));
                
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    Console.WriteLine("FetchHistoric: "+content);
                    var root=JsonConvert.DeserializeObject<TimeSeriesRoot>(content);
                    List<HistoricPrice> historicPrices=new List<HistoricPrice>();
                    foreach(TimeSeriesValue value in root.values){
                        Console.WriteLine($"Adding TimeSeriesValue:{JsonConvert.SerializeObject(value)}");
                        historicPrices.Add(value.ToHistoricPrice(root.meta.exchange,stockId));
                        Console.WriteLine($"Added:{JsonConvert.SerializeObject(historicPrices[historicPrices.Count-1])}");
                    }
                    return historicPrices;
                }
            }catch(Exception err){                
            }            
            return null;
        }

        public int PeriodToDays(string period){
            switch(period){
                case "1D":
                    return 1;
                case "5D":
                    return 5;
                case "1M":
                    return 30;
                case "3M":
                    return 30*3;
                case "6M":
                    return 30*6;
                case "1Y":
                    return 365;
                case "3Y":
                    return 365*3;
                case "5Y":
                case "10Y":
                    return 365*5;
                
            }
            return 1000;

        }


        public async Task<List<HistoricPrice>> UpdatedHistoricData(int stockId,string symbol, string period="1Y", string interval="1day")
        {
            Console.WriteLine($"TwelveDataService.UpdatedHistoricData({stockId},{interval})");
            List<HistoricPrice> historicPrices=await _context.HistoricPrices.Where(x=>x.StockId==stockId).OrderByDescending(hp=>hp.Date).ToListAsync();
            if(historicPrices.Count>0&&historicPrices[0].Date==DateTime.Today){
                Console.WriteLine($"Saved historic prices for {symbol} is up to date.");
                return historicPrices;
            }
            Console.WriteLine("historic prices:"+historicPrices.Count);
            Console.WriteLine($"Querying twelveData {symbol}");
            List<HistoricPrice> fetchedHistorics=await FetchHistoric(symbol,stockId);
            
            if (fetchedHistorics == null) {
                Console.WriteLine($"UpdatedHistoricData no quote found for {symbol}");
                return historicPrices;
            }
            // Get a HashSet of existing dates for efficient lookup
            fetchedHistorics.RemoveAll(x=>historicPrices.Any(hp=>hp.Date==x.Date));
            historicPrices.AddRange(fetchedHistorics);
            await _context.HistoricPrices.AddRangeAsync(fetchedHistorics);
            await _context.SaveChangesAsync();
            return historicPrices.OrderByDescending(x=>x.Date).ToList();
        }
        
        
    }
}
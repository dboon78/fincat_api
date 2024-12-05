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
    public class FinnhubService : IFinnhubService
    {
        
        private readonly ApplicationDBContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string key=>_config["Authentication:Finnhub:Key"];
        private string url=>_config["Authentication:Finnhub:Url"];
        private string apiurl(string endpoint) =>endpoint.Contains("?")?$"{url}{endpoint}&token={key}":$"{url}{endpoint}?token={key}";
        
         public FinnhubService(HttpClient httpClient,IConfiguration config,ApplicationDBContext context){          
            _httpClient=httpClient;
            _config=config;
            _context=context;
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol){
            FinnhubProfile profile=await GetProfile(symbol);
            if(profile==null)return null;
            Stock stock = profile.ToStock();
            Stock existing=await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol==symbol);
            if(existing==null){
                existing=stock;
                await _context.Stocks.AddAsync(existing);
            }
            float price=await GetCurrentPrice(symbol);
            existing.Purchase=(decimal)price;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<FinnhubProfile> GetProfile(string symbol){
            try{
                var result=await _httpClient.GetAsync(apiurl($"stock/profile2?symbol={symbol}"));
                if(result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();
                    //Console.WriteLine($"FinnHub GetCurrentPrice content: {content}");
                    FinnhubProfile profile=JsonConvert.DeserializeObject<FinnhubProfile>(content);
                    if(profile==null)return null;

                    return profile;
                }
            }catch(Exception err){
                Console.WriteLine($"GetProfile error: {err.Message}");
            }
            return null;
        }

        public async Task<float> GetCurrentPrice(string symbol)
        {
            Console.WriteLine($"GetCurrentPrice({symbol})");
            float curPrice=0;
            try{
                var result=await _httpClient.GetAsync(apiurl($"quote?symbol={symbol}"));
                if(result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();
                    //Console.WriteLine($"FinnHub GetCurrentPrice content: {content}");
                    FinnhubQuote quote=JsonConvert.DeserializeObject<FinnhubQuote>(content);
                    if(quote!=null){
                        curPrice=(float)quote.c;
                    }
                }
            }catch(Exception err){                
            }            
            return curPrice;
        }

        //search: https://finnhub.io/api/v1/search?q=apple&token=cslbaj9r01qq49fgq3i0cslbaj9r01qq49fgq3ig
        public async Task<List<FinnhubSearch>> GetSearch(string query){
            FinnhubSearchRoot searchResult=new FinnhubSearchRoot();
            try{
                string endpoint=apiurl($"search?q={query}");
                var result=await _httpClient.GetAsync(endpoint);
                if(result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();
                    searchResult=JsonConvert.DeserializeObject<FinnhubSearchRoot>(content);
                }
            }catch(Exception err){
                Console.WriteLine($"Error: {err.Message}");
            }
            return searchResult.result; 
        }
    }
}
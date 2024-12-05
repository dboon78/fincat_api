using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Currency;
using api.Interfaces;
using api.Models;
using Newtonsoft.Json;

namespace api.Service
{
    public class FreeCurrencyService : IFreeCurrencyService
    { 
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ApplicationDBContext _context;
        private string key=>_config["Authentication:FreeCurrencyApi:Key"];
        private string url=>_config["Authentication:FreeCurrencyApi:Url"];
        private string apiurl(string endpoint) =>endpoint.Contains("?")?$"{url}{endpoint}&apikey={key}":$"{url}{endpoint}?apikey={key}";
        public FreeCurrencyService(HttpClient httpClient,IConfiguration config,ApplicationDBContext context){
            _httpClient=httpClient;
            _config=config;
            _context=context;
        }
        public async Task<CurrencyDetailsRoot> GetCurrencyDetails()
        {
            CurrencyDetailsRoot currencyDetailsRoot=null;
            try{
                Console.WriteLine($"GetCurrencyDetails api");
                var result=await _httpClient.GetAsync(apiurl($"currencies"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    Console.WriteLine("GetFreeCurrencies content: "+content);
                    currencyDetailsRoot=JsonConvert.DeserializeObject<CurrencyDetailsRoot>(content);                      
                }
            }catch(Exception err){
                Console.WriteLine("GetCurrencyDetails Error"+err.Message);
            }
            return currencyDetailsRoot;
        }

        public async Task<List<Currency>> GetCurrencyList()
        {
            var currencies= await _context.Currencies.ToListAsync();
            Console.WriteLine("GetCurrencyList");
            if(currencies.Count>0&&!currencies[0].IsExpired){
                return currencies;
            }
            FreeCurrency freeCurrency=await GetFreeCurrencies();
            if(freeCurrency==null){
                return null;
            }
            Console.WriteLine($"freeCurrency:{freeCurrency.data.Count}");
            CurrencyDetailsRoot details=await GetCurrencyDetails();
            if(details==null){
                return null;
            }
            Console.WriteLine($"details:{details.data.Count}");
            foreach(var c in freeCurrency.data){
                Console.WriteLine("freeCurrency processing:"+c.Key);
                var currency=currencies.FirstOrDefault(x=>x.Code==c.Key);
                if(!details.data.TryGetValue(c.Key, out var detail)){
                    Console.WriteLine($"{c.Key}: no details found");
                }
                if(currency==null){
                    currency=new Currency();
                    currencies.Add(currency);
                    currency.Code=c.Key;
                    await _context.Currencies.AddAsync(currency);
                }
                currency.Value=c.Value;
                if(detail!=null){
                    currency.Symbol=detail.symbol_native;
                    currency.Name=detail.name;
                    currency.Digits=detail.decimal_digits;

                }
                currency.LastUpdated=DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return currencies;
        }

        public async Task<FreeCurrency> GetFreeCurrencies()
        {
            FreeCurrency currencies=null;
            try{
                Console.WriteLine($"GetFreeCurrencies api");
                var result=await _httpClient.GetAsync(apiurl($"latest"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    Console.WriteLine("GetFreeCurrencies content: "+content);
                    currencies=JsonConvert.DeserializeObject<FreeCurrency>(content);                    
                    
                    
                }
            }catch(Exception err){
                Console.WriteLine("GetFreeCurrencies Error"+err.Message);
            }
            return currencies;
        }
        public async Task<Currency> GetCurrency(string code){
            var currency= await _context.Currencies.FirstOrDefaultAsync(x=>x.Code==code);
            if(currency.IsExpired){
                List<Currency> currencyList= await GetCurrencyList();
                currency=currencyList.FirstOrDefault(x=>x.Code==code);
            }
            return currency;
        }
    }
}
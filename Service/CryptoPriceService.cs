using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;

namespace api.Service
{
    public class CryptoPriceService : ICryptoPrices
    { 
        private readonly HttpClient _httpClient;
        private string url="https://cryptoprices.cc/ETH/";
        
         public CryptoPriceService(HttpClient httpClient){          
            _httpClient=httpClient;
        }
        public async Task<float> GetPriceAsync(string symbol)
        {
            Console.WriteLine($"GetPriceAsync()");
            symbol=symbol.Substring(0,symbol.Length-3);
            var result=await _httpClient.GetAsync($"https://cryptoprices.cc/{symbol}/");
            if(result.IsSuccessStatusCode){
                var content = await result.Content.ReadAsStringAsync();
                if(float.TryParse(content, out var price)){
                    return price;
                }
            }
            return 0;
        }
    }
}
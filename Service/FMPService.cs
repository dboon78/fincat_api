using System.Web;
using api.Data;
using api.Dtos.Stock;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Service
{

    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ApplicationDBContext _context;
        private string key=>_config["Authentication:FMP:Key"];
        private string url=>_config["Authentication:FMP:Url"];
        private string apiurl(string endpoint) =>endpoint.Contains("?")?$"{url}{endpoint}&apikey={key}":$"{url}{endpoint}?apikey={key}";
        public FMPService(HttpClient httpClient,IConfiguration config,ApplicationDBContext context){
            _httpClient=httpClient;
            _config=config;
            _context=context;
        }

        public async Task<string> GetRelay(string endpoint){
             try{
                Console.WriteLine("FMPService GetRelay "+endpoint);
                endpoint=HttpUtility.HtmlEncode(endpoint);
                var result=await _httpClient.GetAsync(apiurl(endpoint)) ;
                if(result.IsSuccessStatusCode){
                    return await result.Content.ReadAsStringAsync();;
                }
            }catch(Exception err){
                
            }
            
            return null;
        }

        public async Task<List<FMPSearch>> GetSearch(string query){
            List<FMPSearch> searchResults=new List<FMPSearch>();
            
            //Console.WriteLine("FMPService GetSearch "+query);
            try{
                string endpoint=apiurl($"search?query={query}&limit=100");
                var result=await _httpClient.GetAsync(endpoint);
                if(result.IsSuccessStatusCode){
                    var content = await result.Content.ReadAsStringAsync();
                    // if(content.Contains("Limit Reach")){
                    //     Console.WriteLine("FMP limit reached");
                    // }
                    searchResults.AddRange(JsonConvert.DeserializeObject<FMPSearch[]>(content));
                }
            }catch(Exception err){

            }
            return searchResults; 
        }

        public async Task<PriceChange> GetPriceChange(string symbol){
            try{
                //Console.WriteLine($"GetPriceChange api {symbol}");
                var result=await _httpClient.GetAsync(apiurl($"stock-price-change/{symbol}"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    //Console.WriteLine("GetPriceChange content: "+content);
                    var priceChange=JsonConvert.DeserializeObject<PriceChange[]>(content);                    
                    if(priceChange!=null&&
                        priceChange.Length>0){
                        priceChange[0].dateTimeStamp=DateTime.UtcNow;
                        return priceChange[0];
                    }
                }
            }catch(Exception err){}
            return null;
        }
        public async Task<Stock> FindCryptoBySymbolAsync(string symbol){
            try{

                string url=apiurl($"quote/{symbol}");
                //Console.WriteLine($"FindCryptoBySymbolAsync {url}");
                var result=await _httpClient.GetAsync(url);
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    //Console.WriteLine($"FindCryptoBySymbolAsync: "+content);
                    if(content.Contains("Limit Reach")){
                        return null;
                    }
                    var arr=JsonConvert.DeserializeObject<FMPCrypto[]>(content);
                    var crypto=arr[0];
                    //Console.WriteLine($"fmp crypto {JsonConvert.SerializeObject(crypto)}");
                    StockExchange? exchange=await _context.StockExchanges.FirstOrDefaultAsync(x => x.ExchangeName=="CCC");
                    Stock cryptoStock=crypto.ToStockFromFMPCrypto();
                    cryptoStock.ExchangeId=exchange.ExchangeId; 
                    //Console.WriteLine($"crypto found for {symbol} -> {JsonConvert.SerializeObject(cryptoStock)}");
                    return cryptoStock;

                }
            }catch(Exception err){       
                Console.WriteLine("Error: "+err.Message);         
            }       
            return null;
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"profile/{symbol}"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    //Console.WriteLine("FindStockBySymbolAsync: "+content);
                    var tasks=JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock=tasks[0];
                    if(stock.exchangeShortName=="NASDAQ")stock.exchange="NASDAQ Global Market";

                    if(stock!=null){
                        StockExchange? exchange=await _context.StockExchanges.FirstOrDefaultAsync(x => x.ExchangeName.ToLower()==stock.exchange.ToLower());
                        Stock rtnStock=stock.ToStockFromFMP();
                        if(exchange!=null){
                            rtnStock.ExchangeId=exchange.ExchangeId;
                        }else{
                            Console.WriteLine("No exchange found with name "+stock.exchange);
                        }
                        //Console.WriteLine($"FindStockBySymbol({symbol}) returning {JsonConvert.SerializeObject(rtnStock)}");
                        return rtnStock;
                    }
                }
            }catch(Exception err){                
            }            
            return null;
        }

        public async Task<Stock> UpdateStock(string symbol,bool isCrypto)
        {       
            //Console.WriteLine("FMPService UpdateStock "+symbol);
            
            Stock existing=await _context.Stocks.FirstOrDefaultAsync(x=>x.Symbol==symbol);
            if(existing==null||existing.IsExpired){
                try{
                    var updated=isCrypto?await FindCryptoBySymbolAsync(symbol): await FindStockBySymbolAsync(symbol);
                    if(existing==null&&updated!=null)
                    {
                        //Console.WriteLine($"FMPService stock not found, adding new");
                        existing=updated;
                        await _context.Stocks.AddAsync(existing);
                    }else{
                        //Console.WriteLine($"FMPService updating existing stock");
                        existing.Purchase=updated.Purchase;
                        existing.LastDiv=updated.LastDiv;
                        existing.MarketCap=updated.MarketCap;
                    }
                }catch(Exception err){
                    Console.WriteLine($"Error({symbol}) UpdateStock: "+err.Message);
                }

            }
            existing.LastUpdated=DateTime.UtcNow;
            _context.Stocks.Update(existing);
            //Console.WriteLine($"FMPService Stock updated {existing.CompanyName} {existing.Purchase}  {existing.LastUpdated.ToString()}");
            await _context.SaveChangesAsync();
            return existing;
        
        }

        public async Task<PriceChange> UpdatePriceChange(string symbol)
        {    
            Console.WriteLine($"FMPService UpdatePriceChange for {symbol}");
            var record=_context.PriceChanges.FirstOrDefault(x => x.Symbol==symbol);
            if(record==null||record.IsExpired){
                var updated=await GetPriceChange(symbol);
                if(updated!=null){
                    if(record==null){
                        record=updated;
                        await _context.AddRangeAsync(record);
                    }else{
                        record._1D=updated._1D;
                        record._5D=updated._5D;
                        record._1M=updated._1M;
                        record._3M=updated._3M;
                        record._6M=updated._6M;
                        record._1Y=updated._1Y;
                        record._5Y=updated._5Y;
                        record._10Y=updated._10Y;
                    }
                }  
            }
            record.dateTimeStamp=DateTime.UtcNow;
            //Console.WriteLine($"UpdatePriceChange about to save {symbol} {updated}");
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<List<StockExchange>> UpdateExchanges()
        {
            //Console.WriteLine($"FMPService UpdateExchanges");
            using (var httpClient = new HttpClient()) {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"),apiurl("is-the-market-open-all")))
                {
                    request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                    var response = await _httpClient.SendAsync(request);
                    
                    if(response.IsSuccessStatusCode){

                        var content=await response.Content.ReadAsStringAsync();
                        //Console.WriteLine($"GetExchanges content:{content}");
                        FMPExchangeHours[] exchangeHours=JsonConvert.DeserializeObject<FMPExchangeHours[]>(content);
                        List<StockExchange> result=new List<StockExchange>()
                        {
                            new StockExchange(){
                                ExchangeName="CCC",
                                openingHour=new DateTime(DateTime.UtcNow.Year,DateTime.UtcNow.Month,DateTime.UtcNow.Day,0,0,0),
                                closingHour=new DateTime(DateTime.UtcNow.Year,DateTime.UtcNow.Month,DateTime.UtcNow.Day,23,59,59)
                            }
                        };
                        foreach(var exchange in exchangeHours){
                            //Console.WriteLine($"Adding {exchange.name} with openingHour:{exchange.openingHour.ToString()}");
                            result.Add(new StockExchange(){
                                ExchangeName=exchange.name,
                                openingHour=exchange.OpeningHour,
                                closingHour=exchange.ClosingHour,                                
                            });
                        }

                        await _context.StockExchanges.AddRangeAsync(result);
                        await _context.SaveChangesAsync();
                        //Console.WriteLine($"Saving exchanges complete, returning to controller");
                        return result.ToList();
                    }
                    
                }
            }
            return null;
        }
        public async Task<FMPBalanceSheet[]> GetBalanceSheetsAsync(string symbol){
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"balance-sheet-statement/{symbol}?limit=5&priod=annual"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    
                    // Console.WriteLine("GetBalanceSheetsAsync: "+content);
                    var data=JsonConvert.DeserializeObject<FMPBalanceSheet[]>(content);
                    if(data==null){
                        return null;
                    }
                    return data;
                }
            }catch(Exception err){    
                Console.WriteLine("error:"+err.Message);
            }            
            return null;
        }
        public async Task<FMPCashFlow[]> GetCashFlowsAsync(string symbol){
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"cash-flow-statement/{symbol}?limit=5&priod=annual"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    
                    Console.WriteLine("GetCashFlowsAsync: "+content);
                    var data=JsonConvert.DeserializeObject<FMPCashFlow[]>(content);
                    return data;
                }
            }catch(Exception err){                
            }            
            return null;
        }
        public async Task<FMPKeyRatios[]> GetKeyRatiosAsync(string symbol){
//ratios/AAPL?period=quarter
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"ratios/{symbol}?limit=5&priod=annual"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    
                    //Console.WriteLine("FindStockBySymbolAsync: "+content);
                    var data=JsonConvert.DeserializeObject<FMPKeyRatios[]>(content);
                    return data;
                }
            }catch(Exception err){                
            }            
            return null;
        }
        public async Task<FMPProfile[]> GetProfilesAsync(string symbol){
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"profile/{symbol}"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    
                    //Console.WriteLine("FindStockBySymbolAsync: "+content);
                    var data=JsonConvert.DeserializeObject<FMPProfile[]>(content);
                    return data;
                }
            }catch(Exception err){                
            }            
            return null;
        }
        public async Task<FMPIncomeStatement[]> GetIncomeStatement(string symbol){
            try{
                //Console.WriteLine("FMPService FindStockBySymbolAsync "+symbol);
                var result=await _httpClient.GetAsync(apiurl($"income-statement/{symbol}?limit=5&priod=annual"));
                if(result.IsSuccessStatusCode){
                    var content=await result.Content.ReadAsStringAsync();
                    
                    //Console.WriteLine("FindStockBySymbolAsync: "+content);
                    var data=JsonConvert.DeserializeObject<FMPIncomeStatement[]>(content);
                    return data;
                }
            }catch(Exception err){                
            }            
            return null;
        }

    }
}
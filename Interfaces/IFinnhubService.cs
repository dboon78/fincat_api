using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IFinnhubService
    {
        Task<Stock> FindStockBySymbolAsync(string symbol);
        Task<float> GetCurrentPrice(string symbol);
        Task<List<FinnhubSearch>> GetSearch(string query);
        Task<FinnhubProfile> GetProfile(string symbol);
    }
}
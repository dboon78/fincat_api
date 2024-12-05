using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface ICoinPaprika
    {
        Task<List<CoinPaprikaCoin>> GetCoins();

        Task<CoinPaprikaDto> GetCoinData(string symbol);
        Task<Stock> FindCryptoBySymbolAsync(string symbol);
    }
}
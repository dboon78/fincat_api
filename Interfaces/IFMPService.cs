using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IFMPService
    {
        Task<Stock> FindStockBySymbolAsync(string symbol);
        Task<Stock> FindCryptoBySymbolAsync(string symbol);
        Task<PriceChange> GetPriceChange(string symbol);
        Task<string> GetRelay(string endpoint);
        Task<Stock> UpdateStock(string symbol,bool isCrypto);
        Task<PriceChange> UpdatePriceChange(string symbol);
        Task<List<FMPSearch>> GetSearch(string query);

        Task<List<StockExchange>> UpdateExchanges();
        Task<FMPBalanceSheet[]> GetBalanceSheetsAsync(string symbol);

        Task<FMPCashFlow[]> GetCashFlowsAsync(string symbol);
        Task<FMPKeyRatios[]> GetKeyRatiosAsync(string symbol);
        Task<FMPProfile[]> GetProfilesAsync(string symbol);
        Task<FMPIncomeStatement[]> GetIncomeStatement(string symbol);
        Task<string> GetFinancialAnalysis(string symbol);
        
    }
}
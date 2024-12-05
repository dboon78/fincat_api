using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol,bool isCrypto);
        Task<Stock?> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExists(int id);
        Task<float> Quote(int id);
        Task<List<FMPSearch>> Search(string query);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Portfolio;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        // Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<List<Portfolio>> GetPortfolios(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolio(AppUser appUser, int id);
        Task<Portfolio> UpdatePortfolio(AppUser appUser, UpdatePortfolioRequest request);
        Task<List<PriceDto>> PriceList(AppUser appUSer);
    }
}
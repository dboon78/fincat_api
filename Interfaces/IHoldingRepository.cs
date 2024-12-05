using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Holding;
using api.Models;

namespace api.Interfaces
{
    public interface IHoldingRepository
    {
        Task<Holding?> GetHolding(int id);
        Task<List<Holding>> GetHoldingsAsync(AppUser user);
        Task<Holding> CreateHolding(int stockId, int porfolioId);

        Task<Holding> DeleteHolding(int holdingId);

        Task<Holding> UpdateHolding(UpdateHoldingRequest holding);
    }
}
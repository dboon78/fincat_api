using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Holding;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Repository
{
    public class HoldingRepository : IHoldingRepository
    {        
        private readonly ApplicationDBContext _context;
        private readonly IFMPService _fmpService;
        public HoldingRepository(ApplicationDBContext context,IFMPService fmpService){
            _context=context;
            _fmpService=fmpService;
        }
        public async Task<Holding> CreateHolding(int stockId, int porfolioId)
        {
            Holding? holding = await _context.Holdings.FirstOrDefaultAsync(x => x.StockId == stockId && x.PortfolioId == porfolioId);
            if(holding!=null){
                return holding;
            }
            
            holding=new Holding(){
              StockId=stockId,
              PortfolioId=porfolioId
            };
            await _context.Holdings.AddAsync(holding);
            await _context.SaveChangesAsync();
            return holding;
        }

        public async Task<Holding> DeleteHolding(int holdingId)
        { 
            var existing=await _context.Holdings.FirstOrDefaultAsync(x => x.HoldingId==holdingId);
            if(existing==null)
            {
                return null;
            }
            _context.Holdings.Remove(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Holding>> GetHoldingsAsync(AppUser user)
        {
            Console.WriteLine($"GetHoldingAsync for user:{user.UserName}");
            var holdingsQuery= _context.Holdings
                                            .Where(p=>p.Portfolio.AppUserId==user.Id);
            holdingsQuery=holdingsQuery
                            .Include(p=>p.Stock)
                                .ThenInclude(s=>s.PriceChange);
            holdingsQuery=holdingsQuery
                            .Include(x=>x.Stock)
                                .ThenInclude(e=>e.Exchange);
            var holdings=holdingsQuery.ToList();
            // Console.WriteLine($"found {holdings.Count} holdings for {user.UserName}");
            // Console.WriteLine($"found PriceChange? {(holdings[0].Stock.PriceChange==null?"null":holdings[0].Stock.PriceChange._10Y)}");
            // Console.WriteLine($"")
            foreach(var h in holdings){
                // Console.WriteLine($"Processing holding {h.HoldingId}");

                if(h.Stock.IsExpired){
                    //Update stock info
                    h.Stock=await _fmpService.UpdateStock(h.Stock.Symbol,h.Stock.Exchange.ExchangeName=="CCC");
                }
                    
                if(h.Stock.PriceChange==null||
                    h.Stock.PriceChange.IsExpired){
                    //Update pricechange
                    h.Stock.PriceChange=await _fmpService.UpdatePriceChange(h.Stock.Symbol);
                }                
                // }else if(h.Stock==null){
                //     Console.WriteLine($"GetHoldingsAsync NO STOCK FOUND WITH ID {h.StockId}");
                // }
            }
            return holdings;
        }

        public async Task<Holding> UpdateHolding(UpdateHoldingRequest holding)
        {
            var existing=await _context.Holdings.FirstOrDefaultAsync(x => x.HoldingId==holding.HoldingId);
            if(existing==null){
                return null;
            }
            _context.Entry(existing).State = EntityState.Modified; // Ensure the entity is tracked
            if(holding?.BookCost!=null)existing.BookCost = holding.BookCost;
            if(holding?.Units!=null) existing.Units = holding.Units;
            try { 
                await _context.SaveChangesAsync(); 
            } catch (DbUpdateException ex) { 
                // Log and handle the exception 
                Console.WriteLine(ex.InnerException?.Message); 
                throw;
            } 
            return existing;
        }

        public async Task<Holding?> GetHolding(int id){
            
            return await _context.Holdings.Include(s=>s.Portfolio).FirstOrDefaultAsync(h=>h.HoldingId==id);
        }
    }
}
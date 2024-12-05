using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using api.Helpers;
using api.Dtos.Stock;
using api.Dtos.Portfolio;
using Newtonsoft.Json;
using api.Mappers;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IFMPService _fmpService;
        private readonly IStockRepository _stockRepository;
        public PortfolioRepository(ApplicationDBContext context,IFMPService fmpService,IStockRepository stockRepository){
            _context=context;
            _fmpService=fmpService;
            _stockRepository=stockRepository;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }
        public async Task<Portfolio> UpdatePortfolio(AppUser appUser, UpdatePortfolioRequest request){
            var portfolioModel= await _context.Portfolios.FirstOrDefaultAsync(p=>p.AppUserId==appUser.Id&&p.PortfolioId==request.PortfolioId);
            if(portfolioModel==null)return null;
            
            _context.Entry(portfolioModel).State = EntityState.Modified; // Ensure the entity is tracked
            portfolioModel.PortfolioName=request.PortfolioName;
            try { 
                await _context.SaveChangesAsync(); 
            } catch (DbUpdateException ex) { 
                // Log and handle the exception 
                Console.WriteLine(ex.InnerException?.Message); 
                throw;
            } 
            return portfolioModel;
        }
        public async Task<Portfolio> DeletePortfolio(AppUser appUser, int id)
        {
            var portfolioModel=await _context.Portfolios.FirstOrDefaultAsync(p=>p.AppUserId==appUser.Id&&p.PortfolioId==id);
            if(portfolioModel==null)return null;
            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;

        }

        public async Task<List<PriceDto>> PriceList(AppUser appUSer){
            var portfolios=await _context.Portfolios.Where(p=>p.AppUserId==appUSer.Id).Include(p=>p.Holdings).ThenInclude(h=>h.Stock).ToListAsync();
            List<PriceDto> priceList=new List<PriceDto>();
            foreach(var portfolio in portfolios){
                foreach(Holding h in portfolio.Holdings){
                    float price=await _stockRepository.Quote(h.StockId);
                    if(price==0)continue;
                    priceList.Add(h.ToPrice(price));
                }
            }
            return priceList;
        }

        public async Task<List<Portfolio>> GetPortfolios(AppUser user){
            //Console.WriteLine($"GetPortfolios {user.Id}");
            var portfolios = await _context.Portfolios
                                            .Where(p=>p.AppUserId==user.Id)
                                            .Include(s=>s.Holdings)
                                                .ThenInclude(h=>h.Stock)
                                                    .ThenInclude(pc=>pc.PriceChange)
                                            .Include(s=>s.Holdings)
                                                .ThenInclude(h=>h.Stock)
                                                    .ThenInclude(s=>s.Exchange)
                                            .ToListAsync();
            //Console.WriteLine($"Get Portfolios  {portfolios.Count}");
            
            foreach(Portfolio p in portfolios){
                 foreach(var h in p.Holdings){
              //      Console.WriteLine($"GetPortfolios Holding {h.Stock.Symbol}");
                    if(h.Stock.IsExpired){
                        //Update stock info
                        h.Stock=await _fmpService.UpdateStock(h.Stock.Symbol,h.Stock.Exchange.ExchangeName=="CCC");
                    }
                    
                //    Console.WriteLine($"PriceChange {h.Stock.Symbol}");
                    if(h.Stock.PriceChange==null||
                        h.Stock.PriceChange!.IsExpired){
                        //Update pricechange
                        h.Stock.PriceChange=await _fmpService.UpdatePriceChange(h.Stock.Symbol);
                    }
                 }
            }
            return portfolios;
        }

    }
}
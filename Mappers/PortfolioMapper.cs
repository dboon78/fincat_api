using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Portfolio;
using api.Models;

namespace api.Mappers
{
    public static class PortfolioMapper
    {
        public static PortfolioDto ToDto(this Portfolio portfolio){
            return new PortfolioDto(){
                PortfolioId = portfolio.PortfolioId,
                PortfolioName=portfolio.PortfolioName!,
                Holdings=portfolio.Holdings.Select(x => x.ToDto()).ToList(),
            };
        }   
    }
}
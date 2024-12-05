using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Holding;
using api.Dtos.Portfolio;
using api.Mappers;
using api.Models;

namespace api.Mappers
{
    public static class HoldingMapper
    {
        public static HoldingDto ToDto(this Holding holding){
            return new HoldingDto(){
                HoldingId = holding.HoldingId,
                BookCost=holding.BookCost,
                Units=holding.Units,
                Stock=holding.Stock?.ToStockDto()
            };
        }
        public static PriceDto ToPrice(this Holding holding,float price){
            return new PriceDto(){
                s=holding.Stock.Symbol,
                p=price
            };
        }
    }
}
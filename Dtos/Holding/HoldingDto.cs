using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;

namespace api.Dtos.Holding
{
    public class HoldingDto
    {
        public int HoldingId{ get;set;}      
        // public int PortfolioId { get; set; }
        // public Portfolio Portfolio { get; set; }
        // public int StockId{ get; set; }
        public StockDto Stock { get; set; } 
        public float BookCost{ get; set; }=0;  
        public float Units{ get; set; }=0;
    }
}
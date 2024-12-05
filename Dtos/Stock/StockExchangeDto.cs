using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class StockExchangeDto
    {
        public string ExchangeName { get; set;}
        public DateTime openingHour{ get; set; }    
        public DateTime closingHour{ get; set; }        
    }
}
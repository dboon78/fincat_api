using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Holdings")]
    public class Holding
    {
        public int HoldingId{ get;set;}      
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public int StockId{ get; set; }
        public Stock Stock { get; set; } 
        public float BookCost{ get; set; }=0;  
        public float Units{ get; set; }=0;
    }
}
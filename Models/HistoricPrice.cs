using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("HistoricPrices")]
    public class HistoricPrice
    {
        public int Id { get; set; }
        
        [Index("IX_HistoricPrice_StockId_Date",1)]
        public int StockId{get;set;}
        public Stock Stock;        
        [Index("IX_HistoricPrice_StockId_Date",2)]
        public DateTime Date { get; set; }
        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public long Volume { get; set; }
    }
}
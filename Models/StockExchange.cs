
using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    [Table("StockExchanges")]
    public class StockExchange
    {
        public int ExchangeId { get; set; }
        public string ExchangeName { get; set;}
        public DateTime openingHour{ get; set; }    
        public DateTime closingHour{ get; set; }        
        public List<Stock> Stocks{get; set;}=new List<Stock>();

    }
}
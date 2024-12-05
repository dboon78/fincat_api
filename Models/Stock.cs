
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Extensions;
using Newtonsoft.Json;

namespace api.Models
{
    [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; }
        public int ExchangeId { get; set; }
        public StockExchange Exchange { get; set; }

        public string Symbol { get; set; }=string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName ="decimal(18, 10)")]
        public decimal Purchase{get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal? LastDiv{get; set; }
        public string? Industry{get; set; }  = string.Empty;
        public long? MarketCap{get; set; }
        public string? Logo{get; set; }
        public string? Website{get; set; }
        public List<Comment>? Comments{get; set;}= new List<Comment>();
        public List<Holding>? Holdings{get; set; }= new List<Holding>();
        public List<HistoricPrice>? HistoricPrices{get; set;}    

        public PriceChange? PriceChange{get; set; }=null;
        public DateTime LastUpdated{get; set; }
        public bool IsExpired=>LastUpdated.Age().TotalHours>6;
    }


    [Table("PriceChange")]
    public class PriceChange
    {
        public int PriceChangeId { get; set; }      
        public int StockId { get; set; }      
        public Stock Stock {get;set;} 

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("1D")]
        public double _1D { get; set; }

        [JsonProperty("5D")]
        public double _5D { get; set; }

        [JsonProperty("1M")]
        public double _1M { get; set; }

        [JsonProperty("3M")]
        public double _3M { get; set; }

        [JsonProperty("6M")]
        public double _6M { get; set; }
        public double ytd { get; set; }

        [JsonProperty("1Y")]
        public double _1Y { get; set; }

        [JsonProperty("3Y")]
        public double _3Y { get; set; }

        [JsonProperty("5Y")]
        public double _5Y { get; set; }

        [JsonProperty("10Y")]
        public double _10Y { get; set; }
        public double max { get; set; }
        public DateTime dateTimeStamp   { get; set; }
        
        public bool IsExpired=>dateTimeStamp.Age().TotalHours>6;
    }
}


namespace api.Dtos.Stock
{
    public class CoinPaprikaDto
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public decimal close { get; set; }
        public long market_cap{ get; set; }
    }
}
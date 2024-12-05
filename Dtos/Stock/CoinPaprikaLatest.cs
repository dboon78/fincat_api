using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CoinPaprikaLatest
    {
        public DateTime time_open { get; set; }
        public DateTime time_close { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public decimal close { get; set; }
        public long volume { get; set; }
        public long market_cap { get; set; }
    }
}
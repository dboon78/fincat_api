using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CoinApiHistory
    {
        public DateTime time_period_start { get; set; }
        public DateTime time_period_end { get; set; }
        public DateTime time_open { get; set; }
        public DateTime time_close { get; set; }
        public double price_open { get; set; }
        public double price_high { get; set; }
        public double price_low { get; set; }
        public double price_close { get; set; }
        public long volume_traded { get; set; }
        public int trades_count { get; set; }
    }
}
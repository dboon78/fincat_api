using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class TwelveData
    {
        public class TimeSeriesMeta
        {
            public string symbol { get; set; }
            public string interval { get; set; }
            public string currency { get; set; }
            public string exchange_timezone { get; set; }
            public string exchange { get; set; }
            public string mic_code { get; set; }
            public string type { get; set; }
        }
        public class TimeSeriesValue
        {
            public string datetime { get; set; }
            public string open { get; set; }
            public string high { get; set; }
            public string low { get; set; }
            public string close { get; set; }
            public string volume { get; set; }
        }

        public class TimeSeriesRoot
        {
            public TimeSeriesMeta meta { get; set; }
            public List<TimeSeriesValue> values { get; set;}
        }
    }
}
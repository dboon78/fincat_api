using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class FinnhubProfile{
        public string country { get; set; }
        public string currency { get; set; }
        public string estimateCurrency { get; set; }
        public string exchange { get; set; }
        public string finnhubIndustry { get; set; }
        public string ipo { get; set; }
        public string logo { get; set; }
        public long  marketCapitalization { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public double shareOutstanding { get; set; }
        public string ticker { get; set; }
        public string weburl { get; set; }
    }
    public class FinnhubQuote
    {
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double o { get; set; }
        public double pc { get; set; }
        public int t { get; set; }

        public static implicit operator FinnhubQuote(FinnhubProfile v)
        {
            throw new NotImplementedException();
        }
    }
    public class FinnhubSearch{
        public string description { get; set; }
        public string displaySymbol { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }
    }
    public class FinnhubSearchRoot{
        public int count { get; set; }
        public List<FinnhubSearch> result { get; set; }
    }
}
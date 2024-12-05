using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;

namespace api.Dtos.Stock
{
    public class FMPSearch{
        public string symbol { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public string stockExchange { get; set; }
        public string exchangeShortName { get; set; }
    }
    public class FMPStock
    {
        public string symbol { get; set; }
        public decimal price { get; set; }
        public double beta { get; set; }
        public int volAvg { get; set; }
        public long mktCap { get; set; }
        public double lastDiv { get; set; }
        public string range { get; set; }
        public double changes { get; set; }
        public string companyName { get; set; }
        public string currency { get; set; }
        public string cik { get; set; }
        public string isin { get; set; }
        public string cusip { get; set; }
        public string exchange { get; set; }
        public string exchangeShortName { get; set; }
        public string industry { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string ceo { get; set; }
        public string sector { get; set; }
        public string country { get; set; }
        public string fullTimeEmployees { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public double dcfDiff { get; set; }
        public double dcf { get; set; }
        public string image { get; set; }
        public string ipoDate { get; set; }
        public bool defaultImage { get; set; }
        public bool isEtf { get; set; }
        public bool isActivelyTrading { get; set; }
        public bool isAdr { get; set; }
        public bool isFund { get; set; }
    }

    public class FMPCrypto{
        public string symbol { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double changesPercentage { get; set; }
        public double change { get; set; }
        public double dayLow { get; set; }
        public double dayHigh { get; set; }
        public double yearHigh { get; set; }
        public double yearLow { get; set; }
        public double marketCap { get; set; }
        public double priceAvg50 { get; set; }
        public double priceAvg200 { get; set; }
        public string exchange { get; set; }
        public long volume { get; set; }
        public long avgVolume { get; set; }
        public double open { get; set; }
        public double previousClose { get; set; }
        public object eps { get; set; }
        public object pe { get; set; }
        public object earningsAnnouncement { get; set; }
        public double sharesOutstanding { get; set; }
        public long timestamp { get; set; }
    }

    public class FMPHistoricPrices{
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public int low { get; set; }
        public double close { get; set; }
        public double adjClose { get; set; }
        public long volume { get; set; }
        public long unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }

    }

    public class FMPExchangeHours{
        public string name { get; set; }
        public string openingHour { get; set; }
        public DateTime OpeningHour=>openingHour.UtcTime(timezone);
        public string closingHour { get; set; }
        public DateTime ClosingHour=>closingHour.UtcTime(timezone);
        public string timezone { get; set; }
        public bool isMarketOpen { get; set; }
    }
}
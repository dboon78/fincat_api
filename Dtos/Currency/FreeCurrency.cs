using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Currency
{
    public class FreeCurrency
    {
        public Dictionary<string,decimal> data=new Dictionary<string,decimal>();
    }

    public class CurrencyDetails{        
        public string symbol { get; set; }
        public string name { get; set; }
        public string symbol_native { get; set; }
        public int decimal_digits { get; set; }
        public int rounding { get; set; }
        public string code { get; set; }
        public string name_plural { get; set; }
    }
    public class CurrencyDetailsRoot{
        public Dictionary<string,CurrencyDetails> data = new Dictionary<string, CurrencyDetails>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Currency
{
    public class CurrencyDto
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Digits{ get; set; }
        public decimal Value { get; set; }

    }
}
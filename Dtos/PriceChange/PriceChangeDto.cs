using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.PriceChange
{
    public class PriceChangeDto
    {
        public double _1D { get; set; }
        public double _5D { get; set; }
        public double _1M { get; set; }
        public double _3M { get; set; }
        public double _6M { get; set; }
        public double ytd { get; set; }
        public double _1Y { get; set; }
        public double _3Y { get; set; }
        public double _5Y { get; set; }
        public double _10Y { get; set; }
        public double max { get; set; }
    }
}
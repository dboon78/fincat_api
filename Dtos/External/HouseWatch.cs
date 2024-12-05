using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.External
{
    public class HouseWatch
    {
            
        public int disclosure_year { get; set; }
        public string disclosure_date { get; set; }
        public string transaction_date { get; set; }
        public string owner { get; set; }
        public string ticker { get; set; }
        public string asset_description { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string representative { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string ptr_link { get; set; }
        public bool cap_gains_over_200_usd { get; set; }
        public string industry { get; set; }
        public string sector { get; set; }
        public string party { get; set; }
    
    }
}
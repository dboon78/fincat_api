using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("CoinPaprikaCoins")]
    public class CoinPaprikaCoin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int rank { get; set; }
        public bool is_new { get; set; }
        public bool is_active { get; set; }
        public string type { get; set; }
    }
}
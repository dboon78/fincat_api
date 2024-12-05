using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string? PortfolioName { get; set; }
        public float PorfolioValue{ get; set; } = 0;
        public List<Holding> Holdings { get; set; } = new List<Holding>();
    }
}
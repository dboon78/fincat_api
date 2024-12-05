using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Holding;

namespace api.Dtos.Portfolio
{
    public class PortfolioDto
    {
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public List<HoldingDto> Holdings { get; set; } = new List<HoldingDto>();
    }
}
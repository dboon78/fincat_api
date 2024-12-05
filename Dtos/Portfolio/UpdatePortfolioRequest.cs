using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Portfolio
{
    public class UpdatePortfolioRequest
    {
        [Required]
        public int PortfolioId { get; set; }
        [Required]        
        [MinLength(2,ErrorMessage ="Portfolio name must be 2 characters")]
        [MaxLength(32,ErrorMessage ="Portfolio name cannot be over 32 characters")]
        public string PortfolioName { get; set; }
    }
}
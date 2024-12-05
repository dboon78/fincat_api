using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Holding
{
    public class CreateHoldingRequest
    {
        [Required]
        [MinLength(1,ErrorMessage ="Symbol must be  1 character")]
        [MaxLength(20,ErrorMessage ="Symbol cannot be over 20 characters")]
        public string Symbol { get; set;}=string.Empty;
        
        [Required]
        [Range(0,int.MaxValue)]
        public int PortfolioId { get; set; }=0;

        public bool IsCrypto{get;set;}=false;
    }
}
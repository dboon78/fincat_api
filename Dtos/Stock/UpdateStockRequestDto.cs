using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        public int Id { get; set; }
         [Required]
        [MinLength(1,ErrorMessage ="Symbol must be 1 characters")]
        [MaxLength(10,ErrorMessage ="Symbol cannot be over 10 characters")]        
        public string Symbol { get; set; }=string.Empty;
        
        [Required]
        [MaxLength(100,ErrorMessage ="Company Name cannot be over 100 characters")]
        public string? CompanyName { get; set; } = string.Empty;
        
        [Required]
        [Range(1,10000000)]
        public decimal Purchase{get; set; }
        [Required]
        [Range(0.001,100)]
        public decimal? LastDiv{get; set; }
        [Required]
        [MaxLength(20,ErrorMessage ="Industry cannot be over 20 characters")]
        public string? Industry{get; set; }  = string.Empty;
        
        [Range(1,50000000000)]
        public long? MarketCap{get; set; }    
        public string? Exchange{get; set; } =string.Empty;
    }
}
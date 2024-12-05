using System.ComponentModel.DataAnnotations.Schema;
using api.Extensions;
namespace api.Models
{
    [Table("Currencies")]
    public class Currency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Digits{ get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdated  { get; set; }
        public bool IsExpired=>LastUpdated.Age().TotalHours>24;
        
    }
}
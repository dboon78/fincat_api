using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Currency;

namespace api.Dtos.Account
{
    public class NewUserDto
    {
        [Required]
        [StringValidator(MinLength = 1, MaxLength = 60)]
        public string UserName { get; set; }
        public string Email { get; set; }
        public CurrencyDto UserCurrency{ get; set; }
        [Required]
        public string Token { get; set; }
    }
}
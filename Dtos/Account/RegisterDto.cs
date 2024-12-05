using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [StringValidator(MinLength = 1, MaxLength = 60)]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringValidator(MinLength = 1, MaxLength = 60)]
        public string? Password { get; set; }
    }
}
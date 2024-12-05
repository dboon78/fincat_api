using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser:IdentityUser
    {
        public string? GoogleId { get; set; }
        public Currency? UserCurrency{ get; set; }
        [DefaultValue("USD")]
        public string? CurrencyCode { get; set; }="USD";
        public decimal PriceConversion(decimal price){
            return price*UserCurrency.Value;
        }
        public List<Portfolio> Portfolios{ get; set; }=new List<Portfolio>();
        public void GeneratePasswordHash()
        {
            // Generate a password that meets the complexity requirements
            var securePassword = GenerateComplexPassword(32);
            var _passwordHasher=new PasswordHasher<AppUser>();
            // Generate the password hash for the user
            PasswordHash= _passwordHasher.HashPassword(this, securePassword);
        }

        private string GenerateComplexPassword(int length = 12)
        {
            if (length < 12)
                throw new ArgumentException("Password length must be at least 12 characters.");

            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string nonAlphanumeric = "!@#$%^&*()_-+=<>?";

            StringBuilder passwordBuilder = new StringBuilder();
            Random random = new Random();

            // Ensure at least one uppercase letter, one lowercase, one digit, and one non-alphanumeric character
            for(int i=0; i<3; i++){
                passwordBuilder.Append(upperCase[random.Next(upperCase.Length)]);
                passwordBuilder.Append(lowerCase[random.Next(lowerCase.Length)]);
                passwordBuilder.Append(digits[random.Next(digits.Length)]);
                passwordBuilder.Append(nonAlphanumeric[random.Next(nonAlphanumeric.Length)]);
            }

            // Fill the remaining length with a mix of all allowed characters
            string allChars = upperCase + lowerCase + digits + nonAlphanumeric;
            for (int i = passwordBuilder.Length; i < length; i++)
            {
                passwordBuilder.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password to make it more random
            return ShuffleString(passwordBuilder.ToString());
        }
        private string ShuffleString(string input)
        {
            char[] array = input.ToCharArray();
            Random random = new Random();

            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap array[i] with array[j]
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return new string(array);
        }
    }
}
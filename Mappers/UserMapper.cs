using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Dtos.Currency;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static CurrencyDto ToCurrencyDto(this Currency currency){
            return new CurrencyDto(){
                Value=currency.Value,
                Name=currency.Name,
                Symbol=currency.Symbol,
                Digits=currency.Digits,
                Code=currency.Code,                
            };
        }
        public static NewUserDto ToUserDto(this AppUser user,string token){
            return new NewUserDto{
                UserName = user.UserName,
                Email=user.Email,
                Token=token,
                
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Currency;
using api.Models;

namespace api.Interfaces
{
    public interface IFreeCurrencyService
    {
        Task<FreeCurrency> GetFreeCurrencies();
        Task<CurrencyDetailsRoot> GetCurrencyDetails();
        Task<List<Currency>> GetCurrencyList();

        Task<Currency> GetCurrency(string code);
    }
}
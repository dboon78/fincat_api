using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class CurrencyExtensions
    {
        public static string Price(this double price){
            NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
            return price.ToString("C",nfi);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ITwelveDataService
    {
        Task<List<HistoricPrice>> UpdatedHistoricData(int stockId,string symbol,string period,string interval="1day");
        int PeriodToDays(string period);
    }
}
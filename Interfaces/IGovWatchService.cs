using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.External;

namespace api.Interfaces
{
    public interface IGovWatchService
    {
        List<SenateWatch> FetchSenateWatch();
        List<HouseWatch> FetchHouseWatch();
    }
}
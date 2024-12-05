using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.External;
using api.Interfaces;

namespace api.Service
{
    public class GovWatchService : IGovWatchService
    {
        public List<HouseWatch> FetchHouseWatch()
        {
            throw new NotImplementedException();
        }

        public List<SenateWatch> FetchSenateWatch()
        {
            throw new NotImplementedException();
        }
    }
}
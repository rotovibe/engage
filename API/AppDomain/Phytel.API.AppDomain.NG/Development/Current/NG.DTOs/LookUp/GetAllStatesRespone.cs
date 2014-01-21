using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllStatesRespone : IDomainResponse
    {
        public List<StatesLookUp> States { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }

    }

    public class StatesLookUp
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}

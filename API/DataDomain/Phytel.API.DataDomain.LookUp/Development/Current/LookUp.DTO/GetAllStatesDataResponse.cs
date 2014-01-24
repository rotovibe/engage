using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllStatesDataResponse : IDomainResponse
   {
       public List<StateData> States { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }

    public class StateData : LookUpData
    {
        public string Code { get; set; }
    }
}

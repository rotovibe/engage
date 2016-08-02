using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllStatesDataResponse : IDomainResponse
   {
       public List<StateData> States { get; set; }
       public double Version { get; set; }
       public ResponseStatus Status { get; set; }
    }

    public class StateData : IdNamePair
    {
        public string Code { get; set; }
    }
}

using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllCommModesDataResponse : IDomainResponse
   {
       public List<LookUpData> CommModes { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}

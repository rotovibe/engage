using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class ConditionsResponse
   {
       public List<Condition> Conditions { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }

}

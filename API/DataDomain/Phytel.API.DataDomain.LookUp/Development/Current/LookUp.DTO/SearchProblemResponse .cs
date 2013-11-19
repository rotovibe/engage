using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class SearchProblemResponse 
   {
       public List<Problem> Problems { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}

using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.Objective.DTO
{
       [Api(Description = "Response posted back when all objectives are requested from the API.")]
    public class GetAllObjectivesDataResponse : IDomainResponse
   {

      [ApiMember(DataType = "List<LookUpData>", Description = "List of Objectives.", IsRequired = true, Name = "Objectives", ParameterType = "body")]
       public List<IdNamePair> Objectives { get; set; }

       [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
       public ResponseStatus Status { get; set; }

       [ApiMember(DataType = "string", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
       public string Version { get; set; }
   }

}

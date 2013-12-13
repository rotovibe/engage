using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Api(Description = "Response posted back when an Objective by it's ID is requested from the API.")]
    public class GetObjectiveDataResponse : IDomainResponse
    {
        [ApiMember(DataType = "ObjectiveData", Description = "A single Objective object which has an ID and a Name.", IsRequired = true, Name = "Objective", ParameterType = "body")]
        public ObjectiveData Objective { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "string", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public string Version { get; set; }
    }

    public class ObjectiveData
    {
        
        public string ID { get; set; }
        public string Name { get; set; }
    }
}

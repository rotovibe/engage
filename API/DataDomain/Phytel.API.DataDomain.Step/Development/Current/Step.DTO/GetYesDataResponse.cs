using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Step.DTO
{
    [Api(Description = "Response posted back when an YesNo by it's ID is requested from the API.")]
    public class GetYesNoDataResponse : IDomainResponse
    {
        [ApiMember(DataType = "YesNoData", Description = "A single YesNo object which has an ID and a Name.", IsRequired = true, Name = "YesNo", ParameterType = "body")]
        public YesNoData YesNo { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "string", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public string Version { get; set; }
    }

    public class YesNoData
    {
        
        public string ID { get; set; }
        public string Name { get; set; }
    }
}

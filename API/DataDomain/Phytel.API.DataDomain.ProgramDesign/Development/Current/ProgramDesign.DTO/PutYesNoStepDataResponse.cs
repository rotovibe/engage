using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Api(Description = "Response posted back when a YesNo Step is added to the API.")]
    public class PutYesNoStepDataResponse : IDomainResponse
    {
        public string Id { get; set; }

        [ApiMember(DataType = "YesNoData", Description = "A single YesNo Step object.", IsRequired = true, Name = "YesNoStep", ParameterType = "body")]
        public YesNoData YesNoStep { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }
}
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Api(Description = "Response posted back when a Text Step is updated to the API.")]
    public class PutUpdateTextStepDataResponse : IDomainResponse
    {
        public string Id { get; set; }

        [ApiMember(DataType = "TextData", Description = "A single Text Step object.", IsRequired = true, Name = "YesNoStep", ParameterType = "body")]
        public TextData TextStep { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }
}
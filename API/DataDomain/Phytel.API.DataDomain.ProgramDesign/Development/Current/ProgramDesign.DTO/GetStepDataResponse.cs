using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Api(Description = "Response posted back when a Step by it's ID is requested from the API.")]
    public class GetStepDataResponse : IDomainResponse
    {
        [ApiMember(DataType = "StepData", Description = "A single Step object.", IsRequired = true, Name = "Step", ParameterType = "body")]
        public StepData Step { get; set; }

        [ApiMember(DataType = "ResponseStatus", Description = "HTTP(S) Response Status identifying the result of the request.  This will come in the form of standard HTTP(S) responses (200, 401, 500, etc...)", IsRequired = true, Name = "Status", ParameterType = "body")]
        public ResponseStatus Status { get; set; }

        [ApiMember(DataType = "double", Description = "The specific version of the Response object being returned to support backward compatibility", IsRequired = true, Name = "Version", ParameterType = "body")]
        public double Version { get; set; }
    }

    public class TextData : StepData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TextEntry { get; set; }
    }

    public class YesNoData : StepData
    {
        // YesNo step
        public string Question { get; set; }
        public string Notes { get; set; }
    }

    public class StepData
    {
        // Base step
        public string ID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TextEntry { get; set; }
        public string Question { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }

}

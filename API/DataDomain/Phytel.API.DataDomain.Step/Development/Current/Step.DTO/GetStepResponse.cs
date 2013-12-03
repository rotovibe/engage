using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Step.DTO
{
    public class GetStepResponse : IDomainResponse
    {
        public Step Step { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Step
    {
        public string StepID { get; set; }
        public string Version { get; set; }
    }
}

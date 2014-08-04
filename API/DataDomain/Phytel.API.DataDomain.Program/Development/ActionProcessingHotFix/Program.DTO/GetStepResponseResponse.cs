using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetStepResponseResponse : IDomainResponse
    {
        public StepResponse StepResponse { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

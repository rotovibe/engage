using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    public class StepListResponse
   {
        public List<StepResponse> Steps { get; set; }
        public string Version { get; set; }
    }

}

using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Step.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Steplist", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Steplist/Step/{StepNounID}", "GET")]
    public class StepListRequest : IDataDomainRequest
    {
        /*
         * Put custom fields here
        */

        [ApiMember(Name = "Context", Description = "Product Context requesting the Step", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}

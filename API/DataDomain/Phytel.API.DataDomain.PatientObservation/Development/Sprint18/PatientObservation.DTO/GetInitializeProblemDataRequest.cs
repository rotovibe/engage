using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observation/{ObservationId}/Problem/Initialize", "GET")]
    public class GetInitializeProblemDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the Patient for whom an observation is being created.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ObservationId", Description = "Id of the Observation.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ObservationId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Initial", Description = "Determines if this is an initialized observation or not.", ParameterType = "property", DataType = "double", IsRequired = true)]
        public string Initial { get; set; }
    }
}

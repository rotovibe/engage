using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/Interventions/", "GET")]
    public class GetPatientInterventionByTemplateIdRequest : IDataDomainRequest
    {
        [ApiMember(Name = "TemplateId", Description = "Template ID of the patientIntervention being requested", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TemplateId { get; set; }

        [ApiMember(Name = "PatientId", Description = "Id of the Patient for whom a goal is being created.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "GoalId", Description = "Id of the Goal.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string GoalId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}

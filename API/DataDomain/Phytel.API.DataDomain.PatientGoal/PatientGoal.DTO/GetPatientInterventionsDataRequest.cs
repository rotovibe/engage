using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Goal/Interventions", "POST")]
    public class GetPatientInterventionsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "AssignedToId", Description = "AssignedToId is the Id to which Intervention is assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedToId { get; set; }

        [ApiMember(Name = "CreatedById", Description = "CreatedById is the Id of the user who created the Intervention.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CreatedById { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId is the Id of the patient for whom an intervention is associated.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of Intervention Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }
        
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Intervention", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}

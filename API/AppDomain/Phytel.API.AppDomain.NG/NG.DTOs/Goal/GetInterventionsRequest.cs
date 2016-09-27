using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Goal/Interventions", "POST")]
    public class GetInterventionsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "AssignedToId", Description = "AssignedToId is the Id to which Intervention is assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedToId { get; set; }

        [ApiMember(Name = "CreatedById", Description = "CreatedById is the Id of the user who created the Intervention.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CreatedById { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId is the Id of the patient for whom a todo is associated.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of Intervention Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "InterventionFilterType", Description = "Indicates the filter for Interventions", ParameterType = "property", DataType = "InterventionFilterType", IsRequired = false)]
        [ApiAllowableValues("InterventionFilterType", typeof(DTO.InterventionFilterType))]
        public InterventionFilterType InterventionFilterType { get; set; }
        public GetInterventionsRequest() { }
    }

    public enum InterventionFilterType
    {
        AssignedToOthers = 1,
        ClosedByOthers = 2,
        MyClosedList = 3,
        MyOpenList = 4
    }
}

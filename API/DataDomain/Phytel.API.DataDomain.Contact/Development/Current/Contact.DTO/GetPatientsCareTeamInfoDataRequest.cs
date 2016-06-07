using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to get PatientCareTeamInfo given all patientIds.")]
    //Need a better route.
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/PatientCareTeamInfo", "POST")]
    public class GetPatientsCareTeamInfoDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientIds", Description = "PatientIds", ParameterType = "property", DataType = "List of strings", IsRequired = true)]
        public List<string> PatientIds { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "SQLUserId", Description = "SQLUserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string SQLUserId { get; set; }
    }
}

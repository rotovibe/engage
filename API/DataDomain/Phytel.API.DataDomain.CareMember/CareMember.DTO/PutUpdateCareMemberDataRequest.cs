using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/Update", "PUT")]
    public class PutUpdateCareMemberDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "CareMember", Description = "CareMember object to be updated", ParameterType = "property", DataType = "CareMemberData", IsRequired = true)]
        public CareMemberData CareMember { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}

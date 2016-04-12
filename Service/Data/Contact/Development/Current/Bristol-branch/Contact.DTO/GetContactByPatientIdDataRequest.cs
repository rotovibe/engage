using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to get all contact details for an individual from the API.")]
    [Route("/api/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Contact", "GET")]
    public class GetContactByPatientIdDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "ID of the Patient whose contact details are being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}

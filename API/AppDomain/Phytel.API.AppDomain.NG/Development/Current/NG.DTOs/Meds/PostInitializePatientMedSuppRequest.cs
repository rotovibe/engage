using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/PatientMedSupp/Initialize", "POST")]
    public class PostInitializePatientMedSuppRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient for whom a medication/supplement is being initialized.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "MedSuppId", Description = "Id of the medication/supplement which is getting associted to a patient.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string MedSuppId { get; set; }

        [ApiMember(Name = "CategoryId", Description = "Determines if it is a supplement or a medication", ParameterType = "property", DataType = "int", IsRequired = true)]
        public int CategoryId { get; set; }

        [ApiMember(Name = "TypeId", Description = "Determines if it is an OTC or a prescription", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TypeId { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        public PostInitializePatientMedSuppRequest() { }
    }
}

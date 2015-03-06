using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/PatientMedSupp/Save", "POST")]
    public class PostPatientMedSuppRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientMedSupp", Description = "PatientMedSupp details that need to be upserted.", ParameterType = "property", DataType = "PatientMedSupp", IsRequired = true)]
        public PatientMedSupp PatientMedSupp { get; set; }

        [ApiMember(Name = "Insert", Description = "Indicates if the action is to create or update a patient med/supp", ParameterType = "path", DataType = "boolean", IsRequired = true)]
        public bool Insert { get; set; }

        [ApiMember(Name = "RecalculateNDC", Description = "Indicates if NDC code should be recalculated in Edit mode.", ParameterType = "path", DataType = "boolean", IsRequired = true)]
        public bool RecalculateNDC { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public PostPatientMedSuppRequest() { }
    }
}
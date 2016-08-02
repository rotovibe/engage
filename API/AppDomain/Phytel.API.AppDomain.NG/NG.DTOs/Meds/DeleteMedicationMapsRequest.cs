using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/MedicationMap/{Ids}", "DELETE")]
    public class DeleteMedicationMapsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Ids", Description = "Comma limited MedicationMap Ids that need to be deleted.", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string Ids { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public DeleteMedicationMapsRequest() { }
    }
}
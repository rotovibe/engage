using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/MedicationMap", "GET")]
    public class GetMedicationMapsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Name", Description = "Full Name of the medication map that is being requested", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Form", Description = "Form of the medication map that is being requested", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Form { get; set; }

        [ApiMember(Name = "Route", Description = "Route of the medication map that is being requested", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Route { get; set; }

        [ApiMember(Name = "Strength", Description = "Strength of the medication map that is being requested", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Strength { get; set; }

        [ApiMember(Name = "Type", Description = "Type is custom or global. Type = 1 for Global, Type = 2 for Custom meds.", ParameterType = "Query", DataType = "int", IsRequired = false)]
        public int Type { get; set; }

        [ApiMember(Name = "Skip", Description = "The number of elements to skip before returning the remaining elements..", ParameterType = "Query", DataType = "int", IsRequired = false)]
        public int Skip { get; set; }

        [ApiMember(Name = "Take", Description = "Number of records to return per request.", ParameterType = "Query", DataType = "int", IsRequired = false)]
        public int Take { get; set; }
        
        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "Query", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        public GetMedicationMapsRequest() { }
    }
}

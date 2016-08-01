using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp", "GET")]
    public class GetPatientMedSuppsCountDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Name", Description = "Full Name of the medication.", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Form", Description = "Form of the medication.", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Form { get; set; }

        [ApiMember(Name = "Route", Description = "Route of the medication.", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Route { get; set; }

        [ApiMember(Name = "Strength", Description = "Strength of the medication.", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string Strength { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "Path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "Path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}

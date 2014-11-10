using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Medication/Search", "POST")]
    public class GetMedicationDetailsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Name", Description = "Name of the medication.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Route", Description = "Route of the medication.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Route { get; set; }

        [ApiMember(Name = "Form", Description = "Form of the medication.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Form { get; set; }

        [ApiMember(Name = "Strength", Description = "Strength of the medication.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Strength { get; set; }

        [ApiMember(Name = "Unit", Description = "Unit of the medication.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Unit { get; set; }

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

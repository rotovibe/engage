using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Initialize", "PUT")]
    public class PutInitializePatientMedSuppDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the Patient for whom a medication/supplement is being initialized.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "MedSuppId", Description = "Id of the medication/supplement which is getting associted to a patient.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string MedSuppId { get; set; }

        [ApiMember(Name = "SystemName", Description = "SystemName", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string SystemName { get; set; }

        [ApiMember(Name = "CategoryId", Description = "CategoryId", ParameterType = "property", DataType = "int", IsRequired = true)]
        public int CategoryId { get; set; }

        [ApiMember(Name = "TypeId", Description = "TypeId", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TypeId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}

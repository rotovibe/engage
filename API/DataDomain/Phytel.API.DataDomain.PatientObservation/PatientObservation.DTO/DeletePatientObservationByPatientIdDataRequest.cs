using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientObservation/Patient/{PatientId}/Delete", "DELETE")]
    public class DeletePatientObservationByPatientIdDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Id", Description = "PatientObservation Id", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "PatientId", Description = "Id of the patient whose PatientObservation record needs to be deleted", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

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
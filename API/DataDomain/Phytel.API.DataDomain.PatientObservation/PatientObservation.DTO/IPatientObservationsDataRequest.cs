using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public interface IPatientObservationsDataRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the Patient for whom current observations are requested.", ParameterType = "property", DataType = "string", IsRequired = true)]
        string PatientId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        string UserId { get; set; }

        [ApiMember(Name = "ObservationId", Description = "ObservationId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        string ObservationId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        double Version { get; set; }
    }
}
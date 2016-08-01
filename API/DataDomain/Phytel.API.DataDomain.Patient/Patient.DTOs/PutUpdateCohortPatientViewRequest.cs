using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patient/{PatientID}/CohortPatientView/Update", "PUT")]
    public class PutUpdateCohortPatientViewRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientID", Description = "ID of the patient being requested", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "CohortPatientView", Description = "CohortPatientView to update", ParameterType = "property", DataType = "string", IsRequired = true)]
        public CohortPatientViewData CohortPatientView { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context creating the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}

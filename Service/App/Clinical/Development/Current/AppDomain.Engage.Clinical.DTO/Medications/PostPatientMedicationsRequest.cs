using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Clinical.DTO.Medications
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patients/{PatientId}/Medications/", "POST")]
    public class PostPatientMedicationsRequest : IAppDomainRequest
    {
        public string PatientId { get; set; }
        public List<MedicationData> Medications { get; set; }
        public string Context { get; set; }
        public double Version { get; set; }
        public string ContractNumber { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}

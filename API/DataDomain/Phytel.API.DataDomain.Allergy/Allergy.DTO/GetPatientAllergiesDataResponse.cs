
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class GetPatientAllergiesDataResponse : DomainResponse
    {
        public List<PatientAllergyData> PatientAllergiesData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

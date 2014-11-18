using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PutPatientAllergiesDataResponse : IDomainResponse
    {
        public List<PatientAllergyData> PatientAllergiesData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

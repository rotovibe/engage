using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PutPatientAllergyDataResponse : IDomainResponse
    {
        public PatientAllergyData PatientAllergyData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}


using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetPatientMedSuppsDataResponse : DomainResponse
    {
        public List<PatientMedSuppData> PatientMedSuppsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}


using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetPatientMedFrequenciesDataResponse : DomainResponse
    {
        public List<PatientMedFrequencyData>  PatientMedFrequenciesData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

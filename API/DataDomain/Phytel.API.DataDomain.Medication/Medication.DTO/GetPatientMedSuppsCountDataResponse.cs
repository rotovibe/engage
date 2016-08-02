
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetPatientMedSuppsCountDataResponse : DomainResponse
    {
        public int PatientCount { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

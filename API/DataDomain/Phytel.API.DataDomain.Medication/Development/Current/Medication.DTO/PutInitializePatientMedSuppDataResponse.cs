using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PutInitializePatientMedSuppDataResponse : IDomainResponse
    {
        public PatientMedSuppData PatientMedSuppData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

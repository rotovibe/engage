using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PutMedicationDataResponse : IDomainResponse
    {
        public MedicationData MedicationData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

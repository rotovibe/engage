using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PutMedicationMapDataResponse : IDomainResponse
    {
        public MedicationMapData MedicationMappingData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

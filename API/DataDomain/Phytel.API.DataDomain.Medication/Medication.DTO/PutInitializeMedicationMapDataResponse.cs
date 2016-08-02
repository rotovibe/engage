using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PutInitializeMedicationMapDataResponse : IDomainResponse
    {
        public MedicationMapData MedicationMappingData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

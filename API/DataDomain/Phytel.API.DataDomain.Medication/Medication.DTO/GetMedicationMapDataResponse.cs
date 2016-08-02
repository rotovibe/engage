
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class GetMedicationMapDataResponse : DomainResponse
    {
        public List<MedicationMapData> MedicationMapsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}

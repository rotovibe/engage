using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationMappingDataManager
    {
        DTO.MedicationMapData InsertMedicationMapping(PutInsertMedicationMappingRequest request, DTO.MedicationMapData mm);
        MedicationMapData InitializeMedicationMap(PutInitializeMedicationMapDataRequest request);
        MedicationMapData UpdateMedicationMap(PutMedicationMapDataRequest request);
    }
}
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationMappingDataManager
    {
        DTO.MedicationMapData InsertMedicationMap(PostMedicationMapDataRequest request);
        MedicationMapData InitializeMedicationMap(PutInitializeMedicationMapDataRequest request);
        MedicationMapData UpdateMedicationMap(PutMedicationMapDataRequest request);
        List<MedicationMapData> GetMedicationMap(GetMedicationMapDataRequest request);
    }
}
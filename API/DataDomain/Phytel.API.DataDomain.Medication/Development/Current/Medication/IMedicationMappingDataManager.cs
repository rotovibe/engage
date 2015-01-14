using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationMappingDataManager
    {
        DTO.MedicationMappingData InsertMedicationMapping(PutInsertMedicationMappingRequest request, DTO.MedicationMappingData mm);
    }
}
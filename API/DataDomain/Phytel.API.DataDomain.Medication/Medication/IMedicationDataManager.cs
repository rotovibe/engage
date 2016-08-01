using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationDataManager
    {
        List<DTO.MedicationData> GetMedicationList(GetAllMedicationsRequest request);
        bool BulkInsertMedications(List<DTO.MedicationData> meds, PutBulkInsertMedicationsRequest request);
        GetMedicationNDCsDataResponse GetMedicationNDCs(GetMedicationNDCsDataRequest request);
    }
}
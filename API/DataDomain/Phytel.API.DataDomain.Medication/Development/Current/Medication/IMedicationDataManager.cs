using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationDataManager
    {
        List<DTO.MedicationData> GetMedicationList(string userid, string contract);
        bool BulkInsertMedications(List<DTO.MedicationData> meds, string userId, string contract);
    }
}
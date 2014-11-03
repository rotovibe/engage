using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public interface IMedicationDataManager
    {
        DTO.Medication GetMedicationByID(GetMedicationRequest request);
        List<DTO.Medication> GetMedicationList(GetAllMedicationsRequest request);
    }
}
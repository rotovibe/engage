using System.Collections.Generic;
using AppDomain.Engage.Clinical.DTO.Context;
using AppDomain.Engage.Clinical.DTO.Medications;

namespace AppDomain.Engage.Clinical
{
    public interface IClinicalManager
    {
        UserContext UserContext { get; set; }

        PostPatientMedicationsResponse SavePatientMedications(List<MedicationData> medsData);
    }
}

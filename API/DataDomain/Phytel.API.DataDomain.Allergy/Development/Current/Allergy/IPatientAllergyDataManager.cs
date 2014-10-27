using System.Collections.Generic;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy
{
    public interface IPatientAllergyDataManager
    {
        List<PatientAllergyData> GetPatientAllergyList(GetPatientAllergiesDataRequest request);
    }
}
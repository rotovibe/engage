using System.Collections.Generic;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy
{
    public interface IPatientAllergyDataManager
    {
        List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesDataRequest request);
        PatientAllergyData InitializePatientAllergy(PutInitializePatientAllergyDataRequest request);
        List<PatientAllergyData> UpdatePatientAllergies(PutPatientAllergiesDataRequest request);
        DeleteAllergiesByPatientIdDataResponse DeletePatientAllergies(DeleteAllergiesByPatientIdDataRequest request);
        UndoDeletePatientAllergiesDataResponse UndoDeletePatientAllergies(UndoDeletePatientAllergiesDataRequest request);
        DeletePatientAllergyDataResponse Delete(DeletePatientAllergyDataRequest request);
    }
}
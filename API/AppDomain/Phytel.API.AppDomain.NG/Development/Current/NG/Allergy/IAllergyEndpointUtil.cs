using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface IAllergyEndpointUtil
    {
        List<AllergyData> GetAllergies(GetAllergiesRequest request);
        AllergyData PutNewAllergy(PostInsertNewAllergyRequest request);
        List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesRequest request);
        PatientAllergyData InitializePatientAllergy(PostInitializePatientAllergyRequest request);
        List<PatientAllergyData> BulkUpdatePatientAllergies(PostPatientAllergiesRequest request);
        PatientAllergyData SingleUpdatePatientAllergy(PostPatientAllergyRequest request);
        AllergyData InitializeAllergy(PostInitializeAllergyRequest request);
        AllergyData UpdateAllergy(PostAllergyRequest request);
    }
}
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public interface IAllergyEndpointUtil
    {
        List<DdAllergy> GetAllergies(GetAllergiesRequest request);
        List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesRequest request);
        PatientAllergyData InitializePatientAllergy(GetInitializePatientAllergyRequest request);
        List<PatientAllergyData> UpdatePatientAllergies(PostPatientAllergiesRequest request);
    }
}
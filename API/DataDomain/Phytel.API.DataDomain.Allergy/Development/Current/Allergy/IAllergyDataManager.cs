using System.Collections.Generic;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy
{
    public interface IAllergyDataManager
    {
        List<AllergyData> GetAllergyList(GetAllAllergysRequest request);
        AllergyData PutNewAllergy(PostNewAllergyRequest request);
        AllergyData InitializeAllergy(PutInitializeAllergyDataRequest request);
    }
}
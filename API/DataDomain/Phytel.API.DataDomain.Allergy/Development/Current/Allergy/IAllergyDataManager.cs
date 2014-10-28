using System.Collections.Generic;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy
{
    public interface IAllergyDataManager
    {
        List<DdAllergy> GetAllergyList(GetAllAllergysRequest request);
        DdAllergy PutNewAllergy(PostNewAllergyRequest request);
    }
}
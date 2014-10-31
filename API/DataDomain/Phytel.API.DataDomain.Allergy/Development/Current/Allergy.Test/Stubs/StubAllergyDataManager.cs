using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Allergy.Test.Stubs
{
    public class StubAllergyDataManager : IAllergyDataManager
    {

        public List<DTO.AllergyData> GetAllergyList(DTO.GetAllAllergysRequest request)
        {
            return new List<DTO.AllergyData>();
        }

        public DTO.AllergyData PutNewAllergy(DTO.PostNewAllergyRequest request)
        {
            throw new NotImplementedException();
        }


        public DTO.AllergyData InitializeAllergy(DTO.PutInitializeAllergyDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

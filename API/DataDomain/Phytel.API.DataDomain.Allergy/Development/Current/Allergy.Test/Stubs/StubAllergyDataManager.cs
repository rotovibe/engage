using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Allergy.Test.Stubs
{
    public class StubAllergyDataManager : IAllergyDataManager
    {

        public List<DTO.DdAllergy> GetAllergyList(DTO.GetAllAllergysRequest request)
        {
            return new List<DTO.DdAllergy>();
        }

        public DTO.DdAllergy PutNewAllergy(DTO.PostNewAllergyRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

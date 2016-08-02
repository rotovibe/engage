using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Test
{
    public class StubAllergyDataManager : IAllergyDataManager
    {

        public List<DTO.AllergyData> GetAllergyList(DTO.GetAllAllergysRequest request)
        {
            List<AllergyData> result = null;
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);
            result = repo.SelectAll().Cast<AllergyData>().ToList<AllergyData>();
            return result;
        }

        public DTO.AllergyData InitializeAllergy(DTO.PutInitializeAllergyDataRequest request)
        {
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);
            return (AllergyData)repo.Initialize(request);
        }

        public DTO.AllergyData UpdateAllergy(DTO.PutAllergyDataRequest request)
        {
            AllergyData result = null;
            var repo = StubRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);

            if (request.AllergyData != null)
            {
                bool status = (bool)repo.Update(request);
                if (status)
                {
                    result = (AllergyData)repo.FindByID(request.AllergyData.Id);
                }
            }
            return result;
        }
    }
}

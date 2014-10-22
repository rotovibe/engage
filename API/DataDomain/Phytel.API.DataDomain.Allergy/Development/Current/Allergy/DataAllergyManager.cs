using System.Collections.Generic;
using System.Linq;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using System;

namespace Phytel.API.DataDomain.Allergy
{
    public class AllergyDataManager : IAllergyDataManager
    {
        protected readonly IMongoAllergyRepository AllergyRepository;

        public AllergyDataManager(IMongoAllergyRepository repository)
        {
            AllergyRepository = repository;
        }

        public List<DdAllergy> GetAllergyList(GetAllAllergysRequest request)
        {
            try
            {
                List<DdAllergy> result = null;
                AllergyRepository.UserId = request.UserId;
                result = AllergyRepository.SelectAll().Cast<DdAllergy>().ToList<DdAllergy>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:GetAllergyList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   

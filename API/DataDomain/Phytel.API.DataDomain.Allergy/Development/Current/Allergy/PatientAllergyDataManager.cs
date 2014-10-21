using System.Collections.Generic;
using System.Linq;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using System;

namespace Phytel.API.DataDomain.Allergy
{
    public class PatientAllergyDataManager : IPatientAllergyDataManager
    {
        protected readonly IMongoPatientAllergyRepository AllergyRepository;

        public PatientAllergyDataManager(IMongoPatientAllergyRepository repository)
        {
            AllergyRepository = repository;
        }

        public List<DTO.DdAllergy> GetPatientAllergyList(GetPatientAllergyRequest request)
        {
            try
            {
                List<DTO.DdAllergy> result = null;
                AllergyRepository.UserId = request.UserId;
                result = AllergyRepository.SelectAll().Cast<DTO.DdAllergy>().ToList<DTO.DdAllergy>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:GetAllergyList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   

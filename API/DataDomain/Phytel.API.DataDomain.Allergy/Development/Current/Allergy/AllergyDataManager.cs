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

        public AllergyData PutNewAllergy(PostNewAllergyRequest request)
        {
            try
            {
                AllergyRepository.UserId = request.UserId;
                var result =  AllergyRepository.Insert( new AllergyData{ Name = request.Description }) as AllergyData;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:PutNewAllergy()::" + ex.Message, ex.InnerException);
            }
        }

        public List<AllergyData> GetAllergyList(GetAllAllergysRequest request)
        {
            try
            {
                List<AllergyData> result = null;
                AllergyRepository.UserId = request.UserId;
                result = AllergyRepository.SelectAll().Cast<AllergyData>().ToList<AllergyData>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:GetAllergyList()::" + ex.Message, ex.InnerException);
            }
        }

        public AllergyData InitializeAllergy(PutInitializeAllergyDataRequest request)
        {
            try
            {
                AllergyRepository.UserId = request.UserId;
                return (AllergyData)AllergyRepository.Initialize(request);
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:InitializeAllergy()::" + ex.Message, ex.InnerException);
            }
        }


        public AllergyData UpdateAllergy(PutAllergyDataRequest request)
        {
            try
            {
                AllergyData result = null;
                AllergyRepository.UserId = request.UserId;
                if (request.AllergyData != null)
                {
                    bool status = (bool)AllergyRepository.Update(request);
                    if (status)
                    {
                        result = (AllergyData)AllergyRepository.FindByID(request.AllergyData.Id);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:UpdateAllergy()::" + ex.Message, ex.InnerException);
            }
        }

    }
}   

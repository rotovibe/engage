using System.Collections.Generic;
using System.Linq;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using System;

namespace Phytel.API.DataDomain.Allergy
{
    public class AllergyDataManager : IAllergyDataManager
    {
        //protected readonly IMongoAllergyRepository AllergyRepository;

        //public AllergyDataManager(IMongoAllergyRepository repository)
        //{
        //    AllergyRepository = repository;
        //}

        public List<AllergyData> GetAllergyList(GetAllAllergysRequest request)
        {
            try
            {
                List<AllergyData> result = null;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);
                result = repo.SelectAll().Cast<AllergyData>().ToList<AllergyData>();

                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public AllergyData InitializeAllergy(PutInitializeAllergyDataRequest request)
        {
            try
            {
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);
                return (AllergyData)repo.Initialize(request);
            }
            catch (Exception ex) { throw ex; }
        }


        public AllergyData UpdateAllergy(PutAllergyDataRequest request)
        {
            try
            {
                AllergyData result = null;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.Allergy);

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
            catch (Exception ex) { throw ex; }
        }

    }
}   

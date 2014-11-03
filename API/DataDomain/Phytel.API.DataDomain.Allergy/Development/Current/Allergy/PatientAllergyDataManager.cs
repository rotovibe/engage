using System.Collections.Generic;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using System;

namespace Phytel.API.DataDomain.Allergy
{
    public class PatientAllergyDataManager : IPatientAllergyDataManager
    {
        protected readonly IMongoPatientAllergyRepository PatientAllergyRepository;

        public PatientAllergyDataManager(IMongoPatientAllergyRepository repository)
        {
            PatientAllergyRepository = repository;
        }

        public List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesDataRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
                PatientAllergyRepository.UserId = request.UserId;
                if (request.PatientId != null)
                {
                    var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                    result = repo.FindByPatientId(request) as List<PatientAllergyData>;
                    //result = PatientAllergyRepository.FindByPatientId(request) as List<PatientAllergyData>;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:GetPatientAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientAllergyData InitializePatientAllergy(PutInitializePatientAllergyDataRequest request)
        {
            try
            {
                PatientAllergyRepository.UserId = request.UserId;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                return (PatientAllergyData)repo.Initialize(request);
                //return (PatientAllergyData)PatientAllergyRepository.Initialize(request);
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:InitializePatientAllergy()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientAllergyData UpdateSinglePatientAllergy(PutPatientAllergyDataRequest request)
        {
            try
            {
                PatientAllergyData result = null;
                //PatientAllergyRepository.UserId = request.UserId;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);

                if (request.PatientAllergyData != null)
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        result = (PatientAllergyData)repo.FindByID(request.PatientAllergyData.Id);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:UpdateSinglePatientAllergy()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientAllergyData> UpdateBulkPatientAllergies(PutPatientAllergiesDataRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
                //PatientAllergyRepository.UserId = request.UserId;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);

                if (request.PatientAllergiesData != null && request.PatientAllergiesData.Count > 0)
                {
                    result = new List<PatientAllergyData>();
                    request.PatientAllergiesData.ForEach( p =>
                    {
                        PutPatientAllergyDataRequest req = new PutPatientAllergyDataRequest 
                        { 
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientAllergyData = p,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        bool status = (bool)repo.Update(req);
                        if (status)
                        {
                            PatientAllergyData data = (PatientAllergyData)repo.FindByID(req.PatientAllergyData.Id);
                            result.Add(data);
                        }
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:UpdateBulkPatientAllergy()::" + ex.Message, ex.InnerException);
            }
        } 

    }
}   

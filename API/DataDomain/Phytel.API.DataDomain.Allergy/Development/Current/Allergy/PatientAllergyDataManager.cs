using System.Collections.Generic;
using System.Linq;
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
                    result = PatientAllergyRepository.FindByPatientId(request) as List<PatientAllergyData>;
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
                return (PatientAllergyData)PatientAllergyRepository.Initialize(request);
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
                PatientAllergyRepository.UserId = request.UserId;
                if (request.PatientAllergyData != null)
                {
                    bool status = (bool)PatientAllergyRepository.Update(request);
                    if (status)
                    {
                        result = (PatientAllergyData)PatientAllergyRepository.FindByID(request.PatientAllergyData.Id);
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
                PatientAllergyRepository.UserId = request.UserId;
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
                        bool status = (bool)PatientAllergyRepository.Update(req);
                        if (status)
                        {
                            PatientAllergyData data = (PatientAllergyData)PatientAllergyRepository.FindByID(req.PatientAllergyData.Id);
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

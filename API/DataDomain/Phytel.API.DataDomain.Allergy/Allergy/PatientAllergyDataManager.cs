using System.Collections.Generic;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;
using System;

namespace Phytel.API.DataDomain.Allergy
{
    public class PatientAllergyDataManager : IPatientAllergyDataManager
    {
        //protected readonly IMongoPatientAllergyRepository PatientAllergyRepository;

        //public PatientAllergyDataManager(IMongoPatientAllergyRepository repository)
        //{
        //    PatientAllergyRepository = repository;
        //}

        public List<PatientAllergyData> GetPatientAllergies(GetPatientAllergiesDataRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
                //PatientAllergyRepository.UserId = request.UserId;
                if (request.PatientId != null)
                {
                    var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                    result = repo.FindByPatientId(request) as List<PatientAllergyData>;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientAllergyData InitializePatientAllergy(PutInitializePatientAllergyDataRequest request)
        {
            try
            {
                //PatientAllergyRepository.UserId = request.UserId;
                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                return (PatientAllergyData)repo.Initialize(request);
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientAllergyData> UpdatePatientAllergies(PutPatientAllergiesDataRequest request)
        {
            try
            {
                List<PatientAllergyData> result = null;
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
            catch (Exception ex) { throw ex; }
        }

        #region Delete & UndoDelete
        public DeleteAllergiesByPatientIdDataResponse DeletePatientAllergies(DeleteAllergiesByPatientIdDataRequest request)
        {
            DeleteAllergiesByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteAllergiesByPatientIdDataResponse();

                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                GetPatientAllergiesDataRequest getAllPatientNotesDataRequest = new GetPatientAllergiesDataRequest
                {
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    PatientId = request.PatientId,
                    UserId = request.UserId,
                    Version = request.Version
                };
                List<PatientAllergyData> patientAllergies = repo.FindByPatientId(getAllPatientNotesDataRequest) as List<PatientAllergyData>;
                List<string> deletedIds = null;
                if (patientAllergies != null)
                {
                    deletedIds = new List<string>();
                    patientAllergies.ForEach(u =>
                    {
                        DeletePatientAllergyDataRequest deletePADataRequest = new DeletePatientAllergyDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            Id = u.Id,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        repo.Delete(deletePADataRequest);
                        deletedIds.Add(deletePADataRequest.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientAllergiesDataResponse UndoDeletePatientAllergies(UndoDeletePatientAllergiesDataRequest request)
        {
            UndoDeletePatientAllergiesDataResponse response = null;
            try
            {
                response = new UndoDeletePatientAllergiesDataResponse();

                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientAllergyId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public DeletePatientAllergyDataResponse Delete(DeletePatientAllergyDataRequest request)
        {
            DeletePatientAllergyDataResponse response = null;
            try
            {
                response = new DeletePatientAllergyDataResponse();

                var repo = AllergyRepositoryFactory.GetAllergyRepository(request, RepositoryType.PatientAllergy);
                if (!string.IsNullOrEmpty(request.Id))
                {
                    repo.Delete(request);
                }
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

    }
}   

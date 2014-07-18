using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem
{
    public static class PatientSystemDataManager
    {
        public static GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();

            IPatientSystemRepository<GetPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<GetPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            result.PatientSystem = repo.FindByID(request.PatientSystemID) as PatientSystemData;

            return result;
        }

        public static GetAllPatientSystemsDataResponse GetAllPatientSystems(GetAllPatientSystemsDataRequest request)
        {
            GetAllPatientSystemsDataResponse result = new GetAllPatientSystemsDataResponse();

            IPatientSystemRepository<GetAllPatientSystemsDataResponse> repo = PatientSystemRepositoryFactory<GetAllPatientSystemsDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
            
            return result;
        }

        public static PutPatientSystemDataResponse PutPatientSystem(PutPatientSystemDataRequest request)
        {
            IPatientSystemRepository<PutPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<PutPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);

            PutPatientSystemDataResponse result = repo.Insert(request) as PutPatientSystemDataResponse;
            return result;
        }

        public static DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request)
        {
            DeletePatientSystemByPatientIdDataResponse response = null;
            try
            {
                response = new DeletePatientSystemByPatientIdDataResponse();

                IPatientSystemRepository<PutPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<PutPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
                List<PatientSystemData> patientSystems = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
                List<string> deletedIds = null;
                if (patientSystems != null)
                {
                    deletedIds = new List<string>();
                    patientSystems.ForEach(u =>
                    {
                        request.Id = u.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public static UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(UndoDeletePatientSystemsDataRequest request)
        {
            UndoDeletePatientSystemsDataResponse response = null;
            try
            {
                response = new UndoDeletePatientSystemsDataResponse();

                IPatientSystemRepository<PutPatientSystemDataResponse> repo = PatientSystemRepositoryFactory<PutPatientSystemDataResponse>.GetPatientSystemRepository(request.ContractNumber, request.Context, request.UserId);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientSystemId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   

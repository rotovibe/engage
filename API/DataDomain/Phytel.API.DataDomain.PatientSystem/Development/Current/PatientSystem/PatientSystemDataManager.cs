using Phytel.API.DataDomain.PatientSystem.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemDataManager : IPatientSystemDataManager
    {
        IPatientSystemRepositoryFactory Factory { get; set; }

        public PatientSystemDataManager(IPatientSystemRepositoryFactory repo)
        {
            Factory = repo;
        }
        
        public GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                result.PatientSystem = repo.FindByID(request.PatientSystemID) as PatientSystemData;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public GetPatientSystemsDataResponse GetPatientSystems(GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse result = new GetPatientSystemsDataResponse();
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                result.PatientSystems = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PutPatientSystemDataResponse InsertPatientSystem(PutPatientSystemDataRequest request)
        {
            try
            {
                PutPatientSystemDataResponse result = new PutPatientSystemDataResponse();
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                result.PatientSystemId = repo.Insert(request) as string;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public PutUpdatePatientSystemDataResponse UpdatePatientSystem(PutUpdatePatientSystemDataRequest request)
        {
            PutUpdatePatientSystemDataResponse result = new PutUpdatePatientSystemDataResponse();
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                result.Success = (bool)repo.Update(request);
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request)
        {
            DeletePatientSystemByPatientIdDataResponse response = null;
            try
            {
                response = new DeletePatientSystemByPatientIdDataResponse();
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
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

        public UndoDeletePatientSystemsDataResponse UndoDeletePatientSystems(UndoDeletePatientSystemsDataRequest request)
        {
            UndoDeletePatientSystemsDataResponse response = null;
            try
            {
                response = new UndoDeletePatientSystemsDataResponse();
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
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

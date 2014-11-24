using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientSystem;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemDataManager : IPatientSystemDataManager
    {
        public IPatientSystemRepositoryFactory Factory { get; set; }
        
        public GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse result = new GetPatientSystemDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            result.PatientSystem = repo.FindByID(request.PatientSystemID) as PatientSystemData;

            return result;
        }

        public GetPatientSystemsDataResponse GetPatientSystems(GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse result = new GetPatientSystemsDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
            result.PatientSystems = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
            return result;
        }

        public PutPatientSystemDataResponse InsertPatientSystem(PutPatientSystemDataRequest request)
        {
            PutPatientSystemDataResponse result = new PutPatientSystemDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);

            result.PatientSystemId = repo.Insert(request) as string;
            return result;
        }

        public PutUpdatePatientSystemDataResponse UpdatePatientSystem(PutUpdatePatientSystemDataRequest request)
        {
            PutUpdatePatientSystemDataResponse result = new PutUpdatePatientSystemDataResponse();
            IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);

            result.Success = (bool)repo.Update(request);
            return result;
        }

        public DeletePatientSystemByPatientIdDataResponse DeletePatientSystemByPatientId(DeletePatientSystemByPatientIdDataRequest request)
        {
            DeletePatientSystemByPatientIdDataResponse response = null;
            try
            {
                response = new DeletePatientSystemByPatientIdDataResponse();
                IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
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
                IPatientSystemRepository repo = Factory.GetRepository(request, RepositoryType.PatientSystem);
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

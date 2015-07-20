using Phytel.API.DataDomain.PatientSystem.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ServiceStack.Common.Extensions;

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
                result.PatientSystemData = repo.FindByID(request.PatientSystemID) as PatientSystemData;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemData> GetPatientSystems(GetPatientSystemsDataRequest request)
        {
            List<PatientSystemData> dataList = null;
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                dataList = repo.FindByPatientId(request.PatientId) as List<PatientSystemData>;
                return dataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemOldData> GetAllPatientSystems()
        {
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                var dataList = repo.SelectAll().ToList<PatientSystemOldData>();
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertPatientSystem(InsertPatientSystemDataRequest request)
        {
            string id = null;
            try
            {
                if (request.PatientSystemsData != null)
                {
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    if (request.IsEngageSystem)
                    {
                        request.PatientSystemsData.SystemId = Constants.EngageSystemId;
                        request.PatientSystemsData.Value = EngageId.New();
                        request.PatientSystemsData.Primary = isSystemPrimary(Constants.EngageSystemId);
                        request.PatientSystemsData.StatusId = (int)Status.Active;
                        request.PatientSystemsData.SystemSource = Constants.SystemSource;
                    }
                    id = (string)repo.Insert(request);
                }
                return id;
            }
            catch (Exception ex) { throw ex; }
        }

        private bool isSystemPrimary(string id)
        {
            bool result = false;
            var repo = Factory.GetRepository(RepositoryType.System);
            SystemData data = repo.FindByID(id) as SystemData;
            if (data != null)
                result = data.Primary;
            return result;
        }

        public List<PatientSystemData> InsertPatientSystems(InsertPatientSystemsDataRequest request)
        {
            List<PatientSystemData> dataList = null;
            try
            {
                if (request.PatientSystemsData != null && request.PatientSystemsData.Count > 0)
                {
                    dataList = new List<PatientSystemData>();
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    request.PatientSystemsData.ForEach(p =>
                    {
                        if (string.Equals(Constants.EngageSystemId, p.SystemId, StringComparison.InvariantCultureIgnoreCase))
                        {
                            p.Value = EngageId.New();
                            p.Primary = isSystemPrimary(Constants.EngageSystemId);
                            p.StatusId = (int)Status.Active;
                            p.SystemSource = Constants.SystemSource;
                        }
                        InsertPatientSystemDataRequest insertReq = new InsertPatientSystemDataRequest
                        {
                            PatientId = p.PatientId,
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientSystemsData = p,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        string id = (string)repo.Insert(insertReq);
                        if (!string.IsNullOrEmpty(id))
                        {
                            PatientSystemData result = (PatientSystemData)repo.FindByID(id);
                            if (result != null)
                                dataList.Add(result);
                        }
                    });
                }
                return dataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<PatientSystemData> UpdatePatientSystems(UpdatePatientSystemsDataRequest request)
        {
            List<PatientSystemData> dataList  = null;
            try
            {
                if (request.PatientSystemsData != null && request.PatientSystemsData.Count > 0)
                {
                    dataList = new List<PatientSystemData>();
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    request.PatientSystemsData.ForEach(p =>
                        {
                            UpdatePatientSystemDataRequest updateReq = new UpdatePatientSystemDataRequest
                            {
                                    Id  = p.Id,
                                    PatientId = p.PatientId,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    PatientSystemsData = p,
                                    UserId = request.UserId,
                                    Version = request.Version
                            };
                            bool success = (bool)repo.Update(updateReq);
                            if (success)
                            {
                                PatientSystemData result = (PatientSystemData)repo.FindByID(p.Id);
                                if (result != null)
                                    dataList.Add(result);
                            }   
                        });
                }
                return dataList;
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

        public void DeletePatientSystems(DeletePatientSystemsDataRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Ids))
                { 
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    string[] Ids = request.Ids.Split(',');
                    foreach (string id in Ids)
                    {
                        DeletePatientSystemByPatientIdDataRequest deleteReq = new DeletePatientSystemByPatientIdDataRequest 
                        { 
                            Id = id.Trim(),
                            PatientId = request.PatientId,
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version 
                        };
                        repo.Delete(deleteReq);
                    }
                }
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

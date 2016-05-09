using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Phytel.API.Common;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataAudit;


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
                result.PatientSystemData = repo.FindByID(request.Id) as PatientSystemData;
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
            List<PatientSystemOldData> dataList = null;
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                dataList = repo.SelectAll() as List<PatientSystemOldData>;
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PatientSystemData> GetPatientSystemsByIds(GetPatientSystemByIdsDataRequest request)
        {
            try
            {
                var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                return repo.Select(request.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PatientSystemData InsertPatientSystem(InsertPatientSystemDataRequest request)
        {
            PatientSystemData result = null;
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
                        request.PatientSystemsData.SystemName = Constants.SystemName;
                        request.PatientSystemsData.StatusId = (int)Status.Active;
                        request.PatientSystemsData.DataSource = Constants.DataSource;
                    }
                    string id = (string)repo.Insert(request);
                    if (!string.IsNullOrEmpty(id))
                    {
                        result = (PatientSystemData)repo.FindByID(id);
                    }
                }
                return result;
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
                        InsertPatientSystemDataRequest insertReq = new InsertPatientSystemDataRequest
                        {
                            PatientId = request.PatientId,
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

        public BulkInsertResult InsertBatchEngagePatientSystems(InsertBatchEngagePatientSystemsDataRequest request)
        {
            BulkInsertResult result = null;
            try
            {
                if (request.PatientIds != null && request.PatientIds.Count > 0)
                {
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    List<PatientSystemData> psList = new List<PatientSystemData>();
                    bool status = isSystemPrimary(Constants.EngageSystemId);
                    List<string> patientIds = request.PatientIds;
                    patientIds.ForEach(p =>
                    {
                        PatientSystemData ps = new PatientSystemData
                        {
                            DataSource = Constants.DataSource,
                            PatientId = p,
                            Primary = status,
                            StatusId = (int)Status.Active,
                            SystemId = Constants.EngageSystemId,
                            SystemName = Constants.SystemName,
                            Value = EngageId.New()
                        };
                        psList.Add(ps);
                    });
                    result = (BulkInsertResult)repo.InsertAll(psList.Cast<object>().ToList());
                }
            }
            catch (Exception ex) { throw ex; }
            return result;
        }

        public List<HttpObjectResponse<PatientSystemData>> InsertBatchPatientSystems(InsertBatchPatientSystemsDataRequest request)
        {
            List<HttpObjectResponse<PatientSystemData>> list = null;
            try
            { 
                if (request.PatientSystemsData != null && request.PatientSystemsData.Count > 0)
                {
                    list = new List<HttpObjectResponse<PatientSystemData>>();
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    BulkInsertResult result = (BulkInsertResult)repo.InsertAll(request.PatientSystemsData.Cast<object>().ToList());
                    if (result != null)
                    {
                        if (result.ProcessedIds != null && result.ProcessedIds.Count > 0)
                        {
                            // Get the PatientSystems that were newly inserted. 
                            List<PatientSystemData> insertedPatientSystems = repo.Select(result.ProcessedIds);
                            if (insertedPatientSystems != null && insertedPatientSystems.Count > 0)
                            {
                                #region DataAudit
                                List<string> insertedPatientSystemIds = insertedPatientSystems.Select(p => p.Id).ToList();
                                AuditHelper.LogDataAudit(request.UserId, MongoCollectionName.PatientSystem.ToString(), insertedPatientSystemIds, Common.DataAuditType.Insert, request.ContractNumber);
                                #endregion

                                insertedPatientSystems.ForEach(r =>
                                {
                                    list.Add(new HttpObjectResponse<PatientSystemData> { Code = HttpStatusCode.Created, Body = (PatientSystemData)new PatientSystemData { Id = r.Id, ExternalRecordId = r.ExternalRecordId, PatientId = r.PatientId } });
                                });
                            }
                        }
                        result.ErrorMessages.ForEach(e =>
                        {
                            list.Add(new HttpObjectResponse<PatientSystemData> { Code = HttpStatusCode.InternalServerError, Message = e });
                        });
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return list;
        }

        public bool UpdatePatientSystem(UpdatePatientSystemDataRequest request)
        {
            bool success = false;
            try
            {
                if (request.PatientSystemsData != null)
                {
                    var repo = Factory.GetRepository(RepositoryType.PatientSystem);
                    UpdatePatientSystemDataRequest updateReq = new UpdatePatientSystemDataRequest
                    {
                        Id = request.PatientSystemsData.Id,
                        PatientId = request.PatientSystemsData.PatientId,
                        Context = request.Context,
                        ContractNumber = request.ContractNumber,
                        PatientSystemsData = request.PatientSystemsData,
                        UserId = request.UserId,
                        Version = request.Version
                    };
                    success = (bool)repo.Update(updateReq);
                }
                return success;
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

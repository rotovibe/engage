using Phytel.API.DataDomain.PatientGoal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.PatientGoal
{
    public static class PatientGoalDataManager
    {
        public static PutInitializeGoalDataResponse InitializeGoal(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = null;
            try
            {
                response = new PutInitializeGoalDataResponse();
                IPatientGoalRepository<PutInitializeGoalDataResponse> repo = PatientGoalRepositoryFactory<PutInitializeGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);

                response.Goal = (PatientGoalData)repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static PutInitializeBarrierDataResponse InitializeBarrier(PutInitializeBarrierDataRequest request)
        {
            PutInitializeBarrierDataResponse response = null;
            try
            {
                response = new PutInitializeBarrierDataResponse();
                IPatientGoalRepository<PutInitializeBarrierDataResponse> repo = PatientGoalRepositoryFactory<PutInitializeBarrierDataResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context, request.UserId);

                response.Id = (string)repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static GetPatientGoalDataResponse GetPatientGoal(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse result = null;
            try
            {
                result = new GetPatientGoalDataResponse();
                IPatientGoalRepository<PatientGoalData> goalRepo = PatientGoalRepositoryFactory<PatientGoalData>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);
                
                PatientGoalData patientGoalData = goalRepo.FindByID(request.Id) as PatientGoalData;
                if (patientGoalData != null)
                {
                    //Get all barriers for a given goal
                    patientGoalData.BarriersData = getBarriersByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id, request.UserId);

                    //Get all tasks for a given goal
                    patientGoalData.TasksData = getTasksByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id, request.UserId);
                    
                    //Get all interventions for a given goal
                    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id, request.UserId);
                }

                result.GoalData = patientGoalData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllPatientGoalsDataResponse GetPatientGoalList(GetAllPatientGoalsDataRequest request)
        {
            GetAllPatientGoalsDataResponse result = null;
            try
            {
                result = new GetAllPatientGoalsDataResponse();
                IPatientGoalRepository<PatientGoalViewData> goalRepo = PatientGoalRepositoryFactory<PatientGoalViewData>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);

                List<PatientGoalViewData> goalViewDataList = goalRepo.Find(request.PatientId) as List<PatientGoalViewData>;
                List<PatientGoalViewData> goalDataView = null;
                if (goalViewDataList != null && goalViewDataList.Count > 0)
                {
                    goalDataView = new List<PatientGoalViewData>();
                    foreach(PatientGoalViewData p in goalViewDataList)
                    {
                        string contractNumber = request.ContractNumber;
                        string context  = request.Context;
                        
                        PatientGoalViewData view = new PatientGoalViewData();
                        view = p;

                        //Barriers
                        List<ChildViewData> barrierChildView = null;
                        List<PatientBarrierData> barrierData = getBarriersByPatientGoalId(contractNumber, context, p.Id, request.UserId);
                        if(barrierData != null && barrierData.Count > 0)
                        {   
                            barrierChildView = new List<ChildViewData>();
                            foreach(PatientBarrierData b in barrierData)
                            {
                                barrierChildView.Add(new ChildViewData { Id = b.Id, PatientGoalId = b.PatientGoalId, Name = b.Name, StatusId = ((int)(b.StatusId))});
                                barrierChildView = barrierChildView.OrderBy(o => o.Name).ToList();
                            }
                        }
                        view.BarriersData =  barrierChildView; 

                        //Tasks
                        List<ChildViewData> taskChildView = null;
                        List<PatientTaskData> taskData = getTasksByPatientGoalId(contractNumber, context, p.Id, request.UserId);
                        if(taskData != null && taskData.Count > 0)
                        {   
                            taskChildView = new List<ChildViewData>();
                            foreach(PatientTaskData b in taskData)
                            {
                                taskChildView.Add(new ChildViewData { Id = b.Id, PatientGoalId = b.PatientGoalId, Description = b.Description, StatusId = ((int)(b.StatusId)) });
                                taskChildView = taskChildView.OrderBy(o => o.Description).ToList();
                            }
                        }
                        view.TasksData = taskChildView;

                        //Interventions
                        List<ChildViewData> interChildView = null;
                        List<PatientInterventionData> interData = getInterventionsByPatientGoalId(contractNumber, context, p.Id, request.UserId);
                        if (interData != null && interData.Count > 0)
                        {   
                            interChildView = new List<ChildViewData>();
                            foreach (PatientInterventionData b in interData)
                            {
                                interChildView.Add(new ChildViewData { Id = b.Id, PatientGoalId = b.PatientGoalId, Description = b.Description, StatusId = ((int)(b.StatusId)) });
                                interChildView.OrderBy(o => o.Description).ToList();
                            }
                        }
                        view.InterventionsData = interChildView;
                        goalDataView.Add(view);
                    }
                }

                result.PatientGoalsData = goalDataView;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        #region Private methods
        private static List<PatientBarrierData> getBarriersByPatientGoalId(string contractNumber, string context, string patientGoalId, string userId)
        {
            IPatientGoalRepository<PatientBarrierData> barrierRepo = PatientGoalRepositoryFactory<PatientBarrierData>.GetPatientBarrierRepository(contractNumber, context, userId);
            
            List<PatientBarrierData> barrierDataList = barrierRepo.Find(patientGoalId) as List<PatientBarrierData>; 
            return barrierDataList;
        }

        private static List<PatientTaskData> getTasksByPatientGoalId(string contractNumber, string context, string patientGoalId, string userId)
        {
            IPatientGoalRepository<PatientTaskData> taskRepo = PatientGoalRepositoryFactory<PatientTaskData>.GetPatientTaskRepository(contractNumber, context, userId);
            
            List<PatientTaskData> taskDataList = taskRepo.Find(patientGoalId) as List<PatientTaskData>;
            return taskDataList;
        }

        private static List<PatientInterventionData> getInterventionsByPatientGoalId(string contractNumber, string context, string patientGoalId, string userId)
        {
            IPatientGoalRepository<PatientInterventionData> interventionRepo = PatientGoalRepositoryFactory<PatientInterventionData>.GetPatientInterventionRepository(contractNumber, context, userId);
            
            List<PatientInterventionData> interventionDataList = interventionRepo.Find(patientGoalId) as List<PatientInterventionData>;
            return interventionDataList;
        }

        #endregion

        #region // TASKS
        public static PutPatientGoalDataResponse PutPatientGoal(PutPatientGoalDataRequest request)
        {
            try
            {
                PutPatientGoalDataResponse result = new PutPatientGoalDataResponse();

                IPatientGoalRepository<PutPatientGoalDataResponse> repo = PatientGoalRepositoryFactory<PutPatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);

                bool status = (bool)repo.Update(request);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request)
        {
            try
            {
                PutInitializeTaskResponse result = new PutInitializeTaskResponse();

                IPatientGoalRepository<PutInitializeTaskResponse> repo = PatientGoalRepositoryFactory<PutInitializeTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context, request.UserId);
                
                result.Task = (PatientTaskData)repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutUpdateTaskResponse UpdatePatientTask(PutUpdateTaskRequest request)
        {
            try
            {
                PutUpdateTaskResponse result = new PutUpdateTaskResponse();

                IPatientGoalRepository<PutUpdateTaskResponse> repo = PatientGoalRepositoryFactory<PutUpdateTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientTaskData> ptd = (List<PatientTaskData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> dbTaskIdList = GetTaskIds(ptd);

                // update existing task entries with a delete
                List<string> excludes = dbTaskIdList.Except(request.TaskIdsList).ToList<string>();
                excludes.ForEach(ex =>
                {
                    // create delete task request to insert
                    DeleteTaskDataRequest dtr = new DeleteTaskDataRequest { TaskId = ex, UserId = request.UserId };
                    repo.Delete(dtr);
                });

                bool status = false;
                if (request.TaskIdsList.Count > 0)
                    status = (bool)repo.Update(request);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutUpdateInterventionResponse UpdatePatientIntervention(PutUpdateInterventionRequest request)
        {
            try
            {
                PutUpdateInterventionResponse result = new PutUpdateInterventionResponse();

                IPatientGoalRepository<PutUpdateInterventionResponse> repo = PatientGoalRepositoryFactory<PutUpdateInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientInterventionData> pid = (List<PatientInterventionData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> dbTaskIdList = GetInterventionIds(pid);

                // update existing intervention entries with a delete
                List<string> excludes = dbTaskIdList.Except(request.InterventionIdsList).ToList<string>();
                excludes.ForEach(ex =>
                {
                    // create delete intervention request to insert
                    DeleteInterventionDataRequest dtr = new DeleteInterventionDataRequest { InterventionId = ex, UserId = request.UserId };
                    repo.Delete(dtr);
                });

                bool status = false;
                if (request.InterventionIdsList.Count > 0)
                    status = (bool)repo.Update(request);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutUpdateBarrierResponse UpdatePatientBarrier(PutUpdateBarrierRequest request)
        {
            try
            {
                PutUpdateBarrierResponse result = new PutUpdateBarrierResponse();

                IPatientGoalRepository<PutUpdateBarrierResponse> repo = PatientGoalRepositoryFactory<PutUpdateBarrierResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientBarrierData> pid = (List<PatientBarrierData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> dbBarrierIdList = GetBarrierIds(pid);

                // update existing barrier entries with a delete
                List<string> excludes = dbBarrierIdList.Except(request.BarrierIdsList).ToList<string>();
                excludes.ForEach(ex =>
                {
                    // create delete barrier request to insert
                    DeleteBarrierDataRequest dbr = new DeleteBarrierDataRequest { BarrierId = ex, UserId = request.UserId };
                    repo.Delete(dbr);
                });

                bool status = false;
                if (request.BarrierIdsList.Count > 0)
                    status = (bool)repo.Update(request);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<string> GetInterventionIds(List<PatientInterventionData> ptd)
        {
            List<string> list = new List<string>();
            try
            {
                if (ptd != null && ptd.Count > 0)
                {
                    ptd.ForEach(t =>
                    {
                        list.Add(t.Id);
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<string> GetBarrierIds(List<PatientBarrierData> ptd)
        {
            List<string> list = new List<string>();
            try
            {
                if (ptd != null && ptd.Count > 0)
                {
                    ptd.ForEach(t =>
                    {
                        list.Add(t.Id);
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<string> GetTaskIds(List<PatientTaskData> ptd)
        {
            List<string> list = new List<string>();
            try
            {
                if (ptd != null && ptd.Count > 0)
                {
                    ptd.ForEach(t =>
                    {
                        list.Add(t.Id);
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        public static PutInitializeInterventionResponse InsertNewPatientIntervention(PutInitializeInterventionRequest request)
        {
            try
            {
                PutInitializeInterventionResponse result = new PutInitializeInterventionResponse();

                IPatientGoalRepository<PutInitializeInterventionResponse> repo = PatientGoalRepositoryFactory<PutInitializeInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context, request.UserId);
                
                result.Id = (string)repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Delete
        public static DeletePatientGoalDataResponse DeletePatientGoal(DeletePatientGoalDataRequest request)
        {
            try
            {
                DeletePatientGoalDataResponse result = new DeletePatientGoalDataResponse();

                IPatientGoalRepository<DeletePatientGoalDataResponse> repo = PatientGoalRepositoryFactory<DeletePatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);
                
                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteTaskDataResponse DeleteTask(DeleteTaskDataRequest request)
        {
            try
            {
                DeleteTaskDataResponse result = new DeleteTaskDataResponse();

                IPatientGoalRepository<DeleteTaskDataResponse> repo = PatientGoalRepositoryFactory<DeleteTaskDataResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientTaskData> ptd = (List<PatientTaskData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> deletedIds = null;
                if (ptd != null && ptd.Count > 0)
                {
                    deletedIds = new List<string>();
                    ptd.ForEach(t =>
                    {
                        request.TaskId = t.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.TaskId);
                    });
                }
                result.DeletedIds = deletedIds;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteInterventionDataResponse DeleteIntervention(DeleteInterventionDataRequest request)
        {
            try
            {
                DeleteInterventionDataResponse result = new DeleteInterventionDataResponse();

                IPatientGoalRepository<DeleteInterventionDataResponse> repo = PatientGoalRepositoryFactory<DeleteInterventionDataResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientInterventionData> pid = (List<PatientInterventionData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> deletedIds = null;
                if (pid != null && pid.Count > 0)
                {
                    deletedIds = new List<string>();
                    pid.ForEach(i =>
                    {
                        request.InterventionId = i.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.InterventionId);
                    }); 
                }
                result.DeletedIds = deletedIds;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteBarrierDataResponse DeleteBarrier(DeleteBarrierDataRequest request)
        {
            try
            {
                DeleteBarrierDataResponse result = new DeleteBarrierDataResponse();

                IPatientGoalRepository<DeleteBarrierDataResponse> repo = PatientGoalRepositoryFactory<DeleteBarrierDataResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<PatientBarrierData> pbd = (List<PatientBarrierData>)repo.FindByGoalId(request.PatientGoalId);
                List<string> deletedIds = null;
                if (pbd != null && pbd.Count > 0)
                {
                    deletedIds = new List<string>();
                    pbd.ForEach(b =>
                    {
                        request.BarrierId = b.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.BarrierId);
                    });
                }
                result.DeletedIds = deletedIds;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        public static GetCustomAttributesDataResponse GetCustomAttributesByType(GetCustomAttributesDataRequest request)
        {
            GetCustomAttributesDataResponse result = null;
            try
            {
                result = new GetCustomAttributesDataResponse();
                IAttributeRepository<GetCustomAttributesDataResponse> repo = PatientGoalRepositoryFactory<GetCustomAttributesDataResponse>.GetAttributeLibraryRepository(request.ContractNumber, request.Context, request.UserId);
                
                result.CustomAttributes = repo.FindByType(request.TypeId) as List<CustomAttributeData>;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public static DeletePatientGoalByPatientIdDataResponse DeletePatientGoalByPatientId(DeletePatientGoalByPatientIdDataRequest request)
        {
            DeletePatientGoalByPatientIdDataResponse response = null;
            bool success = false;
            try
            {
                response = new DeletePatientGoalByPatientIdDataResponse();
                IPatientGoalRepository<PatientGoalViewData> goalRepo = PatientGoalRepositoryFactory<PatientGoalViewData>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);

                List<PatientGoalViewData> goalViewDataList = goalRepo.Find(request.PatientId) as List<PatientGoalViewData>;
                List<DeletedPatientGoal> deletedPatientGoals = null;
                if (goalViewDataList != null && goalViewDataList.Count > 0)
                {
                    deletedPatientGoals = new List<DeletedPatientGoal>();
                    goalViewDataList.ForEach(u =>
                    {
                        DeletePatientGoalDataRequest deletePatientGoalDataRequest = new DeletePatientGoalDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientGoalId = u.Id,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        goalRepo.Delete(deletePatientGoalDataRequest);
                        success = true;
                        DeletedPatientGoal deletedPatientGoal = new DeletedPatientGoal { Id = deletePatientGoalDataRequest.PatientGoalId };

                        #region Delete Barriers
                        DeleteBarrierDataResponse deleteBarrierDataResponse = DeleteBarrier(new DeleteBarrierDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientGoalId = deletePatientGoalDataRequest.PatientGoalId,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        });
                        if (deleteBarrierDataResponse.Success)
                        {
                            deletedPatientGoal.PatientBarrierIds = deleteBarrierDataResponse.DeletedIds;
                            success = true;
                        }

                        #endregion

                        #region Delete Tasks
                        DeleteTaskDataResponse deleteTaskDataResponse = DeleteTask(new DeleteTaskDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientGoalId = deletePatientGoalDataRequest.PatientGoalId,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        });
                        if (deleteTaskDataResponse.Success)
                        {
                            deletedPatientGoal.PatientTaskIds = deleteTaskDataResponse.DeletedIds;
                            success = true;
                        }
                        #endregion

                        #region Delete Interventions
                        DeleteInterventionDataResponse deleteInterventionDataResponse = DeleteIntervention(new DeleteInterventionDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            PatientGoalId = deletePatientGoalDataRequest.PatientGoalId,
                            PatientId = request.PatientId,
                            UserId = request.UserId,
                            Version = request.Version
                        });
                        if (deleteInterventionDataResponse.Success)
                        {
                            deletedPatientGoal.PatientInterventionIds = deleteInterventionDataResponse.DeletedIds;
                            success = true;
                        }
                        #endregion

                        deletedPatientGoals.Add(deletedPatientGoal);
                    });
                    response.DeletedPatientGoals = deletedPatientGoals;
                }
                else 
                {
                    success = true;
                }
                response.Success = success;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public static UndoDeletePatientGoalDataResponse UndoDeletePatientGoals(UndoDeletePatientGoalDataRequest request)
        {
            UndoDeletePatientGoalDataResponse response = null;
            try
            {
                response = new UndoDeletePatientGoalDataResponse();
                IPatientGoalRepository<PatientGoalViewData> goalRepo = PatientGoalRepositoryFactory<PatientGoalViewData>.GetPatientGoalRepository(request.ContractNumber, request.Context, request.UserId);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientGoalId = u.Id;
                        goalRepo.UndoDelete(request);

                        #region Delete Barriers
                        if (u.PatientBarrierIds != null)
                        {
                            IPatientGoalRepository<UndoDeleteBarrierDataRequest> barrierRepo = PatientGoalRepositoryFactory<UndoDeleteBarrierDataRequest>.GetPatientBarrierRepository(request.ContractNumber, request.Context, request.UserId);
                            u.PatientBarrierIds.ForEach(b =>
                            {
                                UndoDeleteBarrierDataRequest barrierRequest = new UndoDeleteBarrierDataRequest
                                {
                                    BarrierId = b,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                barrierRepo.UndoDelete(barrierRequest);
                            });
                        }

                        #endregion

                        #region Delete Tasks
                        if (u.PatientTaskIds != null)
                        {
                            IPatientGoalRepository<UndoDeleteTaskDataRequest> taskRepo = PatientGoalRepositoryFactory<UndoDeleteTaskDataRequest>.GetPatientTaskRepository(request.ContractNumber, request.Context, request.UserId);
                            u.PatientTaskIds.ForEach(t =>
                            {
                                UndoDeleteTaskDataRequest taskRequest = new UndoDeleteTaskDataRequest
                                {
                                    TaskId = t,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                taskRepo.UndoDelete(taskRequest);
                            });
                        }
                        #endregion

                        #region Delete Interventions
                        if (u.PatientInterventionIds != null)
                        {
                            IPatientGoalRepository<UndoDeleteInterventionDataRequest> interventionRepo = PatientGoalRepositoryFactory<UndoDeleteInterventionDataRequest>.GetPatientInterventionRepository(request.ContractNumber, request.Context, request.UserId);
                            u.PatientInterventionIds.ForEach(i =>
                            {
                                UndoDeleteInterventionDataRequest interventionRequest = new UndoDeleteInterventionDataRequest
                                {
                                    InterventionId = i,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                interventionRepo.UndoDelete(interventionRequest);
                            });
                        }
                        #endregion
                    });

                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   

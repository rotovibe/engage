using Phytel.API.DataDomain.PatientGoal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.PatientGoal
{
    public  class PatientGoalDataManager : IPatientGoalDataManager
    {
        public IPatientGoalRepositoryFactory Factory { get; set; }
        public IAttributeRepositoryFactory AttrFactory { get; set; }

        #region Initialize
        public PutInitializeGoalDataResponse InitializeGoal(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = null;
            try
            {
                response = new PutInitializeGoalDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);

                response.Goal = (PatientGoalData)repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutInitializeBarrierDataResponse InitializeBarrier(PutInitializeBarrierDataRequest request)
        {
            PutInitializeBarrierDataResponse response = null;
            try
            {
                response = new PutInitializeBarrierDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientBarrier);

                response.Id = (string)repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutInitializeInterventionResponse InsertNewPatientIntervention(PutInitializeInterventionRequest request)
        {
            try
            {
                PutInitializeInterventionResponse result = new PutInitializeInterventionResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientIntervention);

                result.Id = (string)repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request)
        {
            try
            {
                PutInitializeTaskResponse result = new PutInitializeTaskResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientTask);

                result.Task = (PatientTaskData)repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get
        public GetPatientGoalDataResponse GetPatientGoal(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse result = null;
            try
            {
                result = new GetPatientGoalDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);

                PatientGoalData patientGoalData = repo.FindByID(request.Id) as PatientGoalData;
                if (patientGoalData != null)
                {
                    //Get all barriers for a given goal
                    patientGoalData.BarriersData = getBarriersByPatientGoalId(request, patientGoalData.Id);

                    //Get all tasks for a given goal
                    patientGoalData.TasksData = getTasksByPatientGoalId(request, patientGoalData.Id);

                    //Get all interventions for a given goal
                    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request, patientGoalData.Id);
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

        public GetPatientGoalByTemplateIdResponse GetPatientByTemplateIdGoal(GetPatientGoalByTemplateIdRequest request)
        {
            GetPatientGoalByTemplateIdResponse result = null;
            try
            {
                result = new GetPatientGoalByTemplateIdResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);

                PatientGoalData patientGoalData = repo.FindByTemplateId(request.PatientId, request.TemplateId) as PatientGoalData;

                result.GoalData = patientGoalData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetPatientTaskByTemplateIdResponse GetPatientTaskByTemplateId(GetPatientTaskByTemplateIdRequest request)
        {
            GetPatientTaskByTemplateIdResponse result = null;
            try
            {
                result = new GetPatientTaskByTemplateIdResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientTask);

                PatientTaskData patientTaskData = repo.FindByTemplateId(request.GoalId, request.TemplateId) as PatientTaskData;
                //if (patientGoalData != null)
                //{
                //    //Get all barriers for a given goal
                //    patientGoalData.BarriersData = getBarriersByPatientGoalId(request, patientGoalData.Id);

                //    //Get all tasks for a given goal
                //    patientGoalData.TasksData = getTasksByPatientGoalId(request, patientGoalData.Id);

                //    //Get all interventions for a given goal
                //    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request, patientGoalData.Id);
                //}

                result.TaskData = patientTaskData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetPatientInterventionByTemplateIdResponse GetPatientInterventionByTemplateId(GetPatientInterventionByTemplateIdRequest request)
        {
            GetPatientInterventionByTemplateIdResponse result = null;
            try
            {
                result = new GetPatientInterventionByTemplateIdResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientIntervention);

                PatientInterventionData patientInterventionData = repo.FindByTemplateId(request.GoalId, request.TemplateId) as PatientInterventionData;
                //if (patientGoalData != null)
                //{
                //    //Get all barriers for a given goal
                //    patientGoalData.BarriersData = getBarriersByPatientGoalId(request, patientGoalData.Id);

                //    //Get all tasks for a given goal
                //    patientGoalData.TasksData = getTasksByPatientGoalId(request, patientGoalData.Id);

                //    //Get all interventions for a given goal
                //    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request, patientGoalData.Id);
                //}

                result.InterventionData = patientInterventionData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetTaskDataResponse GetTask(GetTaskDataRequest request)
        {
            GetTaskDataResponse result = null;
            try
            {
                result = new GetTaskDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.Task);

                var intData = repo.FindByID(request.Id) as TaskData;

                result.TaskData = intData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetInterventionDataResponse GetIntervention(GetInterventionDataRequest request)
        {
            GetInterventionDataResponse result = null;
            try
            {
                result = new GetInterventionDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.Intervention);

                var intData = repo.FindByID(request.Id) as InterventionData;

                result.InterventionsData = intData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetGoalDataResponse GetGoal(GetGoalDataRequest request)
        {
            GetGoalDataResponse result = null;
            try
            {
                result = new GetGoalDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.Goal);

                GoalData goalData = repo.FindByID(request.Id) as GoalData;

                //if (patientGoalData != null)
                //{
                //    //Get all barriers for a given goal
                //    patientGoalData.BarriersData = getBarriersByPatientGoalId(request, patientGoalData.Id);

                //    //Get all tasks for a given goal
                //    patientGoalData.TasksData = getTasksByPatientGoalId(request, patientGoalData.Id);

                //    //Get all interventions for a given goal
                //    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request, patientGoalData.Id);
                //}

                result.GoalData = goalData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetAllPatientGoalsDataResponse GetPatientGoalList(GetAllPatientGoalsDataRequest request)
        {
            GetAllPatientGoalsDataResponse result = null;
            try
            {
                result = new GetAllPatientGoalsDataResponse();
                IGoalRepository goalRepo = Factory.GetRepository(request, RepositoryType.PatientGoal);

                List<PatientGoalViewData> goalViewDataList = goalRepo.Find(request.PatientId) as List<PatientGoalViewData>;
                List<PatientGoalViewData> goalDataView = null;
                if (goalViewDataList != null && goalViewDataList.Count > 0)
                {
                    goalDataView = new List<PatientGoalViewData>();
                    foreach (PatientGoalViewData p in goalViewDataList)
                    {
                        string contractNumber = request.ContractNumber;
                        string context = request.Context;

                        PatientGoalViewData view = new PatientGoalViewData();
                        view = p;

                        //Barriers
                        List<ChildViewData> barrierChildView = null;
                        List<PatientBarrierData> barrierData = getBarriersByPatientGoalId(request, p.Id);
                        if (barrierData != null && barrierData.Count > 0)
                        {
                            barrierChildView = new List<ChildViewData>();
                            foreach (PatientBarrierData b in barrierData)
                            {
                                barrierChildView.Add(new ChildViewData { Id = b.Id, PatientGoalId = b.PatientGoalId, Name = b.Name, StatusId = ((int)(b.StatusId)) });
                                barrierChildView = barrierChildView.OrderBy(o => o.Name).ToList();
                            }
                        }
                        view.BarriersData = barrierChildView;

                        //Tasks
                        List<ChildViewData> taskChildView = null;
                        List<PatientTaskData> taskData = getTasksByPatientGoalId(request, p.Id);
                        if (taskData != null && taskData.Count > 0)
                        {
                            taskChildView = new List<ChildViewData>();
                            foreach (PatientTaskData b in taskData)
                            {
                                taskChildView.Add(new ChildViewData { Id = b.Id, PatientGoalId = b.PatientGoalId, Description = b.Description, StatusId = ((int)(b.StatusId)) });
                                taskChildView = taskChildView.OrderBy(o => o.Description).ToList();
                            }
                        }
                        view.TasksData = taskChildView;

                        //Interventions
                        List<ChildViewData> interChildView = null;
                        List<PatientInterventionData> interData = getInterventionsByPatientGoalId(request, p.Id);
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

        public GetPatientInterventionsDataResponse GetPatientInterventions(GetPatientInterventionsDataRequest request)
        {
            try
            {
                var result = new GetPatientInterventionsDataResponse();

                IGoalRepository intRepo = Factory.GetRepository(request, RepositoryType.PatientIntervention);
                IGoalRepository goalRepo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                intRepo.UserId = request.UserId;
                // Get all the goals associated to the patient in the request object.
                if (!string.IsNullOrEmpty(request.PatientId))
                {
                    List<PatientGoalViewData> goalViewDataList = goalRepo.Find(request.PatientId) as List<PatientGoalViewData>;
                    List<string> patientGoalIds = goalViewDataList.Select(a => a.Id).ToList();
                    if (patientGoalIds != null && patientGoalIds.Count > 0)
                    {
                        result.InterventionsData = (List<PatientInterventionData>)intRepo.Search(request, patientGoalIds);
                    }
                }
                else
                {
                    result.InterventionsData = (List<PatientInterventionData>)intRepo.Search(request, null);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetPatientTasksDataResponse GetTasks(GetPatientTasksDataRequest request)
        {
            try
            {
                var result = new GetPatientTasksDataResponse();

                IGoalRepository taskRepo = Factory.GetRepository(request, RepositoryType.PatientTask);
                taskRepo.UserId = request.UserId;
                IGoalRepository goalRepo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                // Get all the goals associated to the patient in the request object.
                if (!string.IsNullOrEmpty(request.PatientId))
                {
                    List<PatientGoalViewData> goalViewDataList = goalRepo.Find(request.PatientId) as List<PatientGoalViewData>;
                    List<string> patientGoalIds = goalViewDataList.Select(a => a.Id).ToList();
                    if (patientGoalIds != null && patientGoalIds.Count > 0)
                    {
                        result.TasksData = (List<PatientTaskData>)taskRepo.Search(request, patientGoalIds);
                    }
                }
                else
                {
                    result.TasksData = (List<PatientTaskData>)taskRepo.Search(request, null);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region Update
        public PutUpdateGoalDataResponse PutPatientGoal(PutUpdateGoalDataRequest request)
        {
            try
            {
                PutUpdateGoalDataResponse result = new PutUpdateGoalDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                if (request.Goal != null)
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        PatientGoalData data = repo.FindByID(request.Goal.Id) as PatientGoalData;
                        result.GoalData = data;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PutUpdateTaskResponse UpdatePatientTask(PutUpdateTaskRequest request)
        {
            try
            {
                PutUpdateTaskResponse result = new PutUpdateTaskResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientTask);

                if (request.TaskIdsList != null && request.TaskIdsList.Count > 0)
                {
                    List<PatientTaskData> ptd = (List<PatientTaskData>)repo.Find(request.PatientGoalId);
                    List<string> dbTaskIdList = GetTaskIds(ptd);

                    // update existing task entries with a delete
                    List<string> excludes = dbTaskIdList.Except(request.TaskIdsList).ToList<string>();
                    excludes.ForEach(ex =>
                    {
                        // create delete task request to insert
                        DeleteTaskDataRequest dtr = new DeleteTaskDataRequest { TaskId = ex, UserId = request.UserId };
                        repo.Delete(dtr);
                    });
                }
                if (request.Task != null && request.Task.Id != "0")
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        PatientTaskData data = repo.FindByID(request.Task.Id) as PatientTaskData;
                        result.TaskData = data;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PutUpdateInterventionResponse UpdatePatientIntervention(PutUpdateInterventionRequest request)
        {
            try
            {
                PutUpdateInterventionResponse result = new PutUpdateInterventionResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientIntervention);

                if (request.InterventionIdsList != null && request.InterventionIdsList.Count > 0)
                {
                    List<PatientInterventionData> pid = (List<PatientInterventionData>)repo.Find(request.PatientGoalId);
                    List<string> dbTaskIdList = GetInterventionIds(pid);

                    // update existing intervention entries with a delete
                    List<string> excludes = dbTaskIdList.Except(request.InterventionIdsList).ToList<string>();
                    excludes.ForEach(ex =>
                    {
                        // create delete intervention request to insert
                        DeleteInterventionDataRequest dtr = new DeleteInterventionDataRequest { InterventionId = ex, UserId = request.UserId };
                        repo.Delete(dtr);
                    });
                }
                if (request.Intervention != null && request.Intervention.Id != "0")
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        PatientInterventionData data = repo.FindByID(request.Intervention.Id) as PatientInterventionData;
                        result.InterventionData = data;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PutUpdateBarrierResponse UpdatePatientBarrier(PutUpdateBarrierRequest request)
        {
            try
            {
                PutUpdateBarrierResponse result = new PutUpdateBarrierResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientBarrier);

                if (request.BarrierIdsList != null && request.BarrierIdsList.Count > 0)
                {
                    List<PatientBarrierData> pid = (List<PatientBarrierData>)repo.Find(request.PatientGoalId);
                    List<string> dbBarrierIdList = GetBarrierIds(pid);

                    // update existing barrier entries with a delete
                    List<string> excludes = dbBarrierIdList.Except(request.BarrierIdsList).ToList<string>();
                    excludes.ForEach(ex =>
                    {
                        // create delete barrier request to insert
                        DeleteBarrierDataRequest dbr = new DeleteBarrierDataRequest { BarrierId = ex, UserId = request.UserId };
                        repo.Delete(dbr);
                    });
                }
                if (request.Barrier != null && request.Barrier.Id != "0")
                {
                    bool status = (bool)repo.Update(request);
                    if (status)
                    {
                        PatientBarrierData data = repo.FindByID(request.Barrier.Id) as PatientBarrierData;
                        result.BarrierData = data;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region Delete & UndoDelete
        public  DeletePatientGoalDataResponse DeletePatientGoal(DeletePatientGoalDataRequest request)
        {
            try
            {
                DeletePatientGoalDataResponse result = new DeletePatientGoalDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                
                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DeleteTaskDataResponse DeleteTask(DeleteTaskDataRequest request)
        {
            try
            {
                DeleteTaskDataResponse result = new DeleteTaskDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientTask);
                
                List<PatientTaskData> ptd = (List<PatientTaskData>)repo.Find(request.PatientGoalId);
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

        public  DeleteInterventionDataResponse DeleteIntervention(DeleteInterventionDataRequest request)
        {
            try
            {
                DeleteInterventionDataResponse result = new DeleteInterventionDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientIntervention);
                
                List<PatientInterventionData> pid = (List<PatientInterventionData>)repo.Find(request.PatientGoalId);
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

        public  DeleteBarrierDataResponse DeleteBarrier(DeleteBarrierDataRequest request)
        {
            try
            {
                DeleteBarrierDataResponse result = new DeleteBarrierDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientBarrier);
                
                List<PatientBarrierData> pbd = (List<PatientBarrierData>)repo.Find(request.PatientGoalId);
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

        public  DeletePatientGoalByPatientIdDataResponse DeletePatientGoalByPatientId(DeletePatientGoalByPatientIdDataRequest request)
        {
            DeletePatientGoalByPatientIdDataResponse response = null;
            bool success = false;
            try
            {
                response = new DeletePatientGoalByPatientIdDataResponse();
                IGoalRepository goalRepo = Factory.GetRepository(request, RepositoryType.PatientGoal);

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

        public  UndoDeletePatientGoalDataResponse UndoDeletePatientGoals(UndoDeletePatientGoalDataRequest request)
        {
            UndoDeletePatientGoalDataResponse response = null;
            try
            {
                response = new UndoDeletePatientGoalDataResponse();
                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientGoalId = u.Id;
                        repo.UndoDelete(request);

                        #region Delete Barriers
                        if (u.PatientBarrierIds != null && u.PatientBarrierIds.Count > 0)
                        {
                            IGoalRepository barrierRepo = Factory.GetRepository(request, RepositoryType.PatientBarrier);
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
                        if (u.PatientTaskIds != null && u.PatientTaskIds.Count > 0)
                        {
                            IGoalRepository taskRepo = Factory.GetRepository(request, RepositoryType.PatientTask);
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
                        if (u.PatientInterventionIds != null && u.PatientInterventionIds.Count > 0)
                        {
                            IGoalRepository interventionRepo = Factory.GetRepository(request, RepositoryType.PatientIntervention);
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
        #endregion

        #region CustomAttribute
        public GetCustomAttributesDataResponse GetCustomAttributesByType(GetCustomAttributesDataRequest request)
        {
            GetCustomAttributesDataResponse result = null;
            try
            {
                result = new GetCustomAttributesDataResponse();
                IAttributeRepository repo = AttrFactory.GetRepository(request, RepositoryType.AttributeLibrary);

                result.CustomAttributes = repo.FindByType(request.TypeId) as List<CustomAttributeData>;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        } 
        #endregion

        #region RemoveProgram
        public RemoveProgramInPatientGoalsDataResponse RemoveProgramInPatientGoals(RemoveProgramInPatientGoalsDataRequest request)
        {
            RemoveProgramInPatientGoalsDataResponse response = null;
            try
            {
                response = new RemoveProgramInPatientGoalsDataResponse();

                IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientGoal);
                if (request.ProgramId != null)
                {
                    List<PatientGoalData> goals = repo.FindGoalsWithAProgramId(request.ProgramId) as List<PatientGoalData>;
                    if (goals != null && goals.Count > 0)
                    {
                        goals.ForEach(u =>
                        {
                            request.PatientGoalId = u.Id;
                            if (u.ProgramIds != null && u.ProgramIds.Remove(request.ProgramId))
                            {
                                repo.RemoveProgram(request, u.ProgramIds);
                            }
                        });
                    }
                }
                return response;
            }
            catch (Exception ex) { throw ex; }
        } 
        #endregion

        #region Private methods
        private List<PatientBarrierData> getBarriersByPatientGoalId(IDataDomainRequest request, string patientGoalId)
        {
            IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientBarrier);

            List<PatientBarrierData> barrierDataList = repo.Find(patientGoalId) as List<PatientBarrierData>;
            return barrierDataList;
        }

        private List<PatientTaskData> getTasksByPatientGoalId(IDataDomainRequest request, string patientGoalId)
        {
            IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientTask);

            List<PatientTaskData> taskDataList = repo.Find(patientGoalId) as List<PatientTaskData>;
            return taskDataList;
        }

        private List<PatientInterventionData> getInterventionsByPatientGoalId(IDataDomainRequest request, string patientGoalId)
        {
            IGoalRepository repo = Factory.GetRepository(request, RepositoryType.PatientIntervention);

            List<PatientInterventionData> interventionDataList = repo.Find(patientGoalId) as List<PatientInterventionData>;
            return interventionDataList;
        }

        private List<string> GetInterventionIds(List<PatientInterventionData> ptd)
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

        private List<string> GetBarrierIds(List<PatientBarrierData> ptd)
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

        private List<string> GetTaskIds(List<PatientTaskData> ptd)
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
    }
}   

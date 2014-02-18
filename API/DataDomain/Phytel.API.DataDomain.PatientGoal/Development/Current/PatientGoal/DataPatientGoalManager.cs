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
                IPatientGoalRepository<PutInitializeGoalDataResponse> repo = PatientGoalRepositoryFactory<PutInitializeGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
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
                IPatientGoalRepository<PutInitializeBarrierDataResponse> repo = PatientGoalRepositoryFactory<PutInitializeBarrierDataResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context);
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
                IPatientGoalRepository<PatientGoalData> goalRepo = PatientGoalRepositoryFactory<PatientGoalData>.GetPatientGoalRepository(request.ContractNumber, request.Context);

                PatientGoalData patientGoalData = goalRepo.FindByID(request.Id) as PatientGoalData;
                if (patientGoalData != null)
                {
                    //Get all barriers for a given goal
                    patientGoalData.BarriersData = getBarriersByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id);

                    //Get all tasks for a given goal
                    patientGoalData.TasksData = getTasksByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id);
                    
                    //Get all interventions for a given goal
                    patientGoalData.InterventionsData = getInterventionsByPatientGoalId(request.ContractNumber, request.Context, patientGoalData.Id);
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
                IPatientGoalRepository<PatientGoalViewData> goalRepo = PatientGoalRepositoryFactory<PatientGoalViewData>.GetPatientGoalRepository(request.ContractNumber, request.Context);
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
                        List<PatientBarrierData> barrierData = getBarriersByPatientGoalId(contractNumber, context, p.Id);
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
                        List<PatientTaskData> taskData = getTasksByPatientGoalId(contractNumber, context, p.Id);
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
                        List<PatientInterventionData> interData = getInterventionsByPatientGoalId(contractNumber, context, p.Id);
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
        private static List<PatientBarrierData> getBarriersByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            IPatientGoalRepository<PatientBarrierData> barrierRepo = PatientGoalRepositoryFactory<PatientBarrierData>.GetPatientBarrierRepository(contractNumber, context);
            List<PatientBarrierData> barrierDataList = barrierRepo.Find(patientGoalId) as List<PatientBarrierData>; 
            return barrierDataList;
        }

        private static List<PatientTaskData> getTasksByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            IPatientGoalRepository<PatientTaskData> taskRepo = PatientGoalRepositoryFactory<PatientTaskData>.GetPatientTaskRepository(contractNumber, context);
            List<PatientTaskData> taskDataList = taskRepo.Find(patientGoalId) as List<PatientTaskData>;
            return taskDataList;
        }

        private static List<PatientInterventionData> getInterventionsByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            IPatientGoalRepository<PatientInterventionData> interventionRepo = PatientGoalRepositoryFactory<PatientInterventionData>.GetPatientInterventionRepository(contractNumber, context);
            List<PatientInterventionData> interventionDataList = interventionRepo.Find(patientGoalId) as List<PatientInterventionData>;
            return interventionDataList;
        }

        #endregion

        #region // TASKS
        public static PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request)
        {
            try
            {
                PutInitializeTaskResponse result = new PutInitializeTaskResponse();

                IPatientGoalRepository<PutInitializeTaskResponse> repo = PatientGoalRepositoryFactory<PutInitializeTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);

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

                IPatientGoalRepository<PutUpdateTaskResponse> repo = PatientGoalRepositoryFactory<PutUpdateTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request.Task);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static PutInitializeInterventionResponse InsertNewPatientIntervention(PutInitializeInterventionRequest request)
        {
            try
            {
                PutInitializeInterventionResponse result = new PutInitializeInterventionResponse();

                IPatientGoalRepository<PutInitializeInterventionResponse> repo = PatientGoalRepositoryFactory<PutInitializeInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context);

                result.Id = (string)repo.Initialize(request);
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

                IPatientGoalRepository<PutUpdateInterventionResponse> repo = PatientGoalRepositoryFactory<PutUpdateInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutPatientGoalDataResponse PutPatientGoal(PutPatientGoalDataRequest request)
        {
            try
            {
                PutPatientGoalDataResponse result = new PutPatientGoalDataResponse();

                IPatientGoalRepository<PutPatientGoalDataResponse> repo = PatientGoalRepositoryFactory<PutPatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request.GoalData);

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

                IPatientGoalRepository<PutUpdateBarrierResponse> repo = PatientGoalRepositoryFactory<PutUpdateBarrierResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request);

                result.Updated = status;
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

                IPatientGoalRepository<DeletePatientGoalDataResponse> repo = PatientGoalRepositoryFactory<DeletePatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteTaskResponse DeleteTask(DeleteTaskRequest request)
        {
            try
            {
                DeleteTaskResponse result = new DeleteTaskResponse();

                IPatientGoalRepository<DeleteTaskResponse> repo = PatientGoalRepositoryFactory<DeleteTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);
                List<PatientTaskData> ptd = (List<PatientTaskData>)repo.FindByGoalId(request.PatientGoalId);
                ptd.ForEach(t =>
                {
                    request.TaskId = t.Id;
                    repo.Delete(request);
                });

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteInterventionResponse DeleteIntervention(DeleteInterventionRequest request)
        {
            try
            {
                DeleteInterventionResponse result = new DeleteInterventionResponse();

                IPatientGoalRepository<DeleteInterventionResponse> repo = PatientGoalRepositoryFactory<DeleteInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context);
                List<PatientInterventionData> pid = (List<PatientInterventionData>)repo.FindByGoalId(request.PatientGoalId);
                pid.ForEach(i =>
                {
                    request.InterventionId = i.Id;
                    repo.Delete(request);
                });
                
                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteBarrierResponse DeleteBarrier(DeleteBarrierRequest request)
        {
            try
            {
                DeleteBarrierResponse result = new DeleteBarrierResponse();

                IPatientGoalRepository<DeleteBarrierResponse> repo = PatientGoalRepositoryFactory<DeleteBarrierResponse>.GetPatientBarrierRepository(request.ContractNumber, request.Context);
                List<PatientBarrierData> pbd = (List<PatientBarrierData>)repo.FindByGoalId(request.PatientGoalId);
                pbd.ForEach(b =>
                {
                    request.BarrierId = b.Id;
                    repo.Delete(request);
                });
                
                repo.Delete(request);

                result.Deleted = true;
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
                IAttributeRepository<GetCustomAttributesDataResponse> repo = PatientGoalRepositoryFactory<GetCustomAttributesDataResponse>.GetAttributeLibraryRepository(request.ContractNumber, request.Context);
                result.CustomAttributes = repo.FindByType(request.TypeId) as List<CustomAttributeData>;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
    }
}   

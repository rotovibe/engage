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
                response.Id = repo.Initialize(request);
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
                response.Id = repo.Initialize(request);
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
                List<PatientGoalViewData> goalViewDataList = getGoalsViewByPatientId(request.ContractNumber, request.Context, request.PatientId);
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
                                barrierChildView.Add(new ChildViewData { Id = b.Id, Name = b.Name, StatusId = ((int)(b.StatusId))});
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
                                taskChildView.Add(new ChildViewData { Id = b.Id, Name = b.Description, StatusId = ((int)(b.StatusId)) });
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
                                interChildView.Add(new ChildViewData { Id = b.Id, Name = b.Description, StatusId = ((int)(b.StatusId))});
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
        private static List<PatientGoalViewData> getGoalsViewByPatientId(string contractNumber, string context, string patientId)
        {
            List<PatientGoalViewData> goalViewDataList = null;

            IPatientGoalRepository<PatientGoalViewData> goalRepo = PatientGoalRepositoryFactory<PatientGoalViewData>.GetPatientGoalRepository(contractNumber, context);
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            //PatientId
            SelectExpression pgIdSelectExpression = new SelectExpression();
            pgIdSelectExpression.FieldName = MEPatientGoal.PatientIdProperty;
            pgIdSelectExpression.Type = SelectExpressionType.EQ;
            pgIdSelectExpression.Value = patientId;
            pgIdSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            pgIdSelectExpression.ExpressionOrder = 1;
            pgIdSelectExpression.GroupID = 1;
            selectExpressions.Add(pgIdSelectExpression);

            // DeleteFlag = false.
            SelectExpression deleteFlagSelectExpression = new SelectExpression();
            deleteFlagSelectExpression.FieldName = MEPatientGoal.DeleteFlagProperty;
            deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
            deleteFlagSelectExpression.Value = false;
            deleteFlagSelectExpression.ExpressionOrder = 2;
            deleteFlagSelectExpression.GroupID = 1;
            selectExpressions.Add(deleteFlagSelectExpression);

            //// TTL is null.
            //SelectExpression ttlSelectExpression = new SelectExpression();
            //ttlSelectExpression.FieldName = MEPatientGoal.TTLDateProperty;
            //ttlSelectExpression.Type = SelectExpressionType.EQ;
            //ttlSelectExpression.Value = BsonNull.Value;
            //ttlSelectExpression.ExpressionOrder = 3;
            //ttlSelectExpression.GroupID = 1;
            //selectExpressions.Add(ttlSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> goalViewData = goalRepo.Select(apiExpression);

            if (goalViewData != null)
            {
                goalViewDataList = goalViewData.Item2.Cast<PatientGoalViewData>().ToList();
            }

            return goalViewDataList;
        }

        private static List<PatientBarrierData> getBarriersByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            List<PatientBarrierData> barrierDataList = null;

            IPatientGoalRepository<PatientBarrierData> barrierRepo = PatientGoalRepositoryFactory<PatientBarrierData>.GetPatientBarrierRepository(contractNumber, context);
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            //PatientGoalId
            SelectExpression pgIdSelectExpression = new SelectExpression();
            pgIdSelectExpression.FieldName = MEPatientBarrier.PatientGoalIdProperty;
            pgIdSelectExpression.Type = SelectExpressionType.EQ;
            pgIdSelectExpression.Value = patientGoalId;
            pgIdSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            pgIdSelectExpression.ExpressionOrder = 1;
            pgIdSelectExpression.GroupID = 1;
            selectExpressions.Add(pgIdSelectExpression);

            // DeleteFlag = false.
            SelectExpression deleteFlagSelectExpression = new SelectExpression();
            deleteFlagSelectExpression.FieldName = MEPatientBarrier.DeleteFlagProperty;
            deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
            deleteFlagSelectExpression.Value = false;
            deleteFlagSelectExpression.ExpressionOrder = 2;
            deleteFlagSelectExpression.GroupID = 1;
            selectExpressions.Add(deleteFlagSelectExpression);

            //// TTL is null.
            //SelectExpression ttlSelectExpression = new SelectExpression();
            //ttlSelectExpression.FieldName = MEPatientBarrier.TTLDateProperty;
            //ttlSelectExpression.Type = SelectExpressionType.EQ;
            //ttlSelectExpression.Value = BsonNull.Value;
            //ttlSelectExpression.ExpressionOrder = 3;
            //ttlSelectExpression.GroupID = 1;
            //selectExpressions.Add(ttlSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> barrierData = barrierRepo.Select(apiExpression);

            if (barrierData != null)
            {
                barrierDataList = barrierData.Item2.Cast<PatientBarrierData>().ToList();
            }

            return barrierDataList;
        }

        private static List<PatientTaskData> getTasksByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            List<PatientTaskData> taskDataList = null;

            IPatientGoalRepository<PatientTaskData> taskRepo = PatientGoalRepositoryFactory<PatientTaskData>.GetPatientTaskRepository(contractNumber, context);
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            //PatientGoalId
            SelectExpression pgIdSelectExpression = new SelectExpression();
            pgIdSelectExpression.FieldName = MEPatientTask.PatientGoalIdProperty;
            pgIdSelectExpression.Type = SelectExpressionType.EQ;
            pgIdSelectExpression.Value = patientGoalId;
            pgIdSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            pgIdSelectExpression.ExpressionOrder = 1;
            pgIdSelectExpression.GroupID = 1;
            selectExpressions.Add(pgIdSelectExpression);

            // DeleteFlag = false.
            SelectExpression deleteFlagSelectExpression = new SelectExpression();
            deleteFlagSelectExpression.FieldName = MEPatientTask.DeleteFlagProperty;
            deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
            deleteFlagSelectExpression.Value = false;
            deleteFlagSelectExpression.ExpressionOrder = 2;
            deleteFlagSelectExpression.GroupID = 1;
            selectExpressions.Add(deleteFlagSelectExpression);

            //// TTL is null.
            //SelectExpression ttlSelectExpression = new SelectExpression();
            //ttlSelectExpression.FieldName = MEPatientTask.TTLDateProperty;
            //ttlSelectExpression.Type = SelectExpressionType.EQ;
            //ttlSelectExpression.Value = BsonNull.Value;
            //ttlSelectExpression.ExpressionOrder = 3;
            //ttlSelectExpression.GroupID = 1;
            //selectExpressions.Add(ttlSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> taskData = taskRepo.Select(apiExpression);

            if (taskData != null)
            {
                taskDataList = taskData.Item2.Cast<PatientTaskData>().ToList();
            }

            return taskDataList;
        }

        private static List<PatientInterventionData> getInterventionsByPatientGoalId(string contractNumber, string context, string patientGoalId)
        {
            List<PatientInterventionData> interventionDataList = null;

            IPatientGoalRepository<PatientInterventionData> interventionRepo = PatientGoalRepositoryFactory<PatientInterventionData>.GetPatientInterventionRepository(contractNumber, context);
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            //PatientGoalId
            SelectExpression pgIdSelectExpression = new SelectExpression();
            pgIdSelectExpression.FieldName = MEPatientIntervention.PatientGoalIdProperty;
            pgIdSelectExpression.Type = SelectExpressionType.EQ;
            pgIdSelectExpression.Value = patientGoalId;
            pgIdSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            pgIdSelectExpression.ExpressionOrder = 1;
            pgIdSelectExpression.GroupID = 1;
            selectExpressions.Add(pgIdSelectExpression);

            // DeleteFlag = false.
            SelectExpression deleteFlagSelectExpression = new SelectExpression();
            deleteFlagSelectExpression.FieldName = MEPatientIntervention.DeleteFlagProperty;
            deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
            deleteFlagSelectExpression.Value = false;
            deleteFlagSelectExpression.ExpressionOrder = 2;
            deleteFlagSelectExpression.GroupID = 1;
            selectExpressions.Add(deleteFlagSelectExpression);

            //// TTL is null.
            //SelectExpression ttlSelectExpression = new SelectExpression();
            //ttlSelectExpression.FieldName = MEPatientIntervention.TTLDateProperty;
            //ttlSelectExpression.Type = SelectExpressionType.EQ;
            //ttlSelectExpression.Value = BsonNull.Value;
            //ttlSelectExpression.ExpressionOrder = 3;
            //ttlSelectExpression.GroupID = 1;
            //selectExpressions.Add(ttlSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> interventionData = interventionRepo.Select(apiExpression);

            if (interventionData != null)
            {
                interventionDataList = interventionData.Item2.Cast<PatientInterventionData>().ToList();
            }

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

                result.Id = repo.Initialize(request);
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

                result.Id = repo.Initialize(request);
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
                bool status = (bool)repo.Update(request.Intervention);

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
                repo.Delete(request);

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
                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   

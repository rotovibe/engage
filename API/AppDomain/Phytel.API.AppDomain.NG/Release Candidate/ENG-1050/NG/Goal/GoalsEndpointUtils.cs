using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using AD = Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class GoalsEndpointUtils : IGoalsEndpointUtils
    {
        #region Static Fields
        static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];
        static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        #endregion

        #region Public Methods
        public PatientGoalData GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            try
            {
                PatientGoalData result = null;
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                PutInitializeGoalDataResponse dataDomainResponse = client.Put<PutInitializeGoalDataResponse>(
                    url, new PutInitializeGoalDataRequest() as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Goal;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialGoalRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public string GetInitialBarrierRequest(GetInitializeBarrierRequest request)
        {
            try
            {
                string result = string.Empty;
                //   [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Barrier/Initialize", "POST")]
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId), request.UserId);
                
                PutInitializeBarrierDataResponse dataDomainResponse = client.Put<PutInitializeBarrierDataResponse>(
                    url,
                    new PutInitializeBarrierDataRequest() as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Id;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialBarrierRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientTaskData GetInitialTaskRequest(GetInitializeTaskRequest request)
        {
            try
            {
                PatientTaskData result = null;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId), request.UserId);

                PutInitializeTaskResponse response = client.Put<PutInitializeTaskResponse>(url, new PutInitializeTaskRequest() as object);

                if (response != null)
                {
                    result = response.Task;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialTaskRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientGoal GetPatientGoal(GetPatientGoalRequest request)
        {
            try
            {
                PatientGoal result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id), request.UserId);

                GetPatientGoalDataResponse ddResponse = client.Get<GetPatientGoalDataResponse>(
                    url);

                if (ddResponse != null && ddResponse.GoalData != null)
                {
                    //Make a call to AttributeLibrary to get attributes details for Goal and Task.
                    List<CustomAttribute> goalAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 1);
                    List<CustomAttribute> taskAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 2);

                    result = GoalsUtil.ConvertToGoal(ddResponse.GoalData, goalAttributesLibrary);
                    result.Barriers = GoalsUtil.ConvertToBarriers(ddResponse.GoalData.BarriersData);
                    result.Tasks = GoalsUtil.ConvertToTasks(ddResponse.GoalData.TasksData, taskAttributesLibrary);
                    result.Interventions = GoalsUtil.ConvertToInterventions(request, client, ddResponse.GoalData.InterventionsData);
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientGoal()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientGoalView> GetAllPatientGoals(GetAllPatientGoalsRequest request)
        {
            try
            {
                List<PatientGoalView> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goals", "GET")]
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goals",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                GetAllPatientGoalsDataResponse ddResponse = client.Get<GetAllPatientGoalsDataResponse>(
                    url);

                if (ddResponse != null && ddResponse.PatientGoalsData != null && ddResponse.PatientGoalsData.Count > 0)
                {
                    result = new List<PatientGoalView>();
                    List<PatientGoalViewData> gdvList = ddResponse.PatientGoalsData;
                    foreach(PatientGoalViewData gdv in gdvList)
                    {
                        PatientGoalView gv = new PatientGoalView();
                        gv.Id = gdv.Id;
                        gv.PatientId = gdv.PatientId;
                        gv.FocusAreaIds = gdv.FocusAreaIds;
                        gv.Name = gdv.Name;
                        gv.StatusId = gdv.StatusId;
                        gv.Barriers = GoalsUtil.GetChildView(gdv.BarriersData);
                        gv.Tasks = GoalsUtil.GetChildView(gdv.TasksData); ;
                        gv.Interventions = GoalsUtil.GetChildView(gdv.InterventionsData); ;
                        result.Add(gv);
                    }
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllPatientGoals()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientIntervention> GetInterventions(GetInterventionsRequest request)
        {
            List<PatientIntervention> interventions = null;
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/Goal/Interventions", "POST")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Goal/Interventions",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber), request.UserId);

                GetPatientInterventionsDataResponse ddResponse =
                    client.Post<GetPatientInterventionsDataResponse>(url, new GetPatientInterventionsDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        Version = request.Version,
                        UserId = request.UserId,
                        AssignedToId = request.AssignedToId,
                        CreatedById = request.CreatedById,
                        PatientId = request.PatientId,
                        StatusIds = request.StatusIds

                    } as object);

                if (ddResponse != null && ddResponse.InterventionsData != null)
                {
                    interventions = new List<PatientIntervention>();
                    List<PatientInterventionData> dataList = ddResponse.InterventionsData;
                    foreach (PatientInterventionData n in dataList)
                    {
                        PatientIntervention i = GoalsUtil.ConvertToIntervention(n);
                        // Call Patient DD to get patient details. 
                        i.PatientDetails = GetPatientDetails(request.Version, request.ContractNumber, request.UserId, client, n.PatientId);
                        i.PatientId = n.PatientId;
                        interventions.Add(i);
                    }
                }
            }
            catch
            {
                throw;
            }
            return interventions;
        }

        public List<PatientTask> GetTasks(GetTasksRequest request)
        {
            List<PatientTask> tasks = null;
            try
            {
                //[Route("/{Context}/{Version}/{ContractNumber}/Goal/Tasks", "POST")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Goal/Tasks",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber), request.UserId);

                GetPatientTasksDataResponse ddResponse =
                    client.Post<GetPatientTasksDataResponse>(url, new GetPatientTasksDataRequest
                    {
                        Context = "NG",
                        ContractNumber = request.ContractNumber,
                        Version = request.Version,
                        UserId = request.UserId,
                        StatusIds = request.StatusIds,
                        PatientId = request.PatientId
                    } as object);

                if (ddResponse != null && ddResponse.TasksData != null)
                {
                    tasks = new List<PatientTask>();
                    List<PatientTaskData> dataList = ddResponse.TasksData;
                    foreach (PatientTaskData n in dataList)
                    {
                        //Make a call to AttributeLibrary to get attributes details for Goal and Task.
                        List<CustomAttribute> taskAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 2);
                        PatientTask i = GoalsUtil.ConvertToTask(n, taskAttributesLibrary);
                        tasks.Add(i);
                    }
                }
            }
            catch
            {
                throw;
            }
            return tasks;
        }
        #endregion

        #region Internal Methods

        public bool DeleteGoalRequest(PostDeletePatientGoalRequest request)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Delete/",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId), request.UserId);

                DeletePatientGoalDataResponse response = client.Delete<DeletePatientGoalDataResponse>(
                    url);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteGoalRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public bool DeleteBarrierRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Delete/",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                   id), request.UserId);

                DeleteBarrierDataResponse response = client.Delete<DeleteBarrierDataResponse>(
                    url);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteBarrierRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public bool DeleteTaskRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Delete/",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    id), request.UserId);

                DeleteTaskDataResponse response = client.Delete<DeleteTaskDataResponse>(
                    url);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteTaskRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public bool DeleteInterventionRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Delete/",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    id), request.UserId);

                DeleteInterventionDataResponse response = client.Delete<DeleteInterventionDataResponse>(
                    url);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteInterventionRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public List<CustomAttribute> GetAttributesLibraryByType(IAppDomainRequest request, int typeId)
        {
            List<CustomAttribute> attrList = new List<CustomAttribute>();
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Attributes/{4}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    typeId), request.UserId);

                GetCustomAttributesDataResponse response = client.Get<GetCustomAttributesDataResponse>(
                    url);

                if (response != null && response.CustomAttributes != null)
                {
                    response.CustomAttributes.ForEach(ca =>
                    {
                        attrList.Add(new CustomAttribute
                        {
                            Id = ca.Id,
                            Name = ca.Name,
                            ControlType = ca.ControlType,
                            Order = ca.Order,
                            Options = GoalsUtil.ConvertToOptionList(ca.Options),
                            Required = ca.Required
                        });
                    });
                }
                return attrList;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAttributesLibraryByType()::" + ex.Message, ex.InnerException);
            }
        }

        public bool PostUpdateInterventionRequest(PostPatientGoalRequest request, PatientInterventionData pi)
        {
            try
            {
                bool result = false;

                List<string> interventionsIds = GetInterventionIdsForRequest(request.Goal.Interventions);

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                    pi.Id), request.UserId);

                PutUpdateInterventionResponse response = client.Put<PutUpdateInterventionResponse>(
                    url, new PutUpdateInterventionRequest { Intervention = pi, UserId = request.UserId, InterventionIdsList = interventionsIds } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateInterventionRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public bool PostUpdateBarrierRequest(PostPatientGoalRequest request, PatientBarrierData bd)
        {
            try
            {
                bool result = false;

                List<string> barrierIds = GetBarrierIdsForRequest(request.Goal.Barriers);

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                   bd.Id), request.UserId);

                PutUpdateBarrierResponse response = client.Put<PutUpdateBarrierResponse>(
                    url, new PutUpdateBarrierRequest { UserId = request.UserId, Barrier = bd, BarrierIdsList = barrierIds } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateBarrierRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public bool PostUpdateTaskRequest(PostPatientGoalRequest request, PatientTaskData td)
        {
            try
            {
                bool result = false;

                List<string> taskIds = GetTaskIdsForRequest(request.Goal.Tasks);

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                    td.Id), request.UserId);

                PutUpdateTaskResponse response = client.Put<PutUpdateTaskResponse>(
                    url, new PutUpdateTaskRequest { Task = td, TaskIdsList = taskIds, UserId = request.UserId } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateTaskRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public string GetInitialInterventionRequest(GetInitializeInterventionRequest request)
        {
            try
            {
                string result = string.Empty;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId), request.UserId);

                PutInitializeInterventionResponse response = client.Put<PutInitializeInterventionResponse>(
                    url,
                    new PutInitializeInterventionRequest() as object);

                if (response != null)
                {
                    result = response.Id;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialInterventionRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientGoal PostUpdateGoalRequest(PostPatientGoalRequest request)
        {
            try
            {
                PatientGoal goal = null;

                if (request.Goal == null)
                    throw new Exception("The Goal property is null in the request.");
                else if (string.IsNullOrEmpty(request.Goal.Name) || string.IsNullOrEmpty(request.Goal.SourceId))
                    throw new Exception("The goal name and source are required fields.");

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id), request.UserId);

                PutUpdateGoalDataResponse response = client.Put<PutUpdateGoalDataResponse>(
                    url, new PutUpdateGoalDataRequest { Goal = convertToPatientGoalData(request.Goal), UserId = request.UserId } as object);

                if (response != null && response.GoalData != null)
                {
                    //Make a call to AttributeLibrary to get attributes details for Goal.
                    List<CustomAttribute> goalAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 1);
                    goal = GoalsUtil.ConvertToGoal(response.GoalData, goalAttributesLibrary);
                }

                return goal;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateGoalRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientIntervention PostUpdateInterventionRequest(PostPatientInterventionRequest request)
        {
            try
            {
                PatientIntervention intervention = null;

                if (request.Intervention == null)
                    throw new Exception("The Intervention property is null in the request.");

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Intervention/{InterventionId}/Update"
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    request.Id), request.UserId);

                PutUpdateInterventionResponse response = client.Put<PutUpdateInterventionResponse>(
                    url, new PutUpdateInterventionRequest { Intervention = GoalsUtil.ConvertToInterventionData(request.Intervention) , UserId = request.UserId} as object);

                if (response != null && response.InterventionData != null)
                {
                    intervention = GoalsUtil.ConvertToIntervention(response.InterventionData);
                    // Call Patient DD to get patient details. 
                    intervention.PatientDetails = GetPatientDetails(request.Version, request.ContractNumber, request.UserId, client, response.InterventionData.PatientId);
                    intervention.PatientId = response.InterventionData.PatientId;
                }
                return intervention;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateInterventionRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientTask PostUpdateTaskRequest(PostPatientTaskRequest request)
        {
            try
            {
                PatientTask task = null;

                if (request.Task == null)
                    throw new Exception("The Task property is null in the request.");

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    request.Id), request.UserId);

                PutUpdateTaskResponse response = client.Put<PutUpdateTaskResponse>(
                    url, new PutUpdateTaskRequest { Task = GoalsUtil.ConvertToPatientTaskData(request.Task), UserId = request.UserId } as object);

                if (response != null && response.TaskData != null)
                {
                    //Make a call to AttributeLibrary to get attributes details for Goal and Task.
                    List<CustomAttribute> taskAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 2);
                    task = GoalsUtil.ConvertToTask(response.TaskData, taskAttributesLibrary);
                }
                return task;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateTaskRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PatientBarrier PostUpdateBarrierRequest(PostPatientBarrierRequest request)
        {
            try
            {
                PatientBarrier task = null;

                if (request.Barrier == null)
                    throw new Exception("The Barrier property is null in the request.");

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    request.Id), request.UserId);

                PutUpdateBarrierResponse response = client.Put<PutUpdateBarrierResponse>(
                    url, new PutUpdateBarrierRequest { Barrier = GoalsUtil.ConvertToPatientBarrierData(request.Barrier), UserId = request.UserId } as object);

                if (response != null && response.BarrierData != null)
                {
                    task = GoalsUtil.ConvertToBarrier(response.BarrierData);
                }
                return task;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateBarrierRequest()::" + ex.Message, ex.InnerException);
            }
        }

        #endregion

        #region Private Methods

        public PatientGoalData convertToPatientGoalData(PatientGoal pg)
        {
            PatientGoalData pgd = new PatientGoalData
            {
                Id = pg.Id,
                CustomAttributes = GetPatientGoalAttributes(pg.CustomAttributes),
                EndDate = pg.EndDate,
                FocusAreaIds = pg.FocusAreaIds,
                TemplateId = pg.TemplateId,
                Name = pg.Name,
                PatientId = pg.PatientId,
                ProgramIds = pg.ProgramIds,
                SourceId = pg.SourceId,
                StartDate = pg.StartDate,
                StatusId = pg.StatusId,
                TargetDate = pg.TargetDate,
                TargetValue = pg.TargetValue,
                TypeId = pg.TypeId
            };
            return pgd;
        }

        public List<CustomAttributeData> GetPatientGoalAttributes(List<AD.CustomAttribute> list)
        {
            List<CustomAttributeData> ad = new List<CustomAttributeData>();
            if (list != null)
            {
                list.ForEach(a =>
                {
                    ad.Add(new CustomAttributeData
                    {
                        Values = a.Values,
                        Id = a.Id
                    });
                });
            }
            return ad;
        }

        public List<string> GetTaskIdsForRequest(List<PatientTask> list)
        {
            List<string> taskIds = new List<string>();

            if (list != null && list.Count > 0)
            {
                list.ForEach(t =>
                {
                    taskIds.Add(t.Id);
                });
            }

            return taskIds;
        }

        public List<string> GetInterventionIdsForRequest(List<PatientIntervention> list)
        {
            List<string> taskIds = new List<string>();

            if (list != null && list.Count > 0)
            {
                list.ForEach(t =>
                {
                    taskIds.Add(t.Id);
                });
            }

            return taskIds;
        }

        public List<string> GetBarrierIdsForRequest(List<PatientBarrier> list)
        {
                List<string> barrierIds = new List<string>();

                if (list != null && list.Count > 0)
                {
                    list.ForEach(t =>
                    {
                        barrierIds.Add(t.Id);
                    });
                }

                return barrierIds;
        }

        public PatientDetails GetPatientDetails(double version, string contractNumber, string userId, IRestClient client, string patientId)
        {
            PatientDetails patient = null;
            if (!string.IsNullOrEmpty(patientId))
            {
                string patientUrl = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                    DDPatientServiceURL,
                                                                                    "NG",
                                                                                    version,
                                                                                    contractNumber,
                                                                                    patientId), userId);

                GetPatientDataResponse response = client.Get<GetPatientDataResponse>(patientUrl);

                if (response != null && response.Patient != null)
                {
                    patient = new PatientDetails
                    {
                        Id = response.Patient.Id,
                        FirstName = response.Patient.FirstName,
                        LastName = response.Patient.LastName,
                        MiddleName = response.Patient.MiddleName,
                        PreferredName = response.Patient.PreferredName,
                        Suffix = response.Patient.Suffix
                    };
                }
            }
            return patient;
        }
        #endregion
    }
}

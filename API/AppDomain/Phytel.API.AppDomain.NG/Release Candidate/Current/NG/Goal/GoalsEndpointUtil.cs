using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using AD = Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class GoalsEndpointUtil
    {
        #region Static Fields
        static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];
        static readonly string PhytelUserIDHeaderKey = "x-Phytel-UserID";
        #endregion

        #region Public Methods
        public static PatientGoalData GetInitialGoalRequest(GetInitializeGoalRequest request)
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

        public static string GetInitialBarrierRequest(GetInitializeBarrierRequest request)
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

        public static PatientTaskData GetInitialTaskRequest(GetInitializeTaskRequest request)
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

                PutInitializeTaskResponse response = client.Put<PutInitializeTaskResponse>(
                    url,
                    new PutInitializeTaskRequest() as object);

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

        public static PatientGoal GetPatientGoal(GetPatientGoalRequest request)
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
                    request.Id,
                    request.UserId), request.UserId);

                GetPatientGoalDataResponse ddResponse = client.Get<GetPatientGoalDataResponse>(
                    url);

                if (ddResponse != null && ddResponse.GoalData != null)
                {
                    //Make a call to AttributeLibrary to get attributes details for Goal and Task.
                    List<CustomAttribute> goalAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 1);
                    List<CustomAttribute> taskAttributesLibrary = GoalsEndpointUtil.GetAttributesLibraryByType(request, 2);
                    
                    PatientGoalData g = ddResponse.GoalData;
                    result = new PatientGoal
                    {
                        Id  = g.Id,
                        PatientId = g.PatientId, 
                        Name = g.Name, 
                        FocusAreaIds = g.FocusAreaIds,
                        SourceId = g.SourceId,
                        ProgramIds = g.ProgramIds,
                        TypeId = g.TypeId,
                        StatusId = g.StatusId, 
                        StartDate = g.StartDate, 
                        EndDate = g.EndDate,
                        TargetDate = g.TargetDate,
                        TargetValue = g.TargetValue,
                        CustomAttributes = GoalsUtil.GetCustomAttributeDetails(g.CustomAttributes, goalAttributesLibrary),
                        Barriers = GoalsUtil.GetBarriers(g.BarriersData),
                        Tasks = GoalsUtil.GetTasks(g.TasksData, taskAttributesLibrary),
                        Interventions = GoalsUtil.GetInterventions(g.InterventionsData)
                    };
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientGoal()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<PatientGoalView> GetAllPatientGoals(GetAllPatientGoalsRequest request)
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
                    request.PatientId,
                    request.UserId), request.UserId);

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
        #endregion

        #region Internal Methods

        internal static bool DeleteGoalRequest(PostDeletePatientGoalRequest request)
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
                    request.PatientGoalId,
                    request.UserId), request.UserId);

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

        internal static bool DeleteBarrierRequest(PostDeletePatientGoalRequest request, string id)
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
                   id,
                   request.UserId), request.UserId);

                DeleteBarrierResponse response = client.Delete<DeleteBarrierResponse>(
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

        internal static bool DeleteTaskRequest(PostDeletePatientGoalRequest request, string id)
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
                    id,
                    request.UserId), request.UserId);

                DeleteTaskResponse response = client.Delete<DeleteTaskResponse>(
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

        internal static bool DeleteInterventionRequest(PostDeletePatientGoalRequest request, string id)
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
                    id,
                    request.UserId), request.UserId);

                DeleteInterventionResponse response = client.Delete<DeleteInterventionResponse>(
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

        internal static List<CustomAttribute> GetAttributesLibraryByType(IAppDomainRequest request, int typeId)
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

        internal static bool PostUpdateInterventionRequest(PostPatientGoalRequest request, PatientInterventionData pi)
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

        internal static bool PostUpdateBarrierRequest(PostPatientGoalRequest request, PatientBarrierData bd)
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

        internal static bool PostUpdateTaskRequest(PostPatientGoalRequest request, PatientTaskData td)
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

        internal static string GetInitialInterventionRequest(GetInitializeInterventionRequest request)
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
                throw new WebServiceException("AD:GetInitialTaskRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static bool PostUpdateGoalRequest(PostPatientGoalRequest request)
        {
            try
            {
                bool result = false;

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

                PutPatientGoalDataResponse response = client.Put<PutPatientGoalDataResponse>(
                    url, new PutPatientGoalDataRequest { GoalData = GetGoalData(request.Goal), UserId = request.UserId } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateGoalRequest()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region Private Methods
        private static PatientGoalData GetGoalData(PatientGoal pg)
        {
            PatientGoalData pgd = new PatientGoalData
            {
                Id = pg.Id,
                CustomAttributes = GetPatientGoalAttributes(pg.CustomAttributes),
                EndDate = pg.EndDate,
                FocusAreaIds = pg.FocusAreaIds,
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

        private static List<CustomAttributeData> GetPatientGoalAttributes(List<AD.CustomAttribute> list)
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

        private static List<string> GetTaskIdsForRequest(List<PatientTask> list)
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

        private static List<string> GetInterventionIdsForRequest(List<PatientIntervention> list)
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

        private static List<string> GetBarrierIdsForRequest(List<PatientBarrier> list)
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
        #endregion
    }
}

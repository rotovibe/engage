using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD = Phytel.API.DataDomain.Program.DTO;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class GoalsEndpointUtil
    {
        static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];


        public static PatientGoalData GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            try
            {
                PatientGoalData result = null;
                IRestClient client = new JsonServiceClient();
                PutInitializeGoalDataResponse dataDomainResponse = client.Put<PutInitializeGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId),
                    new PutInitializeGoalDataRequest() as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Goal;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialGoalRequest()" + ex.Message, ex.InnerException);
            }
        }

        public static string GetInitialBarrierRequest(GetInitializeBarrierRequest request)
        {
            try
            {
                string result = string.Empty;
                //   [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Barrier/Initialize", "POST")]
                IRestClient client = new JsonServiceClient();
                PutInitializeBarrierDataResponse dataDomainResponse = client.Put<PutInitializeBarrierDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId),
                    new PutInitializeBarrierDataRequest() as object);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Id;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialBarrierRequest()" + ex.Message, ex.InnerException);
            }
        }

        public static PatientTaskData GetInitialTaskRequest(GetInitializeTaskRequest request)
        {
            try
            {
                PatientTaskData result = null;

                IRestClient client = new JsonServiceClient();
                PutInitializeTaskResponse response = client.Put<PutInitializeTaskResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId),
                    new PutInitializeTaskRequest() as object);

                if (response != null)
                {
                    result = response.Task;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialTaskRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static string GetInitialInterventionRequest(GetInitializeInterventionRequest request)
        {
            try
            {
                string result = string.Empty;

                IRestClient client = new JsonServiceClient();
                PutInitializeInterventionResponse response = client.Put<PutInitializeInterventionResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId),
                    new PutInitializeInterventionRequest() as object);

                if (response != null)
                {
                    result = response.Id;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitialTaskRequest()" + ex.Message, ex.InnerException);
            }
        }

        public static PatientGoal GetPatientGoal(GetPatientGoalRequest request)
        {
            try
            {
                PatientGoal result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                GetPatientGoalDataResponse ddResponse = client.Get<GetPatientGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}?UserId={6}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id,
                    request.UserId));

                if (ddResponse != null && ddResponse.GoalData != null)
                {
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
                        CustomAttributes = GoalsUtil.GetAttributes(g.CustomAttributes),
                        Barriers = GoalsUtil.GetBarriers(g.BarriersData),
                        Tasks = GoalsUtil.GetTasks(g.TasksData),
                        Interventions = GoalsUtil.GetInterventions(g.InterventionsData)
                    };
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientGoal()" + ex.Message, ex.InnerException);
            }
        }

        public static List<PatientGoalView> GetAllPatientGoals(GetAllPatientGoalsRequest request)
        {
            try
            {
                List<PatientGoalView> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goals", "GET")]
                IRestClient client = new JsonServiceClient();
                GetAllPatientGoalsDataResponse ddResponse = client.Get<GetAllPatientGoalsDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goals?UserId={5}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.UserId));

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
                throw new WebServiceException("AD:GetPatientGoal()" + ex.Message, ex.InnerException);
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
                PutPatientGoalDataResponse response = client.Put<PutPatientGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id), new PutPatientGoalDataRequest {  GoalData = GetGoalData(request.Goal)} as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateGoalRequest()" + ex.Message, ex.InnerException);
            }
        }

        private static PatientGoalData GetGoalData(PatientGoal pg)
        {
            try
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
            catch { throw; }
        }

        private static List<CustomAttributeData> GetPatientGoalAttributes(List<AD.CustomAttribute> list)
        {
            try
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
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static bool PostUpdateTaskRequest(PostPatientGoalRequest request, PatientTaskData td)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                PutUpdateTaskResponse response = client.Put<PutUpdateTaskResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                    td.Id), new PutUpdateTaskRequest {  Task = td } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateTaskRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool PostUpdateInterventionRequest(PostPatientGoalRequest request, PatientInterventionData pi)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                PutUpdateInterventionResponse response = client.Put<PutUpdateInterventionResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                    pi.Id), new PutUpdateInterventionRequest { Intervention = pi, UserId=request.UserId  } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateInterventionRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool PostUpdateBarrierRequest(PostPatientGoalRequest request, PatientBarrierData bd)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                PutUpdateBarrierResponse response = client.Put<PutUpdateBarrierResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Update",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Goal.Id,
                   bd.Id), new PutUpdateBarrierRequest { UserId = request.UserId, Barrier = bd } as object);

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:PostUpdateBarrierRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeleteGoalRequest(PostDeletePatientGoalRequest request)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                DeletePatientGoalDataResponse response = client.Delete<DeletePatientGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Delete/?UserId={6}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    request.UserId));

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteGoalRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeleteBarrierRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                DeleteBarrierResponse response = client.Delete<DeleteBarrierResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Barrier/{6}/Delete/?UserId={7}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                   id,
                   request.UserId));

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteBarrierRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeleteTaskRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                DeleteTaskResponse response = client.Delete<DeleteTaskResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Task/{6}/Delete/?UserId={7}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    id,
                    request.UserId));

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteTaskRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeleteInterventionRequest(PostDeletePatientGoalRequest request, string id)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                DeleteInterventionResponse response = client.Delete<DeleteInterventionResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Intervention/{6}/Delete/?UserId={7}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientGoalId,
                    id,
                    request.UserId));

                if (response != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:DeleteInterventionRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static List<AD.CustomAttribute> GetAttributesForInitialize(IAppDomainRequest request, int typeId)
        {
            List<AD.CustomAttribute> attr = new List<AD.CustomAttribute>();
            try
            {
                IRestClient client = new JsonServiceClient();
                GetCustomAttributesDataResponse response = client.Get<GetCustomAttributesDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Attributes/{4}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    typeId));

                response.CustomAttributes.ForEach(ca =>
                {
                    attr.Add(new AD.CustomAttribute
                    {
                        Id = ca.Id,
                        Name = ca.Name,
                        ControlType = ca.ControlType,
                        Order = ca.Order,
                        Options = GoalsUtil.ConvertToOptionList(ca.Options)
                    });
                });

                return attr;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAttributesForInitialize()" + ex.Message, ex.InnerException);
            }
        }
    }
}

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


        public static string GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            try
            {
                string result = string.Empty;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/Initialize", "PUT")]
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
                    result = dataDomainResponse.Id;
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

        public static string GetInitialTaskRequest(GetInitializeTaskRequest request)
        {
            try
            {
                string result = string.Empty;

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
                    result = response.Id;
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
                PatientGoal result = new PatientGoal();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                GetPatientGoalDataResponse response = client.Get<GetPatientGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id));

                if (response != null)
                {
                   // result = response.Id;
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
                List<PatientGoalView> result = new List<PatientGoalView>();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goals", "GET")]
                IRestClient client = new JsonServiceClient();
                GetAllPatientGoalsDataResponse response = client.Get<GetAllPatientGoalsDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goals",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId));

                if (response != null)
                {
                  //  result = response.Id;
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
                    Attributes = GetPatientGoalAttributes(pg.Attributes),
                    EndDate = pg.EndDate,
                    FocusAreaIds = pg.FocusAreaIds,
                    Name = pg.Name,
                    PatientId = pg.PatientId,
                    Programs = pg.ProgramIds,
                    Source = pg.SourceId,
                    StartDate = pg.StartDate,
                    StatusId = pg.StatusId,
                    TargetDate = pg.TargetDate,
                    TargetValue = pg.TargetValue,
                    Type = pg.Type
                };
                return pgd;
            }
            catch { throw; }
        }

        private static List<AttributeData> GetPatientGoalAttributes(List<AD.Attribute> list)
        {
            try
            {
                List<AttributeData> ad = new List<AttributeData>();
                if (list != null)
                {
                    list.ForEach(a =>
                    {
                        ad.Add(new AttributeData
                        {
                            ControlType = a.ControlType,
                            Name = a.Name,
                            Order = a.Order,
                            Value = a.Value
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
                    pi.Id), new PutUpdateInterventionRequest { Intervention = pi  } as object);

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
                    request.Goal.Id,
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
                    request.Goal.Id,
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
                    request.Goal.Id,
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
    }
}

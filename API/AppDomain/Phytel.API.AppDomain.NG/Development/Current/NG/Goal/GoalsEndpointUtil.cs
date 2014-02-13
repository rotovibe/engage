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


        public static string PostInitialGoalRequest(PostInitializeGoalRequest request)
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

        public static string PostInitialBarrierRequest(PostInitializeBarrierRequest request)
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
    }
}

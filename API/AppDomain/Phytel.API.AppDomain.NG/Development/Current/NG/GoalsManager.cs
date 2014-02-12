using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class GoalsManager : ManagerBase
    {

        static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];

        public string PostInitialGoalRequest(PostInitializeGoalRequest request)
        {
            try
            {
                string result = string.Empty;

                IRestClient client = new JsonServiceClient();
                PutInitializeGoalDataResponse response = client.Put<PutInitializeGoalDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/Initialize",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId),
                    new PutInitializeGoalDataRequest() as object);

                if (response != null)
                {
                    result = response.Id;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:PostInitialGoalRequest()" + ex.Message, ex.InnerException);
            }
        }
       
        
        public GetInitializeTaskResponse GetInitialTask(GetInitializeTaskRequest request)
        {
            try
            {
                GetInitializeTaskResponse itr = new GetInitializeTaskResponse();
                string id = GoalsEndpointUtil.GetInitialTaskRequest(request);
                itr.Id = id;
                return itr;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

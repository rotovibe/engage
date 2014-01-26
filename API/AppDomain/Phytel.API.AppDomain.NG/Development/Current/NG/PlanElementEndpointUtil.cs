using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD = Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class PlanElementEndpointUtil
    {
        static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public static bool DoesPatientProblemExist(string probId, PlanElementEventArg e)
        {
            try
            {
                bool result = false;

                IRestClient client = new JsonServiceClient();
                PutNewPatientProblemResponse dataDomainResponse =
                   client.Get<PutNewPatientProblemResponse>(
                   string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}",
                   DDPatientProblemServiceUrl,
                   "NG",
                   "v1",
                   "InHealth001",
                   e.PatientId,
                   probId));

                if (dataDomainResponse.PatientProblem != null)
                {
                    result = true;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:CheckIfPatientProblemExistsForPatient()" + ex.Message, ex.InnerException);
            }
        }

        public static DD.ProgramDetail SaveAction(PostProcessActionRequest request, Program p)
        {
            try
            {
                DD.ProgramDetail pD = NGUtils.FormatProgramDetail(p);

                IRestClient client = new JsonServiceClient();
                DD.PutProgramActionProcessingResponse response = client.Put<DD.PutProgramActionProcessingResponse>(
                    string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ProgramId,
                    request.Token), new DD.PutProgramActionProcessingRequest { Program = pD, UserId = request.UserId });

                return response.program;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public static Program RequestPatientProgramDetail(PostProcessActionRequest request)
        {
            Program pd = null;
            IRestClient client = new JsonServiceClient();
            DD.GetProgramDetailsSummaryResponse resp =
                client.Get<DD.GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",
                DDProgramServiceUrl,
                "NG",
                request.Version,
                request.ContractNumber,
                request.PatientId,
                request.ProgramId,
                request.Token));

            pd = NGUtils.FormatProgramDetail(resp.Program);

            return pd;
        }
    }
}

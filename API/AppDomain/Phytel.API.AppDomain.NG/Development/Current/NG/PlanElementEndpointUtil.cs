using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
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
        static readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public static PatientProblemData GetPatientProblem(string probId, PlanElementEventArg e)
        {
            try
            {
                PatientProblemData result = null;

                IRestClient client = new JsonServiceClient();
                GetPatientProblemsDataResponse dataDomainResponse =
                   client.Get<GetPatientProblemsDataResponse>(
                   string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}",
                   DDPatientProblemServiceUrl,
                   "NG",
                   "v1",
                   "InHealth001",
                   e.PatientId,
                   probId));

                if (dataDomainResponse.PatientProblem != null)
                {
                    result = dataDomainResponse.PatientProblem;
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

        internal static CohortPatientViewData RequestCohortPatientViewData(string patientId)
        {
            try
            {
                string version = "v1";
                string contractNumber = "InHealth001";
                string context = "NG";

                IRestClient client = new JsonServiceClient();
                GetCohortPatientViewResponse response =
                    client.Get<GetCohortPatientViewResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                    DDPatientServiceUrl,
                    context,
                    version,
                    contractNumber,
                    patientId));

                return response.CohortPatientView;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        internal static void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId)
        {
            try
            {
                string version = "v1";
                string contractNumber = "InHealth001";
                string context = "NG";

                IRestClient client = new JsonServiceClient();
                PutProblemInCohortPatientViewResponse response =
                    client.Put<PutProblemInCohortPatientViewResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/update",
                    DDPatientServiceUrl,
                    context,
                    version,
                    contractNumber,
                    patientId), new PutProblemInCohortPatientViewRequest
                    {
                        CohortPatientView = cpvd,
                        ContractNumber = contractNumber,
                        PatientID = patientId
                    } as object);
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        internal static PutNewPatientProblemResponse PutNewPatientProblem(string patientId, string userId, string elementId)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();
                PutNewPatientProblemResponse dataDomainResponse =
                   client.Put<PutNewPatientProblemResponse>(
                   string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Insert",
                   DDPatientProblemServiceUrl,
                   "NG",
                   "v1",
                   "InHealth001",
                   patientId), new PutNewPatientProblemRequest
                   {
                       ProblemId = elementId,
                       Active = true,
                       Featured = true,
                       UserId = userId,
                       Level = 1
                   } as object);
                return dataDomainResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        internal static PutUpdatePatientProblemResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientProblemData ppd, bool _active)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();
                PutUpdatePatientProblemResponse dataDomainResponse =
                   client.Put<PutUpdatePatientProblemResponse>(
                   string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Update/",
                   DDPatientProblemServiceUrl,
                   "NG",
                   "v1",
                   "InHealth001",
                   patientId), new PutUpdatePatientProblemRequest
                   {
                       Id = ppd.ID,
                       ProblemId = elementId,
                       Active = _active,
                       Featured = true,
                       UserId = userId,
                       Level = 1
                   } as object);

                return dataDomainResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }
    }
}

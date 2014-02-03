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

        public static DD.ProgramDetail SaveAction(PostProcessActionRequest request, string actionId, AD.Program p)
        {
            try
            {
                // save responses from action steps
                SaveResponsesFromProgram(p, actionId, request);

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

        private static void SaveResponsesFromProgram(AD.Program p, string actionId, IAppDomainRequest request)
        {
            try
            {
                Actions act = GetActionById(p, actionId);
                act.Steps.ForEach(s =>
                {
                    if (SaveResponses(s.Responses, request))
                    {
                        s.Responses = null;
                    }
                });
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private static bool SaveResponses(List<Response> list, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                if (list != null)
                {
                    list.ForEach(r =>
                    {
                        if (ResponseExistsRequest(r.StepId, r.Id, request))
                        {
                            UpdateResponseRequest(request, r);
                            result = true;
                        }
                    });
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private static bool ResponseExistsRequest(string stepId, string responseId, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                IRestClient client = new JsonServiceClient();
                GetStepResponseResponse resp =
                                    client.Get<GetStepResponseResponse>(
                                    string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Response/?ResponseId={5}",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    stepId,
                                    responseId));

                if (resp.StepResponse != null)
                    result = true;

                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private static void UpdateResponseRequest(IAppDomainRequest request, Response r)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                DD.PutUpdateResponseResponse resp =
                    client.Put<DD.PutUpdateResponseResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Responses/Update",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    r.StepId), new DD.PutUpdateResponseRequest
                    {
                        ResponseDetail = new DD.ResponseDetail
                        {
                            Id = r.Id,
                            NextStepId = r.NextStepId,
                            Nominal = r.Nominal,
                            Order = r.Order,
                            Required = r.Required,
                            SpawnElement = NGUtils.GetDDSpawnElement(r.SpawnElement),
                            StepId = r.StepId,
                            Text = r.Text,
                            Value = r.Value
                        },
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private static Actions GetActionById(AD.Program p, string actionId)
        {
            Actions action = null;
            try
            {
                action = p.Modules.SelectMany(module => module.Actions).Where(act => act.Id == actionId).FirstOrDefault();
                return action;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetActionById():" + ex.Message, ex.InnerException);
            }
        }

        public static AD.Program RequestPatientProgramDetail(PostProcessActionRequest request)
        {
            AD.Program pd = null;
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

        internal static ProgramAttribute GetProgramAttributes(string planElemId)
        {
            ProgramAttribute progAttr = null;
            try
            {
                string version = "v1";
                string contractNumber = "InHealth001";
                string context = "NG";


                IRestClient client = new JsonServiceClient();
                GetProgramAttributeResponse resp =
                                    client.Get<GetProgramAttributeResponse>(
                                    string.Format("{0}/{1}/{2}/{3}/Program/Attributes/?PlanElementId={4}",
                                    DDProgramServiceUrl,
                                    context,
                                    version,
                                    contractNumber,
                                    planElemId));

                return progAttr = resp.ProgramAttribute;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        internal static bool UpdateProgramAttributes(ProgramAttribute pAtt)
        {
            bool result = false;
            try
            {
                string version = "v1";
                string contractNumber = "InHealth001";
                string context = "NG";


                IRestClient client = new JsonServiceClient();
                PutUpdateProgramAttributesResponse resp =
                                    client.Put<PutUpdateProgramAttributesResponse>(
                                    string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Update/",
                                    DDProgramServiceUrl,
                                    context,
                                    version,
                                    contractNumber,
                                    pAtt.PlanElementId), new PutUpdateProgramAttributesRequest
                                    {
                                        ProgramAttributes = pAtt
                                    } as object);

                if (resp.Result)
                    result = true;

                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        internal static bool InsertNewProgramAttribute(ProgramAttribute pa)
        {
            bool result = false;
            try
            {
                string version = "v1";
                string contractNumber = "InHealth001";
                string context = "NG";


                IRestClient client = new JsonServiceClient();
                PutProgramAttributesResponse resp =
                                    client.Put<PutProgramAttributesResponse>(
                                    string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Insert/",
                                    DDProgramServiceUrl,
                                    context,
                                    version,
                                    contractNumber,
                                    pa.PlanElementId), new PutProgramAttributesRequest
                                    {
                                        ProgramAttributes = pa
                                    } as object);

                if (resp.Result)
                    result = true;

                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }
    }
}

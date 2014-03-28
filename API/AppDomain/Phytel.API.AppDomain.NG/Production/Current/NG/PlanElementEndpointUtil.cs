﻿using Phytel.API.AppDomain.NG.DTO;
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
using AD = Phytel.API.AppDomain.NG.DTO;
using DD = Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class PlanElementEndpointUtil
    {
        static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        static readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public static PatientProblemData GetPatientProblem(string probId, PlanElementEventArg e, string userId)
        {
            try
            {
                PatientProblemData result = null;

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}",
                                   DDPatientProblemServiceUrl,
                                   "NG",
                                   e.DomainRequest.Version,
                                   e.DomainRequest.ContractNumber,
                                   e.PatientId,
                                   probId), userId);

                GetPatientProblemsDataResponse dataDomainResponse =
                   client.Get<GetPatientProblemsDataResponse>(
                   url);

                if (dataDomainResponse.PatientProblem != null)
                {
                    result = dataDomainResponse.PatientProblem;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetPatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        public static DD.ProgramDetail SaveAction(IProcessActionRequest request, string actionId, AD.Program p)
        {
            try
            {
                // save responses from action steps
                SaveResponsesFromProgram(p, actionId, request);

                DD.ProgramDetail pD = NGUtils.FormatProgramDetail(p);

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.ProgramId,
                                    request.Token), request.UserId);

                DD.PutProgramActionProcessingResponse response = client.Put<DD.PutProgramActionProcessingResponse>(
                    url, new DD.PutProgramActionProcessingRequest { Program = pD, UserId = request.UserId });

                return response.program;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveAction()::" + ex.Message, ex.InnerException);
            }
        }

        private static void SaveResponsesFromProgram(AD.Program p, string actionId, IProcessActionRequest request)
        {
            try
            {
                Actions act = GetActionById(p, actionId);
                act.Steps.ForEach(s =>
                {
                    // add stepresponse ids and step source id
                    if (SaveResponses(s, request))
                    {
                        s.Responses = null;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveResponsesFromProgram()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool SaveResponses(Step step, IProcessActionRequest request)
        {
            bool result = false;
            List<Response> list = step.Responses;
            try
            {
                if (list != null)
                {
                    list.ForEach(r =>
                    {
                        if (ResponseExistsRequest(r.StepId, r.Id, request))
                        {
                            SetSelectedResponseProperty(step, r);
                            SetDeleteFlagByStepCompletion(step, r);
                            UpdateResponseRequest(request, r);
                            result = true;
                        }
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveResponses()::" + ex.Message, ex.InnerException);
            }
        }

        private static void SetSelectedResponseProperty(Step step, Response r)
        {
            try
            {
                r.Selected = false;
                if (step.SelectedResponseId.Equals(r.Id))
                {
                    if ((step.StepTypeId.Equals(1)) || (step.StepTypeId.Equals(4)))
                    {
                        r.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SetSelectedResponseProperty()::" + ex.Message, ex.InnerException);
            }
        }

        private static void SetDeleteFlagByStepCompletion(Step step, Response r)
        {
            try
            {
                if (step != null && r != null)
                {
                    if (step.Completed)
                        r.Delete = false;
                    else
                        r.Delete = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SetDeleteFlagByStepCompletion()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<StepResponse> GetResponsesForStep(string stepId, IAppDomainRequest request)
        {
            List<StepResponse> result = null;
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Responses/",
                                                    DDProgramServiceUrl,
                                                    "NG",
                                                    request.Version,
                                                    request.ContractNumber,
                                                    stepId), request.UserId);

                GetStepResponseListResponse resp =
                                    client.Get<GetStepResponseListResponse>(
                                    url);

                if (resp != null)
                {
                    result = resp.StepResponseList;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetResponsesForStep()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool ResponseExistsRequest(string stepId, string responseId, IProcessActionRequest request)
        {
            bool result = false;
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Response/?ResponseId={5}",
                                                    DDProgramServiceUrl,
                                                    "NG",
                                                    request.Version,
                                                    request.ContractNumber,
                                                    stepId,
                                                    responseId), request.UserId);

                GetStepResponseResponse resp =
                                    client.Get<GetStepResponseResponse>(
                                    url);

                if (resp.StepResponse != null)
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:ResponseExistsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        private static void UpdateResponseRequest(IProcessActionRequest request, Response r)
        {
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Responses/Update",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    r.StepId), request.UserId);

                DD.PutUpdateResponseResponse resp =
                    client.Put<DD.PutUpdateResponseResponse>(
                    url, new DD.PutUpdateResponseRequest
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
                            Value = r.Value,
                            Selected = r.Selected,
                            Delete = r.Delete
                        },
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdateResponseRequest()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:PlanElementEndpointUtil:GetActionById()::" + ex.Message, ex.InnerException);
            }
        }

        public static AD.Program RequestPatientProgramDetail(IProcessActionRequest request)
        {
            try
            {
                AD.Program pd = null;
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.ProgramId,
                                    request.Token), request.UserId);

                DD.GetProgramDetailsSummaryResponse resp =
                    client.Get<DD.GetProgramDetailsSummaryResponse>(
                    url);

                pd = NGUtils.FormatProgramDetail(resp.Program);

                return pd;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:RequestPatientProgramDetail()::" + ex.Message, ex.InnerException);
            }
        }

        internal static CohortPatientViewData RequestCohortPatientViewData(string patientId, IAppDomainRequest request)
        {
            try
            {
                double version = request.Version;
                string contractNumber = request.ContractNumber;
                string context = "NG";

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                                    DDPatientServiceUrl,
                                    context,
                                    version,
                                    contractNumber,
                                    patientId), request.UserId);

                GetCohortPatientViewResponse response =
                    client.Get<GetCohortPatientViewResponse>(url);

                return response.CohortPatientView;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:RequestCohortPatientViewData()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId, IAppDomainRequest request)
        {
            try
            {
                double version = request.Version;
                string contractNumber = request.ContractNumber;
                string context = "NG";

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/update",
                                    DDPatientServiceUrl,
                                    context,
                                    version,
                                    contractNumber,
                                    patientId), request.UserId);

                PutUpdateCohortPatientViewResponse response =
                    client.Put<PutUpdateCohortPatientViewResponse>(url, new PutUpdateCohortPatientViewRequest
                    {
                        CohortPatientView = cpvd,
                        ContractNumber = contractNumber,
                        PatientID = patientId
                    } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdateCohortPatientViewProblem()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PutNewPatientProblemResponse PutNewPatientProblem(string patientId, string userId, string elementId, IAppDomainRequest request)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Insert",
                                   DDPatientProblemServiceUrl,
                                   "NG",
                                   request.Version,
                                   request.ContractNumber,
                                   patientId), request.UserId);

                PutNewPatientProblemResponse dataDomainResponse =
                   client.Put<PutNewPatientProblemResponse>(
                   url, new PutNewPatientProblemRequest
                   {
                       ProblemId = elementId,
                       Active = true,
                       Featured = true,
                       UserId = userId,
                       Level = 1
                   } as object);
                return dataDomainResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:PutNewPatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PutUpdatePatientProblemResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientProblemData ppd, bool _active, IAppDomainRequest request)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Update/",
                                   DDPatientProblemServiceUrl,
                                   "NG",
                                   request.Version,
                                   request.ContractNumber,
                                   patientId), request.UserId);

                PutUpdatePatientProblemResponse dataDomainResponse =
                   client.Put<PutUpdatePatientProblemResponse>(
                   url, new PutUpdatePatientProblemRequest
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
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdatePatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        internal static ProgramAttribute GetProgramAttributes(string planElemId, IAppDomainRequest request)
        {
            ProgramAttribute progAttr = null;
            try
            {
                double version = request.Version;
                string contractNumber = request.ContractNumber;
                string context = "NG";

                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Attributes/?PlanElementId={4}",
                                                    DDProgramServiceUrl,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    planElemId), request.UserId);

                GetProgramAttributeResponse resp =
                                    client.Get<GetProgramAttributeResponse>(
                                    url);

                return progAttr = resp.ProgramAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        internal static bool UpdateProgramAttributes(ProgramAttribute pAtt, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                double version = request.Version;
                string contractNumber = request.ContractNumber;
                string context = "NG";


                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Update/",
                                                    DDProgramServiceUrl,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    pAtt.PlanElementId), request.UserId);

                PutUpdateProgramAttributesResponse resp =
                                    client.Put<PutUpdateProgramAttributesResponse>(
                                    url, new PutUpdateProgramAttributesRequest
                                    {
                                        ProgramAttributes = pAtt
                                    } as object);

                if (resp.Result)
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdateProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        internal static bool InsertNewProgramAttribute(ProgramAttribute pa, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                double version = request.Version;
                string contractNumber = request.ContractNumber;
                string context = "NG";
                
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Insert/",
                                                    DDProgramServiceUrl,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    pa.PlanElementId), request.UserId);

                PutProgramAttributesResponse resp =
                                    client.Put<PutProgramAttributesResponse>(
                                    url, new PutProgramAttributesRequest
                                    {
                                        ProgramAttributes = pa
                                    } as object);

                if (resp.Result)
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:InsertNewProgramAttribute()::" + ex.Message, ex.InnerException);
            }
        }
    }
}

using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.DTO.Scheduling;
using Phytel.API.AppDomain.NG.PlanCOR;
//using Phytel.API.AppDomain.NG.Program;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
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
    public class EndpointUtils : IEndpointUtils
    {
        static readonly string DDPatientObservationServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];
        static readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];
        static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];
        static readonly string DDSchedulingUrl = ConfigurationManager.AppSettings["DDSchedulingUrl"];
        static readonly string DDPatientGoalsServiceUrl = ConfigurationManager.AppSettings["DDPatientGoalUrl"];

        public PatientObservation GetPatientProblem(string probId, PlanElementEventArg e, string userId)
        {
            try
            {
                PatientObservation result = null;

                IRestClient client = new JsonServiceClient();

                //Patient/{PatientId}/Observation/{ObservationID}
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/{5}",
                                   DDPatientObservationServiceUrl,
                                   "NG",
                                   e.DomainRequest.Version,
                                   e.DomainRequest.ContractNumber,
                                   e.PatientId,
                                   probId), userId);

                GetPatientObservationResponse dataDomainResponse =
                   client.Get<GetPatientObservationResponse>(
                   url);

                if (dataDomainResponse.PatientObservation != null)
                {
                    result = new PatientObservation
                    {
                        Id = dataDomainResponse.PatientObservation.Id,
                        StateId = dataDomainResponse.PatientObservation.StateId
                    };
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetPatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        public ProgramDetail SaveAction(IProcessActionRequest request, string actionId, AD.Program p, bool repeat)
        {
            try
            {
                // save responses from action steps
                SaveResponsesFromProgramAction(p, actionId, request, repeat);

                var pD = NGUtils.FormatProgramDetail(p);

                var client = new JsonServiceClient();

                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.ProgramId,
                                    request.Token), request.UserId);

                var response = client.Put<DD.PutProgramActionProcessingResponse>(
                    url, new PutProgramActionProcessingRequest { Program = pD, UserId = request.UserId });

                return response.program;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveAction()::" + ex.Message, ex.InnerException);
            }
        }

        public List<StepResponse> GetResponsesForStep(string stepId, IAppDomainRequest request)
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

        public AD.Program RequestPatientProgramDetail(IProcessActionRequest request)
        {
            try
            {
                AD.Program pd = null;
                IRestClient client = new JsonServiceClient();

                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",              
                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.ProgramId,
                                    request.Token), request.UserId);

                GetProgramDetailsSummaryResponse resp = client.Get<GetProgramDetailsSummaryResponse>(url);
                pd = NGUtils.FormatProgramDetail(resp.Program);

                return pd;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:RequestPatientProgramDetail()::" + ex.Message, ex.InnerException);
            }
        }

        public CohortPatientViewData RequestCohortPatientViewData(string patientId, IAppDomainRequest request)
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

        public void UpdateCohortPatientViewProblem(CohortPatientViewData cpvd, string patientId, IAppDomainRequest request)
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

        public PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, IAppDomainRequest request)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Insert",
                                   DDPatientObservationServiceUrl,
                                   "NG",
                                   request.Version,
                                   request.ContractNumber,
                                   patientId), request.UserId);

                PutRegisterPatientObservationResponse dataDomainResponse =
                   client.Put<PutRegisterPatientObservationResponse>(
                   url, new PutRegisterPatientObservationRequest
                   {
                       Id = elementId,
                       StateId = 2,
                       DisplayId = 1,
                       UserId = userId
                   } as object);
                return dataDomainResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:PutNewPatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        public PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientObservation pod, bool _active, IAppDomainRequest request)
        {
            try
            {
                //register call to remote serivce.
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Update",
                                   DDPatientObservationServiceUrl,
                                   "NG",
                                   request.Version,
                                   request.ContractNumber,
                                   patientId), request.UserId);

                PatientObservationRecordData pdata = new PatientObservationRecordData
                {
                    Id = pod.Id,
                    StateId = pod.StateId,
                    DeleteFlag = false
                };

                PutUpdateObservationDataRequest purequest = new PutUpdateObservationDataRequest
                {
                    PatientObservationData = pdata,
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                };

                PutUpdateObservationDataResponse dataDomainResponse =
                   client.Put<PutUpdateObservationDataResponse>(
                   url, purequest as object);

                return dataDomainResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdatePatientProblem()::" + ex.Message, ex.InnerException);
            }
        }

        public ProgramAttributeData GetProgramAttributes(string planElemId, IAppDomainRequest request)
        {
            ProgramAttributeData progAttr = null;
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

        public bool UpdateProgramAttributes(ProgramAttributeData pAtt, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                double version        = request.Version;
                string contractNumber = request.ContractNumber;
                string context        = "NG";


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

        public bool InsertNewProgramAttribute(ProgramAttributeData pa, IAppDomainRequest request)
        {
            bool result = false;
            try
            {
                double version        = request.Version;
                string contractNumber = request.ContractNumber;
                string context        = "NG";

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

        public DD.GetProgramDetailsSummaryResponse RequestPatientProgramDetailsSummary(GetPatientProgramDetailsSummaryRequest request)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientProgramId), request.UserId);

                DD.GetProgramDetailsSummaryResponse resp =
                    client.Get<DD.GetProgramDetailsSummaryResponse>(url);
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:RequestPatientProgramDetailsSummary()::" + ex.Message, ex.InnerException);
            }
        }

        #region private methods
        private void SaveResponsesFromProgramAction(AD.Program p, string actionId, IProcessActionRequest request, bool repeat)
        {
            try
            {
                Actions act = GetActionById(p, actionId);
                SaveResponses(act, request, repeat);
                // clear response collections
                foreach (Step stp in act.Steps) { stp.Responses = null; }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveResponsesFromProgramAction()::" + ex.Message, ex.InnerException);
            }
        }

        private void InsertResponseRequest(IProcessActionRequest request, List<Response> r)
        {
            try
            {
                IRestClient client = new JsonServiceClient();

                string url =
                    Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/Responses/Insert",
                        DDProgramServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber), request.UserId);

                DD.PutInsertResponseResponse resp =
                    client.Put<DD.PutInsertResponseResponse>(
                        url, new DD.PutInsertResponseRequest
                        {
                            ResponseDetails = FormatResponseDetails(r),
                            UserId = request.UserId,
                            Version = request.Version
                        } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:InsertResponseRequest()::" + ex.Message,
                    ex.InnerException);
            }
        }

        private void UpdateResponseRequest(IProcessActionRequest request, List<Response> r)
        {
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/Responses/Update",
                                    DDProgramServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                DD.PutUpdateResponseResponse resp =
                    client.Put<DD.PutUpdateResponseResponse>(
                    url, new DD.PutUpdateResponseRequest
                    {
                        ResponseDetails = FormatResponseDetails(r),
                        UserId = request.UserId,
                        Version = request.Version
                    } as object);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:UpdateResponseRequest()::" + ex.Message, ex.InnerException);
            }
        }

        private List<ResponseDetail> FormatResponseDetails(List<Response> rsps)
        {
            try
            {
                var rs = new List<ResponseDetail>();

                rsps.ForEach(r =>
                {
                    var rd = new DD.ResponseDetail
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
                    };
                    rs.Add(rd);
                });
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:FormatResponseDetails()::" + ex.Message,
                    ex.InnerException);
            }
        }

        private Actions GetActionById(AD.Program p, string actionId)
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

        public void SaveResponses(Actions action, IProcessActionRequest request, bool repeat)
        {
            try
            {
                var rlist = new List<Response>();
                action.Steps.ForEach(step =>
                {
                    var sResp = step.Responses;
                    if (sResp != null)
                    {
                        sResp.ForEach(r =>
                        {
                            SetSelectedResponseProperty(step, r);
                            SetDeleteFlagByStepCompletion(step, r);
                            rlist.Add(r);
                        });
                    }
                });

                if (repeat)
                    InsertResponseRequest(request, rlist);
                else
                    UpdateResponseRequest(request, rlist);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveResponses()::" + ex.Message, ex.InnerException);
            }
        }

        private void SetSelectedResponseProperty(Step step, Response r)
        {
            try
            {
                r.Selected = false;
                if (step.SelectedResponseId.Equals(r.Id))
                {
                    if ((step.StepTypeId == 1) || (step.StepTypeId == 4 ))
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

        private void SetDeleteFlagByStepCompletion(Step step, Response r)
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
        #endregion

        public PutProgramToPatientResponse AssignPatientToProgram(PostPatientToProgramsRequest request, string primaryCM)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
                                                        DDProgramServiceUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId,
                                                        request.ContractProgramId), request.UserId);

                DD.PutProgramToPatientResponse dataDomainResponse =
                    client.Put<DD.PutProgramToPatientResponse>(url, new DD.PutProgramToPatientRequest
                    {
                        UserId = request.UserId,
                        CareManagerId = !string.IsNullOrEmpty(primaryCM) ? primaryCM : null
                    } as object);

                return dataDomainResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:RequestPatientProgramDetailsSummary()::" + ex.Message, ex.InnerException);
            }
        }

        public string GetPrimaryCareManagerForPatient(PostPatientToProgramsRequest request)
        {
            try
            {
                string pcmId = null;
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PrimaryCareManager",
                                                        DDCareMemberUrl,
                                                        "NG",
                                                        request.Version,
                                                        request.ContractNumber,
                                                        request.PatientId), request.UserId);

                GetPrimaryCareManagerDataResponse dataDomainResponse =
                    client.Get<GetPrimaryCareManagerDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    if (dataDomainResponse.CareMember != null)
                    {
                        pcmId = dataDomainResponse.CareMember.ContactId;
                    }
                }

                return pcmId;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetPrimaryCareManagerForPatient()::" + ex.Message, ex.InnerException);
            }
        }

        public AD.Outcome SaveProgramAttributeChanges(
            PostProgramAttributesChangeRequest request, ProgramDetail pD)
        {
            try
            {
                PostProgramAttributesChangeResponse sResponse = new PostProgramAttributesChangeResponse();

                IRestClient client = new JsonServiceClient();

                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ProgramId,
                    request.Token), request.UserId);

                PutProgramActionProcessingResponse response = client.Put<PutProgramActionProcessingResponse>(
                    url, new PutProgramActionProcessingRequest {Program = pD, UserId = request.UserId});

                if (response != null)
                {
                    sResponse.Outcome = new AD.Outcome
                    {
                        Reason = "Success",
                        Result = 1
                    };
                }

                return sResponse.Outcome;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:SaveProgramAttributeChanges()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public Goal GetGoalById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetGoalDataRequest();

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Goal/{Id}"
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/{4}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetGoalDataResponse>(url);
                if (response == null) throw new Exception("Goal template was not found or initialized.");
                var goal = Mapper.Map<Goal>(response.GoalData);
                goal.StatusId = response.GoalData.StatusId;

                return goal;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:EndpointUtils:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public Task GetTaskById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                IRestClient client = new JsonServiceClient();
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Tasks?Id={4}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetTaskDataResponse>(url);
                if (response == null) throw new Exception("Task template was not found or initialized.");
                var task = Mapper.Map<Task>(response.TaskData);

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetTaskById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public Intervention GetInterventionById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Goal/Interventions?Id={Id}"
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Goal/Interventions?Id={4}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetInterventionDataResponse>(url);
                if (response == null) throw new Exception("Intervention template was not found or initialized.");
                var intervention = Mapper.Map<Intervention>(response.InterventionsData);

                return intervention;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetInterventionById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public PatientGoal GetOpenNotMetPatientGoalByTemplateId(string gid, string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                PatientGoal goal = null;

                IRestClient client = new JsonServiceClient();

                //"/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/"
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/?TemplateId={5}",
                    DDPatientGoalsServiceUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    patientId,
                    gid), userId);

                var response = client.Get<GetPatientGoalByTemplateIdResponse>(url);

                if (response.GoalData != null)
                    goal = Mapper.Map<PatientGoal>(response.GoalData);

                return goal;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetOpenNotMetPatientGoalByTemplateId()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public PatientTask GetOpenNotMetPatientTaskByTemplateId(string taskid, string tempId, string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetPatientTaskByTemplateIdRequest();
                PatientTask task = null;

                IRestClient client = new JsonServiceClient();

                var url =
                    Common.Helper.BuildURL(
                        string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/Tasks?TemplateId={5}&GoalId={6}",
                            DDPatientGoalsServiceUrl,
                            "NG",
                            req.Version,
                            req.ContractNumber,
                            patientId,
                            tempId,
                            taskid), userId);

                var response = client.Get<GetPatientTaskByTemplateIdResponse>(url);
                if (response.TaskData != null)
                    task = Mapper.Map<PatientTask>(response.TaskData);

                return task;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetGoalById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public PatientIntervention GetOpenNotMetPatientInterventionByTemplateId(string goalid, string tempId, string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetPatientInterventionByTemplateIdRequest();
                PatientIntervention intervention = null;

                IRestClient client = new JsonServiceClient();

                var url =
                    Common.Helper.BuildURL(
                        string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Goal/Interventions?TemplateId={5}&GoalId={6}",
                            DDPatientGoalsServiceUrl,
                            "NG",
                            req.Version,
                            req.ContractNumber,
                            patientId,
                            tempId,
                            goalid), userId);

                var response = client.Get<GetPatientInterventionByTemplateIdResponse>(url);
                if (response.InterventionData != null)
                    intervention = Mapper.Map<PatientIntervention>(response.InterventionData);

                return intervention;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetOpenNotMetPatientInterventionByTemplateId()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public Schedule GetScheduleToDoById(string sid, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetScheduleDataRequest(); 

                IRestClient client = new JsonServiceClient();

                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Scheduling/Schedule/{4}",
                    DDSchedulingUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber,
                    sid), userId);

                var response = client.Get<GetScheduleDataResponse>(url);

                if (response == null) throw new Exception("Schedule template was not found or initialized.");

                var schedule = new Schedule
                {
                    AssignedToId = response.Schedule.AssignedToId,
                    CategoryId = response.Schedule.CategoryId,
                    ClosedDate = response.Schedule.ClosedDate,
                    CreatedById = response.Schedule.CreatedById,
                    CreatedOn = response.Schedule.CreatedOn,
                    Description = response.Schedule.Description,
                    DueDate = response.Schedule.DueDate,
                    StartTime = response.Schedule.StartTime,
                    Duration = response.Schedule.Duration,
                    DueDateRange = response.Schedule.DueDateRange,
                    Id = response.Schedule.Id,
                    PatientId = response.Schedule.PatientId,
                    PriorityId = response.Schedule.PriorityId,
                    ProgramIds = response.Schedule.ProgramIds,
                    StatusId = response.Schedule.StatusId,
                    Title = response.Schedule.Title,
                    TypeId = response.Schedule.TypeId,
                    UpdatedOn = response.Schedule.UpdatedOn,
                    DefaultAssignment = response.Schedule.DefaultAssignment
                };

                return schedule;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetScheduleToDoById()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public object PutInsertToDo(ToDoData todo, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new PutInsertToDoDataRequest();
                request.ToDoData = todo;
                IRestClient client = new JsonServiceClient();

                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Scheduling/ToDo/Insert",
                    DDSchedulingUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber), userId);

                var response = client.Put<PutInsertToDoDataResponse>(url, request);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:PutInsertToDo()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ToDoData> GetPatientToDos(string patientId, string userId, IAppDomainRequest req)
        {
            try
            {
                var request = new GetToDosDataRequest();
                request.PatientId = patientId;
                request.UserId = userId;
                request.PatientId = patientId;
                IRestClient client = new JsonServiceClient();

                //{Context}/{Version}/{ContractNumber}/Scheduling/ToDos
                var url = Common.Helper.BuildURL(string.Format(@"{0}/{1}/{2}/{3}/Scheduling/ToDos",
                    DDSchedulingUrl,
                    "NG",
                    req.Version,
                    req.ContractNumber), userId);

                var response = client.Post<GetToDosDataResponse>(url, request);
                return response.ToDos;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementEndpointUtil:GetPatientToDos()::" + ex.Message, ex.InnerException);
            }
        }    
    }
}

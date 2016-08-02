using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AD = Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class PlanElementEndpointUtil
    {
        static readonly string DDPatientObservationServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];
        static readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];


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

        //public static PutRegisterPatientObservationResponse PutNewPatientProblem(string patientId, string userId, string elementId, IAppDomainRequest request)
        //{
        //    try
        //    {
        //        //register call to remote serivce.
        //        IRestClient client = new JsonServiceClient();

        //        string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Insert",
        //                           DDPatientObservationServiceUrl,
        //                           "NG",
        //                           request.Version,
        //                           request.ContractNumber,
        //                           patientId), request.UserId);

        //        PutRegisterPatientObservationResponse dataDomainResponse =
        //           client.Put<PutRegisterPatientObservationResponse>(
        //           url, new PutRegisterPatientObservationRequest
        //           {
        //               Id = elementId,
        //               StateId = 2,
        //               DisplayId = 1,
        //               UserId = userId
        //           } as object);
        //        return dataDomainResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("AD:PlanElementEndpointUtil:PutNewPatientProblem()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //public static PutUpdateObservationDataResponse UpdatePatientProblem(string patientId, string userId, string elementId, PatientObservation pod, bool _active, IAppDomainRequest request)
        //{
        //    try
        //    {
        //        //register call to remote serivce.
        //        IRestClient client = new JsonServiceClient();

        //        string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Update",
        //                           DDPatientObservationServiceUrl,
        //                           "NG",
        //                           request.Version,
        //                           request.ContractNumber,
        //                           patientId), request.UserId);

        //        PatientObservationRecordData pdata = new PatientObservationRecordData
        //        {
        //            Id = pod.Id,
        //            StateId = pod.StateId,
        //            DeleteFlag = false
        //        };

        //        PutUpdateObservationDataRequest purequest = new PutUpdateObservationDataRequest
        //        {
        //            PatientObservationData = pdata,
        //            Context = "NG",
        //            ContractNumber = request.ContractNumber,
        //            UserId = request.UserId,
        //        };

        //        PutUpdateObservationDataResponse dataDomainResponse =
        //           client.Put<PutUpdateObservationDataResponse>(
        //           url, purequest as object);

        //        return dataDomainResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("AD:PlanElementEndpointUtil:UpdatePatientProblem()::" + ex.Message, ex.InnerException);
        //    }
        //}

        internal static ProgramAttributeData GetProgramAttributes(string planElemId, IAppDomainRequest request)
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

        internal static bool UpdateProgramAttributes(ProgramAttributeData pAtt, IAppDomainRequest request)
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

        internal static bool InsertNewProgramAttribute(ProgramAttributeData pa, IAppDomainRequest request)
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

using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Observation
{
    public class ObservationEndpointUtil : IObservationEndpointUtil
    {
        static readonly string DDPatientObservationsServiceUrl = ConfigurationManager.AppSettings["DDPatientObservationUrl"];

        internal static List<PatientObservationData> GetStandardObservationsRequest(DTO.GetStandardObservationItemsRequest request)
        {
            try
            {
                List<PatientObservationData> result = null;
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observation/?TypeId={4}&PatientId={5}",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.TypeId,
                                    request.PatientId), request.UserId);

                GetStandardObservationsResponse dataDomainResponse = client.Get<GetStandardObservationsResponse>(
                    url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.StandardObservations;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<PatientObservationData> GetCurrentPatientObservations(GetCurrentPatientObservationsRequest request)
        {
            try
            {
                List<PatientObservationData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observations/Current", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observations/Current",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                GetCurrentPatientObservationsDataResponse dataDomainResponse = client.Get<GetCurrentPatientObservationsDataResponse>(
                    url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientObservationsData;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientObservationData> GetHistoricalPatientObservations(IPatientObservationsRequest request)
        {
            try
            {
                List<PatientObservationData> result = null;
                IRestClient client = new JsonServiceClient();

                var url =
                    Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observations/{5}/Historical",
                        DDPatientObservationsServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber,
                        request.PatientId,
                        request.ObservationId), request.UserId);

                var dataDomainResponse = client.Get<GetHistoricalPatientObservationsDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientObservationsData;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<ObservationData> GetObservations(GetObservationsRequest request)
        {
            try
            {
                List<ObservationData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Observations", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observations",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetObservationsDataResponse dataDomainResponse = client.Get<GetObservationsDataResponse>(
                    url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.ObservationsData;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAdditionalObservationsLibraryRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PostUpdateObservationItemsResponse UpdatePatientObservation(PostUpdateObservationItemsRequest request, List<PatientObservationRecordData> pord)
        {
            PostUpdateObservationItemsResponse response = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observations/Update", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observations/Update/",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                PutUpdatePatientObservationsDataResponse dataDomainResponse = client.Put<PutUpdatePatientObservationsDataResponse>(
                    url, new PutUpdatePatientObservationsDataRequest
                    {
                        PatientObservationsRecordData = pord,
                        UserId = request.UserId
                    } as object);

                if (dataDomainResponse.Result)
                {
                    response = new PostUpdateObservationItemsResponse();
                    response.PatientObservations = ObservationsUtil.GetPatientObservations(dataDomainResponse.PatientObservationsData);
                    response.Result = dataDomainResponse.Result;
                }

                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdatePatientObservation()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservationData GetAdditionalObservationItemRequest(GetAdditionalObservationItemRequest request)
        {
            try
            {
                PatientObservationData result = null;
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observation/{4}/Additional/",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.ObservationId), request.UserId);

                GetAdditionalObservationDataItemResponse dataDomainResponse = client.Post<GetAdditionalObservationDataItemResponse>(
                    url, new GetAdditionalObservationDataItemRequest
                    {
                        UserId = request.UserId,
                        PatientId = request.PatientId
                    });

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Observation;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAdditionalObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<State> GetAllowedObservationStates(GetAllowedStatesRequest request)
        {
            try
            {
                List<State> result = null;
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/Observation/States", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observation/States",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetAllowedStatesDataResponse dataDomainResponse = client.Get<GetAllowedStatesDataResponse>(url);

                if (dataDomainResponse != null && dataDomainResponse.StatesData.Count > 0)
                {
                    result = new List<State>();
                    dataDomainResponse.StatesData.ForEach(a =>
                        {
                            result.Add(new State 
                            { 
                                Id = a.Id,
                                Name = a.Name,
                                TypeIds = a.TypeIds
                            });
                        });
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllowedObservationStates()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<PatientObservation> GetPatientProblemSummary(GetPatientProblemsRequest request)
        {
            try
            {
                List<PatientObservation> result = new List<PatientObservation>();
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Problems",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                GetPatientProblemsSummaryResponse dataDomainResponse = client.Get<GetPatientProblemsSummaryResponse>(url);

                if (dataDomainResponse != null)
                {
                    dataDomainResponse.PatientObservations.ForEach(r =>
                    {
                        result.Add(
                            new PatientObservation
                            {
                                Id = r.Id,
                                ObservationId = r.ObservationId,
                                Name = r.Name,
                                PatientId = r.PatientId,
                                StateId = r.StateId,
                                DisplayId = r.DisplayId,
                                StartDate = r.StartDate,
                                EndDate = r.EndDate,
                                Source = r.Source,
                                Standard = r.Standard,
                                TypeId = r.TypeId,
                                DataSource = r.DataSource,
                                ExternalRecordId = r.ExternalRecordId
                            });
                    });
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetPatientProblemSummary()::" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservationData GetInitializeProblem(GetInitializeProblemRequest request)
        {
            try
            {
                PatientObservationData result = null;
                IRestClient client = new JsonServiceClient();
                //  [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observation/{ObservationId}/Problem/Initialize", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/{5}/Problem/Initialize",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId,
                                    request.ObservationId), request.UserId);

                GetInitializeProblemDataResponse dataDomainResponse = client.Get<GetInitializeProblemDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.PatientObservation;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInitializeProblem()::" + ex.Message, ex.InnerException);
            }
        }
    }
}

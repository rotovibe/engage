using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
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
    public class ObservationEndpointUtil
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

        internal static List<ObservationLibraryItemData> GetAdditionalObservationsLibraryRequest(GetAdditionalObservationLibraryRequest request)
        {
            try
            {
                List<ObservationLibraryItemData> result = null;
                IRestClient client = new JsonServiceClient();
                // [Route("/{Version}/{ContractNumber}/Observation/Type/{TypeId}/MatchLibrary/{Standard}"
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observation/Type/{4}/MatchLibrary/{5}",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.TypeId,
                                    request.Standard), request.UserId);

                GetAdditionalLibraryObservationsResponse dataDomainResponse = client.Get<GetAdditionalLibraryObservationsResponse>(
                    url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Library;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAdditionalObservationsLibraryRequest()::" + ex.Message, ex.InnerException);
            }
        }

        internal static bool UpdatePatientObservation(PostUpdateObservationItemsRequest request, PatientObservationRecordData pord)
        {
            bool result = false;
            try
            {
                IRestClient client = new JsonServiceClient();

                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Update/",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.PatientId), request.UserId);

                PutUpdateObservationDataResponse dataDomainResponse = client.Put<PutUpdateObservationDataResponse>(
                    url, new PutUpdateObservationDataRequest
                    {
                        PatientObservationData = pord,
                        UserId = request.UserId
                    } as object);

                if (dataDomainResponse.Result)
                {
                    result = dataDomainResponse.Result;
                }

                return result;
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

        internal static List<IdNamePair> GetAllowedObservationStates(GetAllowedStatesRequest request)
        {
            try
            {
                List<IdNamePair> result = null;
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/Observation/States/{TypeName}", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Observation/States/{4}",
                                    DDPatientObservationsServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.TypeName), request.UserId);

                GetAllowedStatesDataResponse dataDomainResponse = client.Get<GetAllowedStatesDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.StatesData;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllowedObservationStates()::" + ex.Message, ex.InnerException);
            }
        }

        internal static List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation> GetPatientProblemSummary(GetPatientProblemsRequest request)
        {
            try
            {
                List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation> result = new List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation>();
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
                            new Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation
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
                                Standard = r.Standard
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
    }
}

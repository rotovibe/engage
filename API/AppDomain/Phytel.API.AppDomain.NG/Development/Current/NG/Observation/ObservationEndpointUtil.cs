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
                GetStandardObservationsResponse dataDomainResponse = client.Get<GetStandardObservationsResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Observation/?TypeId={4}&PatientId={5}",
                    DDPatientObservationsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.TypeId,
                    request.PatientId));

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.StandardObservations;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:GetStandardObservationsRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static List<ObservationLibraryItemData> GetAdditionalObservationsLibraryRequest(GetAdditionalObservationLibraryRequest request)
        {
            try
            {
                List<ObservationLibraryItemData> result = null;
                IRestClient client = new JsonServiceClient();
                GetAdditionalLibraryObservationsResponse dataDomainResponse = client.Get<GetAdditionalLibraryObservationsResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Observation/Type/{4}/MatchLibrary/?PatientId={5}",
                    DDPatientObservationsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.TypeId,
                    request.PatientId));

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Library;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:GetAdditionalObservationsLibraryRequest()" + ex.Message, ex.InnerException);
            }
        }

        internal static bool UpdatePatientObservation(PostUpdateObservationItemsRequest request, PatientObservationRecordData pord, List<string> observationIds)
        {
            bool result = false;
            try
            {
                IRestClient client = new JsonServiceClient();
                PutUpdateObservationDataResponse dataDomainResponse = client.Put<PutUpdateObservationDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Observation/Update/",
                    DDPatientObservationsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), new PutUpdateObservationDataRequest
                    {
                        PatientObservationData = pord,
                        UserId = request.UserId,
                        PatientObservationIdsList = observationIds
                    } as object);

                if (dataDomainResponse.Result)
                {
                    result = dataDomainResponse.Result;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("App Domain:UpdatePatientObservation()" + ex.Message, ex.InnerException);
            }
        }

        internal static PatientObservationData GetAdditionalObservationItemRequest(GetAdditionalObservationItemRequest request)
        {
            try
            {
                PatientObservationData result = null;
                IRestClient client = new JsonServiceClient();
                GetAdditionalObservationDataItemResponse dataDomainResponse = client.Post<GetAdditionalObservationDataItemResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Observation/{4}/Additional/",
                    DDPatientObservationsServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.ObservationId), new GetAdditionalObservationDataItemRequest
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
                throw new WebServiceException("App Domain:GetAdditionalObservationsRequest()" + ex.Message, ex.InnerException);
            }
        }
    }
}

﻿using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
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
    }
}

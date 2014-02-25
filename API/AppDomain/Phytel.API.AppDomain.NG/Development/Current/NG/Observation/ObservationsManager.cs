using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Observation
{
    public class ObservationsManager : ManagerBase
    {
        //public GetInitializeObservationResponse GetInitialGoalRequest(GetInitializeObservationRequest request)
        //{
        //    try
        //    {
        //        GetInitializeObservationResponse response = new GetInitializeObservationResponse();
        //        PatientObservationData po = (PatientObservationData)ObservationsUtil.GetInitialObservationRequest(request);
        //        response.Observation = ObservationsUtil.GetObservationForInitialize(request, po);
        //        response.Version = request.Version;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public GetStandardObservationItemsResponse GetStandardObservationsRequest(GetStandardObservationItemsRequest request)
        {
            try
            {
                GetStandardObservationItemsResponse response = new GetStandardObservationItemsResponse();
                List<PatientObservationData> po = (List<PatientObservationData>)ObservationEndpointUtil.GetStandardObservationsRequest(request);
                response.Observations = ObservationsUtil.GetStandardObservationsForPatient(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

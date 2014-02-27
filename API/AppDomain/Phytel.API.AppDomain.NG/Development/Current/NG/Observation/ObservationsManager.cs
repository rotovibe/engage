using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Globalization;

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

        public GetAdditionalObservationLibraryResponse GetAdditionalObservationsLibraryRequest(GetAdditionalObservationLibraryRequest request)
        {
            try
            {
                GetAdditionalObservationLibraryResponse response = new GetAdditionalObservationLibraryResponse();
                List<ObservationLibraryItemData> po = (List<ObservationLibraryItemData>)ObservationEndpointUtil.GetAdditionalObservationsLibraryRequest(request);
                response.Library = ObservationsUtil.GetAdditionalLibraryObservations(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PostUpdateObservationItemsResponse SavePatientObservations(PostUpdateObservationItemsRequest request)
        {
            try
            {
                PostUpdateObservationItemsResponse response = new PostUpdateObservationItemsResponse();

                List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation> obsl = request.Observations;

                List<string> patientObservationIds = ObservationsUtil.GetPatientObservationIds(obsl);

                if (request.Observations != null && request.Observations.Count > 0)
                {
                    foreach (Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation po in obsl)
                    {
                        foreach (ObservationValue ov in po.Values)
                        {
                            PatientObservationRecordData pord = ObservationsUtil.CreatePatientObservationRecord(po, ov);

                            ObservationEndpointUtil.UpdatePatientObservation(request, pord, patientObservationIds);
                        }
                    }
                }
                else
                {
                    PatientObservationRecordData epord = new PatientObservationRecordData();
                    ObservationEndpointUtil.UpdatePatientObservation(request, epord, patientObservationIds);
                }

                response.Result = true;
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

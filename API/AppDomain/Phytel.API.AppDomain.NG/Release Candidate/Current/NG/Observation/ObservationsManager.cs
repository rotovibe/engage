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
                throw new Exception("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetAdditionalObservationsLibraryRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public PostUpdateObservationItemsResponse SavePatientObservations(PostUpdateObservationItemsRequest request)
        {
            try
            {
                PostUpdateObservationItemsResponse response = new PostUpdateObservationItemsResponse();

                List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation> obsl = request.Observations;

                //List<string> patientObservationIds = ObservationsUtil.GetPatientObservationIds(obsl);

                if (request.Observations != null && request.Observations.Count > 0)
                {
                    foreach (Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation po in obsl)
                    {
                        foreach (ObservationValue ov in po.Values)
                        {
                            PatientObservationRecordData pord = ObservationsUtil.CreatePatientObservationRecord(po, ov);

                            ObservationEndpointUtil.UpdatePatientObservation(request, pord);
                        }
                    }
                }
                else
                {
                    PatientObservationRecordData epord = new PatientObservationRecordData();
                    ObservationEndpointUtil.UpdatePatientObservation(request, epord);
                }

                response.Result = true;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientObservations()::" + ex.Message, ex.InnerException);
            }
        }

        public GetAdditionalObservationItemResponse GetAdditionalObservationsRequest(GetAdditionalObservationItemRequest request)
        {
            try
            {
                GetAdditionalObservationItemResponse response = new GetAdditionalObservationItemResponse();
                PatientObservationData po = (PatientObservationData)ObservationEndpointUtil.GetAdditionalObservationItemRequest(request);
                response.Observation = ObservationsUtil.GetAdditionalObservationItemForPatient(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAdditionalObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }
    }
}

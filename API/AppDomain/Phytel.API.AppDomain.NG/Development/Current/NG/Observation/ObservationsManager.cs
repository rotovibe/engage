using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Globalization;
using Phytel.API.Common.CustomObject;

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
                        PatientObservationRecordData pord = null;
                        if (po.Values != null && po.Values.Count > 0) // Labs and Vitals have values
                        {
                            foreach (ObservationValue ov in po.Values)
                            {
                                pord = ObservationsUtil.CreatePatientObservationRecord(po, ov);
                                ObservationEndpointUtil.UpdatePatientObservation(request, pord);
                            }
                        }
                        else // Problem does not have values
                        {
                            pord = ObservationsUtil.CreatePatientObservationRecord(po, null);
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


        public GetAllowedStatesResponse GetAllowedObservationStates(GetAllowedStatesRequest request)
        {
            try
            {
                GetAllowedStatesResponse response = new GetAllowedStatesResponse();
                List<IdNamePair> states = ObservationEndpointUtil.GetAllowedObservationStates(request);
                response.States = states;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAllowedObservationStates()::" + ex.Message, ex.InnerException);
            }
        }

        public GetPatientProblemsResponse GetPatientProblemsSummary(GetPatientProblemsRequest request)
        {
            try
            {
                GetPatientProblemsResponse response = new GetPatientProblemsResponse();
                List<Phytel.API.AppDomain.NG.DTO.Observation.PatientObservation> problems = ObservationEndpointUtil.GetPatientProblemSummary(request);
                response.Problems = problems;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPatientProblemsSummary()::" + ex.Message, ex.InnerException);
            }
        }

        public GetInitializeProblemResponse GetInitializeProblem(GetInitializeProblemRequest request)
        {
            try
            {
                GetInitializeProblemResponse response = new GetInitializeProblemResponse();
                PatientObservationData po = ObservationEndpointUtil.GetInitializeProblem(request);
                response.Observation = ObservationsUtil.GetInitializeProblem(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAllowedObservationStates()::" + ex.Message, ex.InnerException);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Globalization;
using Phytel.API.Common.CustomObject;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.Observation
{
    public class ObservationsManager : ManagerBase, IObservationsManager
    {
        public IObservationEndpointUtil EndpointUtil { get; set; }

        public GetStandardObservationItemsResponse GetStandardObservationsRequest(GetStandardObservationItemsRequest request)
        {
            try
            {
                GetStandardObservationItemsResponse response = new GetStandardObservationItemsResponse();
                List<PatientObservationData> po = (List<PatientObservationData>)ObservationEndpointUtil.GetStandardObservationsRequest(request);
                response.PatientObservations = ObservationsUtil.GetStandardObservationsForPatient(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public GetObservationsResponse GetObservations(GetObservationsRequest request)
        {
            try
            {
                GetObservationsResponse response = new GetObservationsResponse();
                List<ObservationData> po = (List<ObservationData>)ObservationEndpointUtil.GetObservations(request);
                response.Observations = ObservationsUtil.GetObservations(request, po);
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

                List<PatientObservation> obsl = request.PatientObservations;

                //List<string> patientObservationIds = ObservationsUtil.GetPatientObservationIds(obsl);
                List<PatientObservationRecordData> poRecordDataList = new List<PatientObservationRecordData>();
                if (request.PatientObservations != null && request.PatientObservations.Count > 0)
                {
                    foreach (PatientObservation po in obsl)
                    {
                        PatientObservationRecordData pord = null;
                        if (po.Values != null && po.Values.Count > 0) // Labs and Vitals have values
                        {
                            foreach (ObservationValue ov in po.Values)
                            {
                                pord = ObservationsUtil.CreatePatientObservationRecord(po, ov);
                                //ObservationEndpointUtil.UpdatePatientObservation(request, pord);
                                poRecordDataList.Add(pord);
                            }
                        }
                        else // Problem does not have values
                        {
                            pord = ObservationsUtil.CreatePatientObservationRecord(po, null);
                            //ObservationEndpointUtil.UpdatePatientObservation(request, pord);
                            poRecordDataList.Add(pord);
                        }
                    }
                    response = ObservationEndpointUtil.UpdatePatientObservation(request, poRecordDataList);
                }
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
                response.PatientObservation = ObservationsUtil.GetAdditionalObservationItemForPatient(request, po);
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
                response.States = ObservationEndpointUtil.GetAllowedObservationStates(request);
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
                List<PatientObservation> problems = ObservationEndpointUtil.GetPatientProblemSummary(request);
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
                response.PatientObservation = ObservationsUtil.GetInitializeProblem(request, po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAllowedObservationStates()::" + ex.Message, ex.InnerException);
            }
        }

        public GetCurrentPatientObservationsResponse GetCurrentPatientObservations(GetCurrentPatientObservationsRequest request)
        {
            try
            {
                GetCurrentPatientObservationsResponse response = new GetCurrentPatientObservationsResponse();
                List<PatientObservationData> po = (List<PatientObservationData>)ObservationEndpointUtil.GetCurrentPatientObservations(request);
                response.PatientObservations = ObservationsUtil.GetPatientObservations(po);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetStandardObservationsRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientObservation> GetHistoricalPatientObservations(IPatientObservationsRequest request)
        {
            try
            {
                var po = EndpointUtil.GetHistoricalPatientObservations(request);
                var historyData = ObservationsUtil.GetPatientObservations(po);
                return historyData;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetHistoricalPatientObservations()::" + ex.Message, ex.InnerException);
            }
        }
    }
}

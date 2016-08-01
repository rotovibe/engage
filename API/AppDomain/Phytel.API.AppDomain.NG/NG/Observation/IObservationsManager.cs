using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Observation
{
    public interface IObservationsManager
    {
        GetStandardObservationItemsResponse GetStandardObservationsRequest(GetStandardObservationItemsRequest request);
        GetObservationsResponse GetObservations(GetObservationsRequest request);
        PostUpdateObservationItemsResponse SavePatientObservations(PostUpdateObservationItemsRequest request);
        GetAdditionalObservationItemResponse GetAdditionalObservationsRequest(GetAdditionalObservationItemRequest request);
        GetAllowedStatesResponse GetAllowedObservationStates(GetAllowedStatesRequest request);
        GetPatientProblemsResponse GetPatientProblemsSummary(GetPatientProblemsRequest request);
        GetInitializeProblemResponse GetInitializeProblem(GetInitializeProblemRequest request);
        GetCurrentPatientObservationsResponse GetCurrentPatientObservations(GetCurrentPatientObservationsRequest request);
        List<PatientObservation> GetHistoricalPatientObservations(IPatientObservationsRequest request);
        void LogException(Exception ex);
    }
}
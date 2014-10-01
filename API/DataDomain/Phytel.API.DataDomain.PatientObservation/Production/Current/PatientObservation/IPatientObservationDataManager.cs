using System.Collections.Generic;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation
{
    public interface IPatientObservationDataManager
    {
        GetPatientObservationResponse GetPatientObservationByID(GetPatientObservationRequest request);
        GetPatientProblemsSummaryResponse GetPatientProblemList(GetPatientProblemsSummaryRequest request);
        GetStandardObservationsResponse GetStandardObservationsByType(GetStandardObservationsRequest request);
        ObservationValueData InitializePatientObservation(IDataDomainRequest request, string patientId, List<ObservationValueData> list, ObservationData od, string initSetId);
        void GetPreviousValuesForObservation(ObservationValueData ovd, string patientId, string observationTypeId, IDataDomainRequest request);
        GetObservationsDataResponse GetObservationsData(GetObservationsDataRequest request);
        bool PutUpdateOfPatientObservationRecord(PutUpdateObservationDataRequest request);
        GetAdditionalObservationDataItemResponse GetAdditionalObservationItemById(GetAdditionalObservationDataItemRequest request);
        GetAllowedStatesDataResponse GetAllowedStates(GetAllowedStatesDataRequest request);
        GetInitializeProblemDataResponse GetInitializeProblem(GetInitializeProblemDataRequest request);
        PutRegisterPatientObservationResponse PutRegisteredObservation(PutRegisterPatientObservationRequest request);
        GetCurrentPatientObservationsDataResponse GetCurrentPatientObservations(GetCurrentPatientObservationsDataRequest request);
        List<PatientObservationData> GetHistoricalPatientObservations(GetHistoricalPatientObservationsDataRequest request);
        DeletePatientObservationByPatientIdDataResponse DeletePatientObservationByPatientId(DeletePatientObservationByPatientIdDataRequest request);
        UndoDeletePatientObservationsDataResponse UndoDeletePatientObservations(UndoDeletePatientObservationsDataRequest request);
        PutUpdatePatientObservationsDataResponse UpdatePatientObservations(PutUpdatePatientObservationsDataRequest request);
    }
}
using Phytel.API.DataDomain.Patient.DTO;
using System;
namespace Phytel.API.DataDomain.Patient
{
    public interface IPatientDataManager
    {
        GetCohortPatientsDataResponse GetCohortPatients(GetCohortPatientsDataRequest request);
        GetCohortPatientViewResponse GetCohortPatientView(GetCohortPatientViewRequest request);
        GetPatientDataResponse GetPatientByID(GetPatientDataRequest request);
        GetPatientsDataResponse GetPatients(GetPatientsDataRequest request);
        GetPatientSSNDataResponse GetPatientSSN(GetPatientSSNDataRequest request);
        PutCohortPatientViewDataResponse InsertCohortPatientView(PutCohortPatientViewDataRequest request);
        PutPatientDataResponse InsertPatient(PutPatientDataRequest request);
        PutUpdateCohortPatientViewResponse UpdateCohortPatientViewProblem(PutUpdateCohortPatientViewRequest request);
        PutUpdatePatientDataResponse UpdatePatient(PutUpdatePatientDataRequest request);
        PutPatientBackgroundDataResponse UpdatePatientBackground(PutPatientBackgroundDataRequest request);
        PutPatientFlaggedResponse UpdatePatientFlagged(PutPatientFlaggedRequest request);
        PutPatientPriorityResponse UpdatePatientPriority(PutPatientPriorityRequest request);
        DeletePatientDataResponse DeletePatient(DeletePatientDataRequest request);
        DeletePatientUserByPatientIdDataResponse DeletePatientUserByPatientId(DeletePatientUserByPatientIdDataRequest request);
        DeleteCohortPatientViewDataResponse DeleteCohortPatientViewByPatientId(DeleteCohortPatientViewDataRequest request);
        UndoDeletePatientDataResponse UndoDeletePatient(UndoDeletePatientDataRequest request);
        UndoDeletePatientUserByPatientIdDataResponse UndoDeletePatientUserByPatientId(UndoDeletePatientUserByPatientIdDataRequest request);
        UndoDeleteCohortPatientViewDataResponse UndoDeleteCohortPatientViewByPatientId(UndoDeleteCohortPatientViewDataRequest request);
    }
}

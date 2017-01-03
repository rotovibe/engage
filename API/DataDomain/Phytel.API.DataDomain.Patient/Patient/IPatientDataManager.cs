using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
namespace Phytel.API.DataDomain.Patient
{
    public interface IPatientDataManager
    {
        GetCohortPatientsDataResponse GetCohortPatients(GetCohortPatientsDataRequest request);
        GetCohortPatientViewResponse GetCohortPatientView(GetCohortPatientViewRequest request);
        GetPatientDataResponse GetPatientByID(GetPatientDataRequest request);
        GetPatientDataResponse GetPatientDataByNameDOB(GetPatientDataByNameDOBRequest request);
        GetPatientsDataResponse GetPatients(GetPatientsDataRequest request);
        GetPatientSSNDataResponse GetPatientSSN(GetPatientSSNDataRequest request);
        PutCohortPatientViewDataResponse InsertCohortPatientView(PutCohortPatientViewDataRequest request);
        PutPatientDataResponse InsertPatient(PutPatientDataRequest request);
        PutUpdateCohortPatientViewResponse UpdateCohortPatientViewProblem(PutUpdateCohortPatientViewRequest request);
        PutUpdatePatientDataResponse UpdatePatient(PutUpdatePatientDataRequest request);
        PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request);
        PutPatientFlaggedResponse UpdatePatientFlagged(PutPatientFlaggedRequest request);
        PutPatientPriorityResponse UpdatePatientPriority(PutPatientPriorityRequest request);
        DeletePatientDataResponse DeletePatient(DeletePatientDataRequest request);
        DeletePatientUserByPatientIdDataResponse DeletePatientUserByPatientId(DeletePatientUserByPatientIdDataRequest request);
        DeleteCohortPatientViewDataResponse DeleteCohortPatientViewByPatientId(DeleteCohortPatientViewDataRequest request);
        UndoDeletePatientDataResponse UndoDeletePatient(UndoDeletePatientDataRequest request);
        UndoDeletePatientUsersDataResponse UndoDeletePatientUser(UndoDeletePatientUsersDataRequest request);
        UndoDeleteCohortPatientViewDataResponse UndoDeleteCohortPatientView(UndoDeleteCohortPatientViewDataRequest request);
        PutInitializePatientDataResponse InitializePatient(PutInitializePatientDataRequest request);
        List<PatientData> GetAllPatients(GetAllPatientsDataRequest request);
        InsertBatchPatientsDataResponse InsertBatchPatients(InsertBatchPatientsDataRequest request);
        SyncPatientInfoDataResponse SyncPatient(SyncPatientInfoDataRequest request);

        #region Cohort Patient View

        AddPCMToCohortPatientViewDataResponse AddPcmToCohortPatientView(AddPCMToCohortPatientViewDataRequest request);
        RemovePCMFromCohortPatientViewDataResponse RemovePcmFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request);
        AssignContactsToCohortPatientViewDataResponse AssignContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request);

        #endregion
    }
}

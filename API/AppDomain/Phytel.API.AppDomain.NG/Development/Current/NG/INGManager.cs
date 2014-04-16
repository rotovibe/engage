using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.Common.CustomObject;
using System;
using System.Collections.Generic;
namespace Phytel.API.AppDomain.NG
{
    public interface INGManager
    {
        IPlanElementUtils PlanElementUtils { get; set; }
        IPlanElementEndpointUtils PlanElementEndpointUtils { get; set; }
        GetActiveProgramsResponse GetActivePrograms(GetActiveProgramsRequest request);
        List<IdNamePair> GetAllCommModes(GetAllCommModesRequest request);
        List<CommTypeLookUp> GetAllCommTypes(GetAllCommTypesRequest request);
        List<LanguagesLookUp> GetAllLanguages(GetAllLanguagesRequest request);
        GetAllSettingsResponse GetAllSettings(GetAllSettingsRequest request);
        List<StatesLookUp> GetAllStates(GetAllStatesRequest request);
        List<IdNamePair> GetAllTimesOfDays(GetAllTimesOfDaysRequest request);
        List<TimeZonesLookUp> GetAllTimeZones(GetAllTimeZonesRequest request);
        List<Contact> GetCareManagers(GetAllCareManagersRequest request);
        GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request);
        List<Cohort> GetCohorts(GetAllCohortsRequest request);
        Contact GetContactByPatientId(GetContactRequest request);
        List<IdNamePair> GetLookUps(GetLookUpsRequest request);
        GetPatientResponse GetPatient(GetPatientRequest request);
        List<PatientProblem> GetPatientProblems(GetAllPatientProblemsRequest request);
        GetPatientProgramDetailsSummaryResponse GetPatientProgramDetailsSummary(GetPatientProgramDetailsSummaryRequest request);
        GetPatientActionDetailsResponse GetPatientActionDetails(GetPatientActionDetailsRequest request);
        GetPatientProgramsResponse GetPatientPrograms(GetPatientProgramsRequest request);
        GetPatientSSNResponse GetPatientSSN(GetPatientSSNRequest request);
        List<IdNamePair> GetProblems(GetAllProblemsRequest request);
        PostPatientToProgramsResponse PostPatientToProgram(PostPatientToProgramsRequest request);
        PutPatientDetailsUpdateResponse PutPatientDetailsUpdate(PutPatientDetailsUpdateRequest request);
        PutPatientFlaggedUpdateResponse PutPatientFlaggedUpdate(PutPatientFlaggedUpdateRequest request);
        PutUpdateContactResponse PutUpdateContact(PutUpdateContactRequest request);
        PutPatientBackgroundResponse UpdateBackground(PutPatientBackgroundRequest request);
        void LogException(Exception ex);
    }
}

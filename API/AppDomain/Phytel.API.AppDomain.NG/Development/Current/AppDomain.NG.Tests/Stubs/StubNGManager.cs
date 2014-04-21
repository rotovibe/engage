using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubNGManager: INGManager
    {
        public DTO.GetActiveProgramsResponse GetActivePrograms(DTO.GetActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Common.CustomObject.IdNamePair> GetAllCommModes(DTO.GetAllCommModesRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.CommTypeLookUp> GetAllCommTypes(DTO.GetAllCommTypesRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.LanguagesLookUp> GetAllLanguages(DTO.GetAllLanguagesRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetAllSettingsResponse GetAllSettings(DTO.GetAllSettingsRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.StatesLookUp> GetAllStates(DTO.GetAllStatesRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Common.CustomObject.IdNamePair> GetAllTimesOfDays(DTO.GetAllTimesOfDaysRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.TimeZonesLookUp> GetAllTimeZones(DTO.GetAllTimeZonesRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.Contact> GetCareManagers(DTO.GetAllCareManagersRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetCohortPatientsResponse GetCohortPatients(DTO.GetCohortPatientsRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.Cohort> GetCohorts(DTO.GetAllCohortsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Contact GetContactByPatientId(DTO.GetContactRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Common.CustomObject.IdNamePair> GetLookUps(DTO.GetLookUpsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientResponse GetPatient(DTO.GetPatientRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.PatientProblem> GetPatientProblems(DTO.GetAllPatientProblemsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientProgramDetailsSummaryResponse GetPatientProgramDetailsSummary(DTO.GetPatientProgramDetailsSummaryRequest request)
        {
            DTO.GetPatientProgramDetailsSummaryResponse response = new DTO.GetPatientProgramDetailsSummaryResponse
            {
                Program = new DTO.Program { Name = "Test Program", Description = "test description" },
                 Version = 1.0
            };
            return response;
        }

        public DTO.GetPatientProgramsResponse GetPatientPrograms(DTO.GetPatientProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientActionDetailsResponse GetPatientActionDetails(DTO.GetPatientActionDetailsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientSSNResponse GetPatientSSN(DTO.GetPatientSSNRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Common.CustomObject.IdNamePair> GetProblems(DTO.GetAllProblemsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PostPatientToProgramsResponse PostPatientToProgram(DTO.PostPatientToProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientDetailsUpdateResponse PutPatientDetailsUpdate(DTO.PutPatientDetailsUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientFlaggedUpdateResponse PutPatientFlaggedUpdate(DTO.PutPatientFlaggedUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdateContactResponse PutUpdateContact(DTO.PutUpdateContactRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientBackgroundResponse UpdateBackground(DTO.PutPatientBackgroundRequest request)
        {
            throw new NotImplementedException();
        }


        public void LogException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public IPlanElementUtils PlanElementUtils
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Programs.IPlanElementEndpointUtils PlanElementEndpointUtils
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

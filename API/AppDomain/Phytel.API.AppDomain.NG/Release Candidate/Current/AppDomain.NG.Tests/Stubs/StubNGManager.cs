using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.DataDomain.Contact.DTO;
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

        public List<DTO.ObjectivesLookUp> GetAllObjectives(DTO.GetAllObjectivesRequest request)
        {
            List<DTO.ObjectivesLookUp> list = new List<DTO.ObjectivesLookUp>();
            list.Add(new DTO.ObjectivesLookUp { Id = "123", Name = "test", Categories = null });
            return list;

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

        public DTO.Contact GetContactByPatientId(DTO.GetContactByPatientIdRequest request)
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
                Program = new DTO.Program
                {
                    Name = "Test Program",
                    Description = "test description",
                    AssignById = request.UserId,
                    AssignDate = System.DateTime.UtcNow,
                    StateUpdatedOn = System.DateTime.UtcNow,
                    Attributes = new DTO.ProgramAttribute
                    {
                        //AssignedBy = "me",
                        //AssignedOn = System.DateTime.UtcNow,
                        Id = "0000000000000000000000000"
                    },
                    Modules = new List<Module>() { 
                    new Module { Id = "000000000000000000000000", 
                        Name = "Test stub module 1",
                         Description = "BSHSI - Outreach & Enrollment",
                          SourceId ="532b5585a381168abe00042c",
                        Actions = new List<Actions>(){ 
                            new Actions{ Id = "000000000000000000000000", ElementState = 4, Name ="test action from stub", Text = "test action 1"} } 
                    }
                },
                    Objectives = new List<DTO.ObjectiveInfo> { new DTO.ObjectiveInfo{ Id="123456789012345678901234",
                     Status = 1,
                      Unit = "lbs",
                       Value = "12"
                    } }
                },
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
            DTO.GetPatientActionDetailsResponse response = new DTO.GetPatientActionDetailsResponse
            {
                Action = new DTO.Actions
                {
                    Name = "Program Completion",
                    Description = "Program Completion test description"
                },
                Version = 1.0
            };
            return response;
        }

        public DTO.GetPatientSSNResponse GetPatientSSN(DTO.GetPatientSSNRequest request)
        {
            throw new NotImplementedException();
        }

        public PostDeletePatientResponse DeletePatient(PostDeletePatientRequest request)
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

        public DTO.PutPatientDetailsUpdateResponse UpsertPatient(DTO.PutPatientDetailsUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientFlaggedUpdateResponse PutPatientFlaggedUpdate(DTO.PutPatientFlaggedUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UpdateContactResponse PutUpdateContact(DTO.UpdateContactRequest request)
        {
            throw new NotImplementedException();
        }

        public GetRecentPatientsResponse GetRecentPatients(GetRecentPatientsRequest request)
        {
            GetRecentPatientsResponse response = new GetRecentPatientsResponse();
            List<CohortPatient> cohortpatients = new List<CohortPatient>();
            cohortpatients.Add(new CohortPatient { Id = "abc", MiddleName = "e",  LastName = "lopez", FirstName = "eric", Gender = "M", DOB = "01/02/1978", PreferredName ="", Suffix ="Jr" });
            cohortpatients.Add(new CohortPatient { Id = "efg", MiddleName = "e", LastName = "johnson", FirstName = "miles", Gender = "M", DOB = "01/02/1978", PreferredName = "", Suffix = "II" });
            cohortpatients.Add(new CohortPatient { Id = "hij", MiddleName = "r", LastName = "miles", FirstName = "raul", Gender = "M", DOB = "01/02/1978", PreferredName = "", Suffix = "Sr" });
            cohortpatients.Add(new CohortPatient { Id = "lmn", MiddleName = "t", LastName = "hopkins", FirstName = "john", Gender = "M", DOB = "01/02/1978", PreferredName = "", Suffix = "IV" });
            cohortpatients.Add(new CohortPatient { Id = "opq", MiddleName = "w", LastName = "kennedy", FirstName = "peter", Gender = "M", DOB = "01/02/1943", PreferredName = "", Suffix = "X" });
            response.Patients = new List<CohortPatient>();
            response.ContactId = request.ContactId;
            response.Limit = 5;
            response.Patients = cohortpatients;
            return response;
        }


        public InsertContactResponse InsertContact(InsertContactRequest request)
        {
            InsertContactResponse response = new InsertContactResponse()
            {
                Id = "56ea228e64e91cf53bbfca66",
                Version = 1.0
            };
            return response;
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


        public PostProgramAttributesChangeResponse PostProgramAttributeChanges(PostProgramAttributesChangeRequest request)
        {
            PostProgramAttributesChangeResponse response = new PostProgramAttributesChangeResponse
            {
                Version = 1.0
            };

            return response;
        }


        public IEndpointUtils EndpointUtils
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


        public PostRemovePatientProgramResponse RemovePatientProgram(PostRemovePatientProgramRequest request)
        {
            throw new NotImplementedException();
        }


        public GetInitializePatientResponse GetInitializePatient(GetInitializePatientRequest request)
        {
            throw new NotImplementedException();
        }


        public List<LookUpDetails> GetLookUpDetails(GetLookUpDetailsRequest request)
        {
            throw new NotImplementedException();
        }

        public InsertPatientSystemsResponse SavePatientSystem(InsertPatientSystemsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetContactTypeLookupResponse GetContactTypeLookup(GetContactTypeLookupRequest request)
        {
            var res = new GetContactTypeLookupResponse();

            if (request.GroupType==0)
            {
                var fakeLookups = new List<ContactTypeLookUp>()
                {
                    new ContactTypeLookUp
                    {
                        Id = "56ea0c2c64e91cf53bbfca5f",
                        Name = "Doctor",
                        Role = "Doctor(M.D)",
                        CreatedOn = DateTime.UtcNow,
                        Group = (int) ContactLookUpGroupType.ContactType,
                        
                        
                    },
                    new ContactTypeLookUp
                    {
                        Id = "56f310910894eccd367b980e",
                        Name = "Addiction Medicine",
                        Role = "Addiction Medicine",
                        CreatedOn = DateTime.UtcNow,
                        Group = (int) ContactLookUpGroupType.CareTeam,
                       
                    }
                };
                res.ContactTypeLookUps = fakeLookups;
            }
            else
            {
                var fakeLookups = new List<ContactTypeLookUp>()
                {
                    new ContactTypeLookUp
                    {
                        Id = "56ea0c2c64e91cf53bbfca5f",
                        Name = "Doctor",
                        Role = "Doctor(M.D)",
                        CreatedOn = DateTime.UtcNow,
                        Group = request.GroupType,
                        
                    }
                };
                res.ContactTypeLookUps = fakeLookups;
            }
                       
            return res;
        }


        public DTO.Contact GetContactByContactId(GetContactByContactIdRequest request)
        {
            throw new NotImplementedException();
        }


        public SearchContactsResponse SearchContacts(SearchContactsRequest request)
        {
            throw new NotImplementedException();
        }


        public SaveCareTeamResponse SaveCareTeam(SaveCareTeamRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

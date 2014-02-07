using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Hosting;
using DD = Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using System.Runtime.CompilerServices;
using Phytel.API.Common.Audit;
using System.Threading;
using Phytel.API.Interface;
using System.Web;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        protected static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];
        protected static readonly string DDCohortServiceUrl = ConfigurationManager.AppSettings["DDCohortServiceUrl"];
        protected static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        #endregion

        #region Patient Requests
        public NG.DTO.GetPatientResponse GetPatient(NG.DTO.GetPatientRequest request)
        {
            NG.DTO.GetPatientResponse pResponse = new NG.DTO.GetPatientResponse();

            try
            {
                //Execute call(s) to Patient Data Domain
                IRestClient client = new JsonServiceClient();

                Phytel.API.DataDomain.Patient.DTO.GetPatientDataResponse response = client.Get<Phytel.API.DataDomain.Patient.DTO.GetPatientDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}?UserId={5}",
                                                                                            DDPatientServiceURL,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.PatientID,
                                                                                            request.UserId));

                if (response != null && response.Patient != null)
                {
                    Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse sysResponse = null;

                    if (string.IsNullOrEmpty(response.Patient.DisplayPatientSystemID) == false)
                    {
                        sysResponse = client.Get<Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse>(string.Format("{0}/{1}/{2}/{3}/PatientSystem/{4}",
                                                                                    DDPatientSystemUrl,
                                                                                    "NG",
                                                                                    request.Version,
                                                                                    request.ContractNumber,
                                                                                    response.Patient.DisplayPatientSystemID));
                    }

                    List<CareTeamMember> careTeam = null;
                    //Get the List of contact Ids from the PatientDataResponse.
                    if(response.Patient.CareTeamData != null && response.Patient.CareTeamData.Count > 0)
                    {
                        List<CareTeamMemberData> careTeamMembersData = response.Patient.CareTeamData;
                        List<string> contactIds = new List<string>();
                        foreach (CareTeamMemberData ctm in careTeamMembersData)
                        {
                            contactIds.Add(ctm.ContactId);
                        }

                        SearchContactsDataResponse contactDataResponse = null;
                        //[Route("/{Context}/{Version}/{ContractNumber}/Contact", "POST")]
                        contactDataResponse = client.Post<SearchContactsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Contact",
                                                                DDContactServiceUrl,
                                                                "NG",
                                                                request.Version,
                                                                request.ContractNumber), new SearchContactsDataRequest 
                                                                {
                                                                    ContactIds = contactIds, 
                                                                    Context = "NG",
                                                                    ContractNumber = request.ContractNumber,
                                                                    UserId = request.UserId,
                                                                    Version = request.Version
                                                                } as object
                                                                );


                        if(contactDataResponse != null && contactDataResponse.Contacts != null)
                        {
                            careTeam = new List<CareTeamMember>();
                            foreach(CareTeamMemberData ctmData in careTeamMembersData)
                            {
                               var contactData = contactDataResponse.Contacts.Where(c => c.ContactId == ctmData.ContactId).FirstOrDefault();
                               if (contactData != null)
                               {

                                   CareTeamMember newCM = new CareTeamMember {
                                       Gender = contactData.Gender, 
                                       PreferredName = contactData.PreferredName,
                                       Primary = ctmData.Primary, 
                                       Type = ctmData.Type 
                                   };
                                   careTeam.Add(newCM);
                               }
                            }
                        }
                    }

                    pResponse.Patient = new NG.DTO.Patient
                    {
                        Id = response.Patient.ID,
                        FirstName = response.Patient.FirstName,
                        LastName = response.Patient.LastName,
                        DOB = NGUtils.IsDateValid(response.Patient.DOB) ? response.Patient.DOB : string.Empty,
                        Gender = response.Patient.Gender,
                        MiddleName = response.Patient.MiddleName,
                        Suffix = response.Patient.Suffix,
                        PreferredName = response.Patient.PreferredName,
                        Priority = (int)response.Patient.PriorityData,
                        Flagged = Convert.ToInt32(response.Patient.Flagged),
                        CareTeam = careTeam
                    };

                    if (sysResponse != null && sysResponse.PatientSystem != null)
                    {
                        pResponse.Patient.DisplaySystemId = sysResponse.PatientSystem.SystemID;
                        pResponse.Patient.DisplaySystemName = sysResponse.PatientSystem.SystemName;
                    }
                }

               return pResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<NG.DTO.PatientProblem> GetPatientProblems(NG.DTO.GetAllPatientProblemsRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PatientID))
                    throw new ArgumentException("PatientID is null or empty.");

                List<Phytel.API.AppDomain.NG.DTO.PatientProblem> response = new List<Phytel.API.AppDomain.NG.DTO.PatientProblem>();

                IRestClient client = new JsonServiceClient();
                
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientID}/Problems", "GET")]
                Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemsDataResponse>
                    (string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problems",
                        DDPatientProblemServiceUrl,
                        //"NG",
                        "Nightingale",
                        request.Version,
                        request.ContractNumber,
                        request.PatientID));

                List<Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemData> problems = dataDomainResponse.PatientProblems;

                foreach (Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemData p in problems)
                {
                    Phytel.API.AppDomain.NG.DTO.PatientProblem pp = new Phytel.API.AppDomain.NG.DTO.PatientProblem();
                    pp.ID = p.ID;
                    pp.PatientID = p.PatientID;
                    pp.ProblemID = p.ProblemID;
                    pp.Level = p.Level;
                    response.Add(pp);
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }
        #endregion

        #region Problem Requests
        public List<LookUp> GetProblems(GetAllProblemsRequest request)
        {
            try
            {
                List<LookUp> response = new List<LookUp>();

                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/problems", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/problems",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<ProblemData> problems = dataDomainResponse.Problems;

                foreach (ProblemData c in problems)
                {
                    LookUp lookUp = new LookUp();
                    lookUp.Id = c.ID;
                    lookUp.Name = c.Name;
                    response.Add(lookUp);
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        #endregion

        #region Cohort Requests
        public List<Phytel.API.AppDomain.NG.DTO.Cohort> GetCohorts(NG.DTO.GetAllCohortsRequest request)
        {
            try
            {
                List<Phytel.API.AppDomain.NG.DTO.Cohort> response = new List<Phytel.API.AppDomain.NG.DTO.Cohort>();

                IRestClient client = new JsonServiceClient();

                GetAllCohortsDataResponse dataDomainResponse = client.Get<GetAllCohortsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Cohorts",
                                                                                                                DDCohortServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<CohortData> cohorts = dataDomainResponse.Cohorts;

                foreach (CohortData c in cohorts)
                {
                    Phytel.API.AppDomain.NG.DTO.Cohort cohort = new Phytel.API.AppDomain.NG.DTO.Cohort();
                    cohort.ID = c.ID;
                    cohort.SName = c.SName;
                    response.Add(cohort);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request, IRequestContext httpContext)
        {
            GetCohortPatientsResponse pResponse = new GetCohortPatientsResponse();
            pResponse.Patients = new List<CohortPatient>();

            try
            {
                IRestClient client = new JsonServiceClient();

                // call cohort data domain
                GetCohortPatientsDataResponse qResponse = client.Get<GetCohortPatientsDataResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip={5}&Take={6}&SearchFilter={7}",
                                                                                            DDPatientServiceURL,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.CohortID,
                                                                                            request.Skip,
                                                                                            request.Take,
                                                                                            request.SearchFilter));

                //take qResponse Patient details and map them to "Patient" in the GetCohortPatientsResponse
                qResponse.CohortPatients.ForEach(x => pResponse.Patients.Add(new CohortPatient
                {
                    Id = x.ID,
                    DOB = x.DOB,
                    FirstName = x.FirstName,
                    Gender = x.Gender,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    PreferredName = x.PreferredName,
                    Suffix = x.Suffix
                }));

                if (qResponse.Status != null)
                {
                    pResponse.Status = qResponse.Status;
                }

                return pResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }
        #endregion

        #region SettingRequest
        public GetAllSettingsResponse GetAllSettings(GetAllSettingsRequest request)
        {
            try
            {
                GetAllSettingsResponse response = new GetAllSettingsResponse();
                using (StreamReader r = new StreamReader(HostingEnvironment.MapPath("/Nightingale/settings.json")))
                {
                    using (MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(r.ReadToEnd())))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Setting>));
                        var settings = serializer.ReadObject(stream1) as List<Setting>;
                        response.Settings = settings;
                    }
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        } 
        #endregion

        public PutPatientDetailsUpdateResponse PutPatientDetailsUpdate(PutPatientDetailsUpdateRequest request)
        {
            try
            {
                PutPatientDetailsUpdateResponse response = new PutPatientDetailsUpdateResponse();

                IRestClient client = new JsonServiceClient();
                PutPatientDetailsUpdateResponse dataDomainResponse =
                    client.Put<PutPatientDetailsUpdateResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                DDPatientServiceURL,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.Id,
                                                                                request.Priority), new PutPatientDetailsUpdateRequest
                                                                                {
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    DOB = request.DOB,
                                                                                    FirstName = request.FirstName,
                                                                                    LastName = request.LastName,
                                                                                    Gender = request.Gender,
                                                                                    MiddleName = request.MiddleName,
                                                                                    PreferredName = request.PreferredName,
                                                                                    Priority = request.Priority,
                                                                                    Suffix = request.Suffix,
                                                                                    Token = request.Token,
                                                                                    UserId = request.UserId,
                                                                                    Version = request.Version,
                                                                                    Id = request.Id
                                                                                } as object);
                return dataDomainResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public PutPatientFlaggedUpdateResponse PutPatientFlaggedUpdate(PutPatientFlaggedUpdateRequest request)
        {
            try
            {
                PutPatientFlaggedUpdateResponse response = new PutPatientFlaggedUpdateResponse();

                IRestClient client = new JsonServiceClient();
                PutPatientFlaggedUpdateResponse dataDomainResponse =
                    client.Put<PutPatientFlaggedUpdateResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/flagged/{5}?UserId={6}",
                                                                                DDPatientServiceURL,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId,
                                                                                request.Flagged,
                                                                                request.UserId), new PutPatientFlaggedUpdateResponse { } as object);
                return dataDomainResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public GetActiveProgramsResponse GetActivePrograms(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse pResponse = new GetActiveProgramsResponse();

            try
            {
                IRestClient client = new JsonServiceClient();

                GetActiveProgramsResponse dataDomainResponse;
                try
                {
                    dataDomainResponse =
                        client.Get<GetActiveProgramsResponse>(
                        string.Format("{0}/{1}/{2}/{3}/Programs/Active",
                        DDProgramServiceUrl, 
                        "NG", 
                        request.Version,
                        request.ContractNumber));
                }
                catch(Exception ex)
                {
                    throw new WebServiceException(ex.Message, ex.InnerException);
                }

                pResponse.Programs = dataDomainResponse.Programs;
                pResponse.Version = "v1";
                return pResponse;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public PostPatientToProgramsResponse PostPatientToProgram(PostPatientToProgramsRequest request)
        {
            try
            {
                PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();

                IRestClient client = new JsonServiceClient();
                DD.PutProgramToPatientResponse dataDomainResponse =
                    client.Put<DD.PutProgramToPatientResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ContractProgramId), new DD.PutProgramToPatientRequest { UserId = request.UserId } as object);

                if (dataDomainResponse.program != null)
                {
                    response.Program = new DTO.ProgramInfo()
                    {
                        Id = dataDomainResponse.program.Id,
                        Name = dataDomainResponse.program.Name,
                        ProgramState = dataDomainResponse.program.ProgramState,
                        ShortName = dataDomainResponse.program.ShortName,
                        Status = dataDomainResponse.program.Status,
                         PatientId = dataDomainResponse.program.PatientId
                    };
                }

                response.Result = new DTO.Outcome()
                {
                    Reason = dataDomainResponse.Outcome.Reason,
                    Result = dataDomainResponse.Outcome.Result
                };

                if (dataDomainResponse.Status != null)
                    response.Status = dataDomainResponse.Status;

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public GetPatientProgramDetailsSummaryResponse GetPatientProgramDetailsSummary(GetPatientProgramDetailsSummaryRequest request)
        {
            try
            {
                GetPatientProgramDetailsSummaryResponse result = new GetPatientProgramDetailsSummaryResponse();

                IRestClient client = new JsonServiceClient();
                DD.GetProgramDetailsSummaryResponse resp =
                    client.Get<DD.GetProgramDetailsSummaryResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.PatientProgramId,
                    request.Token));

                if (resp != null)
                {
                    if (resp.Program != null)
                    {
                        result.Program = new Program
                        {
                            Id = resp.Program.Id.ToString(),
                            Client = resp.Program.Client,
                            Name = resp.Program.Name,
                            ContractProgramId = resp.Program.ContractProgramId.ToString(),
                            Description = resp.Program.Description,
                            EligibilityEndDate = resp.Program.EligibilityEndDate,
                            EligibilityRequirements = resp.Program.EligibilityRequirements,
                            EligibilityStartDate = resp.Program.EligibilityStartDate,
                            EndDate = resp.Program.EndDate,
                            PatientId = resp.Program.PatientId.ToString(),
                            ProgramState = resp.Program.ProgramState,
                            ShortName = resp.Program.ShortName,
                            StartDate = resp.Program.StartDate,
                            Status = resp.Program.Status,
                            Version = resp.Program.Version,
                            Completed = resp.Program.Completed,
                            Enabled = resp.Program.Enabled,
                            Next = resp.Program.Next,
                            Order = resp.Program.Order,
                            Previous = resp.Program.Previous,
                            AssignBy = resp.Program.AssignBy,
                            AssignDate = resp.Program.AssignDate,
                            ElementState = resp.Program.ElementState,
                            SpawnElement = GetSpawnElement(resp.Program),
                            SourceId = resp.Program.SourceId,
                             CompletedBy = resp.Program.CompletedBy,
                              DateCompleted = resp.Program.DateCompleted,
                            Modules = GetModuleInfo(resp, request),
                            ObjectivesInfo = resp.Program.ObjectivesInfo.Select(r => new Objective
                            {
                                Id = r.Id.ToString(),
                                Unit = r.Unit,
                                Status = r.Status,
                                Value = r.Value
                            }).ToList()
                        };

                        if (resp.Status != null)
                            result.Status = resp.Status;
                    }
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public GetPatientProgramsResponse GetPatientPrograms(GetPatientProgramsRequest request)
        {
            try
            {
                GetPatientProgramsResponse result = new GetPatientProgramsResponse();

                IRestClient client = new JsonServiceClient();
                DD.GetPatientProgramsResponse resp =
                    client.Get<DD.GetPatientProgramsResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Token));

                if (resp != null)
                {
                    if (resp.programs != null)
                    {
                        List<ProgramInfo> adPs = new List<ProgramInfo>();
                        resp.programs.ForEach(p => adPs.Add(new ProgramInfo
                        {
                            Id = p.Id,
                            Name = p.Name,
                            PatientId = p.PatientId,
                            ProgramState = p.ProgramState,
                            ShortName = p.ShortName,
                            Status = p.Status,
                            ElementState = p.ElementState
                        }));

                        result.Programs = adPs;
                        if (resp.Status != null)
                            result.Status = resp.Status;
                    }
                }
                return result;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private List<Module> GetModuleInfo(DD.GetProgramDetailsSummaryResponse resp, IAppDomainRequest request)
        {
            return resp.Program.Modules.Select(r => new Module
            {
                Id = r.Id,
                ProgramId = r.ProgramId,
                Description = r.Description,
                Name = r.Name,
                Status = (int)r.Status,
                Completed = r.Completed,
                Enabled = r.Enabled,
                Next = r.Next,
                Order = r.Order,
                Previous = r.Previous,
                SpawnElement = GetSpawnElement(r),
                SourceId = r.SourceId,
                AssignBy = r.AssignBy,
                AssignDate = r.AssignDate,
                ElementState = r.ElementState,
                CompletedBy = r.CompletedBy,
                DateCompleted = r.DateCompleted,
                Objectives = r.Objectives.Select(o => new Objective
                {
                    Id = o.Id.ToString(),
                    Value = o.Value,
                    Status = (int)o.Status,
                    Unit = o.Unit
                }).ToList(),
                Actions = GetActionsInfo(r, request)
            }).ToList();
        }

        private List<Actions> GetActionsInfo(DD.ModuleDetail r, IAppDomainRequest request)
        {
            List<Actions> action = null;
            action = r.Actions.Select(a => new Actions
            {
                CompletedBy = a.CompletedBy,
                Description = a.Description,
                Id = a.Id,
                ModuleId = a.ModuleId,
                Name = a.Name,
                Status = (int)a.Status,
                Completed = a.Completed,
                Enabled = a.Enabled,
                Next = a.Next,
                Order = a.Order,
                Previous = a.Previous,
                SpawnElement = GetSpawnElement(a),
                SourceId = a.SourceId,
                AssignBy = a.AssignBy,
                AssignDate = a.AssignDate,
                ElementState = a.ElementState,
                DateCompleted = a.DateCompleted,
                Objectives = a.Objectives.Select(x => new Objective
                {
                    Id = x.Id.ToString(),
                    Unit = x.Unit,
                    Status = (int)x.Status,
                    Value = x.Value
                }).ToList(),
                Steps = GetStepsInfo(a, request)
            }).ToList();

            return action;
        }

        private List<Step> GetStepsInfo(DD.ActionsDetail a, IAppDomainRequest request)
        {
            List<Step> steps = a.Steps.Select(s => new Step
            {
                Description = s.Description,
                Ex = s.Ex,
                Id = s.Id,
                SourceId = s.SourceId,
                ActionId = s.ActionId,
                Notes = s.Notes,
                Question = s.Question,
                Status = (int)s.Status,
                Title = s.Title,
                Text = s.Text,
                StepTypeId = s.StepTypeId,
                Completed = s.Completed,
                ControlType = s.ControlType,
                Enabled = s.Enabled,
                Header = s.Header,
                Next = s.Next,
                Order = s.Order,
                Previous = s.Previous,
                IncludeTime = s.IncludeTime,
                SelectType = s.SelectType,
                AssignBy = s.AssignBy,
                AssignDate = s.AssignDate,
                ElementState = s.ElementState,
                Responses = GetResponses(s, request),
                SelectedResponseId = s.SelectedResponseId,
                CompletedBy = s.CompletedBy,
                DateCompleted = s.DateCompleted,
                SpawnElement = GetSpawnElement(s)
            }).ToList();

            return steps;
        }

        private List<Response> GetResponses(DD.StepsDetail s, IAppDomainRequest request)
        {
            List<Response> resps = null;
            if (s.Responses != null && s.Responses.Count > 0)
            {
                resps = s.Responses.Select(z => new Response
                                            {
                                                Id = z.Id,
                                                NextStepId = z.NextStepId,
                                                Nominal = z.Nominal,
                                                Order = z.Order,
                                                Required = z.Required,
                                                StepId = z.StepId,
                                                Text = z.Text,
                                                Value = z.Value,
                                                SpawnElement = GetADSpawnElement(z.SpawnElement)
                                            }).ToList();
            }
            else
            {
                // get the responses for step.
                resps = new List<Response>();
                List<DD.StepResponse> rsps = PlanElementEndpointUtil.GetResponsesForStep(s.Id, request);
                rsps.ForEach(r => resps.Add(new Response
                {
                    Id = r.Id,
                    NextStepId = r.NextStepId,
                    Nominal = r.Nominal,
                    Order = r.Order,
                    Required = r.Required,
                    SpawnElement = GetADSpawnElement(r.Spawn),
                    StepId = r.StepId,
                    Text = r.Text,
                    Value = r.Value
                }));
            }
            return resps;
        }

        private List<SpawnElement> GetADSpawnElement(List<DD.SpawnElementDetail> sed)
        {
            try
            {
                List<SpawnElement> se = new List<SpawnElement>();
                if (sed != null)
                {
                    sed.ForEach(x =>
                    {
                        se.Add(new SpawnElement
                        {
                            ElementId = x.ElementId,
                            ElementType = x.ElementType,
                            Tag = x.Tag
                        });
                    });
                }
                return se;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        private List<SpawnElement> GetSpawnElement(DD.PlanElementDetail planElement)
        {
            List<SpawnElement> spawn = new List<SpawnElement>();

            if (planElement.SpawnElement != null)
            {
                spawn = planElement.SpawnElement.Select(s => new SpawnElement
                {
                    ElementId = s.ElementId,
                    ElementType = s.ElementType,
                    Tag = s.Tag
                }).ToList();
            }
            return spawn;
        }

        #region LookUp - ContactRelated
        public List<LookUp> GetAllCommModes(GetAllCommModesRequest request)
        {
            try
            {
                List<LookUp> response = new List<LookUp>();
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/commmodes", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse>(string.Format("{0}/{1}/{2}/{3}/commmodes",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<LookUpData> dataList  = dataDomainResponse.CommModes;

                foreach (LookUpData d in dataList)
                {
                    LookUp lookUp = new LookUp();
                    lookUp.Id = d.ID;
                    lookUp.Name = d.Name;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<StatesLookUp> GetAllStates(GetAllStatesRequest request)
        {
            try
            {
                List<StatesLookUp> response = new List<StatesLookUp>();
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/states", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllStatesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllStatesDataResponse>(string.Format("{0}/{1}/{2}/{3}/states",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<StateData> dataList = dataDomainResponse.States;

                foreach (StateData d in dataList)
                {
                    StatesLookUp lookUp = new StatesLookUp();
                    lookUp.Id = d.ID;
                    lookUp.Name = d.Name;
                    lookUp.Code = d.Code;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<LookUp> GetAllTimesOfDays(GetAllTimesOfDaysRequest request)
        {
            try
            {
                List<LookUp> response = new List<LookUp>();
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/timesOfDays", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllTimesOfDaysDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllTimesOfDaysDataResponse>(string.Format("{0}/{1}/{2}/{3}/timesOfDays",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<LookUpData> dataList = dataDomainResponse.TimesOfDays;

                foreach (LookUpData d in dataList)
                {
                    LookUp lookUp = new LookUp();
                    lookUp.Id = d.ID;
                    lookUp.Name = d.Name;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<CommTypeLookUp> GetAllCommTypes(GetAllCommTypesRequest request)
        {
            try
            {
                List<CommTypeLookUp> response = new List<CommTypeLookUp>();
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/commtypes", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllCommTypesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllCommTypesDataResponse>(string.Format("{0}/{1}/{2}/{3}/commtypes",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<CommTypeData> dataList = dataDomainResponse.CommTypes;

                foreach (CommTypeData d in dataList)
                {
                    CommTypeLookUp lookUp = new CommTypeLookUp();
                    lookUp.Id = d.ID;
                    lookUp.Name = d.Name;
                    lookUp.CommModes = d.CommModes;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<LanguagesLookUp> GetAllLanguages(GetAllLanguagesRequest request)
        {
            try
            {
                List<LanguagesLookUp> response = new List<LanguagesLookUp>();
                IRestClient client = new JsonServiceClient();
                // [Route("/{Context}/{Version}/{ContractNumber}/languages", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllLanguagesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllLanguagesDataResponse>(string.Format("{0}/{1}/{2}/{3}/languages",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<Phytel.API.DataDomain.LookUp.DTO.LanguageData> dataList = dataDomainResponse.Languages;

                if (dataList != null && dataList.Count > 0)
                {
                    // Get all active languages only.
                    List<Phytel.API.DataDomain.LookUp.DTO.LanguageData> activeLanguages = dataList.Where(d => d.Active == true).ToList();

                    foreach (Phytel.API.DataDomain.LookUp.DTO.LanguageData d in activeLanguages)
                    {
                        LanguagesLookUp lookUp = new LanguagesLookUp();
                        lookUp.Id = d.ID;
                        lookUp.Name = d.Name;
                        lookUp.Code = d.Code;
                        response.Add(lookUp);
                    }
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        public List<TimeZonesLookUp> GetAllTimeZones(GetAllTimeZonesRequest request)
        {
            try
            {
                List<TimeZonesLookUp> response = new List<TimeZonesLookUp>();
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/timeZones", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllTimeZonesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllTimeZonesDataResponse>(string.Format("{0}/{1}/{2}/{3}/timeZones",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<TimeZoneData> dataList = dataDomainResponse.TimeZones;

                foreach (TimeZoneData d in dataList)
                {
                    TimeZonesLookUp lookUp = new TimeZonesLookUp();
                    lookUp.Id = d.ID;
                    lookUp.Name = d.Name;
                    lookUp.DefaultZone = d.Default;
                    response.Add(lookUp);
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private TimeZonesLookUp getDefaultTimeZone(GetTimeZoneDataRequest request)
        {
            TimeZonesLookUp response = null;
            try
            {
                response = new TimeZonesLookUp();
                IRestClient client = new JsonServiceClient();
                //  [Route("/{Context}/{Version}/{ContractNumber}/TimeZone/Default", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse>(string.Format("{0}/{1}/{2}/{3}/TimeZone/Default",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                TimeZoneData data = dataDomainResponse.TimeZone;
                response.Id = data.ID;
                response.Name = data.Name;
                response.DefaultZone = data.Default;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
            return response;
        }

        private Contact insertContactForPatient(string context, string version, string contractNumber, string patientId, string userId)
        {

            Contact newContact = null;
            try
            {
                // Get the default TimeZone that is set in TimeZone LookUp table. 
                string defaultTimeZone = null;
                GetTimeZoneDataRequest tzDataRequest = new GetTimeZoneDataRequest { ContractNumber = contractNumber, Version = version };
                TimeZonesLookUp tz = getDefaultTimeZone(tzDataRequest);
                if (tz != null)
                {
                    defaultTimeZone = tz.Id;
                }
                
                //Get all the available comm modes in the lookup.
                List<CommModeData> commModeData = new List<CommModeData>();
                List<CommMode> commMode = new List<CommMode>();
                GetAllCommModesRequest commRequest = new GetAllCommModesRequest { ContractNumber  = contractNumber, UserId = userId, Version = version};
                List<LookUp> modesLookUp = GetAllCommModes(commRequest);
                if (modesLookUp != null && modesLookUp.Count > 0)
                {
                    foreach(LookUp l in modesLookUp)
                    {
                        commModeData.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                        commMode.Add(new CommMode { LookUpModeId = l.Id, OptOut = false, Preferred = false });
                    }
                }
                
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/Contact/{PatientId}", "PUT")]
                IRestClient client = new JsonServiceClient();
                PutContactDataResponse dataDomainResponse =
                    client.Put<PutContactDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/contact/{4}?UserId={5}",
                                                                                DDContactServiceUrl,
                                                                                context,
                                                                                version,
                                                                                contractNumber,
                                                                                patientId,
                                                                                userId), new PutContactDataRequest
                                                                                {
                                                                                    PatientId = patientId,
                                                                                    TimeZoneId = defaultTimeZone,
                                                                                    Modes = commModeData,
                                                                                    Context = context, 
                                                                                    ContractNumber = contractNumber, 
                                                                                    Version = version,
                                                                                    UserId = userId
                                                                                    
                                                                                } as object);

                if(dataDomainResponse != null && !string.IsNullOrEmpty(dataDomainResponse.ContactId))
                {
                    newContact = new Contact();
                    newContact.Id = dataDomainResponse.ContactId;
                    newContact.PatientId = patientId;
                    newContact.TimeZoneId = defaultTimeZone;
                    newContact.Modes = commMode;
                }
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
            return newContact;
        }
        #endregion

        #region Contact
        public Contact GetContactByPatientId(GetContactRequest request)
        {
            Contact contact = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Contact", "GET")]
                GetContactDataResponse dataDomainResponse;
                    dataDomainResponse =
                        client.Get<GetContactDataResponse>(
                        string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Contact?UserId={5}",
                        DDContactServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber,
                        request.PatientID,
                        request.UserId));

                if (dataDomainResponse != null)
                {
                    if (dataDomainResponse.Contact == null)
                    {
                        // Insert a new contact for that patient
                        contact = insertContactForPatient("NG", request.Version, request.ContractNumber, request.PatientID, request.UserId);
                    }
                    else
                    {
                        ContactData cd = dataDomainResponse.Contact;
                        contact = new Contact
                        {
                            Id = cd.ContactId,
                            PatientId = cd.PatientId,
                            UserId = cd.UserId,
                            FirstName = cd.FirstName,
                            MiddleName = cd.MiddleName,
                            LastName = cd.LastName,
                            PreferredName = cd.PreferredName,
                            Gender = cd.Gender,
                            TimeZoneId = cd.TimeZoneId,
                            WeekDays = cd.WeekDays,
                            TimesOfDaysId = cd.TimesOfDaysId
                        };

                        //Modes
                        List<CommModeData> commModeData = cd.Modes;
                        if (commModeData != null && commModeData.Count > 0)
                        {
                            List<CommMode> modes = new List<CommMode>();
                            foreach (CommModeData cm in commModeData)
                            {
                                CommMode commMode = new CommMode { LookUpModeId = cm.ModeId, OptOut = cm.OptOut, Preferred = cm.Preferred };
                                modes.Add(commMode);
                            }
                            contact.Modes = modes;
                        }

                        //Phones
                        List<PhoneData> phoneData = cd.Phones;
                        if (phoneData != null && phoneData.Count > 0)
                        {
                            List<Phone> phones = new List<Phone>();
                            foreach (PhoneData ph in phoneData)
                            {
                                Phone phone = new Phone { Id = ph.Id, TypeId = ph.TypeId, Number = ph.Number, IsText = ph.IsText, PhonePreferred = ph.PhonePreferred, TextPreferred = ph.TextPreferred, OptOut = ph.OptOut };
                                phones.Add(phone);
                            }
                            contact.Phones = phones;
                        }

                        //Emails
                        List<EmailData> emailData = cd.Emails;
                        if (emailData != null && emailData.Count > 0)
                        {
                            List<Email> emails = new List<Email>();
                            foreach (EmailData e in emailData)
                            {
                                Email email = new Email { Id = e.Id, Text = e.Text, TypeId = e.TypeId, Preferred = e.Preferred, OptOut = e.OptOut };
                                emails.Add(email);
                            }
                            contact.Emails = emails;
                        }

                        //Address
                        List<AddressData> addressData = cd.Addresses;
                        if (addressData != null && addressData.Count > 0)
                        {
                            List<Address> addresses = new List<Address>();
                            foreach (AddressData a in addressData)
                            {
                                Address address = new Address { Id = a.Id, Line1 = a.Line1, Line2 = a.Line2, Line3 = a.Line3, City = a.City, StateId = a.StateId, PostalCode = a.PostalCode, TypeId = a.TypeId, Preferred = a.Preferred, OptOut = a.OptOut };
                                addresses.Add(address);
                            }
                            contact.Addresses = addresses;
                        }

                        //Language
                        List<Phytel.API.DataDomain.Contact.DTO.LanguageData> languageData = cd.Languages;
                        if (languageData != null && languageData.Count > 0)
                        {
                            List<Language> languages = new List<Language>();
                            foreach (Phytel.API.DataDomain.Contact.DTO.LanguageData l in languageData)
                            {
                                Language language = new Language { LookUpLanguageId = l.LookUpLanguageId, Preferred = l.Preferred };
                                languages.Add(language);
                            }
                            contact.Languages = languages;
                        }
                    }
                }
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
            return contact;
        }

        public PutUpdateContactResponse PutUpdateContact(PutUpdateContactRequest request)
        {
            try
            {
                List<CommModeData> modesData = null;
                if (request.Contact.Modes != null)
                {
                    List<CommMode> modes = request.Contact.Modes;
                    modesData = new List<CommModeData>();
                    foreach (CommMode m in modes)
                    {
                        CommModeData cd = new CommModeData { ModeId = m.LookUpModeId, OptOut = m.OptOut, Preferred  = m.Preferred};
                        modesData.Add(cd);
                    }
                }

                List<PhoneData> phonesData = null;
                if (request.Contact.Phones != null)
                {
                    List<Phone> phones = request.Contact.Phones;
                    phonesData = new List<PhoneData>();
                    foreach (Phone p in phones)
                    {
                        PhoneData d = new PhoneData {  Id = p.Id,  IsText = p.IsText, Number = p.Number, OptOut = p.OptOut, PhonePreferred = p.PhonePreferred, TextPreferred = p.TextPreferred, TypeId =  p.TypeId};
                        phonesData.Add(d);
                    }
                }
                List<EmailData> emailsData = null;
                if (request.Contact.Emails != null)
                {
                    List<Email> emails = request.Contact.Emails;
                    emailsData = new List<EmailData>();
                    foreach (Email e in emails)
                    {
                        EmailData d = new EmailData {  Id = e.Id, Text = e.Text, TypeId = e.TypeId, OptOut = e.OptOut, Preferred =  e.Preferred};
                        emailsData.Add(d);
                    }
                }

                List<AddressData> addressesData = null;
                if (request.Contact.Addresses != null)
                {
                    List<Address> addresses = request.Contact.Addresses;
                    addressesData = new List<AddressData>();
                    foreach (Address a in addresses)
                    {
                        AddressData d = new AddressData { Id = a.Id, Line1 = a.Line1, Line2 = a.Line2, Line3 = a.Line3, City = a.City, StateId = a.StateId, PostalCode = a.PostalCode, OptOut = a.OptOut, Preferred = a.Preferred, TypeId =  a.TypeId};
                        addressesData.Add(d);
                    }
                }

                List<Phytel.API.DataDomain.Contact.DTO.LanguageData> languagesData = null;
                if (request.Contact.Languages != null)
                {
                    List<Language> langs = request.Contact.Languages;
                    languagesData = new List<Phytel.API.DataDomain.Contact.DTO.LanguageData>();
                    foreach (Language l in langs)
                    {
                        Phytel.API.DataDomain.Contact.DTO.LanguageData d = new Phytel.API.DataDomain.Contact.DTO.LanguageData {  LookUpLanguageId  = l.LookUpLanguageId, Preferred  = l.Preferred};
                        languagesData.Add(d);
                    }
                }


                PutUpdateContactResponse response = new PutUpdateContactResponse();
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/Contact", "PUT")]
                IRestClient client = new JsonServiceClient();
                PutUpdateContactDataResponse dataDomainResponse =
                    client.Put<PutUpdateContactDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/contact?UserId={4}",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.UserId), new PutUpdateContactDataRequest
                                                                                {
                                                                                   ContactId = request.Contact.Id,
                                                                                   Modes = modesData,
                                                                                   Phones = phonesData,
                                                                                   Emails = emailsData,
                                                                                   Addresses = addressesData,
                                                                                   WeekDays = request.Contact.WeekDays,
                                                                                   TimesOfDaysId = request.Contact.TimesOfDaysId,
                                                                                   Languages = languagesData,
                                                                                   TimeZoneId = request.Contact.TimeZoneId,
                                                                                   Context = "NG",
                                                                                   ContractNumber = request.ContractNumber,
                                                                                   Version = request.Version,
                                                                                   UserId = request.UserId
                                                                                } as object);
                
                response.Version = dataDomainResponse.Version;
                if (dataDomainResponse.SuccessData)
                {
                    // If the update was successful, send back the updated map for new phone inserts.
                    if (dataDomainResponse.UpdatedPhoneData != null && dataDomainResponse.UpdatedPhoneData.Count > 0)
                    {
                        List<CleanupId> updatedPhones = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedPhoneData)
                        {
                            updatedPhones.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedPhone = updatedPhones;
                    }

                    // If the update was successful, send back the updated map for new phone emails.
                    if (dataDomainResponse.UpdatedEmailData != null && dataDomainResponse.UpdatedEmailData.Count > 0)
                    {
                        List<CleanupId> updatedEmails = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedEmailData)
                        {
                            updatedEmails.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedEmail = updatedEmails;
                    }

                    // If the update was successful, send back the updated map for new address inserts.
                    if (dataDomainResponse.UpdatedAddressData != null && dataDomainResponse.UpdatedAddressData.Count > 0)
                    {
                        List<CleanupId> updatedAddresses = new List<CleanupId>();
                        foreach (CleanupIdData c in dataDomainResponse.UpdatedAddressData)
                        {
                            updatedAddresses.Add(new CleanupId { OldId = c.OldId, NewId = c.NewId });
                        }
                        response.UpdatedAddress = updatedAddresses;
                    }
                }
                else 
                {
                    response.Status = dataDomainResponse.Status;
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }
        #endregion


        public void LogAuditData(IAppDomainRequest request, HttpRequest webreq, string returnTypeName)
        {
            //hand to a new thread here, and immediately return this thread to caller
            try
            {
                //throw new SystemException("test before new thread starts");
                    new Thread(() =>
                                {
                                    try
                                    {
                                        AuditAsynch(request, webreq, returnTypeName);
                                     }   
                                    catch (Exception)
                                    {
                                        //if there's an error from the new thread, handle it here, so we don't black the main thread
                                        //drop into queue
                                        //write to log
                                        //email to admin
                                        //NLog?
                    
                                    }
                                    
                                }).Start();
                
            }
            catch (Exception)
            {
                //handle the exception here, to make sure we don't block the main thread

             }

            return;


        }

        private static void AuditAsynch(IAppDomainRequest request, HttpRequest webreq, string returnTypeName)
        {
            throw new SystemException("test error in new thread starts");

            string callingMethod = AuditHelper.FindMethodType(returnTypeName);
            int auditTypeId = AuditHelper.GetAuditTypeID(callingMethod);
            AuditData data = AuditHelper.GetAuditLog(auditTypeId, request, webreq, callingMethod);

            AuditDispatcher.WriteAudit(data);
        }


    }
}

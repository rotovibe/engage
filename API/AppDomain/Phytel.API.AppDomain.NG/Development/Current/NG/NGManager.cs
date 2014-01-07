using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Hosting;
using DataDomain.LookUp.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.CohortPatient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using Phytel.API.AppDomain.Audit.DTO;
using System.Net;
using Phytel.API.DataDomain.Program.DTO;
using System.Linq;

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
        protected static readonly string DDCohortPatientServiceUrl = ConfigurationManager.AppSettings["DDCohortPatientServiceUrl"];
        protected static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
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
                        Priority = (int)response.Patient.Priority,
                        Flagged = Convert.ToInt32(response.Patient.Flagged)
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
                //[Route("/{Context}/{Version}/{ContractNumber}/patientproblems/{PatientID}", "GET")]
                Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemsDataResponse>
                    (string.Format("{0}/{1}/{2}/{3}/patientproblems/{4}",
                        DDPatientProblemServiceUrl,
                        "NG",
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
        public List<ProblemLookUp> GetProblems(GetAllProblemsRequest request)
        {
            try
            {
                List<ProblemLookUp> response = new List<ProblemLookUp>();

                IRestClient client = new JsonServiceClient();

                Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllProblemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/problems",
                                                                                                                DDLookupServiceUrl,
                                                                                                                "NG",
                                                                                                                request.Version,
                                                                                                                request.ContractNumber));

                List<ProblemData> problems = dataDomainResponse.Problems;

                foreach (ProblemData c in problems)
                {
                    ProblemLookUp problemLookUp = new ProblemLookUp();
                    problemLookUp.ID = c.ProblemID;
                    problemLookUp.Name = c.Name;
                    response.Add(problemLookUp);
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
            pResponse.Patients = new List<Phytel.API.AppDomain.NG.DTO.Patient>();

            try
            {
                IRestClient client = new JsonServiceClient();

                // call cohort data domain
                GetCohortPatientsDataResponse qResponse = client.Get<GetCohortPatientsDataResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip={5}&Take={6}&SearchFilter={7}",
                                                                                            DDCohortPatientServiceUrl,
                                                                                            "NG",
                                                                                            request.Version,
                                                                                            request.ContractNumber,
                                                                                            request.CohortID,
                                                                                            request.Skip,
                                                                                            request.Take,
                                                                                            request.SearchFilter));

                //take qResponse Patient details and map them to "Patient" in the GetCohortPatientsResponse
                qResponse.CohortPatients.ForEach(x => pResponse.Patients.Add(new Phytel.API.AppDomain.NG.DTO.Patient
                {
                    Id = x.ID,
                    DOB = x.DOB,
                    FirstName = x.FirstName,
                    Gender = x.Gender,
                    LastName = x.LastName,
                    MiddleName = x.MiddleName,
                    PreferredName = x.PreferredName,
                    Suffix = x.Suffix,
                    Priority = 0 // need to enable if needed.
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

        public PutPatientDetailsUpdateResponse PutPatientPriorityUpdate(PutPatientDetailsUpdateRequest request)
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
                catch
                {
                    throw new ArgumentException("dataDomainResponse is coming back null.");
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
                PutProgramToPatientResponse dataDomainResponse =
                    client.Put<PutProgramToPatientResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ContractProgramId), new PutProgramToPatientRequest { UserId = request.UserId } as object);

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
                GetProgramDetailsSummaryResponse resp =
                    client.Get<GetProgramDetailsSummaryResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ProgramId,
                    request.Token));

                if (resp != null)
                {
                    if (resp.Program != null)
                    {
                        result.Program = new ProgramDetailNG
                        {
                            Id = resp.Program.Id.ToString(),
                            Client = resp.Program.Client,
                            ContractProgramId = resp.Program.ContractProgramId.ToString(),
                            Description = resp.Program.Description,
                            EligibilityEndDate = resp.Program.EligibilityEndDate,
                            EligibilityRequirements = resp.Program.EligibilityRequirements,
                            EligibilityStartDate = resp.Program.EligibilityStartDate,
                            EndDate = resp.Program.EndDate,
                            Modules = resp.Program.Modules.Select(r => new ModuleDetailNG
                            {
                                Id = r.Id.ToString(),
                                Description = r.Description,
                                Name = r.Name,
                                Status = (int)r.Status,
                                Objectives = r.Objectives.Select(o => new ObjectivesDetailNG
                                {
                                    Id = o.Id.ToString(),
                                    Value = o.Value,
                                    Status = (int)o.Status,
                                    Unit = o.Unit
                                }).ToList(),
                                Actions = r.Actions.Select(a => new ActionsDetailNG
                                {
                                    CompletedBy = a.CompletedBy,
                                    Description = a.Description,
                                    Id = a.Id.ToString(),
                                    Name = a.Name,
                                    Status = (int)a.Status,
                                    Objectives = a.Objectives.Select(x => new ObjectivesDetailNG
                                    {
                                        Id = x.Id.ToString(),
                                        Unit = x.Unit,
                                        Status = (int)x.Status,
                                        Value = x.Value
                                    }).ToList(),
                                    Steps = a.Steps.Select(s => new StepsDetailNG
                                    {
                                        Description = s.Description,
                                        Ex = s.Ex,
                                        Id = s.Id.ToString(),
                                        Notes = s.Notes,
                                        Question = s.Question,
                                        Status = (int)s.Status,
                                        T = s.T,
                                        Text = s.Text,
                                        Type = s.Type
                                    }).ToList()
                                }).ToList()
                            }).ToList(),
                            Name = resp.Program.Name,
                            ObjectivesInfo = resp.Program.ObjectivesInfo.Select(r => new ObjectivesDetailNG
                            {
                                Id = r.Id.ToString(),
                                Unit = r.Unit,
                                Status = r.Status,
                                Value = r.Value
                            }).ToList(),
                            PatientId = resp.Program.PatientId.ToString(),
                            ProgramState = resp.Program.ProgramState,
                            ShortName = resp.Program.ShortName,
                            StartDate = resp.Program.StartDate,
                            Status = resp.Program.Status,
                            Version = resp.Program.Version
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
    }
}

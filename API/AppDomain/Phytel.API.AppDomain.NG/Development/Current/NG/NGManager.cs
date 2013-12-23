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
                        PriorityId = (int)response.Patient.Priority,
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
            catch (Exception)
            {
                //SendAuditDispatch();
                throw;
            }
        }

        public List<NG.DTO.PatientProblem> GetPatientProblems(NG.DTO.GetAllPatientProblemsRequest request)
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
        #endregion

        #region Problem Requests
        public List<ProblemLookUp> GetProblems(GetAllProblemsRequest request)
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

        #endregion

        #region Cohort Requests
        public List<Phytel.API.AppDomain.NG.DTO.Cohort> GetCohorts(NG.DTO.GetAllCohortsRequest request)
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
                    PriorityId = 0 // need to enable if needed.
                }));

                if (qResponse.Status != null)
                {
                    pResponse.Status = qResponse.Status;
                }

                return pResponse;
            }
            catch (Exception)
            {
                //SendAuditDispatch(new PutAuditErrorRequest
                //{
                //    Browser = httpContext.GetHeader("User-Agent"),
                //    SourceIp = httpContext.IpAddress,
                //    Context = "NG",
                //    ContractNumber = request.ContractNumber,
                //    ErrorText = ex.Message,
                //    StackTrace = ex.StackTrace,
                //    Version = request.Version,
                //    SessionId = request.Token
                //});
                throw;
            }
        }
        #endregion

        public GetAllSettingsResponse GetAllSettings(GetAllSettingsRequest request)
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

        public PutPatientPriorityUpdateResponse PutPatientPriorityUpdate(PutPatientPriorityUpdateRequest request)
        {
            PutPatientPriorityUpdateResponse response = new PutPatientPriorityUpdateResponse();

            IRestClient client = new JsonServiceClient();
            PutPatientPriorityUpdateResponse dataDomainResponse = 
                client.Post<PutPatientPriorityUpdateResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/priority/{5}",
                                                                            DDPatientServiceURL,
                                                                            "NG",
                                                                            request.Version,
                                                                            request.ContractNumber,
                                                                            request.PatientId,
                                                                            request.Priority), new PutPatientPriorityUpdateResponse { } as object);
            return dataDomainResponse;
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
            catch (WebServiceException)
            {
                throw;
            }
        }


        public GetActiveProgramsResponse GetActivePrograms(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse pResponse = new GetActiveProgramsResponse();

            try
            {
                List<ProgramInfo> response = new List<ProgramInfo>();

                IRestClient client = new JsonServiceClient();

                GetActiveProgramsResponse dataDomainResponse =
                    client.Get<GetActiveProgramsResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Programs/Active", DDProgramServiceUrl, "NG", request.Version, request.ContractNumber));

                pResponse.Programs = dataDomainResponse.Programs;
                pResponse.Version = "v1";
                return pResponse;
            }
            catch (Exception)
            {
                //SendAuditDispatch();
                throw;
            }
        }

        public PostPatientToProgramsResponse PostPatientToProgram(PostPatientToProgramsRequest request)
        {
            try
            {
                PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();

                IRestClient client = new JsonServiceClient();
                PostPatientToProgramsResponse dataDomainResponse =
                    client.Post<PostPatientToProgramsResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}",
                    DDPatientServiceURL, 
                    "NG", 
                    request.Version, 
                    request.ContractNumber, 
                    request.PatientId, 
                    request.ContractProgramId), new PostPatientToProgramsResponse { } as object);
                return dataDomainResponse;
            }
            catch (WebServiceException)
            {
                throw;
            }
        }
    }
}

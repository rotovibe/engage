using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.CohortPatient.DTO;
using DataDomain.LookUp.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        protected static readonly string DDCohortServiceUrl = ConfigurationManager.AppSettings["DDCohortServiceUrl"];
        protected static readonly string DDCohortPatientServiceUrl = ConfigurationManager.AppSettings["DDCohortPatientServiceUrl"];
        protected static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        #endregion

        #region Patient Requests
        public NG.DTO.GetPatientResponse GetPatient(NG.DTO.GetPatientRequest request)
        {
            NG.DTO.GetPatientResponse pResponse = new NG.DTO.GetPatientResponse();
            
            //Execute call(s) to Patient Data Domain
            IRestClient client = new JsonServiceClient();

            Phytel.API.DataDomain.Patient.DTO.GetPatientDataResponse response = client.Get<Phytel.API.DataDomain.Patient.DTO.GetPatientDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                        DDPatientServiceURL,
                                                                                        "NG",
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.PatientID));

            if (response != null && response.Patient != null)
            {
                Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse sysResponse = null;

                if (string.IsNullOrEmpty(response.Patient.DisplayPatientSystemID) == false)
                {
                    sysResponse = client.Get<Phytel.API.DataDomain.PatientSystem.DTO.GetPatientSystemDataResponse>(string.Format("{0}/{1}/{2}/{3}/PatientSystem/{4}",
                                                                                DDPatientServiceURL,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                response.Patient.DisplayPatientSystemID));
                }

                pResponse.Patient = new NG.DTO.Patient
                {
                    ID = response.Patient.ID,
                    FirstName = response.Patient.FirstName,
                    LastName = response.Patient.LastName,
                    DOB = NGUtils.IsDateValid(response.Patient.DOB) ? response.Patient.DOB : string.Empty,
                    Gender = response.Patient.Gender,
                    MiddleName = response.Patient.MiddleName,
                    Suffix = response.Patient.Suffix,
                    PreferredName = response.Patient.PreferredName
                };

                if(sysResponse != null && sysResponse.PatientSystem != null)
                {
                    pResponse.Patient.DisplaySystemID = sysResponse.PatientSystem.SystemID;
                    pResponse.Patient.DisplaySystemName = sysResponse.PatientSystem.SystemName;
                }
            }

            //SendAuditDispatch();

            return pResponse;
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

            foreach(ProblemData c in problems)
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

            foreach(CohortData c in cohorts)
            {
                Phytel.API.AppDomain.NG.DTO.Cohort cohort = new Phytel.API.AppDomain.NG.DTO.Cohort();
                cohort.ID = c.ID;
                cohort.SName = c.SName;
                response.Add(cohort);
            }
            return response;
        }

        public GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse pResponse = new GetCohortPatientsResponse();
            pResponse.Patients = new List<Phytel.API.AppDomain.NG.DTO.Patient>();

            IRestClient client = new JsonServiceClient();

            // call cohort data domain
            GetCohortPatientsDataResponse qResponse = client.Get<GetCohortPatientsDataResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatients/{4}?Skip={5}&Take={6}",
                                                                                        DDCohortPatientServiceUrl,
                                                                                        "NG",
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.CohortID,
                                                                                        request.Skip,
                                                                                        request.Take));

            //take qResponse Patient details and map them to "Patient" in the GetCohortPatientsResponse
            qResponse.CohortPatients.ForEach(x => pResponse.Patients.Add(new Phytel.API.AppDomain.NG.DTO.Patient
            {
                ID = x.ID,
                DOB = x.DOB,
                FirstName = x.FirstName,
                Gender = x.Gender,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                PreferredName = x.PreferredName,
                Suffix = x.Suffix
            }));

            //SendAuditDispatch();

            return pResponse;
        }
        #endregion
    }
}

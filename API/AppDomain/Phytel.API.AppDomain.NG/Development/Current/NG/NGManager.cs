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
        #endregion

        #region Patient Requests
        public NG.DTO.GetPatientResponse GetPatient(NG.DTO.GetPatientRequest request)
        {
            NG.DTO.GetPatientResponse pResponse = new NG.DTO.GetPatientResponse();
            
            //Execute call(s) to Patient Data Domain
            IRestClient client = new JsonServiceClient();

            Phytel.API.DataDomain.Patient.DTO.PatientResponse response = client.Get<Phytel.API.DataDomain.Patient.DTO.PatientResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                        DDPatientServiceURL,
                                                                                        request.Context,
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.PatientID));
            if (response != null)
            {
                pResponse.Patient = new Patient
                {
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    PatientID = response.PatientID,
                    DOB = NGUtils.IsDateValid(response.DOB) ? response.DOB : string.Empty,
                    Gender = response.Gender,
                    MiddleName = response.MiddleName,
                    Suffix = response.Suffix,
                    PreferredName = response.PreferredName
                };
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
            ///{Context}/{Version}/{ContractNumber}/patientproblems"
            Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemResponse dataDomainResponse = client.Post<Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemResponse>
                (string.Format("{0}/{1}/{2}/{3}/patientproblems",
                    DDPatientProblemServiceUrl, 
                    request.Context, 
                    request.Version, 
                    request.ContractNumber),
                    new Phytel.API.DataDomain.PatientProblem.DTO.GetAllPatientProblemRequest
                    {
                        PatientID = request.PatientID,
                        Category = request.Category,
                        Status = request.Status,
                        Context = request.Context,
                        Version = request.Version,
                        ContractNumber = request.ContractNumber
                    }
                as object);

            List<Phytel.API.DataDomain.PatientProblem.DTO.PProb> problems = dataDomainResponse.PatientProblems;

            foreach (Phytel.API.DataDomain.PatientProblem.DTO.PProb p in problems)
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

            Phytel.API.DataDomain.LookUp.DTO.GetAllProblemResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllProblemResponse>(string.Format("{0}/{1}/{2}/{3}/problems",
                                                                                                            DDLookupServiceUrl,
                                                                                                            request.Context,
                                                                                                            request.Version,
                                                                                                            request.ContractNumber));

            List<Problem> problems = dataDomainResponse.Problems;

            foreach(Problem c in problems)
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
        public List<Phytel.API.AppDomain.NG.DTO.GetAllCohortsResponse> GetCohorts(Phytel.API.AppDomain.NG.DTO.GetAllCohortsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse pResponse = new GetCohortPatientsResponse();

            IRestClient client = new JsonServiceClient();

            // call cohort data domain
            CohortPatientDetailsResponse qResponse = client.Get<CohortPatientDetailsResponse>(string.Format("{0}/{1}/{2}/{3}/CohortPatientDetails/{4}?Skip={5}&Take={6}",
                                                                                        DDCohortPatientServiceUrl,
                                                                                        request.Context,
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.CohortID,
                                                                                        request.Skip,
                                                                                        request.Take));

            //take qResponse Patient details and map them to "Patient" in the GetCohortPatientsResponse

            //SendAuditDispatch();

            return pResponse;
        }
        #endregion
    }
}

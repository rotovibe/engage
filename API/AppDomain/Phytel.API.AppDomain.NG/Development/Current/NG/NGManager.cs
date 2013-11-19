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
using Phytel.API.DataDomain.CohortPatients.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        protected static readonly string DDCohortServiceUrl = ConfigurationManager.AppSettings["DDCohortServiceUrl"];
        #endregion

        public NG.DTO.PatientResponse GetPatientByID(NG.DTO.PatientRequest request)
        {
            NG.DTO.PatientResponse pResponse = new NG.DTO.PatientResponse();

            //Execute call(s) to Patient Data Domain
            IRestClient client = new JsonServiceClient();

            Phytel.API.DataDomain.Patient.DTO.PatientResponse response = client.Get<Phytel.API.DataDomain.Patient.DTO.PatientResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                                                        DDPatientServiceURL,
                                                                                        request.Context,
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.PatientID));
            pResponse.FirstName = response.FirstName;
            pResponse.LastName = response.LastName;
            pResponse.PatientID = response.PatientID;
            pResponse.DOB = NGUtils.IsDateValid(response.DOB) ? response.DOB : string.Empty;
            pResponse.Gender = response.Gender;
            pResponse.MiddleName = response.MiddleName;
            pResponse.Suffix = response.Suffix;
            pResponse.PreferredName = response.PreferredName;

            //SendAuditDispatch();

            return pResponse;
        }

        /// <summary>
        ///     Gets all active chronic problems for a patient
        /// </summary>
        /// <param name="request">PatientProblemRequest object</param>
        /// <returns>PatientProblem object</returns>
        public List<Phytel.API.AppDomain.NG.DTO.PatientProblem> GetProblemsByPatientID(Phytel.API.AppDomain.NG.DTO.PatientProblemRequest request)
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

        public List<ProblemLookUp> GetProblems(ProblemLookUpRequest request)
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


        public List<Phytel.API.AppDomain.NG.DTO.CohortResponse> GetCohorts(Phytel.API.AppDomain.NG.DTO.CohortRequest request)
        {
            throw new NotImplementedException();
        }

        public GetCohortPatientsResponse GetCohortPatients(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse pResponse = new GetCohortPatientsResponse();

            IRestClient client = new JsonServiceClient();

            // call cohort data domain
            CohortPatientDetailsResponse qResponse = client.Get<CohortPatientDetailsResponse>(string.Format("{0}/{1}/{2}/Contract/{3}/CohortPatientDetails/{4}?Skip={5}&Take={6}",
                                                                                        DDCohortServiceUrl,
                                                                                        request.Context,
                                                                                        request.Version,
                                                                                        request.ContractNumber,
                                                                                        request.CohortID,
                                                                                        request.Skip,
                                                                                        request.Take));

            //SendAuditDispatch();

            return pResponse;
        }
    }
}

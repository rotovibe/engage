using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
        #endregion

        public NG.DTO.PatientResponse GetPatientByID(NG.DTO.PatientRequest request)
        {
            NG.DTO.PatientResponse pResponse = new NG.DTO.PatientResponse();

            //Execute call(s) to Patient Data Domain
            IRestClient client = new JsonServiceClient();

            Phytel.API.DataDomain.Patient.DTO.PatientResponse response = client.Get<Phytel.API.DataDomain.Patient.DTO.PatientResponse>(string.Format("{0}/{1}/{2}/Contract/{3}/patient/{4}",
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
        public List<PatientProblem> GetProblemsByPatientID(Phytel.API.AppDomain.NG.DTO.PatientProblemRequest request)
        {
            if (string.IsNullOrEmpty(request.PatientID))
                throw new ArgumentException("PatientID is null or empty.");

            List<PatientProblem> response = new List<PatientProblem>();

           IRestClient client = new JsonServiceClient();
            ///{Context}/{Version}/Contract/{ContractNumber}/patientproblems"
            Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemsResponse dataDomainResponse = client.Post<Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemsResponse>
                (string.Format("{0}/{1}/{2}/Contract/{3}/patientproblems",
                    DDPatientProblemServiceUrl, 
                    request.Context, 
                    request.Version, 
                    request.ContractNumber),
                    new Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemRequest
                    {
                        PatientID = request.PatientID,
                        Category = request.Category,
                        Status = request.Status,
                        Context = request.Context,
                        Version = request.Version,
                        ContractNumber = request.ContractNumber
                    }
                as object);
                
            List<Problem> problems = dataDomainResponse.PatientProblems;

            foreach(Problem p in problems)
            {
                PatientProblem pp = new PatientProblem();
                pp.ID = p.ProblemID;
                pp.PatientID = p.PatientID;
                pp.ConditionID = p.ConditionID;
                response.Add(pp);
            }

            return response;
        }

        public List<LookUpCondition> GetConditions(LookUpConditionRequest request)
        {
            List<LookUpCondition> response = new List<LookUpCondition>();

            IRestClient client = new JsonServiceClient();

            Phytel.API.DataDomain.LookUp.DTO.ConditionsResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.ConditionsResponse>(string.Format("{0}/{1}/{2}/Contract/{3}/conditions",
                                                                                                            DDLookupServiceUrl,
                                                                                                            request.Context,
                                                                                                            request.Version,
                                                                                                            request.ContractNumber));

            List<Condition> conditions = dataDomainResponse.Conditions;

            foreach(Condition c in conditions)
            {
                LookUpCondition lookUpCondition = new LookUpCondition();
                lookUpCondition.ID = c.ConditionID;
                lookUpCondition.Name = c.DisplayName;
                response.Add(lookUpCondition);
            }

            return response;
        }


        public List<CohortResponse> GetCohorts(CohortRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

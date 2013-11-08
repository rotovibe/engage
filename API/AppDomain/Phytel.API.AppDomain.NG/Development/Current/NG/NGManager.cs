using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        protected static readonly string DDPatientProblemServiceUrl = ConfigurationManager.AppSettings["DDPatientProblemServiceUrl"];
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
            pResponse.DOB = response.DOB;
            pResponse.Gender = response.Gender;

            //SendAuditDispatch();

            return pResponse;
        }

        /// <summary>
        ///     Gets all active chronic problems for a patient
        /// </summary>
        /// <param name="request">PatientProblemRequest object</param>
        /// <returns>PatientProblem object</returns>
        public List<PatientProblem> GetProblemsByPatientID(PatientProblemRequest request)
        {
            List<PatientProblem> response = new List<PatientProblem>();

            //IRestClient client = new JsonServiceClient();

            //Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemResponse>(string.Format("{0}/{1}/{2}/Contract/{3}/patientproblem/{4}",
            //                                                                            DDPatientProblemServiceUrl,
            //                                                                            request.Context,
            //                                                                            request.Version,
            //                                                                            request.ContractNumber,
            //                                                                            request.PatientID));

            //dataDomainResponse.

            //foreach()
            //{
            //    response.DisplayName = response.FirstName;
            //    response.ProblemID = response.LastName;
            //    response.PatientID = response.PatientID;
            //}

            return response;
        }

    }
}

using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
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

            return pResponse;
        }

    }
}

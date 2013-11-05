using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.NG
{
    public static class NGManager
    {
        public static PatientResponse GetPatientByID(string patientID, string product, string contractID)
        {
            //Execute call(s) to Patient Data Domain
            IRestClient client = new JsonServiceClient();

            DataPatientResponse response = client.Post<DataPatientResponse>("http://localhost:8888/" + product + "/data/patient",
                new DataPatientRequest { PatientID = patientID, ContractID = contractID, Context = product } as object);

            return new PatientResponse { FirstName = response.FirstName, LastName = response.LastName, ID = response.PatientID };
        }

        public static bool IsUserValidated(string token)
        {
            bool result = false;
            IRestClient client = new JsonServiceClient();

            ValidateTokenResponse response = client.Post<ValidateTokenResponse>("http://localhost:999/api/security/Token",
                new ValidateTokenRequest { Token = token } as object);

            if (response.IsValid) result = true;

            return result;
        }
    }
}

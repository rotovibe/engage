using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.NG
{
    public class NGManager : ManagerBase
    {
        public PatientResponse GetPatientByID(string token, string patientID, string product, string contractID, out bool validated)
        {
            PatientResponse pResponse = new PatientResponse();
            // validate user against apiuser datastore
            bool result = IsUserValidated(token); 

            if (result)
            {
                //Execute call(s) to Patient Data Domain
                IRestClient client = new JsonServiceClient();

                DataPatientResponse response = client.Post<DataPatientResponse>("http://localhost:8888/" + product + "/data/patient",
                    new DataPatientRequest { PatientID = patientID, ContractID = contractID, Context = product } as object);

                pResponse.FirstName = response.FirstName;
                pResponse.LastName = response.LastName;
                pResponse.ID = response.PatientID;
                validated = true;
            }
            else
            {
                validated = false;
                pResponse.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("User Validation Failed", "User was not authenticated.");
            }

            return pResponse;
        }

    }
}

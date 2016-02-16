using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Patient_Priority_Test
    {
        [TestMethod]
        public void Update_Patient_Priority_By_PatientID()
        {
            // http://localhost:8888/Patient/NG/1.0/InHealth001/patient/999/priority/2
            string patientID = "528f6dc2072ef708ecd90e3a";
            string contractNumber = "InHealth001";
            string context ="NG";
            int priority = 1;
            double version = 1.0;
            string firstname = null;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutUpdatePatientDataResponse response = client.Put<PutUpdatePatientDataResponse>(
                string.Format(@"http://localhost:8888/Patient/{0}/{1}/{2}/patient/{3}", context, version, contractNumber, patientID),
                new PutUpdatePatientDataRequest
                {
                    Context = context,
                    ContractNumber = contractNumber,
                    PatientData = new PatientData {                     
                        PriorityData = 3,
                        DOB = "12-12-2013",
                        FirstName = "Reggie",
                        LastName = "Bobzilla",
                        Gender = "M",
                        PreferredName = "A manny",
                        Suffix = "mr",
                        MiddleName = "Ignacio"
                    },
                    UserId = "ba9b277d-4b53-4a53-a2c5-15d4969423ec"
                } as object);

        }
    }
}

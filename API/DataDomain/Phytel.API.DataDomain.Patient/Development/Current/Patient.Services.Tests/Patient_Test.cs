using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetPatientByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Post<GetPatientDataResponse>("http://localhost:8888/Patient/NG/data/patient",
                new GetPatientDataRequest { PatientID = patientID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = response.Patient.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void Get_Patient_with_Flag_ByID_and_UserID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientId = "528f6dc2072ef708ecd90e87";
            string userId = "BB241C64-A0FF-4E01-BA5F-4246EF50780E";
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Get<GetPatientDataResponse>(string.Format
                ("http://localhost:8888/Patient/NG/1.0/InHealth001/patient/{0}?UserId={1}",
                patientId,
                userId));

            sampleValue = response.Patient.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void Get_PatientByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientID = "528f6dc2072ef708ecd91040";
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Get<GetPatientDataResponse>("http://localhost:8888/Patient/NG/1.0/InHealth001/patient/" + patientID);

            sampleValue = response.Patient.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }


        [TestMethod]
        public void GetPatientDetailsListByID()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "5325c821072ef705080d3488";
            JsonServiceClient client = new JsonServiceClient();
            GetPatientsDataResponse response = client.Post<GetPatientsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/Patients", "http://localhost:8888/Patient", context, version, contractNumber),
                new GetPatientsDataRequest { PatientIds = new string[]{
                    "5325d9f2d6a4850adcbba4ca", "5325db50d6a4850adcbba8e6"}, UserId = userId
                } as object);
        }
    }
}

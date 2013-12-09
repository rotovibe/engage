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

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            GetPatientDataResponse response = client.Post<GetPatientDataResponse>("http://localhost:8888/Patient/NG/data/patient",
                new GetPatientDataRequest { PatientID = patientID, ContractNumber = contractNumber, Context = context } as object);

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

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            GetPatientDataResponse response = client.Get<GetPatientDataResponse>("http://localhost:8888/Patient/NG/v1/InHealth001/patient/" + patientID);

            sampleValue = response.Patient.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }


        [TestMethod]
        public void GetPatientDetailsListByID()
        {
            string patientID = "528b972f072ef70eec772872";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";

            JsonServiceClient client = new JsonServiceClient();
            GetPatientsDataResponse response = client.Post<GetPatientsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/patientdetails", "http://localhost:8888/Patient", context, version, contractNumber),
                new GetPatientsDataRequest { PatientIDs = new string[]{patientID} } as object);
        }
    }
}

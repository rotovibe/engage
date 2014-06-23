using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Update_Patient_Details_Test
    {
        [TestMethod]
        public void Update_Patient_Details_By_PatientID()
        {
            string patientID = "528f6dc2072ef708ecd90e3a";
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "52cc3dcbd6a4850cf4c0ce58";
            IRestClient client = new JsonServiceClient();

            PutPatientDetailsUpdateResponse response = client.Post<PutPatientDetailsUpdateResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/patient/Update/?Id={2}",
                version,
                contractNumber,
                patientID),
                new PutPatientDetailsUpdateRequest
                {
                    DOB = "12-12-2013",
                    Priority = 3,
                    PreferredName = "Samuel",
                    Token = token
                } as object);

            //PutPatientDetailsUpdateResponse response = client.Post<PutPatientDetailsUpdateResponse>(
            //    string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/patient/Update/?Id={2}",
            //    version,
            //    contractNumber,
            //    patientID),
            //    new PutPatientDetailsUpdateRequest
            //    {
            //        DOB = "12-12-2013",
            //        Priority = 1,
            //        PreferredName = "Sammy",
            //        Token = token
            //    } as object);
        }
    }
}

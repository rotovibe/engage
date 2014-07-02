using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Contact.Services.Test
{
    [TestClass]
    public class ContactTest_Service
    {
        [TestMethod]
        public void Get_Contact_By_Id_Response_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string ContactId = "5325c821072ef705080d3488";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", ContactId));

            GetContactByContactIdDataResponse response =
                client.Get<GetContactByContactIdDataResponse>(string.Format("{0}/{1}/{2}/{3}/Contact/{4}/?UserId={5}",
                "http://localhost:8888/Contact",
                context,
                version,
                contractNumber,
                ContactId,
                ContactId));
        }

        [TestMethod]
        public void DeleteContactByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba948";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Contact";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/Contact/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeleteContactByPatientIdDataResponse response = client.Delete<DeleteContactByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }
    }
}

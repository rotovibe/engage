using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Contact.Services.Test
{
    [TestClass]
    public class User_Contact_Test
    {
        [TestMethod]
        public void Post_ContactByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetContactResponse response = client.Post<GetContactResponse>(
                string.Format("{0}/{1}/{2}/{3}/Contact/{4}", url, context, version, contractNumber, ProgramID),
                new GetContactResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }
    }
}

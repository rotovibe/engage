using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientNote.Services.Test
{
    [TestClass]
    public class User_PatientNote_Test
    {
        [TestMethod]
        public void Post_PatientNoteByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetPatientNoteResponse response = client.Post<GetPatientNoteResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientNote/{4}", url, context, version, contractNumber, ProgramID),
                new GetPatientNoteResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }
    }
}

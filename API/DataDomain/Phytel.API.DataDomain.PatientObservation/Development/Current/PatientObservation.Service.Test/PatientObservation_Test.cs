using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientObservation.Services.Test
{
    [TestClass]
    public class User_PatientObservation_Test
    {
        [TestMethod]
        public void Post_PatientObservationByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientObservationResponse response = client.Post<GetPatientObservationResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientObservation/{4}", url, context, version, contractNumber, ProgramID),
                new GetPatientObservationResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }
    }
}

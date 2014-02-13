using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class User_PatientGoal_Test
    {
        [TestMethod]
        public void Post_PatientGoalByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetPatientGoalDataResponse response = client.Post<GetPatientGoalDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientGoal/{4}", url, context, version, contractNumber, ProgramID),
                new GetPatientGoalDataResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }
    }
}

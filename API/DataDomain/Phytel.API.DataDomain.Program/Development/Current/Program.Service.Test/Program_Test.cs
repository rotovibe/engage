using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Program_Service_Test
    {
        [TestMethod]
        public void Get_ProgramByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context ="NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramResponse response = client.Get<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/{4}", url, context, version, contractNumber, ProgramID));

            Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }

        [TestMethod]
        public void Post_ProgramByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramResponse response = client.Post<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/{4}", url, context, version, contractNumber, ProgramID), 
                new GetProgramResponse() as object);

            Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }

        [TestMethod]
        public void DeletePatientProgramByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325da6cd6a4850adcbba636";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Program";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/Program/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientProgramByPatientIdDataResponse response = client.Delete<DeletePatientProgramByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }
    }
}

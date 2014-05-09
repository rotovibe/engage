using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Patient_Program_Details_Test
    {
        [TestMethod]
        public void Get_ProgramDetails_For_Patient_Assignment()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "531f2dcc072ef727c4d29e1a";
            string ProgramID = "53208904fe7a592440a8ef64";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "5320885dd6a4850b34644db5";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramDetailsSummaryResponse response = client.Get<GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?ProgramId={6}&Token={7}", 
                url, 
                context, 
                version, 
                contractNumber,
                patientID,
                ProgramID,
                ProgramID,
                token));

            Assert.IsNotNull(response.Program);
        }

        [TestMethod]
        public void Get_Patient_Programs_summary()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "531f2dcc072ef727c4d29e1a";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "5320885dd6a4850b34644db5";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientProgramsResponse response = client.Get<GetPatientProgramsResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/",
                url,
                context,
                version,
                contractNumber,
                patientID,
                token));
        }
    }
}

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
            string patientID = "5325db94d6a4850adcbba9ae";
            string ProgramID = "538dda81d6a4850854eb5232";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "538ddbe8d6a485085491751b";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramDetailsSummaryResponse response = client.Get<GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?ProgramId={6}&Token={7}&UserId=nguser", 
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
        public void Get_ProgramDetails_For_Patient_Assignment_With_Module_Description()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "5325da98d6a4850adcbba6be";
            string ProgramID = "535aa59ad6a485044c4e4832";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "535aa534d6a485044cc9f2ee";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramDetailsSummaryResponse response = client.Get<GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?ProgramId={6}&Token={7}&UserId=nguser",
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
        public void Get_ProgramDetails_For_Patient_Assignment_With_Module_AssignBy()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "5325da98d6a4850adcbba6be";
            string ProgramID = "535aa59ad6a485044c4e4832";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "536799d6d6a485044ccae2ad";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramDetailsSummaryResponse response = client.Get<GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?ProgramId={6}&Token={7}&UserId=nguser",
                url,
                context,
                version,
                contractNumber,
                patientID,
                ProgramID,
                ProgramID,
                token));

            string ctrl = "";
            string smpl = response.Program.Modules[0].AssignBy;
        }

        [TestMethod]
        public void Get_Patient_Programs_summary()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "5325dabfd6a4850adcbba732";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "535aa534d6a485044cc9f2ee";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientProgramsResponse response = client.Get<GetPatientProgramsResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?UserId=nguser",
                url,
                context,
                version,
                contractNumber,
                patientID,
                token));
        }

        [TestMethod]
        public void GetPatientActionDetailsTest()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string userId = "000000000000000000000000";
            IRestClient client = new JsonServiceClient();

            GetPatientActionDetailsDataRequest request = new GetPatientActionDetailsDataRequest { PatientId = "5325d9f2d6a4850adcbba4ca", PatientProgramId = "534c4fb2d6a48504b053346f", PatientModuleId = "534c4fb2d6a48504b05335c2", PatientActionId = "534c4fb2d6a48504b05335c3", UserId = userId };

            // [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/Module/{PatientModuleId}/Action/{PatientActionId}", "GET")]
            GetPatientActionDetailsDataResponse response = client.Get<GetPatientActionDetailsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Module/{6}/Action/{7}?UserId={8}",
                url,
                context,
                version,
                contractNumber,
                request.PatientId,
                request.PatientProgramId,
                request.PatientModuleId,
                request.PatientActionId,
                request.UserId));

            Assert.IsNotNull(response);
        }
    }
}

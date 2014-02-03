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
            string patientID = "52e26f63072ef7191c11f7e6";
            string ProgramID = "52e294eed6a4850cf026ea21";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "52efa855d6a4850fb4f20fc3";
            IRestClient client = new JsonServiceClient();

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
            string patientID = "52e26f5b072ef7191c11e0b6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "52ec0a0fd6a4850b7861931a";
            IRestClient client = new JsonServiceClient();

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

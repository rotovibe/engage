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
            string patientID = "528f6cfa072ef708ecd68c94";
            string ProgramID = "52def1add6a4850fac29e3f0";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "52dedf06d6a4850faca910b4";
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
    }
}

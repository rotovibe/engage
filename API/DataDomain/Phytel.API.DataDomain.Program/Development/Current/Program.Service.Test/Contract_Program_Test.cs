using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Contract_Program_Test
    {
        [TestMethod]
        public void Get_AllActivePrograms()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context ="NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetProgramResponse response = client.Get<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Programs/Active", url, context, version, contractNumber, ProgramID));

            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }

        [TestMethod]
        public void Put_ContractProgramWithPatient()
        {
            string url = "http://localhost:8888/Program";
            string patientID = "528f6dc2072ef708ecd91cf7";
            string ContractProgramID = "52d7161d1e60154e2422c960";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            PutProgramToPatientResponse response = client.Put<PutProgramToPatientResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Programs/?ContractProgramId={5}", 
                url, 
                context, 
                version, 
                contractNumber,
                patientID,
                ContractProgramID), new PutProgramToPatientRequest() as object );

            Assert.AreEqual(response.Outcome.Result, 0);
            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }
    }
}

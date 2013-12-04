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
            string ProgramID = "529e534ffe7a5918c89ffd28";
            string contractNumber = "InHealth001";
            string context ="NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetProgramResponse response = client.Get<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/{4}", url, context, version, contractNumber, ProgramID));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Post_ProgramByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "529e534ffe7a5918c89ffd28";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetProgramResponse response = client.Post<GetProgramResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/{4}", url, context, version, contractNumber, ProgramID), 
                new GetProgramResponse() as object);

            Assert.IsNotNull(response);
        }
    }
}

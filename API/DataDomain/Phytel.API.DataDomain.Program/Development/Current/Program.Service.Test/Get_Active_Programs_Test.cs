using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Get_Active_Programs_Test
    {
        [TestMethod]
        public void Get_AllActivePrograms()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context ="NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetAllActiveProgramsResponse response = client.Get<GetAllActiveProgramsResponse>(
                string.Format("{0}/{1}/{2}/{3}/Programs/Active",
                url, 
                context, 
                version, 
                contractNumber));

            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }
    }
}

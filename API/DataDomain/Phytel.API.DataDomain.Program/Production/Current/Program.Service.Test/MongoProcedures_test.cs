using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class Mongo_Procedures_Test
    {
        [TestMethod]
        public void Execute_Procedure()
        {
            string url = "http://localhost:8888/Program";
            //string url = "http://azurephytel.cloudapp.net:59901/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;

            IRestClient client = new JsonServiceClient();
            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string Name = "mp_UpdateProgramStartDateToFirstActionStartDate";
            string DocumentVersion = "1.0";
            string UserId = "user";

            PostMongoProceduresResponse response = client.Get<PostMongoProceduresResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/Procedures/Execute/?Name={4}&DocumentVersion={5}&UserId={6}",
                url,
                context,
                version,
                contractNumber,
                Name,
                DocumentVersion,
                UserId
                ));
        }
    }
}

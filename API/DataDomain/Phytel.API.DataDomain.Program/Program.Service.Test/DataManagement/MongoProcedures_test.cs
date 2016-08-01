using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test.DataManagement
{
    [TestClass]
    public class Mongo_Procedures_Test
    {

        [TestMethod]
        public void UpdatePatientProgram_HTNAndDiabetesText_Test()
        {
            string procName = "mp_UpdatePatientProgram_HTNAndDiabetesText";
            Execute_Procedure(procName);
        }

        [TestMethod]
        public void UpdateProgramStartDateToFirstActionStartDate_Test()
        {
            string procName = "mp_UpdateProgramStartDateToFirstActionStartDate";
            Execute_Procedure(procName);
        }

        private void Execute_Procedure(string procName)
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;

            IRestClient client = new JsonServiceClient();
            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            //string Name = "mp_UpdateProgramStartDateToFirstActionStartDate";
            string DocumentVersion = "1.0";
            string UserId = "user";

            GetMongoProceduresResponse response = client.Get<GetMongoProceduresResponse>(
                string.Format("{0}/{1}/{2}/{3}/Program/Procedures/Execute/?Name={4}&DocumentVersion={5}&UserId={6}",
                url,
                context,
                version,
                contractNumber,
                procName,
                DocumentVersion,
                UserId
                ));


            //PostMongoProceduresRequest request = new PostMongoProceduresRequest
            //{
            //    Name = procName,
            //    DocumentVersion = 1.0,
            //    Context = "NG",
            //    ContractNumber = "InHealth001",
            //    Version = 1.0,
            //    UserId = "user"
            //};

            //PostMongoProceduresResponse response = client.Post<PostMongoProceduresResponse>(
            //    string.Format("{0}/{1}/{2}/{3}/Program/Procedures/Execute",
            //    url,
            //    context,
            //    version,
            //    contractNumber
            //    ), request);
        }
    }
}

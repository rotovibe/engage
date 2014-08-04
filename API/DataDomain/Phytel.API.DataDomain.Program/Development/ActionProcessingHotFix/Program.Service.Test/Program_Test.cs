using System.Collections.Generic;
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
            string patientId = "5325da59d6a4850adcbba5fa";
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

        [TestMethod]
        public void UndoDeletePatientProgram_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Program";
            IRestClient client = new JsonServiceClient();

            //    [Route("/{Context}/{Version}/{ContractNumber}/Program/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<DeletedPatientProgram> ids = new List<DeletedPatientProgram>();
            DeletedPatientProgram item1 = new DeletedPatientProgram();
            item1.Id = "53baa8ccd6a48515f8d74bc2";
            item1.PatientProgramAttributeId = "53baa8cdd6a48515f8d75280";
            item1.PatientProgramResponsesIds = new List<string> { "53baa8ccd6a48515f8d751eb", "53baa8ccd6a48515f8d751ec", "53baa8ccd6a48515f8d74dcc", "53baa8ccd6a48515f8d74dcd", "53baa8ccd6a48515f8d74dce" };
            ids.Add(item1);
            UndoDeletePatientProgramDataResponse response = client.Put<UndoDeletePatientProgramDataResponse>(url, new UndoDeletePatientProgramDataRequest 
            { 
                Context = context,
                ContractNumber = contractNumber,
                Ids = ids,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DeletePatientProgram_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string programId = "53d0349bd6a4850d589d67b1";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/Program";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/Program/{Id}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Program/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        programId), userId);
            DeletePatientProgramDataResponse response = client.Delete<DeletePatientProgramDataResponse>(url);
            Assert.IsNotNull(response);
        }
    }
}

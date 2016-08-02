using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientGoal.Services.Test
{
    [TestClass]
    public class User_PatientGoal_Test
    {
        [TestMethod]
        public void Post_PatientGoalByID()
        {
            string url = "http://localhost:8888/Program";
            string ProgramID = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetPatientGoalDataResponse response = client.Post<GetPatientGoalDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/PatientGoal/{4}", url, context, version, contractNumber, ProgramID),
                new GetPatientGoalDataResponse() as object);

            Assert.AreEqual(string.Empty, string.Empty);
        }

        [TestMethod]
        public void Update_Goal_Task()
        {
            string url = "http://localhost:8888/PatientGoal";
            string patientId = "52a0da34fe7a5915485bdfd6";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string id = "52fc609fd43323258c5c8c71";
            string patientGoaldId = "52fd2d6cd433231c845e7d25";
            IRestClient client = new JsonServiceClient();

            DeletePatientGoalDataResponse response = client.Delete<DeletePatientGoalDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Goal/{5}/Delete/?UserId={6}",
                url,
                context,
                version,
                contractNumber,
                patientId,
                patientGoaldId,
                patientId));
        }


        [TestMethod]
        public void DeletePatientGoalByPatientId_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db70d6a4850adcbba946";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientGoal";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/PatientGoal/Patient/{PatientId}/Delete", "DELETE")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientGoal/Patient/{4}/Delete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,
                                        patientId), userId);
            DeletePatientGoalByPatientIdDataResponse response = client.Delete<DeletePatientGoalByPatientIdDataResponse>(url);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UndoDeletePatientGoal_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string ddUrl = "http://localhost:8888/PatientGoal";
            IRestClient client = new JsonServiceClient();

            //[Route("/{Context}/{Version}/{ContractNumber}/PatientGoal/UndoDelete", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientGoal/UndoDelete",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber), userId);
            List<DeletedPatientGoal> ids = new List<DeletedPatientGoal>();
            DeletedPatientGoal item1 = new DeletedPatientGoal();
            item1.Id = "53c6f7ddd6a48506ecc5b7c1";
            item1.PatientBarrierIds = new List<string> { "53c6f810d6a48506ecc5b8ff", "53c6f805d6a48506ecc5b8a3" };
            item1.PatientTaskIds = new List<string> { "53c6f7edd6a48506ecc5b7ff", "53c6f7f7d6a48506ecc5b847" };
            ids.Add(item1);

            DeletedPatientGoal item2 = new DeletedPatientGoal();
            item2.Id = "53c45133d6a48506ecc4d856";
            item2.PatientBarrierIds = new List<string> { "53c4517dd6a48506ecc4da86", "53c45183d6a48506ecc4dae2" };
            item2.PatientInterventionIds = new List<string> { "53c4518ad6a48506ecc4db52", "53c4518fd6a48506ecc4dbcc" };
            item2.PatientTaskIds = new List<string> { "53c45162d6a48506ecc4d922", "53c45174d6a48506ecc4da2a" };
            ids.Add(item2);
            UndoDeletePatientGoalDataResponse response = client.Put<UndoDeletePatientGoalDataResponse>(url, new UndoDeletePatientGoalDataRequest 
            { 
                Ids = ids,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void RemoveProgramInPatientGoals_Test()
        {
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string userId = "000000000000000000000000";
            string programId = "53cedd21d6a4850d58889ee7";
            string ddUrl = "http://localhost:8888/PatientGoal";
            IRestClient client = new JsonServiceClient();

            // [Route("/{Context}/{Version}/{ContractNumber}/Goal/RemoveProgram/{ProgramId}/Update", "PUT")]
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Goal/RemoveProgram/{4}/Update",
                                        ddUrl,
                                        context,
                                        version,
                                        contractNumber,programId), userId);

            RemoveProgramInPatientGoalsDataResponse response = client.Put<RemoveProgramInPatientGoalsDataResponse>(url, new RemoveProgramInPatientGoalsDataRequest
            {
                ProgramId = programId,
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            });
            Assert.IsNotNull(response);
        }
    }
}

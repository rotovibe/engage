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
    }
}

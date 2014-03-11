using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientProblem.Service.Test
{
    [TestClass]
    public class PatientProblemServicesTest
    {
        [TestMethod]
        public void GetPatientProblem_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528bdccc072ef7071c2e22ae";
            IRestClient client = new JsonServiceClient();

            // Act
            GetAllPatientProblemsDataResponse response = client.Get<GetAllPatientProblemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/patientproblems/{4}", "http://localhost:8888/PatientProblem", context, version, contractNumber, patientID));

           // Assert
            Assert.AreNotEqual(0, response.PatientProblems.Count);
        }
    }
}

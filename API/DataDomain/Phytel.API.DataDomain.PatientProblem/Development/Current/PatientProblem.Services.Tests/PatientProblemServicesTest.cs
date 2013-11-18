using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientProblem.Services.Test
{
    [TestClass]
    public class PatientProblemServicesTest
    {
        [TestMethod]
        public void GetPatientProblemByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "527a933efe7a590ad417d3b0";
            IRestClient client = new JsonServiceClient();

            // Act
            PatientProblemsResponse response = client.Post<PatientProblemsResponse>(string.Format("{0}/{1}/{2}/{3}/patientproblems", "http://localhost:8888/PatientProblem", context, version, contractNumber),
                new PatientProblemRequest
                {
                    PatientID = patientID,
                    Category = "",
                    Status = "active",
                    Context = context,
                    Version = version,
                    ContractNumber = contractNumber
                }
            as object);

           // Assert
            Assert.AreNotEqual(0, response.PatientProblems.Count);
        }
    }
}

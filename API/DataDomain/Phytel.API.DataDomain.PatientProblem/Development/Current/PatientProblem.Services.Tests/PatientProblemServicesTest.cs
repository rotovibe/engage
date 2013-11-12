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
            ///{Context}/{Version}/Contract/{ContractNumber}/patientproblems"
            //PatientProblemsResponse response = client.POST<PatientProblemsResponse>("http://localhost:8888/PatientProblem", new PatientProblemRequest { 
            //    PatientID = patientID,
            //    Category = Category.Chronic.ToString(),
            //    Status = Status.Active.ToString(),
            //    Context = context,
            //    Version = version,
            //    ContractNumber = contractNumber}
            //as object);


            PatientProblemsResponse response = client.Post<PatientProblemsResponse>(string.Format("{0}/{1}/{2}/Contract/{3}/patientproblems", "http://localhost:8888/PatientProblem", context, version, contractNumber),
                new PatientProblemRequest
                {
                    PatientID = patientID,
                    Category = Category.Chronic,
                    Status = Status.Active,
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

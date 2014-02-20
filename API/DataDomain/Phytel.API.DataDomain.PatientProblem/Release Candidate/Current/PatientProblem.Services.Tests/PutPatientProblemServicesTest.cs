using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientProblem.Service.Test
{
    [TestClass]
    public class PutPatientProblemServicesTest
    {
        [TestMethod]
        public void Insert_Patient_Problem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528f6d46072ef708ecd78728";
            string problemID = "528a66ced4332317acc5095c";
            IRestClient client = new JsonServiceClient();

            // Act
            PutNewPatientProblemResponse response = client.Put<PutNewPatientProblemResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/Insert",
                "http://localhost:8888/PatientProblem",
                context,
                version,
                contractNumber,
                patientID),
                new PutNewPatientProblemRequest
                {
                    PatientId = patientID,
                    ProblemId = problemID,
                    Active = true,
                    Featured = true,
                    Level = 1,
                    Version = "v1"
                });

           // Assert
            Assert.IsNotNull(response.PatientProblem);
        }
    }
}

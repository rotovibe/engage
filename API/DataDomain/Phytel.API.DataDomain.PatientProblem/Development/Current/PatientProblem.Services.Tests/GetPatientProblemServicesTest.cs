using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientProblem.Service.Test
{
    [TestClass]
    public class GetPatientProblemServicesTest
    {
        [TestMethod]
        public void Get_Patient_Problem_True_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528f6d46072ef708ecd78728";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();

            // Act
            GetPatientProblemsDataResponse response = client.Get<GetPatientProblemsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}", 
                "http://localhost:8888/PatientProblem", 
                context, 
                version, 
                contractNumber,
                patientID,
                problemID));

           // Assert
            Assert.IsNotNull(response.PatientProblem);
        }

        [TestMethod]
        public void Get_Patient_Problem_False_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientID = "528f6d46072ef708ecd78711";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();

            // Act
            GetPatientProblemsDataResponse response = client.Get<GetPatientProblemsDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Problem/?ProblemId={5}",
                "http://localhost:8888/PatientProblem",
                context,
                version,
                contractNumber,
                patientID,
                problemID));

            // Assert
            Assert.IsNull(response.PatientProblem);
        }
    }
}

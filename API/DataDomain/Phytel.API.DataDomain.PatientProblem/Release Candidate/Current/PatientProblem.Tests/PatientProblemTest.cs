using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem.DTO;

namespace Phytel.API.DataDomain.PatientProblem.Test
{
    [TestClass]
    public class PatientProblemTest
    {
        [TestMethod]
        public void GetPatientProblemByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllPatientProblemsDataRequest request = new GetAllPatientProblemsDataRequest
            {
                PatientID = "528f6d46072ef708ecd7872f",
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };

            // Act
            GetAllPatientProblemsDataResponse response = DataPatientProblemManager.GetAllPatientProblem(request);

            // Assert
            Assert.IsTrue(response.PatientProblems.Count != 0);
        }

    }
}

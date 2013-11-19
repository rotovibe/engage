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
            GetAllPatientProblemRequest request = new GetAllPatientProblemRequest
            {
                PatientID = "528bdccc072ef7071c2e22ae",
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };

            // Act
            GetAllPatientProblemResponse response = DataPatientProblemManager.GetAllPatientProblem(request);

            // Assert
            Assert.IsTrue(response.PatientProblems.Count != 0);
        }

    }
}

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
            GetAllPatientProblemRequest request = new GetAllPatientProblemRequest { PatientID = "527a933efe7a590ad417d3b0",
                Category = "chronic",
                Status = "",
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };

            // Act
            GetAllPatientProblemResponse response = DataPatientProblemManager.GetProblemsByPatientID(request);

            // Assert
            Assert.IsTrue(response.PatientProblems.Count != 0);
        }

    }
}

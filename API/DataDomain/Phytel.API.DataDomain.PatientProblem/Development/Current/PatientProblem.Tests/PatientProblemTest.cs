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
            PatientProblemRequest request = new PatientProblemRequest { PatientID = "527a933efe7a590ad417d3b0",
                Category = Category.Chronic,
                Status = Status.Active,
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };

            // Act
            PatientProblemsResponse response = DataPatientProblemManager.GetProblemsByPatientID(request);

            // Assert
            Assert.IsTrue(response.PatientProblems.Count != 0);
        }

    }
}

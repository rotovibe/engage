using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Step.DTO;

namespace Phytel.API.DataDomain.Step.Test
{
    [TestClass]
    public class StepTest
    {
        [TestMethod]
        public void GetYesNoStepByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetYesNoStepDataRequest request = new GetYesNoStepDataRequest { YesNoStepID = "52a641e8d433231824878c8f", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetYesNoStepDataResponse response = StepDataManager.GetYesNoStepByID(request);

            // Assert
            Assert.IsTrue(response.YesNoStep.Question == "Are you an ABC Employee?");

        }

        [TestMethod]
        public void GetTextStepByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetTextStepDataRequest request = new GetTextStepDataRequest { TextStepID = "52a64270d433231824878c93", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetTextStepDataResponse response = StepDataManager.GetTextStepByID(request);

            // Assert
            Assert.IsTrue(response.TextStep.Description == "P4H Enrollment detail script and eligibility details");

        }
    }
}

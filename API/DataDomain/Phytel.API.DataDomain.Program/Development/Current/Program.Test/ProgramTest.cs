using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Test
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void GetProgramByID()
        {
            ProgramRequest request = new ProgramRequest{ ProgramID = "5"};

            ProgramResponse response = ProgramDataManager.GetProgramByID(request);

            Assert.IsTrue(response.ProgramID == "Tony");
        }
    }
}

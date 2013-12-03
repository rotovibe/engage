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
            GetProgramRequest request = new GetProgramRequest{ ProgramID = "5"};

            GetProgramResponse response = ProgramDataManager.GetProgramByID(request);

            Assert.IsTrue(response.Program.ProgramID == "Tony");
        }
    }
}

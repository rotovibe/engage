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

        [TestMethod]
        public void GetAllActiveContractPrograms_Test()
        {
            GetAllActiveProgramsRequest request = new GetAllActiveProgramsRequest();

            GetAllActiveProgramsResponse response = ProgramDataManager.GetAllActiveContractPrograms(request);

            Assert.IsTrue(response.Programs.Count > 0);
        }
    }
}

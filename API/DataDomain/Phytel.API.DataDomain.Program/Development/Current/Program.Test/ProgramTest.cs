using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.Test.Stubs;

namespace Phytel.API.DataDomain.Program.Test
{
    [TestClass]
    public class ProgramTest
    {
        private IProgramDataManager pm;
        [TestInitialize()]
        public void Initialize()
        {
            pm = new StubProgramDataManager();
        }

        [TestMethod]
        public void GetProgramByID()
        {
            GetProgramRequest request = new GetProgramRequest{ ProgramID = "5"};

            GetProgramResponse response = pm.GetProgramByID(request);

            Assert.IsTrue(response.Program.ProgramID == "Tony");
        }

        [TestMethod]
        public void GetAllActiveContractPrograms_Test()
        {
            GetAllActiveProgramsRequest request = new GetAllActiveProgramsRequest();

            GetAllActiveProgramsResponse response = pm.GetAllActiveContractPrograms(request);

            Assert.IsTrue(response.Programs.Count > 0);
        }

        [TestMethod]
        public void GetPatientActionDetailsTest()
        {
            string userId = "000000000000000000000000";
            GetPatientActionDetailsDataRequest request = new GetPatientActionDetailsDataRequest { PatientId = "5325d9f2d6a4850adcbba4ca", PatientProgramId = "534c4fb2d6a48504b053346f", PatientModuleId = "534c4fb2d6a48504b05335c2", PatientActionId = "534c4fb2d6a48504b05335c3", UserId = userId };

            GetPatientActionDetailsDataResponse response = pm.GetActionDetails(request);

            Assert.IsNotNull(response);
        }
    }
}

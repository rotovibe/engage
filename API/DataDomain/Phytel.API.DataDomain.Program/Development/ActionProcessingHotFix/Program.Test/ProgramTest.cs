using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
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
            GetPatientActionDetailsDataRequest request = new GetPatientActionDetailsDataRequest { PatientId = "5325da17d6a4850adcbba532", PatientProgramId = "535a90dbd6a485044cb7d90e", PatientModuleId = "535a90dbd6a485044cb7dac7", PatientActionId = "535a90dbd6a485044cb7dc24", UserId = userId };

            ProgramDataManager pm = new ProgramDataManager();
            
            GetPatientActionDetailsDataResponse response = pm.GetActionDetails(request);

            Assert.IsNotNull(response);
        }

        
        [TestMethod]
        public void PutProgramToPatientTest()
        {
            string userId = "000000000000000000000000";
            PutProgramToPatientRequest request = new PutProgramToPatientRequest { PatientId = "5325da17d6a4850adcbba532", CareManagerId = "5325c81f072ef705080d347e", ContractProgramId = "5330920da38116ac180009d2", UserId = userId };

            ProgramDataManager pm = new ProgramDataManager { Factory = new ProgramRepositoryFactory(), DTOUtility = new DTOUtility { Factory = new ProgramRepositoryFactory() } };
            
            PutProgramToPatientResponse response = pm.PutPatientToProgram(request);

            Assert.IsNotNull(response);
        }
    }
}

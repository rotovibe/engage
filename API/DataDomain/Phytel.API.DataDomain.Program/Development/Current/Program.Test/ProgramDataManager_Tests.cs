using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using System.Diagnostics;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using MongoDB.Bson;
using Phytel.API.Interface;
using Phytel.API.DataDomain.Program.Test.Stubs;
namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class ProgramDataManager_Tests
    {
        [TestClass()]
        public class GetPatientProgramDetailsById_Method
        {
            public string _programId;
            public string _patientId;
            
            [TestInitialize()]
            public void Initialize()
            {
                _programId = "535ab038d6a485044c502b28";
                _patientId = "5325db50d6a4850adcbba8e6";
            }

            [TestMethod()]
            public void Get_Valid_Program_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { Factory = new ProgramRepositoryFactory()};
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = _programId,
                    PatientId = _patientId,
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program);
            }

            [TestMethod()]
            public void Get_With_Program_Attributes_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { Factory = new StubProgramRepositoryFactory(), DTOUtility = new DTOUtility { Factory = new StubProgramRepositoryFactory() } };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = _programId,
                    PatientId = _patientId,
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program.Attributes);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1899")]
            public void DD_Get_With_Module_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";
//                ProgramDataManager pm = new ProgramDataManager { Factory = new ProgramRepositoryFactory(), DTOUtility = new DTOUtility { Factory = new ProgramRepositoryFactory() } };
                ProgramDataManager pm = new ProgramDataManager { Factory = new StubProgramRepositoryFactory(), DTOUtility = new StubDTOUtility { Factory = new StubProgramRepositoryFactory() } };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = _programId,
                    PatientId = _patientId,
                    UserId = "000000000000000000000000",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }

            [TestMethod()]
            public void Get_With_Objectives_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { Factory = new ProgramRepositoryFactory() };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = _programId,
                    PatientId = _patientId,
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program.ObjectivesData);
            }
        }

        [TestClass()]
        public class PutProgramActionUpdate_Method
        {
            [TestMethod()]
            public void PutProgramActionUpdate_Test()
            {
                //Assert.Fail();
            }
        }

        [TestClass()]
        public class GetProgramAttributes_Method
        {
            [TestMethod()]
            public void Processing_Time_Test()
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                ProgramDataManager dm = new ProgramDataManager { Factory = new ProgramRepositoryFactory() };
                IDataDomainRequest request = new GetPatientProgramsRequest { ContractNumber = "InHealth001", Context = "NG", UserId = "user" };
                ProgramAttributeData pad = dm.GetProgramAttributes("535808a7d6a485044cedecd6", request);
                st.Stop();
                int seconds = st.Elapsed.Milliseconds;
            }
        }

        [TestClass()]
        public class getLimitedProgramDetails_Method
        {
            [TestMethod()]
            public void Processing_Time_Test()
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                ProgramDataManager dm = new ProgramDataManager { Factory = new ProgramRepositoryFactory() };
                IDataDomainRequest request = new GetPatientProgramsRequest { ContractNumber = "InHealth001", Context = "NG", UserId = "user" };
                MEProgram p = dm.getLimitedProgramDetails("5330920da38116ac180009d2", request);
                st.Stop();
                int seconds = st.Elapsed.Milliseconds;
            }
        }

        [TestClass()]
        public class GetObjectivesData_Method
        {
            [TestMethod()]
            public void Get_With_One()
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                ProgramDataManager dm = new ProgramDataManager { Factory = new ProgramRepositoryFactory() };
                List<Objective> objs = new List<Objective> { new Objective{ Id= ObjectId.Parse("000000000000000000000000"), Status= Status.Active, Value = "Nanny", Units="lbs" } };
                List<ObjectiveInfoData> objl = dm.GetObjectivesData(objs);
                Assert.AreEqual("000000000000000000000000", objl[0].Id);
                st.Stop();
                int seconds = st.Elapsed.Milliseconds;
            }
        }
    }
}

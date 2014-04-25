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
namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class ProgramDataManager_Tests
    {
        [TestClass()]
        public class GetPatientProgramDetailsById_Method
        {
            [TestMethod()]
            public void Get_Valid_Program_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = "535595ddd6a485044c7f0cdb",
                    PatientId = "5325da5ed6a4850adcbba60a",
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
                ProgramDataManager pm = new ProgramDataManager { };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = "535595ddd6a485044c7f0cdb",
                    PatientId = "5325da5ed6a4850adcbba60a",
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program.Attributes);
            }

            [TestMethod()]
            public void Get_With_Objectives_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = "535595ddd6a485044c7f0cdb",
                    PatientId = "5325da5ed6a4850adcbba60a",
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
                Assert.Fail();
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
                ProgramDataManager dm = new ProgramDataManager();
                ProgramAttributeData pad = dm.GetProgramAttributes("535808a7d6a485044cedecd6", "InHealth001", "NG", "user");
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
                ProgramDataManager dm = new ProgramDataManager();
                MEProgram p = dm.getLimitedProgramDetails("5330920da38116ac180009d2", "InHealth001", "NG", "user");
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
                ProgramDataManager dm = new ProgramDataManager();
                List<Objective> objs = new List<Objective> { new Objective{ Id= ObjectId.Parse("000000000000000000000000"), Status= Status.Active, Value = "Nanny", Units="lbs" } };
                List<ObjectiveInfoData> objl = dm.GetObjectivesData(objs);
                Assert.AreEqual("000000000000000000000000", objl[0].Id);
                st.Stop();
                int seconds = st.Elapsed.Milliseconds;
            }
        }
    }
}

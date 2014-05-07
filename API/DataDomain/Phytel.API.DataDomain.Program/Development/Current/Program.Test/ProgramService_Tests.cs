using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Service.Tests
{
    [TestClass()]
    public class ProgramService_Tests
    {
        [TestClass()]
        public class GET_ProgramDetailsSummary
        {
            [TestMethod()]
            public void Get_Response_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                Assert.IsNotNull(response.Program);
            }

            [TestMethod()]
            public void Get_With_Description_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                Assert.IsNotNull(response.Program.Description);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1899")]
            public void DD_Get_With_Module_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_StartDate_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1900");

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                DateTime? mTime = module.AttrStartDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_EndDate_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1901");

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                DateTime? mTime = module.AttrEndDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_AssignedOn_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1999");

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                DateTime? mTime = module.AssignDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_AssignedTo_Test()
            {
                string ctrl = "123456789011111111112222";

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string smpl = module.AssignTo;
                Assert.AreEqual(ctrl, smpl);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_AssignedBy_Test()
            {
                string ctrl = "123456789011111111112223";

                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string smpl = module.AssignBy;
                Assert.AreEqual(ctrl, smpl);
            }

            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.Service")]
            public void DD_Get_Module_Objectives_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                List<ObjectiveInfoData> objs = module.Objectives;
                Assert.IsNotNull(objs);
            }

            [TestMethod()]
            public void Get_With_Eligibility_Requirements_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                string val = "Test eligibility requirements";
                Assert.AreEqual(val, response.Program.EligibilityRequirements);
            }

            [TestMethod()]
            public void Get_With_Eligibility_StartDate_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                DateTime val = System.DateTime.UtcNow;
                Assert.AreEqual(val, response.Program.EligibilityStartDate);
            }

            [TestMethod()]
            public void Get_With_Eligibility_EndDate_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                DateTime val = System.DateTime.UtcNow;
                Assert.AreEqual(val, response.Program.EligibilityEndDate);
            }

            [TestMethod()]
            public void Get_With_Objectives_Test()
            {
                ProgramService ps = new ProgramService
                {
                    ProgramDataManager = new StubProgramDataManager(),
                    Helpers = new StubHelper(),
                    CommonFormatterUtil = new StubCommonFormatterUtil()
                };

                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "",
                    ProgramId = "",
                    UserId = "nguser",
                    Version = 1.0
                };

                GetProgramDetailsSummaryResponse response = ps.Get(request);
                Assert.IsNotNull(response.Program.ObjectivesData);
            }
        }
    }
}

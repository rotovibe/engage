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
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_Attr_StartDate_Test() 
            {
                DateTime? time = Convert.ToDateTime("1/1/1900");
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
                DateTime? mTime = module.AttrStartDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_Attr_EndDate_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1901");
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
                DateTime? mTime = module.AttrEndDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_AssignedOn_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1999");
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
                DateTime? mTime = module.AssignDate;
                Assert.AreEqual(time, mTime);
            }

            // assignedby

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_AssignedTo_Test()
            {
                string assnTC = "123456789011111111112222";
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
                string assnT = module.AssignTo;
                Assert.AreEqual(assnTC, assnT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_AssignedBy()
            {
                string ctrl = "123456789011111111112223";
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
                string smpl = module.AssignBy;
                Assert.AreEqual(ctrl, smpl);
            }

            #region Actions attributes night-921
            [TestMethod()]
            [TestCategory("NIGHT-921")]
            [TestProperty("TFS", "4957")]
            public void DD_Get_With_Action_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment action description";
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                string mDesc = action.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_Attr_StartDate_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1800");
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                DateTime? mTime = action.AttrStartDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_Attr_EndDate_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1801");
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                DateTime? mTime = action.AttrEndDate;
                Assert.AreEqual(time, mTime);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_AssignedOn_Test()
            {
                DateTime? time = Convert.ToDateTime("1/1/1899");
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                DateTime? mTime = action.AssignDate;
                Assert.AreEqual(time, mTime);
            }

            // assignedby

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_AssignedTo_Test()
            {
                string assnTC = "123456789011111111112232";
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                string assnT = action.AssignTo;
                Assert.AreEqual(assnTC, assnT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_AssignedBy()
            {
                string ctrl = "123456789011111111112233";
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                string smpl = action.AssignBy;
                Assert.AreEqual(ctrl, smpl);
            }
            #endregion 

            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Module_Objectives()
            {
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
                Assert.IsNotNull(module.Objectives);
            }

            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6109")]
            [TestProperty("Layer", "DD.DataManager")]
            public void DD_Get_With_Action_Objectives()
            {
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
                ActionsDetail action = module.Actions.Find(a => a.SourceId == "123456789012345678901234");
                List<ObjectiveInfoData> objs = action.Objectives;
                Assert.IsNotNull(objs);
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
                IDataDomainRequest request = new GetPatientProgramsDataRequest { ContractNumber = "InHealth001", Context = "NG", UserId = "user" };
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
                IDataDomainRequest request = new GetPatientProgramsDataRequest { ContractNumber = "InHealth001", Context = "NG", UserId = "user" };
                //MEProgram p = dm.getLimitedProgramDetails("5330920da38116ac180009d2", request);
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
                //List<ObjectiveInfoData> objl = dm.GetObjectivesData(objs);
                //Assert.AreEqual("000000000000000000000000", objl[0].Id);
                st.Stop();
                int seconds = st.Elapsed.Milliseconds;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program.Test.Stubs;
namespace Phytel.API.DataDomain.Program.MongoDB.DTO.Tests
{
    [TestClass()]
    public class DTOUtility_Test
    {
        const string control = "5368ff2ad4332316288f3e3e";

        [TestClass()]
        public class GetModules
        {
            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_StartDate()
            {
                DateTime? sD = Convert.ToDateTime("1/1/1900");
                DTOUtility util = new DTOUtility() {Factory = new StubProgramRepositoryFactory()};
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods =
                    new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>()
                    {
                        new Phytel.API.DataDomain.Program.MongoDB.DTO.Action
                        {
                            Id = ObjectId.Parse("000000000000000000000000"),
                            State = ElementState.InProgress,
                            Name = "test action from stub",
                            Description = "test action 1"
                        }
                    },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901")
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                DateTime? startD = mds[0].AttrStartDate;
                Assert.AreEqual(sD, startD);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_EndDate()
            {
                DateTime? eD = Convert.ToDateTime("1/1/1901");
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901")
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                DateTime? endD = mds[0].AttrEndDate;
                Assert.AreEqual(eD, endD);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedOn()
            {
                DateTime? assOn = Convert.ToDateTime("1/1/1999");
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999")
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                DateTime? assD = mds[0].AssignDate;
                Assert.AreEqual(assOn, assD);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedTo()
            {
                string asT = "123456789011111111112222";
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222")
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                string assT = mds[0].AssignTo;
                Assert.AreEqual(asT, assT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-919")]
            [TestProperty("TFS", "3838")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_AssignedBy()
            {
                string asT = "123456789011111111112223";
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222"),
                    AssignedBy = ObjectId.Parse("123456789011111111112223")
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                string assT = mds[0].AssignBy;
                Assert.AreEqual(asT, assT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_Objectives()
            {
                string asT = "123456789011111111112223";
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                util.Factory = new StubProgramRepositoryFactory();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222"),
                    AssignedBy = ObjectId.Parse("123456789011111111112223"),
                    Objectives = new List<Objective> { 
                            new Objective{ 
                                Id = ObjectId.Parse("123456789012345678901234"), 
                                Value = "testing", 
                                Units = "lbs", 
                                Status =  Status.Active} }
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                List<ObjectiveInfoData> assT = mds[0].Objectives;
                Assert.IsNotNull(assT);
            }

            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Module_Objectives_One_Active()
            {
                string asT = "123456789011111111112223";
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                util.Factory = new StubProgramRepositoryFactory();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> mods = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();

                mods.Add(new Phytel.API.DataDomain.Program.MongoDB.DTO.Module
                {
                    Id = ObjectId.Parse("000000000000000000000000"),
                    Name = "Test stub module 1",
                    Description = "BSHSI - Outreach & Enrollment",
                    SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                    Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"),  
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "test action 1"} },
                    AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                    AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                    AssignedOn = Convert.ToDateTime("1/1/1999"),
                    AssignedTo = ObjectId.Parse("123456789011111111112222"),
                    AssignedBy = ObjectId.Parse("123456789011111111112223"),
                    Objectives = new List<Objective> { 
                            new Objective{ 
                                Id = ObjectId.Parse("123456789012345678901234"), 
                                Value = "testing", 
                                Units = "lbs", 
                                Status =  Status.Active} }
                });

                var request = new DataDomainRequest
                {
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    UserId = "000000000000000000000000"
                };

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", request);
                List<ObjectiveInfoData> assT = mds[0].Objectives;
                Assert.AreEqual(1, assT.Count);
            }
        }

        [TestClass()]
        public class GetObjectivesForModule_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_Objectives_One_Active()
            {
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                List<Module> mods = new List<Module> { 
                    new Module{
                     Id = ObjectId.GenerateNewId(),
                     SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                     Name = "testmodule",
                     Objectives = new List<Objective>{ new Objective{ Id=ObjectId.GenerateNewId(), Status = Status.Active, Value = "90", Units="lbs"},
                     new Objective{ Id=ObjectId.GenerateNewId(), Status = Status.Inactive, Value = "99", Units="hdl"}}
                    }
                };
                List<ObjectiveInfoData> objs = util.GetObjectivesForModule(mods, ObjectId.Parse("532b5585a381168abe00042c"));

                Assert.AreEqual(1, objs.Count);
            }
        }


        [TestClass()]
        public class GetFromProgramObjectives
        {
            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_One_Active_Objective()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest { ContractNumber = "InHealth001", UserId = "000000000000000000000000" };
                List<ObjectiveInfoData> odata = dtoUtil.GetFromProgramObjectives("000000000000000000000000", request);
                Assert.AreEqual(1, odata.Count);
            }
        }

        [TestClass()]
        public class GetActions
        {
            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_Valid_Actions_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!" });

                var request = new DataDomainRequest { ContractNumber= "InHealth001", UserId="123456789012345678901234" };
                List<ActionsDetail> aDetails = dtoUtil.GetActions(acts, request, new Module());
                Assert.AreEqual(acts[0].Id.ToString(), aDetails[0].Id);
            }

            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_One_Objectives_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                ObjectId act1 = ObjectId.GenerateNewId();
                ObjectId actSourceId = ObjectId.GenerateNewId();
                ObjectId obj1 = ObjectId.GenerateNewId();
                ObjectId obj2 = ObjectId.GenerateNewId();
                ObjectId modid = ObjectId.GenerateNewId();

                Module mod = new Module
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "test Module",
                    SourceId = modid,
                    Actions = new List<Action>{
                     new Action{
                         Id = ObjectId.GenerateNewId(),
                        SourceId = actSourceId,
                        Objectives = new List<Objective> { 
                            new Objective{ Id = obj1, Status = Status.Inactive, Units = "oz", Value ="50"},
                            new Objective{ Id = obj2, Status = Status.Active, Units = "oz", Value ="9"}}
                     }
                    }
                };

                acts.Add(new Action { Id = act1, SourceId = actSourceId, Name = "Test Action", Description = "test action!!!" });
                var request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };
                List<ActionsDetail> aDetails = dtoUtil.GetActions(acts, request, mod);
                Assert.AreEqual(1, aDetails[0].Objectives.Count);
            }
        }

        [TestClass()]
        public class GetAction
        {
            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_Valid_Action_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!" });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);
                Assert.AreEqual(acts[0].Id.ToString(), aDetail.Id);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_AssignTo_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                ObjectId? ctrl = ObjectId.GenerateNewId();
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!", AssignedTo = ctrl });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);

                string sample = acts[0].AssignedTo.ToString();
                Assert.AreEqual(sample, ctrl.ToString());
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_AssignBy_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                ObjectId? ctrl = ObjectId.GenerateNewId();
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!", AssignedBy = ctrl });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);

                string sample = acts[0].AssignedBy.ToString();
                Assert.AreEqual(sample, ctrl.ToString());
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_AssignDate_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                DateTime? ctrl = System.DateTime.UtcNow;
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!", AssignedOn = ctrl });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);

                DateTime? sample = acts[0].AssignedOn;
                Assert.AreEqual(sample, ctrl);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_Attr_StartDate_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                DateTime? ctrl = System.DateTime.UtcNow;
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!", AttributeStartDate = ctrl });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);

                DateTime? sample = acts[0].AttributeStartDate;
                Assert.AreEqual(sample, ctrl);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_With_Attr_EndDate_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                DateTime? ctrl = System.DateTime.UtcNow;
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name = "TestAction", Description = "test action!!!", AttributeEndDate = ctrl });
                ActionsDetail aDetail = dtoUtil.GetAction("InHealth001", "123456789012345678901234", acts[0]);

                DateTime? sample = acts[0].AttributeEndDate;
                Assert.AreEqual(sample, ctrl);
            }
        }

        [TestClass()]
        public class GetObjectives
        {
            [TestMethod()]
            public void Get_With_One_Objective()
            {
                List<Objective> list = new List<Objective>
                {
                    new Objective{ Id = ObjectId.GenerateNewId(), Status = Status.Active, Value = "57", Units = "hdl"},
                    new Objective{ Id = ObjectId.GenerateNewId(), Status = Status.Inactive, Value = "120", Units = "hdl"}
                };

                List<ObjectiveInfoData> objList = new DTOUtility().GetObjectives(list);
                Assert.AreEqual(1, objList.Count);
            }
        }

        [TestClass()]
        public class GetTemplateObjectives
        {
            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_Action_Template_Objectives()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                ObjectId act1 = ObjectId.GenerateNewId();
                ObjectId actSourceId = ObjectId.GenerateNewId();
                ObjectId obj1 = ObjectId.GenerateNewId();
                ObjectId obj2 = ObjectId.GenerateNewId();
                ObjectId modid = ObjectId.GenerateNewId();

                Module mod = new Module
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "test Module",
                    SourceId = modid,
                    Actions = new List<Action>{
                     new Action{
                         Id = ObjectId.GenerateNewId(),
                        SourceId = actSourceId,
                        Objectives = new List<Objective> { 
                            new Objective{ Id = obj1, Status = Status.Inactive, Units = "oz", Value ="50"},
                            new Objective{ Id = obj2, Status = Status.Active, Units = "oz", Value ="9"}}
                     }
                    }
                };

                acts.Add(new Action { Id = act1, SourceId = actSourceId, Name = "Test Action", Description = "test action!!!" });
                List<Objective> objs = dtoUtil.GetTemplateObjectives(acts[0].SourceId, mod);
                Assert.AreEqual(2, objs.Count);
            }
        }

        [TestClass()]
        public class GetTemplateModulesList
        {
            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6100")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_One_Module_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                string contractProgId = "123456789012345678901234";
                List<Module> tMod = dtoUtil.GetTemplateModulesList(contractProgId, "InHealth001", "nguser");
                Assert.AreEqual(1, tMod.Count);
            }
        }

        [TestClass()]
        public class CreateInitialMEPatientProgram_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-832")]
            [TestProperty("TFS", "11137")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Create_With_AssignedBy()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111"
                };

                MEProgram program = new MEProgram(userid)
                {
                     Id = ObjectId.GenerateNewId()
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };
                
                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);
                
                Assert.AreEqual(userid, mepp.AssignedBy.ToString());
            }

            [TestMethod()]
            [TestCategory("NIGHT-831")]
            [TestProperty("TFS", "11171")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Create_With_AssignedOn()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                DateTime now = System.DateTime.UtcNow.Date;

                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111"
                };

                MEProgram program = new MEProgram(userid)
                {
                    Id = ObjectId.GenerateNewId()
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };

                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);

                Assert.AreEqual(now, ((DateTime)mepp.AssignedOn).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Create_With_StateChangedOn()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                DateTime now = System.DateTime.UtcNow.Date;

                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111"
                };

                MEProgram program = new MEProgram(userid)
                {
                    Id = ObjectId.GenerateNewId(),
                    StateUpdatedOn = now
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };

                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);

                Assert.AreEqual(now, ((DateTime)mepp.AssignedOn).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_CM_AssignToType_PCM()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                string cmid = "123456789055554444441234";

                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111",
                    CareManagerId = cmid
                };

                MEProgram program = new MEProgram(userid)
                {
                    Id = ObjectId.GenerateNewId(),
                    AssignToType = AssignToType.PCM
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };

                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);
                string result = mepp.AssignedTo.ToString();

                Assert.AreEqual(cmid, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_CM_AssignToType_Unassigned()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                string cmid = "123456789055554444441234";

                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111",
                    CareManagerId = cmid
                };

                MEProgram program = new MEProgram(userid)
                {
                    Id = ObjectId.GenerateNewId(),
                    AssignToType = AssignToType.Unassigned
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };

                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);
                ObjectId? result = mepp.AssignedTo;

                Assert.IsNull(result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_Action_CM_AssignTo_Assigned_From_Program()
            {
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                string userid = "123456789012345678901234";
                string cmid = "123456789055554444441234";

                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    UserId = userid,
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901111",
                    CareManagerId = cmid
                };

                ObjectId assignedTo = ObjectId.GenerateNewId();

                MEProgram program = new MEProgram(userid)
                {
                    Id = ObjectId.GenerateNewId(),
                    AssignedTo = assignedTo,
                    Modules =
                        new List<Module>
                        {
                            new Module
                            {
                                Id = ObjectId.GenerateNewId(),
                                Enabled = true,
                                Status = Program.DTO.Status.Active,
                                Actions = new List<Action>
                                {
                                    new Action
                                    {
                                        Status = Program.DTO.Status.Active,
                                        Enabled = true,
                                        Id = ObjectId.GenerateNewId()
                                    }
                                }
                            }
                        }
                };

                List<ObjectId> sil = new List<ObjectId>() { ObjectId.GenerateNewId() };

                MEPatientProgram mepp = dtoUtil.CreateInitialMEPatientProgram(request, program, sil);
                ObjectId? result = mepp.AssignedTo;

                Assert.AreEqual("123456789055554444441234", result.ToString());
            }
        }

        [TestClass()]
        public class GetCareManagerValueByRule_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_ZERO_With_PCM_For_AssignToType()
            {
                string cmid = "999999999999999999999999";
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    CareManagerId = cmid
                };

                MEProgram mep = new MEProgram("111111111111111111111234") { AssignToType = 0 };
                string result = dtoUtil.GetCareManagerValueByRule(request, mep);

                Assert.AreEqual(cmid, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_ZERO_With_NO_PCM_For_AssignToType()
            {
                string cmid = null;
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    CareManagerId = cmid
                };

                MEProgram mep = new MEProgram("111111111111111111111234") { AssignToType = 0 };
                string result = dtoUtil.GetCareManagerValueByRule(request, mep);

                Assert.AreEqual(cmid, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_CM_For_AssignToType()
            {
                string cmid = "999999999999999999999999";
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    CareManagerId = cmid
                };

                MEProgram mep = new MEProgram("111111111111111111111234") { AssignToType = AssignToType.PCM };
                string result = dtoUtil.GetCareManagerValueByRule(request, mep);

                Assert.AreEqual(cmid, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11222")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void With_Unassigned_For_AssignToType()
            {
                string cmid = null;
                IDTOUtility dtoUtil = new DTOUtility { Factory = new StubProgramRepositoryFactory() };
                PutProgramToPatientRequest request = new PutProgramToPatientRequest
                {
                    CareManagerId = cmid
                };

                MEProgram mep = new MEProgram("111111111111111111111234") { AssignToType = AssignToType.Unassigned };
                string result = dtoUtil.GetCareManagerValueByRule(request, mep);

                Assert.AreEqual(cmid, result);
            }
        }

        [TestClass()]
        public class GetClonedModules_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-948")]
            [TestProperty("TFS", "11495")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_Assignby_Set_To_System()
            {
                DTOUtility util = new DTOUtility() {Factory = new StubProgramRepositoryFactory()};
                var mods = new List<Module>
                {
                    new Module
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };
                
                ObjectId? cmId = new ObjectId();
                var md = util.GetClonedModules(cmId,mods, request, new List<ObjectId>());
                var result = md[0].AssignedBy.ToString();
                Assert.AreEqual(control, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-949")]
            [TestProperty("TFS", "11444")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_AssignOn_Set_To_Now()
            {
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                var mods = new List<Module>
                {
                    new Module
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };

                ObjectId? cmId = new ObjectId();
                var md = util.GetClonedModules(cmId, mods, request, new List<ObjectId>());

                var result = ((DateTime)md[0].AssignedOn).Date;
                Assert.AreEqual(DateTime.UtcNow.Date, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_AssignTo_Set()
            {
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                var mods = new List<Module>
                {
                    new Module
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };

                ObjectId? cmId = new ObjectId();
                var md = util.GetClonedModules(cmId, mods, request, new List<ObjectId>());

                var result = md[0].AssignedTo;
                Assert.AreEqual(cmId, result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_AssignTo_Set_Null()
            {
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                var mods = new List<Module>
                {
                    new Module
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };

                ObjectId? cmId = null;
                var md = util.GetClonedModules(cmId, mods, request, new List<ObjectId>());

                var result = md[0].AssignedTo;
                Assert.AreEqual(cmId, result);
            }
        }

        [TestClass()]
        public class GetClonedActions_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11759")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_AssignDate_Set()
            {
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory() };
                var acts = new List<Action>
                {
                    new Action
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest { ContractNumber = "InHealth001", UserId = "123456789012345678901234" };

                ObjectId? cmId = null;
                //public  List<Action> GetClonedActions(List<Action> list, string contractNumber, string userId, List<ObjectId> sil, bool pEnabled)
                var md = util.GetClonedActions(null, acts, "InHealth001", "123456789012345678901234", new List<ObjectId>(), true);

                var result = md[0].AssignedOn;
                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime)result).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11759")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_With_AssignTo_Set()
            {
                DTOUtility util = new DTOUtility() {Factory = new StubProgramRepositoryFactory()};
                var acts = new List<Action>
                {
                    new Action
                    {
                        Id = ObjectId.GenerateNewId(),
                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                        Name = "testmodule",
                        Status = Status.Active,
                        Enabled = true,
                        Objectives = new List<Objective>
                        {
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Active,
                                Value = "90",
                                Units = "lbs"
                            },
                            new Objective
                            {
                                Id = ObjectId.GenerateNewId(),
                                Status = Status.Inactive,
                                Value = "99",
                                Units = "hdl"
                            }
                        }
                    }
                };

                DataDomainRequest request = new DataDomainRequest
                {
                    ContractNumber = "InHealth001",
                    UserId = "123456789012345678901234"
                };

                ObjectId? cmId = ObjectId.GenerateNewId();
                var md = util.GetClonedActions(cmId, acts, "InHealth001", "123456789012345678901234",
                    new List<ObjectId>(), true);

                var result = md[0].AssignedTo;
                Assert.AreEqual(cmId, result);
            }
        }

        
        [TestClass()]
        public class GetActionElements
        {
            [TestMethod()]
            [TestCategory("NIGHT-952")]
            [TestProperty("TFS", "15646")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_Action_With_StateUpdateDate()
            {
                DateTime date = System.DateTime.Now;

                List<ActionsDetail> actDetails = new List<ActionsDetail>{ new ActionsDetail{ 
                     Id = ObjectId.GenerateNewId().ToString(),
                     ModuleId = ObjectId.GenerateNewId().ToString(),
                     SourceId = ObjectId.GenerateNewId().ToString(),
                      ElementState = (int)ElementState.Completed,
                    StateUpdatedOn = date
                }};

                List<Action> actions = DTOUtils.GetActionElements(actDetails, "nguser");
                Assert.AreEqual(date, actions[0].StateUpdatedOn);
            }
        }

        [TestMethod()]
        public void CanInsertPatientProgramTest_True_Closed_State()
        {
            DTOUtility util = new DTOUtility();
            List<MEPatientProgram> pp = new List<MEPatientProgram>
            {
                new MEPatientProgram(ObjectId.GenerateNewId().ToString()){State = ElementState.Closed}
            };
            var result = util.CanInsertPatientProgram(pp);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void CanInsertPatientProgramTest_True_Completed_State()
        {
            DTOUtility util = new DTOUtility();
            List<MEPatientProgram> pp = new List<MEPatientProgram>
            {
                new MEPatientProgram(ObjectId.GenerateNewId().ToString()){State = ElementState.Completed}
            };
            var result = util.CanInsertPatientProgram(pp);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void CanInsertPatientProgramTest_False_InProgress_State()
        {
            DTOUtility util = new DTOUtility();
            List<MEPatientProgram> pp = new List<MEPatientProgram>
            {
                new MEPatientProgram(ObjectId.GenerateNewId().ToString()) {State = ElementState.InProgress}
            };
            var result = util.CanInsertPatientProgram(pp);

            Assert.IsFalse(result);
        }
    }
}

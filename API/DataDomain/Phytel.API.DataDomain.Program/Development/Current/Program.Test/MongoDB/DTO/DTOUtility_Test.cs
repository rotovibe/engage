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

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", "InHealth001", "000000000000000000000000");
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

                List<ModuleDetail> mds = util.GetModules(mods,"123456789012345678901234", "InHealth001", "000000000000000000000000");
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

                List<ModuleDetail> mds = util.GetModules(mods,"123456789012345678901234", "InHealth001", "000000000000000000000000");
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

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234",  "InHealth001", "000000000000000000000000");
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

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", "InHealth001", "000000000000000000000000");
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
                DTOUtility util = new DTOUtility() { Factory = new StubProgramRepositoryFactory()};
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

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", "InHealth001", "000000000000000000000000");
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

                List<ModuleDetail> mds = util.GetModules(mods, "123456789012345678901234", "InHealth001", "000000000000000000000000");
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
                List<ObjectiveInfoData> objs =  util.GetObjectivesForModule(mods, ObjectId.Parse("532b5585a381168abe00042c"));

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
            [TestCategory("NIGHT-921")]
            [TestProperty("TFS", "4957")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void DD_Get_Valid_Actions_Test()
            {
                DTOUtility dtoUtil = new DTOUtility();
                dtoUtil.Factory = new StubProgramRepositoryFactory();
                List<Action> acts = new List<Action>();
                acts.Add(new Action { Id = ObjectId.GenerateNewId(), SourceId = ObjectId.GenerateNewId(), Name="TestAction", Description ="test action!!!" });
                List<ActionsDetail> aDetails = dtoUtil.GetActions(acts, "InHealth001", "123456789012345678901234");
                Assert.AreEqual(acts[0].Id.ToString(), aDetails[0].Id);
            }
        }

        [TestClass()]
        public class GetAction
        {
            [TestMethod()]
            [TestCategory("NIGHT-921")]
            [TestProperty("TFS", "4957")]
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
        }
    }
}

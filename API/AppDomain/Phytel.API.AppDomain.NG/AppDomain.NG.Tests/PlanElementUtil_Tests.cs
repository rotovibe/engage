using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.AppDomain.NG.Test.Factories;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.Programs.ProgramAttributes;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementUtil_Tests
    {
        public const string SystemId = "5368ff2ad4332316288f3e3e";

        [TestMethod()]
        public void ResponseSpawnAllowed_Test()
        {
            DTO.Step s = new DTO.Step {StepTypeId = 15};
            DTO.Response r = new DTO.Response {Id = "000000000000000000000000", Value = ""};
            new PlanElementUtils().ResponseSpawnAllowed(s, r);
            Assert.Fail();
        }

        [TestClass()]
        public class SetProgramAttributes_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Element_State_Change_Date()
            {
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.SpawnElement spwn = new AD.SpawnElement
                {
                    ElementId = "123456789012345678901234",
                    ElementType = 12,
                    Tag = "3"
                };
                AD.Program program = new AD.Program {ElementState = 1};
                ProgramAttributeData progAttr = new ProgramAttributeData();
                peUtil.SetProgramAttributes(spwn, program, "UserId", progAttr);

                DateTime control = DateTime.UtcNow.Date;
                DateTime sample = ((DateTime) program.StateUpdatedOn).Date;
                Assert.AreEqual(control, sample);
            }
        }

        [TestClass()]
        public class CloneProgram_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Clone_With_StateChangeDate()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Program program = new AD.Program {ElementState = 1, StateUpdatedOn = control};
                peUtil.CloneProgram(program);

                DateTime sample = ((DateTime) program.StateUpdatedOn).Date;
                Assert.AreEqual(control, sample);
            }
        }

        [TestClass()]
        public class CloneAction_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11270")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Clone_Action_With_AssignTo()
            {
                ObjectId control = ObjectId.GenerateNewId();
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Actions action = new AD.Actions {ElementState = 1, AssignToId = control.ToString()};
                peUtil.CloneAction(action);

                string sample = action.AssignToId;
                Assert.AreEqual(control.ToString(), sample);
            }
        }

        [TestClass()]
        public class SetInitialProperties_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-948")]
            [TestProperty("TFS", "11495")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_By()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program();

                peUtil.SetInitialProperties("123456789012345678901234", mod, false);

                Assert.AreEqual(SystemId, mod.AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-949")]
            [TestProperty("TFS", "11444")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_Date()
            {
                DateTime control = DateTime.UtcNow.Date;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program();

                peUtil.SetInitialProperties("123456789012345678901234", mod, false);

                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mod.AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_To()
            {
                ObjectId? assignedTO = new ObjectId();
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program {AssignToId = assignedTO.ToString()};

                peUtil.SetInitialProperties(assignedTO.ToString(), mod, false);

                Assert.AreEqual(assignedTO.ToString(), mod.AssignToId);
            }

            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11746")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Get_Assigned_To_Null()
            {
                string assignedTO = null;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program {AssignToId = assignedTO};

                peUtil.SetInitialProperties(assignedTO, mod, false);

                Assert.AreEqual(assignedTO, mod.AssignToId);
            }


            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_Date_Null()
            {
                string assignedTO = null;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                AD.Module mod = new AD.Module();
                AD.Program prog = new AD.Program {AssignToId = assignedTO};

                peUtil.SetInitialProperties(assignedTO, mod, false);

                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mod.AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_Date_Already_Assigned()
            {
                string assignedDate = null;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                DateTime assigned = DateTime.UtcNow;
                AD.Actions act = new AD.Actions {AssignDate = assigned};
                AD.Program prog = new AD.Program {AssignToId = assignedDate};

                peUtil.SetInitialProperties(assignedDate, act, false);

                Assert.AreEqual(assigned.Date, ((DateTime) act.AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_AssignedTo_CM_Already_Assigned_In_Program()
            {
                string assignedTO = ObjectId.GenerateNewId().ToString();
                IPlanElementUtils peUtil = new PlanElementUtils {};
                DateTime assigned = DateTime.UtcNow;
                AD.Actions act = new AD.Actions {AssignDate = assigned};
                AD.Program prog = new AD.Program {AssignToId = assignedTO};

                peUtil.SetInitialProperties(assignedTO, act, false);

                Assert.AreEqual(assignedTO, act.AssignToId);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_To_No_CM_Assigned_In_Program()
            {
                string assignedTO = null;
                IPlanElementUtils peUtil = new PlanElementUtils {};
                DateTime assigned = DateTime.UtcNow;
                AD.Actions act = new AD.Actions {AssignDate = assigned};
                AD.Program prog = new AD.Program {AssignToId = assignedTO};

                peUtil.SetInitialProperties(assignedTO, act, false);

                Assert.IsNull(act.AssignToId);
            }
        }

        [TestClass()]
        public class SetEnabledState_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-949")]
            [TestProperty("TFS", "11449")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_On_With_Previous_Complete()
            {
                IPlanElementUtils peUtil = new PlanElementUtils {};
                var mods = new List<AD.Module> {new AD.Module {Id = "123456789012345678901234", Completed = true}};
                AD.Module mod = new DTO.Module {Previous = "123456789012345678901234"};
                peUtil.SetEnabledState(mods, mod, "123456789012345612341234", true);
                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mod.AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-949")]
            [TestProperty("TFS", "11449")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_On_With_Previous_Not_Complete()
            {
                IPlanElementUtils peUtil = new PlanElementUtils {};
                var mods = new List<AD.Module> {new AD.Module {Id = "123456789012345678901234", Completed = false}};
                AD.Module mod = new DTO.Module {Previous = "123456789012345678901234"};
                peUtil.SetEnabledState(mods, mod, "123456789012345612341234", true);
                Assert.IsNull(mod.AssignDate);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            [TestProperty("Layer", "AD.PlanElementUtils")]
            public void Set_Assigned_On_With_Previous_Complete_Current()
            {
                IPlanElementUtils peUtil = new PlanElementUtils {};
                var mods = new List<AD.Module> {new AD.Module {Id = "000006789012345678901234", Completed = true}};
                AD.Module mod = new DTO.Module {Previous = "000006789012345678901234"};
                peUtil.SetEnabledState(mods, mod, "123456789012345612341234", true);
                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mod.AssignDate).Date);
                Assert.AreEqual(mods[0].Id, mod.Previous);
            }
        }

        [TestClass()]
        public class CloneModule_Test
        {
            [TestMethod()]
            public void Clone_Module_With_StateUpdateOn()
            {
                IPlanElementUtils peUtil = new PlanElementUtils();
                peUtil.CloneModule(new AD.Module());

            }
        }

        [TestClass()]
        public class InitializePlanElementSettings_Test
        {
            //[TestMethod()]
            //public void 
        }

        [TestClass()]
        public class SetEnabledStatusByPrevious_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_Status_As_Enabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();

                var mods = new List<AD.PlanElement>
                {
                    new AD.PlanElement {Id = "000006789012345678901234", Completed = true},
                    new AD.PlanElement {Previous = "000006789012345678901234"}
                };

                pUtils.SetEnabledStatusByPrevious(mods, "123456789012345678901234", true);
                Assert.IsTrue(mods[1].Enabled);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_As_Enabled_Module()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();

                var mods = new List<AD.Module>
                {
                    new AD.Module {Id = "000006789012345678901234", Completed = true},
                    new AD.Module {Previous = "000006789012345678901234"}
                };

                pUtils.SetEnabledStatusByPrevious(mods, "123456789012345678901234", true);
                Assert.IsTrue(mods[1].Enabled);
            }

            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11759")]
            public void Set_Status_As_Enabled_AssignDate()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();

                var mods = new List<AD.PlanElement>
                {
                    new AD.PlanElement {Id = "000006789012345678901234", Completed = true},
                    new AD.PlanElement {Previous = "000006789012345678901234"}
                };

                pUtils.SetEnabledStatusByPrevious(mods, "123456789012345678901234", true);
                Assert.IsTrue(mods[1].Enabled);
            }

            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11759")]
            public void Set_As_Enabled_Module_AssignDate()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();

                var mods = new List<AD.Module>
                {
                    new AD.Module {Id = "000006789012345678901234", Completed = true},
                    new AD.Module
                    {
                        Enabled = true,
                        Previous = "000006789012345678901234",
                        Actions = new List<AD.Actions> {new AD.Actions {Enabled = true}}
                    }
                };

                pUtils.SetEnabledStatusByPrevious(mods, "123456789012345678901234", true);
                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mods[1].Actions[0].AssignDate).Date);
            }
        }

        [TestClass()]
        public class SetElementEnabledState_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_No_Action_Enabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                string modId = ObjectId.GenerateNewId().ToString();
                AD.Program prog = new AD.Program
                {
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = modId,
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Enabled = false
                                },
                                new AD.Actions
                                {
                                    Enabled = false
                                }
                            }
                        }
                    }
                };

                pUtils.SetElementEnabledState(modId, prog);
                Assert.IsNull(prog.Modules[0].Actions[0].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_One_Action_Enabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                string modId = ObjectId.GenerateNewId().ToString();
                AD.Program prog = new AD.Program
                {
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = modId,
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Enabled = true
                                },
                                new AD.Actions
                                {
                                    Enabled = false
                                }
                            }
                        }
                    }
                };

                pUtils.SetElementEnabledState(modId, prog);
                Assert.IsNotNull(prog.Modules[0].Actions[0].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11759")]
            public void Set_One_Action_Enabled_AssignDate()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                string modId = ObjectId.GenerateNewId().ToString();
                AD.Program prog = new AD.Program
                {
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = modId,
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Enabled = true
                                },
                                new AD.Actions
                                {
                                    Enabled = false
                                }
                            }
                        }
                    }
                };

                pUtils.SetElementEnabledState(modId, prog);
                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) prog.Modules[0].Actions[0].AssignDate).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11759")]
            public void Set_One_Action_Enabled_AssignTo()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                string modId = ObjectId.GenerateNewId().ToString();
                ObjectId assingtoID = ObjectId.GenerateNewId();

                AD.Program prog = new AD.Program
                {
                    AssignToId = assingtoID.ToString(),
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = modId,
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Enabled = true
                                },
                                new AD.Actions
                                {
                                    Enabled = false
                                }
                            }
                        }
                    }
                };

                pUtils.SetElementEnabledState(modId, prog);
                Assert.AreEqual(assingtoID.ToString(), prog.Modules[0].Actions[0].AssignToId);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11759")]
            public void Set_Two_Actions_Enabled_AssignTo()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                string modId = ObjectId.GenerateNewId().ToString();
                ObjectId assingtoID = ObjectId.GenerateNewId();

                AD.Program prog = new AD.Program
                {
                    AssignToId = assingtoID.ToString(),
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = modId,
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Enabled = true
                                },
                                new AD.Actions
                                {
                                    Enabled = true
                                }
                            }
                        }
                    }
                };

                pUtils.SetElementEnabledState(modId, prog);
                Assert.AreEqual(assingtoID.ToString(), prog.Modules[0].Actions[0].AssignToId);
                Assert.AreEqual(assingtoID.ToString(), prog.Modules[0].Actions[1].AssignToId);
            }
        }

        [TestClass()]
        public class SetInitialActions_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_No_Action_Module_Not_Enabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = false,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true},
                        new AD.Actions {Enabled = true}
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.IsNull(mod.Actions[0].AssignById);
                Assert.IsNull(mod.Actions[1].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_Two_Action()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = true,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true},
                        new AD.Actions {Enabled = true}
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.IsNotNull(mod.Actions[0].AssignById);
                Assert.IsNotNull(mod.Actions[1].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_One_Action_With_State_Complete()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = true,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true, ElementState = 5}, //complete
                        new AD.Actions {Enabled = true, ElementState = 2} // not started
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.IsNotNull(mod.Actions[1].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_One_Action_With_State_InProgress()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = true,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true, ElementState = 4}, // inprogress
                        new AD.Actions {Enabled = true, ElementState = 2} // not started
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.IsNotNull(mod.Actions[1].AssignById);
            }

            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_One_Action()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = true,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true},
                        new AD.Actions {Enabled = false}
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.IsNotNull(mod.Actions[0].AssignById);
            }


            [TestMethod()]
            [TestCategory("NIGHT-835")]
            [TestProperty("TFS", "11759")]
            public void Set_One_Action_Assign_Date()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.Module mod = new AD.Module
                {
                    Enabled = true,
                    Actions = new List<AD.Actions>
                    {
                        new AD.Actions {Enabled = true},
                        new AD.Actions {Enabled = false}
                    }
                };

                pUtils.SetInitialActions(mod, "123456789012345678901234");

                Assert.AreEqual(DateTime.UtcNow.Date, ((DateTime) mod.Actions[0].AssignDate).Date);
            }
        }

        [TestClass()]
        public class UpdatePlanElementAttributes_Test
        {
            [TestMethod()]
            [TestProperty("TFS", "12107")]
            [TestProperty("Layer", "AD.PlanElementUtil")]
            public void Update_Action_Assign_To()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();

                var targetAction = ObjectId.GenerateNewId().ToString();

                AD.Program prog = new AD.Program
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Modules = new List<AD.Module>
                    {
                        new AD.Module
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Id = targetAction,
                                    ElementState = 2
                                },
                                new AD.Actions
                                {
                                    Id = ObjectId.GenerateNewId().ToString()
                                }
                            }
                        },
                        new AD.Module
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Actions = new List<AD.Actions>
                            {
                                new AD.Actions
                                {
                                    Id = ObjectId.GenerateNewId().ToString()
                                },
                                new AD.Actions
                                {
                                    Id = ObjectId.GenerateNewId().ToString()
                                }
                            }
                        },
                    }
                };

                var assignToId = ObjectId.GenerateNewId().ToString();

                AD.PlanElement pe = new AD.PlanElement
                {
                    Id = targetAction,
                    AssignToId = assignToId
                };

                var planElems = new DTO.PlanElements
                {
                    Actions = new List<DTO.Actions>(),
                    Modules = new List<DTO.Module>(),
                    Programs = new List<DTO.Program>(),
                    Steps = new List<DTO.Step>()
                };

                pUtils.UpdatePlanElementAttributes(prog, pe, "111111111111111111111111", planElems);

                Assert.AreEqual(assignToId, prog.Modules[0].Actions[0].AssignToId);
            }
        }

        [TestMethod()]
        public void CloneRepeatActionTest()
        {
            IPlanElementUtils pUtils = new PlanElementUtils();

            var actionId = ObjectId.GenerateNewId().ToString();
            var step1Id = ObjectId.GenerateNewId().ToString();
            var step2Id = ObjectId.GenerateNewId().ToString();
            var selectedRespId = ObjectId.GenerateNewId().ToString();

            var action = SampleFactory.CreateCloneAction(actionId, step1Id, step2Id, selectedRespId);

            var newAction = pUtils.CloneRepeatAction(action, ObjectId.GenerateNewId().ToString());
            Assert.AreNotEqual(action.Id, newAction.Id);
            Assert.AreEqual(action.ElementState, newAction.ElementState);
        }

        [TestClass()]
        public class SetInitialValuesTest
        {
            [TestMethod()]
            public void SetInitialValuesTest_enabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                var pe = new AD.PlanElement {Enabled = true};
                var id = "999999999999999999999999";
                pUtils.SetInitialValues(id, pe);

                Assert.AreEqual(pe.AssignDate, null);
            }

            [TestMethod()]
            public void SetInitialValuesTest_disabled()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                var pe = new AD.PlanElement {Enabled = false};
                var id = "999999999999999999999999";
                pUtils.SetInitialValues(id, pe);

                Assert.AreNotEqual(pe.AssignDate, null);
            }
        }

        [TestClass()]
        public class InitializePlanElementSettingsTest
        {
            [TestMethod()]
            public void InitializePlanElementSettings_true()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.PlanElement pe = new AD.PlanElement {Enabled = true};
                pUtils.InitializePlanElementSettings(pe, pe, new AD.Program {AssignToId = "999999999999999999999999"});
                Assert.AreEqual(pe.AssignDate, null);
            }

            [TestMethod()]
            public void InitializePlanElementSettings_false()
            {
                IPlanElementUtils pUtils = new PlanElementUtils();
                AD.PlanElement pe = new AD.PlanElement {Enabled = false};
                pUtils.InitializePlanElementSettings(pe, pe, new AD.Program {AssignToId = "999999999999999999999999"});
                Assert.AreNotEqual(pe.AssignDate, null);
            }
        }


        [TestClass()]
        public class SetProgramAttributesTest
        {
            [TestMethod()]
            public void SetProgramAttributes_Null_attributes()
            {
                try
                {
                    IPlanElementUtils pUtils = new PlanElementUtils();
                    AD.PlanElement pe = new AD.PlanElement {Enabled = false};
                    pUtils.SetProgramAttributes(new AD.SpawnElement {Tag = "0", ElementType = 19, ElementId = "1234"},
                        new AD.Program {Name = "test"}, "user", null);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex != null);
                }
            }

            [TestMethod()]
            public void SetProgramAttributes_DoNot_Set_Program_State_state_1()
            {
                IPlanElementUtils pUtils = new PlanElementUtils{ ProgramAttributeStrategy = new ProgramAttributeStrategy()};
                var pe = new AD.PlanElement {Enabled = false};
                var progAttr = new ProgramAttributeData();
                var prog = new AD.Program {Name = "test"};
                pUtils.SetProgramAttributes(new AD.SpawnElement {Tag = "1", ElementType = 10, ElementId = "1234"}, prog, "user", progAttr);

                Assert.AreEqual(0, prog.ElementState);
            }

            [TestMethod()]
            public void SetProgramAttributes_DoNot_Set_Program_State_state_2()
            {
                IPlanElementUtils pUtils = new PlanElementUtils { ProgramAttributeStrategy = new ProgramAttributeStrategy() };
                var pe = new AD.PlanElement {Enabled = false};
                var progAttr = new ProgramAttributeData();
                var prog = new AD.Program {Name = "test"};
                pUtils.SetProgramAttributes(new AD.SpawnElement {Tag = "2", ElementType = 10, ElementId = "1234"}, prog, "user", progAttr);

                Assert.AreEqual(0, prog.ElementState);
            }
        }
    }
}

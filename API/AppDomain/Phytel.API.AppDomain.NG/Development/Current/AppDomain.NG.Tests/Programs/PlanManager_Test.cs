using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Funq;
using ServiceStack.WebHost.Endpoints;
using MongoDB.Bson;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanManager_Test
    {
        [TestClass()]
        public class ProcessActionResults_Test : StubAppHostBase
        {
            [TestMethod()]
            [TestCategory("NIGHT-876")]
            [TestProperty("TFS", "11633")]
            public void Set_Action_State_Changed_To_Complete()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new StubPlanElementEndpointUtils()
                };

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new DTO.Actions
                    {
                        Id = "000000000000000000000000",
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = System.DateTime.UtcNow,
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                ElementState = 1
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Action_Returned()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new StubPlanElementEndpointUtils()
                };

                ObjectId actionId = ObjectId.Parse("538ca77dfe7a59112c3649e4");

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new DTO.Actions
                    {
                        Id = actionId.ToString(),
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = System.DateTime.UtcNow,
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                ElementState = 1
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response.PlanElems);
                Assert.IsNotNull(response.PlanElems.Actions);
                //Assert.AreEqual(actionId.ToString(), response.PlanElems.Actions[0].Id);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Goal_Spawn_by_step()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new StubPlanElementEndpointUtils(),
                    StepProcessor =
                        new StubStepPlanProcessor()
                        {
                            ElementActivationStrategy = new StubElementActivationStrategy(),
                            PEUtils = new StubPlanElementUtils()
                        }
                };

                //mapper
                Mapper.CreateMap<Goal, PatientGoal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute {Id = c.Id, Values = c.Values})));

                ObjectId actionId = ObjectId.Parse("538ca77dfe7a59112c3649e4");

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new DTO.Actions
                    {
                        Id = actionId.ToString(),
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = System.DateTime.UtcNow,
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                Completed = true,
                                ElementState = 1,
                                SpawnElement =
                                    new List<SpawnElement>
                                    {
                                        new SpawnElement
                                        {
                                            ElementId = "545a91a1fe7a59218cef2d6d",
                                            ElementType = 301,
                                            Tag = ""
                                        }
                                    }
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response.PlanElems);
                Assert.IsNotNull(response.PlanElems.Actions);
                Assert.IsNotNull(response.PlanElems.Goals);
            }

            [TestMethod()]
            [TestCategory("NIGHT-952")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Action_Updatedstatus()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new EndpointUtils()
                };

                ObjectId actionId = ObjectId.Parse("538ca77dfe7a59112c3649e4");

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new DTO.Actions
                    {
                        Id = actionId.ToString(),
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = System.DateTime.UtcNow,
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                ElementState = 1
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response.PlanElems);
                Assert.IsNotNull(response.PlanElems.Actions);
                Assert.AreEqual(actionId.ToString(), response.PlanElems.Actions[0].Id);
            }

            [TestMethod()]
            [TestCategory("NIGHT-952")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_StateUpdatedOn()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new StubPlanElementEndpointUtils()
                };

                ObjectId actionId = ObjectId.Parse("538ca77dfe7a59112c3649e4");

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new DTO.Actions
                    {
                        Id = actionId.ToString(),
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = System.DateTime.UtcNow,
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                ElementState = 1
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response.PlanElems);
                Assert.IsNotNull(response.PlanElems.Actions);
                Assert.AreEqual(actionId.ToString(), response.PlanElems.Actions[0].Id);
            }

            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Action_Spawn_Activated_Returned()
            {
                IPlanManager pm = new PlanManager
                {
                    PEUtils = new StubPlanElementUtils(),
                    EndPointUtils = new StubPlanElementEndpointUtils()
                };

                ObjectId actionId = ObjectId.Parse("538ca77dfe7a59112c3649e4");

                PostProcessActionRequest request = new PostProcessActionRequest
                {
                    ContractNumber = "InHealth001",
                    PatientId = "123456789012345678901234",
                    ProgramId = "000000000000000000000000",
                    UserId = "111111111111111111111111",
                    Version = 1.0,
                    Action = new Actions
                    {
                        Id = actionId.ToString(),
                        Completed = true,
                        ElementState = 4,
                        Name = "test action from stub",
                        Description = "action Description",
                        Text = "test action 1",
                        AttrEndDate = DateTime.UtcNow.AddDays(10),
                        AttrStartDate = DateTime.UtcNow,
                        AssignDate = DateTime.UtcNow,
                        SpawnElement =
                            new List<SpawnElement>
                            {
                                new SpawnElement {ElementId = "111116789012345678901234", ElementType = 3}
                            },
                        Steps = new List<Step>
                        {
                            new Step
                            {
                                Id = "000000000011111111111234",
                                ElementState = 1
                            }
                        }
                    }
                };

                PostProcessActionResponse response = pm.ProcessActionResults(request);
                Assert.IsNotNull(response.PlanElems);
                Assert.IsNotNull(response.PlanElems.Actions);
                Assert.AreEqual(actionId.ToString(), response.PlanElems.Actions[0].Id);
            }
        }
    }
}

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
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.WebHost.Endpoints;
using MongoDB.Bson;
using Task = Phytel.API.AppDomain.NG.DTO.Goal.Task;

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
            [TestCategory("NIGHT-826")]
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

                //mappers
                Mapper.CreateMap<PatientGoalData, PatientGoal>()
                                    .ForMember(d => d.CustomAttributes,
                                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                                            c => new CustomAttribute { Id = c.Id, Values = c.Values })))
                                    .ForMember(d => d.Barriers,
                                        opt => opt.MapFrom(src => src.BarriersData.ConvertAll(
                                            c =>
                                                new PatientBarrier
                                                {
                                                    Id = c.Id,
                                                    CategoryId = c.CategoryId,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Name = c.Name,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Tasks,
                                        opt => opt.MapFrom(src => src.TasksData.ConvertAll(
                                            c =>
                                                new PatientTask
                                                {
                                                    Id = c.Id,
                                                    BarrierIds = c.BarrierIds,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    CustomAttributes =
                                                        c.CustomAttributes.ConvertAll(
                                                            ca =>
                                                                new CustomAttribute
                                                                {
                                                                    ControlType = ca.ControlType,
                                                                    Id = ca.Id,
                                                                    Name = ca.Name,
                                                                    Order = ca.Order,
                                                                    Required = ca.Required,
                                                                    Type = ca.Type,
                                                                    Values = ca.Values,
                                                                    Options = NGUtils.FormatOptions(ca.Options)
                                                                }),
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StartDate = c.StartDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    TargetDate = c.TargetDate,
                                                    TargetValue = c.TargetValue,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Interventions,
                                        opt => opt.MapFrom(src => src.InterventionsData.ConvertAll(
                                            c =>
                                                new PatientIntervention
                                                {
                                                    AssignedToId = c.AssignedToId,
                                                    BarrierIds = c.BarrierIds,
                                                    CategoryId = c.CategoryId,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    Id = c.Id,
                                                    PatientGoalId = c.PatientGoalId,
                                                    PatientId = c.PatientId,
                                                    StartDate = c.StartDate,
                                                    DueDate = c.DueDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })));

                Mapper.CreateMap<Goal, PatientGoal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute {Id = c.Id, Values = c.Values})));

                Mapper.CreateMap<GoalData, Goal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

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
            [TestCategory("NIGHT-827")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Intervention_Spawn_by_step()
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

                //mappers
                Mapper.CreateMap<PatientInterventionData, PatientIntervention>();
                Mapper.CreateMap<InterventionData, Intervention>();
                Mapper.CreateMap<GoalData, Goal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute {Id = c.Id, Values = c.Values})));
                Mapper.CreateMap<Goal, PatientGoal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

                Mapper.CreateMap<PatientGoalData, PatientGoal>()
                                    .ForMember(d => d.CustomAttributes,
                                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                                            c => new CustomAttribute { Id = c.Id, Values = c.Values })))
                                    .ForMember(d => d.Barriers,
                                        opt => opt.MapFrom(src => src.BarriersData.ConvertAll(
                                            c =>
                                                new PatientBarrier
                                                {
                                                    Id = c.Id,
                                                    CategoryId = c.CategoryId,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Name = c.Name,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Tasks,
                                        opt => opt.MapFrom(src => src.TasksData.ConvertAll(
                                            c =>
                                                new PatientTask
                                                {
                                                    Id = c.Id,
                                                    BarrierIds = c.BarrierIds,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    CustomAttributes =
                                                        c.CustomAttributes.ConvertAll(
                                                            ca =>
                                                                new CustomAttribute
                                                                {
                                                                    ControlType = ca.ControlType,
                                                                    Id = ca.Id,
                                                                    Name = ca.Name,
                                                                    Order = ca.Order,
                                                                    Required = ca.Required,
                                                                    Type = ca.Type,
                                                                    Values = ca.Values,
                                                                    Options = NGUtils.FormatOptions(ca.Options)
                                                                }),
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StartDate = c.StartDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    TargetDate = c.TargetDate,
                                                    TargetValue = c.TargetValue,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Interventions,
                                        opt => opt.MapFrom(src => src.InterventionsData.ConvertAll(
                                            c =>
                                                new PatientIntervention
                                                {
                                                    AssignedToId = c.AssignedToId,
                                                    BarrierIds = c.BarrierIds,
                                                    CategoryId = c.CategoryId,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    Id = c.Id,
                                                    PatientGoalId = c.PatientGoalId,
                                                    PatientId = c.PatientId,
                                                    StartDate = c.StartDate,
                                                    DueDate = c.DueDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })));

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
                                            ElementId = "5461bb4ffe7a59064cd074c0",
                                            ElementType = 401,
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
                Assert.IsNotNull(response.PlanElems.Interventions);
            }

            [TestMethod()]
            [TestCategory("NIGHT-827")]
            [TestProperty("TFS", "11633")]
            public void Process_Action_With_Task_Spawn_by_step()
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

                //mappers
                Mapper.CreateMap<Task, PatientTask>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

                Mapper.CreateMap<PatientTaskData, PatientTask>().ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute {Id = c.Id, Values = c.Values})));

                Mapper.CreateMap<PatientTask, PatientTaskData>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

                Mapper.CreateMap<TaskData, Task>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));
                
                Mapper.CreateMap<GoalData, Goal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

                Mapper.CreateMap<Goal, PatientGoal>()
                    .ForMember(d => d.CustomAttributes,
                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                            c => new CustomAttribute { Id = c.Id, Values = c.Values })));

                Mapper.CreateMap<PatientGoalData, PatientGoal>()
                                    .ForMember(d => d.CustomAttributes,
                                        opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                                            c => new CustomAttribute { Id = c.Id, Values = c.Values })))
                                    .ForMember(d => d.Barriers,
                                        opt => opt.MapFrom(src => src.BarriersData.ConvertAll(
                                            c =>
                                                new PatientBarrier
                                                {
                                                    Id = c.Id,
                                                    CategoryId = c.CategoryId,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Name = c.Name,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Tasks,
                                        opt => opt.MapFrom(src => src.TasksData.ConvertAll(
                                            c =>
                                                new PatientTask
                                                {
                                                    Id = c.Id,
                                                    BarrierIds = c.BarrierIds,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    CustomAttributes =
                                                        c.CustomAttributes.ConvertAll(
                                                            ca =>
                                                                new CustomAttribute
                                                                {
                                                                    ControlType = ca.ControlType,
                                                                    Id = ca.Id,
                                                                    Name = ca.Name,
                                                                    Order = ca.Order,
                                                                    Required = ca.Required,
                                                                    Type = ca.Type,
                                                                    Values = ca.Values,
                                                                    Options = NGUtils.FormatOptions(ca.Options)
                                                                }),
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    PatientGoalId = c.PatientGoalId,
                                                    StartDate = c.StartDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    TargetDate = c.TargetDate,
                                                    TargetValue = c.TargetValue,
                                                    Details = c.Details
                                                })))
                                    .ForMember(d => d.Interventions,
                                        opt => opt.MapFrom(src => src.InterventionsData.ConvertAll(
                                            c =>
                                                new PatientIntervention
                                                {
                                                    AssignedToId = c.AssignedToId,
                                                    BarrierIds = c.BarrierIds,
                                                    CategoryId = c.CategoryId,
                                                    ClosedDate = c.ClosedDate,
                                                    CreatedById = c.CreatedById,
                                                    DeleteFlag = c.DeleteFlag,
                                                    Description = c.Description,
                                                    GoalName = c.GoalName,
                                                    Id = c.Id,
                                                    PatientGoalId = c.PatientGoalId,
                                                    PatientId = c.PatientId,
                                                    StartDate = c.StartDate,
                                                    DueDate = c.DueDate,
                                                    StatusDate = c.StatusDate,
                                                    StatusId = c.StatusId,
                                                    Details = c.Details
                                                })));

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
                                            ElementId = "5463cd99fe7a592668f67a15",
                                            ElementType = 501,
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
                Assert.IsNotNull(response.PlanElems.Interventions);
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

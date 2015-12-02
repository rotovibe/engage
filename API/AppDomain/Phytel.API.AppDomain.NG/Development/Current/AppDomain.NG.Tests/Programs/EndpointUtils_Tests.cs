using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.Test;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using MongoDB.Bson;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class EndpointUtils_Tests
    {
        [TestClass()]
        public class RequestPatientProgramDetailsSummary_Method
        {
            [TestMethod()]
            public void Valid_Response_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response);
            }

            [TestMethod()]
            public void With_Description_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Description);
            }

            [TestMethod()]
            [TestCategory("NIGHT-917")]
            [TestProperty("TFS", "1886")]
            public void AD_With_Module_Description_Test()
            {
                string desc = "BSHSI - Outreach & Enrollment";
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                ModuleDetail module = response.Program.Modules.Find(m => m.SourceId == "532b5585a381168abe00042c");
                string mDesc = module.Description.Trim();
                Assert.AreEqual(desc, mDesc, true);
            }

            [TestMethod()]
            public void With_EligiblityRequirements_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.EligibilityRequirements);
                Assert.IsNotNull(response.Program.EligibilityStartDate);
                Assert.IsNotNull(response.Program.EligibilityEndDate);
            }

            [TestMethod()]
            public void With_Attributes_AsNull_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNull(response.Program.Attributes);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_State_Test()
            {
                int actualValue = 4;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                int expectedValue = response.Program.Modules[0].Actions[0].ElementState;
                Assert.AreEqual(expectedValue, actualValue);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AssignedBy_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.Modules[0].Actions[0].AssignBy;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-832")]
            [TestProperty("TFS", "11170")]
            public void Get_Program_AssignedBy_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest { UserId = "123456789012345678901234" };
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.AssignBy;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-831")]
            [TestProperty("TFS", "11172")]
            public void Get_Program_AssignedDate_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime expected = System.DateTime.UtcNow.Date;
                DateTime? value = response.Program.AssignDate;

                Assert.AreEqual(expected, ((DateTime)value).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-868")]
            [TestProperty("TFS", "11270")]
            public void Get_Program_StateChangeDate_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime expected = System.DateTime.UtcNow.Date;
                DateTime? value = response.Program.StateUpdatedOn;

                Assert.AreEqual(expected, ((DateTime)value).Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AssignedTo_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string expectedValue = response.Program.Modules[0].Actions[0].AssignTo;
                ObjectId objectId;
                bool success = ObjectId.TryParse(expectedValue, out objectId);
                Assert.IsTrue(success);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AttrStartDate_Test()
            {
                DateTime expectedValue = DateTime.UtcNow.Date;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime actualValue = (DateTime)response.Program.Modules[0].Actions[0].AttrStartDate;
                Assert.AreEqual(expectedValue, actualValue.Date);
            }

            [TestMethod()]
            [TestCategory("NIGHT-920")]
            [TestProperty("TFS", "6099")]
            public void GetActionIndividualAttributes_AttrEndDate_Test()
            {
                DateTime expectedValue = DateTime.UtcNow.AddDays(10).Date;
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                DateTime actualValue = (DateTime)response.Program.Modules[0].Actions[0].AttrEndDate;
                Assert.AreEqual(expectedValue, actualValue.Date);
            }


            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6108")]
            public void GetActionObjectives_Value_Test()
            {
                string expectedValue = "5325da08d6a4850adcbba50e";
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);

                string actualValue = response.Program.Modules[0].Actions[0].Objectives[0].Id;
                Assert.AreEqual(expectedValue, actualValue);
            }

            [TestMethod()]
            [TestCategory("NIGHT-924")]
            [TestProperty("TFS", "6108")]
            public void GetActionObjectives_Test()
            {
                StubPlanElementEndpointUtils peu = new StubPlanElementEndpointUtils { Client = new StubJsonRestClient() };
                GetPatientProgramDetailsSummaryRequest request = new GetPatientProgramDetailsSummaryRequest();
                GetProgramDetailsSummaryResponse response = peu.RequestPatientProgramDetailsSummary(request);
                Assert.IsNotNull(response.Program.Modules[0].Actions[0].Objectives[0]);
            }
        }

        //[TestClass()]
        //public class UpdatePatientProblem_Method
        //{
        //    [TestMethod()]
        //    public void Active_Is_True_Test()
        //    {
        //        string patientId = "5325da03d6a4850adcbba4fe";
        //        string userId = "531f2df9072ef727c4d2a3df";
        //        string elementId = "5346c582d6a48504b44b4f77";
        //        PatientObservation pod = new PatientObservation { Id = elementId, StateId = 2 };
        //        bool _active = true;
        //        IAppDomainRequest request = new GetActiveProgramsRequest { UserId = userId, Context = "NG", ContractNumber = "InHealth001", Version = 1.0 };

        //        PutUpdateObservationDataResponse result = PlanElementEndpointUtil.UpdatePatientProblem(patientId, userId, elementId, pod, _active, request);

        //        Assert.IsTrue(result.Result);
        //    }
        //}

        //[TestClass()]
        //public class PutNewPatientProblem_Method
        //{
        //    [TestMethod()]
        //    public void New_Patient_Problem_Test()
        //    {
        //        string patientId = "5325da03d6a4850adcbba4fe";
        //        string userId = "531f2df9072ef727c4d2a3df";
        //        string elementId = "533ed16cd4332307bc592baa";
        //        PatientObservation pod = new PatientObservation { Id = elementId, StateId = 2 };
        //        bool _active = true;
        //        IAppDomainRequest request = new GetActiveProgramsRequest { UserId = userId, Context = "NG", ContractNumber = "InHealth001", Version = 1.0 }; // request object is arbitrary. use any.

        //        PutRegisterPatientObservationResponse result = PlanElementEndpointUtil.PutNewPatientProblem(patientId, userId, elementId, request);

        //        //Assert.IsTrue(result.Result);
        //    }
        //}

        //[TestClass()]
        //public class GetPatientProblem_Method
        //{
        //    [TestMethod()]
        //    public void Get_Patient_Problem_Not_Null_Test()
        //    {
        //        PlanElementEventArg e = new PlanElementEventArg();
        //        PostProcessActionRequest dr = new PostProcessActionRequest
        //        {
        //            ContractNumber = "InHealth001",
        //            Version = 1.0,
        //            PatientId = "5325db5ed6a4850adcbba912"
        //        };

        //        e.DomainRequest = dr as IAppDomainRequest;
        //        e.PatientId = "5325db5ed6a4850adcbba912";

        //        PatientObservation po = new EndpointUtils().GetPatientProblem("533ed16dd4332307bc592baf", e, "000000000000000000000000");
        //        Assert.IsNotNull(po);
        //    }
        //}

        [TestClass()]
        public class GetPrimaryCareManagerForPatient_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-833")]
            [TestProperty("TFS", "11199")]
            [TestProperty("Layer", "DD.DTOUtility")]
            public void Get_PCM_Valid_For_Patient()
            {
                string control = "5325c81f072ef705080d347e";
                string result = null;
                //string url = "http://azurephyteldev.cloudapp.net:59901/CareMember";
                string urls = "http://localhost:8888/CareMember";
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PrimaryCareManager/?UserId=nguser",
                                                        urls,
                                                        "NG",
                                                        "1",
                                                        "InHealth001",
                                                        "5325db1ad6a4850adcbba83a"), "nguser");

                GetPrimaryCareManagerDataResponse dataDomainResponse =
                    client.Get<GetPrimaryCareManagerDataResponse>(url);

                if (dataDomainResponse.CareMember != null)
                {
                    result = dataDomainResponse.CareMember.ContactId;
                }

                Assert.AreEqual(control, result);
            }
        }

        [TestMethod()]
        public void GetScheduleToDoByIdTest()
        {
            EndpointUtils utils = new EndpointUtils();
            utils.GetScheduleToDoById("53ff6b92d4332314bcab46e0", "5325c821072ef705080d3488", new GetToDosRequest{ Version=1.0, ContractNumber="InHealth001", UserId="1234"});
        }

        [TestMethod()]
        public void GetGoalByIdTest()
        {
            Mapper.CreateMap<GoalData, Goal>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute {Id = c.Id, Values = c.Values})));

            EndpointUtils utils = new EndpointUtils();
            var goal = utils.GetGoalById("545a91a1fe7a59218cef2d6d", "5325c821072ef705080d3488",
                new GetToDosRequest {Version = 1.0, ContractNumber = "InHealth001", UserId = "1234"});

            Assert.IsNotNull(goal);
        }

        [TestMethod()]
        public void GetPatientGoalByTemplateId_Found()
        {
            #region 
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
            #endregion

            EndpointUtils utils = new EndpointUtils();
            var patientGoal = utils.GetOpenNotMetPatientGoalByTemplateId("545a91a1fe7a59218cef2d6d", "5325db9cd6a4850adcbba9ca", "1234",
                new AppDomainRequest { Version = 1.0, ContractNumber = "InHealth001", UserId = "1234" });

            Assert.IsNotNull(patientGoal);
        }

        [TestMethod()]
        public void GetPatientGoalByTemplateId_Null()
        {
            #region
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
            #endregion

            EndpointUtils utils = new EndpointUtils();
            var patientGoal = utils.GetOpenNotMetPatientGoalByTemplateId("123491a1fe7a59218cef2d6d", "5325db9cd6a4850adcbba9ca", "1234",
                new AppDomainRequest { Version = 1.0, ContractNumber = "InHealth001", UserId = "1234" });

            Assert.IsNull(patientGoal);
        }

        [TestMethod()]
        public void GetPatientToDosTest()
        {
            EndpointUtils utils = new EndpointUtils();
            var todos = utils.GetPatientToDos("5325da35d6a4850adcbba58e", "1234",
                new AppDomainRequest {Version = 1.0, ContractNumber = "InHealth001", UserId = "1234"});
        }    
    }
}

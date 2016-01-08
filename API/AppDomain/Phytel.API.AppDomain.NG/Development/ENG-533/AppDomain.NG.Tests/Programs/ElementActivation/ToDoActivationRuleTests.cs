using System;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation.Tests
{
    [TestClass()]
    public class ToDoActivationRuleTests
    {
        [TestMethod()]
        public void HandleToDoTemplateRegistrationTest()
        {
            ToDoActivationRule rule = new ToDoActivationRule
            {
                EndpointUtil = new EndpointUtils(),
                PlanUtils = new PlanElementUtils()
            };

            var patientId = ObjectId.GenerateNewId().ToString();

            PlanElementEventArg arg = new PlanElementEventArg
            {
                Program = new Program
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = patientId
                },
                UserId = "5325c821072ef705080d3488",
                Action = new Actions {Id = ObjectId.GenerateNewId().ToString()},
                PatientId = patientId,
                PlanElement = new Step()
            };

            var se = new SpawnElement
            {
                ElementType = 201,
                ElementId = "53ff6b92d4332314bcab46e0"
            };

            var type = rule.Execute(arg.UserId, arg, se, new ProgramAttributeData());
            Assert.AreEqual(type, 200);
        }

        [TestMethod()]
        public void HandleToDoTemplateRegistration_Status_Met()
        {
            ToDoActivationRule rule = new ToDoActivationRule
            {
                EndpointUtil = new StubEndpointUtils(),
                PlanUtils = new PlanElementUtils()
            };

            var patientId = ObjectId.GenerateNewId().ToString();

            PlanElementEventArg arg = new PlanElementEventArg
            {
                Program = new Program
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = patientId
                },
                UserId = "5325c821072ef705080d3488",
                Action = new Actions { Id = ObjectId.GenerateNewId().ToString() },
                PatientId = patientId,
                PlanElement = new Step()
            };

            var se = new SpawnElement
            {
                ElementType = 201,
                ElementId = "53ff6b92d4332314bcab46e0"
            };

            var type = rule.Execute(arg.UserId, arg, se, new ProgramAttributeData());
            Assert.AreEqual(type, 200);
        }

        [TestMethod()]
        public void HandleToDoTemplateRegistration_Status_Abandoned()
        {
            ToDoActivationRule rule = new ToDoActivationRule
            {
                EndpointUtil = new StubEndpointUtils(),
                PlanUtils = new PlanElementUtils()
            };

            var patientId = ObjectId.GenerateNewId().ToString();

            PlanElementEventArg arg = new PlanElementEventArg
            {
                Program = new Program
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    PatientId = patientId
                },
                UserId = "5325c821072ef705080d3488",
                Action = new Actions { Id = ObjectId.GenerateNewId().ToString() },
                PatientId = patientId,
                PlanElement = new Step()
            };

            var se = new SpawnElement
            {
                ElementType = 201,
                ElementId = "53ff6b92d4332314bcab46e0"
            };

            var type = rule.Execute(arg.UserId, arg, se, new ProgramAttributeData());
            Assert.AreEqual(type, 200);
        }

        [TestClass()]
        public class HandleDueDateTest
        {
            [TestMethod()]
            public void HandleDueDate_HasRange()
            {
                var days = 10;
                ToDoActivationRule rule = new ToDoActivationRule();
                var duedate = rule.HandleDueDate(days);

                var nDt = DateTime.UtcNow.AddDays(days);
                var dDate = new DateTime(nDt.Year, nDt.Month, nDt.Day, 12, 0, 0);

                Assert.AreEqual(duedate, dDate);
            }

            [TestMethod()]
            public void HandleDueDate_Null()
            {
                ToDoActivationRule rule = new ToDoActivationRule();
                var val = rule.HandleDueDate(null);
                Assert.IsNull(val);
            }
        }

        [TestClass()]
        public class SetDefaultAssignmentTest
        {
            [TestMethod()]
            public void SetDefaultAssignment_True()
            {
                var todoRule = new ToDoActivationRule
                {
                    EndpointUtil = new StubEndpointUtils(),
                    PlanUtils = new StubPlanElementUtils()
                };

                var userid = "Testing";
                var schedule = new DTO.Scheduling.Schedule
                {
                    DefaultAssignment = true,
                    AssignedToId = userid,
                    CreatedById = userid
                };

                var todoData = new DataDomain.Scheduling.DTO.ToDoData();

                todoRule.SetDefaultAssignment(userid, schedule, todoData);

                Assert.AreEqual(userid, todoData.AssignedToId);
            }

            [TestMethod()]
            public void SetDefaultAssignment_False()
            {
                var todoRule = new ToDoActivationRule
                {
                    EndpointUtil = new StubEndpointUtils(),
                    PlanUtils = new StubPlanElementUtils()
                };

                var userid = "Testing";
                var schedule = new DTO.Scheduling.Schedule
                {
                    DefaultAssignment = false,
                    AssignedToId = userid,
                    CreatedById = userid
                };

                var todoData = new DataDomain.Scheduling.DTO.ToDoData();

                todoRule.SetDefaultAssignment(userid, schedule, todoData);

                Assert.AreNotEqual(userid, todoData.AssignedToId);
            }
        }

        [TestClass()]
        public class PatientToDoExistsTest
        {
            [TestMethod()]
            public void PatientToDoExistsTest_true()
            {
                var todoRule = new ToDoActivationRule
                {
                    EndpointUtil = new EndpointUtils(),
                    PlanUtils = new StubPlanElementUtils()
                };

                const string patientId = "5325da76d6a4850adcbba656";
                const string userId = "1234";
                const string elemId = "5407527d1e6015290c203d36";

                var eventArg = new PlanElementEventArg
                {
                    Action = new Actions(),
                    DomainRequest =
                        new AppDomainRequest {Version = 1.0, ContractNumber = "InHealth001", UserId = "1234"},
                    PatientId = patientId,
                    UserId = userId,
                    PlanElement = new PlanElement {Id = ""}
                };

                var result = todoRule.PatientToDoExists(eventArg, elemId);

                Assert.IsTrue(result);
            }

            [TestMethod()]
            public void PatientToDoExistsTest_false()
            {
                var todoRule = new ToDoActivationRule
                {
                    EndpointUtil = new EndpointUtils(),
                    PlanUtils = new StubPlanElementUtils()
                };

                const string patientId = "5325da35d6a4850adcbba58e";
                const string userId = "1234";
                const string elemId = "53ff6b92d4332314bcab46e0";

                var eventArg = new PlanElementEventArg
                {
                    Action = new Actions(),
                    DomainRequest =
                        new AppDomainRequest { Version = 1.0, ContractNumber = "InHealth001", UserId = "1234" },
                    PatientId = patientId,
                    UserId = userId,
                    PlanElement = new PlanElement { Id = "" }
                };

                var result = todoRule.PatientToDoExists(eventArg, elemId);

                Assert.IsFalse(result);
            }
        }
    }
}

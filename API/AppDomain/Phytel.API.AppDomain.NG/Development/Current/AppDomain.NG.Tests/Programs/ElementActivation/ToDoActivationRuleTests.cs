using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                var days = 5;
                ToDoActivationRule rule = new ToDoActivationRule();
                var duedate = rule.HandleDueDate(days);
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(days).Day,
                    0, 0, 0);
                var utcDate = TimeZoneInfo.ConvertTimeToUtc(date);

                Assert.AreEqual(duedate, utcDate);
            }

            [TestMethod()]
            public void HandleDueDate_Null()
            {
                ToDoActivationRule rule = new ToDoActivationRule();
                var val = rule.HandleDueDate(null);
                Assert.IsNull(val);
            }
        }
    }
}

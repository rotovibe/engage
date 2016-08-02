using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation.Tests
{
    [TestClass()]
    public class GoalActivationRuleTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            // goal to patientgoal
            Mapper.CreateMap<Goal, PatientGoal>()
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.CustomAttributes.ConvertAll(
                        c => new CustomAttribute { Id = c.Id, Values = c.Values })));

            var gar = new GoalActivationRule{ EndpointUtil = new StubEndpointUtils(), PlanUtils = new StubPlanElementUtils() };
            string userId = "1234";
            PlanElementEventArg arg = new PlanElementEventArg
            {
                Program = new DTO.Program {Id = "111111111111111111111111"},
                PatientId = "2222222222222222222222pt",
                UserId = "1234"
            };

            SpawnElement pe = new SpawnElement { };
            ProgramAttributeData pad = new ProgramAttributeData{};
            
            gar.Execute(userId, arg, pe, pad);
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Test.Stubs;
using Funq;
using ServiceStack.WebHost.Endpoints;

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
        }
    }
}

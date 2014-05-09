using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class PlanElementUtil_Tests
    {
        [TestMethod()]
        public void ResponseSpawnAllowed_Test()
        {
            DTO.Step s = new DTO.Step { StepTypeId = 15 };
            DTO.Response r = new DTO.Response {  Id = "000000000000000000000000", Value=""};
            PlanElementUtil.ResponseSpawnAllowed(s, r);
            Assert.Fail();
        }
    }
}

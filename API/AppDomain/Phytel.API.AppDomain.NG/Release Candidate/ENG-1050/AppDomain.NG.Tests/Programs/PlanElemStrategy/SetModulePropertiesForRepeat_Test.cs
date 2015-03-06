using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Phytel.API.AppDomain.NG.PlanElementStrategy.Tests
{
    [TestClass()]
    public class SetModulePropertiesForRepeat_Test
    {
        [TestMethod()]
        public void Execute_Test()
        {
            Module mod = new Module
            {
                ElementState = 5,
                Completed = true,
                DateCompleted = DateTime.UtcNow,
                StateUpdatedOn = DateTime.UtcNow,
                CompletedBy = "Testing"
            };

            IElementAction action = new SetModulePropertiesForRepeat(mod);
            action.Execute();

            Assert.AreEqual(mod.ElementState, 4);
            Assert.AreEqual(mod.Completed, false);
            Assert.AreEqual(mod.DateCompleted, null);
            Assert.AreEqual(((DateTime)mod.StateUpdatedOn).Date, DateTime.UtcNow.Date);
            Assert.AreEqual(mod.CompletedBy, string.Empty);
        }
    }
}

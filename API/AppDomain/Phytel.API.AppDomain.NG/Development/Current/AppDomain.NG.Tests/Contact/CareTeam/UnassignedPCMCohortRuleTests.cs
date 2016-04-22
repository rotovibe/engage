using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Phytel.API.AppDomain.NG.Test.Contact

{
    [TestClass]
    public class UnassignedPCMCohortRuleTests
    {
        [TestInitialize]
        public void SetUp()
        {
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnassignedPCMCohortRule_Run_Null_Input_ShouldThrow()
        {
            
            var rule = new UnAssignedPCMRule();

            rule.Run(null);

        }

    }
}

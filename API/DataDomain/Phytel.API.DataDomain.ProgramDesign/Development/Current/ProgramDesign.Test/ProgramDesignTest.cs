using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ProgramDesign.DTO;

namespace Phytel.API.DataDomain.ProgramDesign.Test
{
    [TestClass]
    public class ProgramDesignTest
    {
        private ProgramDesignDataManager pm;

        [TestMethod]
        public void GetProgramDesignByID()
        {
            GetProgramDesignRequest request = new GetProgramDesignRequest{ ProgramDesignID = "5"};

            GetProgramDesignResponse response = pm.GetProgramDesignByID(request);

            Assert.IsTrue(response.ProgramDesign.ProgramDesignID == "??");
        }
    }
}

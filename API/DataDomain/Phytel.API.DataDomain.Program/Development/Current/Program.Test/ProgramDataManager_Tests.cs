using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class ProgramDataManager_Tests
    {
        [TestMethod()]
        public void GetPatientProgramDetailsById_Test()
        {
            ProgramDataManager pm = new ProgramDataManager { };
            GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
            {
                Version = 1.0,
                ProgramId = "",
                PatientId = "",
                UserId = "",
                ContractNumber = "InHealth001",
                Context = "NG"
            };
            GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
            Assert.IsNotNull(response.Program);
        }
    }
}

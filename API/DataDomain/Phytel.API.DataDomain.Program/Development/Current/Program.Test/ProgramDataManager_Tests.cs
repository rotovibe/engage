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
        [TestClass()]
        public class GetPatientProgramDetailsById_Method
        {
            [TestMethod()]
            public void Get_Valid_Program_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = "535595ddd6a485044c7f0cdb",
                    PatientId = "5325da5ed6a4850adcbba60a",
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program);
            }

            [TestMethod()]
            public void Get_With_Program_Attributes_Test()
            {
                ProgramDataManager pm = new ProgramDataManager { };
                GetProgramDetailsSummaryRequest request = new GetProgramDetailsSummaryRequest
                {
                    Version = 1.0,
                    ProgramId = "535595ddd6a485044c7f0cdb",
                    PatientId = "5325da5ed6a4850adcbba60a",
                    UserId = "nguser",
                    ContractNumber = "InHealth001",
                    Context = "NG"
                };
                GetProgramDetailsSummaryResponse response = pm.GetPatientProgramDetailsById(request);
                Assert.IsNotNull(response.Program.Attributes);
            }
        }

        [TestClass()]
        public class PutProgramActionUpdate_Method
        {
            [TestMethod()]
            public void PutProgramActionUpdate_Test()
            {
                Assert.Fail();
            }
        }
    }
}

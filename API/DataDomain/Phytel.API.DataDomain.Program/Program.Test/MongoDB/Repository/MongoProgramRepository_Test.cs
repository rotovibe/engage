using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.Test.Stubs;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program.Tests
{
    [TestClass()]
    public class MongoProgramRepository_Test
    {
        [TestClass()]
        public class GetLimitedProgramFields_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.Repository")]
            public void Get_With_Objectives_Test()
            {
                string ctrl = "123456789011111111112223";
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.Program);

                MEProgram mep = repo.GetLimitedProgramFields("000000000000000000000000") as MEProgram;
                Assert.IsNotNull(mep.Objectives);
            }

            [TestMethod()]
            [TestCategory("NIGHT-923")]
            [TestProperty("TFS", "3840")]
            [TestProperty("Layer", "DD.Repository")]
            public void Get_With_Two_Objectives_Test()
            {
                StubProgramRepositoryFactory factory = new StubProgramRepositoryFactory();
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "000000000000000000000000"
                };

                IProgramRepository repo = factory.GetRepository(request, RepositoryType.Program);

                MEProgram mep = repo.GetLimitedProgramFields("000000000000000000000000") as MEProgram;
                Assert.AreEqual(2, mep.Objectives.Count);
            }
        }
    }
}

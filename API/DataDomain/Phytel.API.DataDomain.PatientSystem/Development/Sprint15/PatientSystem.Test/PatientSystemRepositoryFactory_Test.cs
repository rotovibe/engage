using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;
namespace Phytel.API.DataDomain.PatientSystem.Tests
{
    [TestClass()]
    public class PatientSystemRepositoryFactory_Test
    {
        [TestClass()]
        public class GetRepository_Method
        {
            [TestMethod()]
            public void Get_PatientSystem()
            {
                MongoPatientSystemRepository cRepo = new MongoPatientSystemRepository("InHealth001");
                IPatientSystemRepositoryFactory factory = new PatientSystemRepositoryFactory();
                GetAllPatientSystemsDataRequest request = new GetAllPatientSystemsDataRequest
                {
                    ContractNumber = "InHealth001",
                    Context = "NG",
                    UserId = "NGUser"
                };

                IPatientSystemRepository repo = factory.GetRepository(request, RepositoryType.PatientSystem);

                Assert.AreEqual(cRepo.GetType(), repo.GetType());
            }
        }
    }
}

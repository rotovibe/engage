using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;

namespace DataDomain.Medication.Repo.Tests
{
    [TestClass()]
    public class MedicationRepositoryFactoryTests
    {
        [TestMethod()]
        public void GetMedicationRepositoryTest()
        {
            var contract = "InHealth001";
            var userid = "123";
            var version = 1;

            var repo =
                MedicationRepositoryFactory.GetMedicationRepository(
                    new DomainRequest {Context = "NG", ContractNumber = contract, UserId = userid, Version = version},
                    RepositoryType.MedicationMapping);

            Assert.AreEqual(repo.GetType(), typeof (MongoMedicationMappingRepository<MedicationMongoContext>));
        }
    }
}

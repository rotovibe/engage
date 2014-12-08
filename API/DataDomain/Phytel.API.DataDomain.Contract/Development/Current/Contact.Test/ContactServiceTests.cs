using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contract.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contract.Test;
using Phytel.API.DataDomain.Contract.Test.Stubs;
using Phytel.API.DataDomain.Contract.DTO;

namespace Phytel.API.DataDomain.Contract.Service.Tests
{
    [TestClass()]
    public class ContractServiceTests
    {
        [TestClass()]
        public class Put
        {
            [TestMethod()]
            public void Put_PatientId_With_ContractId()
            {
                ContractService cs = new ContractService
                {
                    CommonFormat = new StubCommonFormatter(),
                    Helpers = new StubHelpers(),
                    Manager = new ContractDataManager { Factory = new StubContractRepositoryFactory() }
                };

                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "111156789012345678901111",
                    ContractId = "123456789012345678901234",
                    UserId = "666656789012345678906666",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cs.Put(request);

                bool result = response.SuccessData;
                Assert.IsTrue(result);
            }
        }
    }
}

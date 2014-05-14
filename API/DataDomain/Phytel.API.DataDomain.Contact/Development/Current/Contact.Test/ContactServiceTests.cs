using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.Test;
using Phytel.API.DataDomain.Contact.Test.Stubs;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact.Service.Tests
{
    [TestClass()]
    public class ContactServiceTests
    {
        [TestClass()]
        public class Put
        {
            [TestMethod()]
            public void Put_PatientId_With_ContactId()
            {
                ContactService cs = new ContactService
                {
                    CommonFormat = new StubCommonFormatter(),
                    Helpers = new StubHelpers(),
                    Manager = new ContactDataManager { Factory = new StubContactRepositoryFactory() }
                };

                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "111156789012345678901111",
                    ContactId = "123456789012345678901234",
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

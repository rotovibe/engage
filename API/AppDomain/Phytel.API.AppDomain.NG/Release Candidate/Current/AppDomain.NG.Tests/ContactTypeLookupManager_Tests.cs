using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Test.Stubs;
using ServiceStack.Service;
using ServiceStack.ServiceInterface.Testing;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass()]
    public class ContactTypeLookupManager_Tests
    {
        private INGManager GetNGManager()
        {
            INGManager ngManager = new StubNGManager();

            return ngManager;
        }

        [TestMethod]
        public void GetContactTypeLookup_ShouldNot_BeNull()
        {
            INGManager ngManager = GetNGManager();

            GetContactTypeLookupRequest request = new GetContactTypeLookupRequest()
            {
                GroupType = 1,
                ContractNumber = "InHealth001",
                Token = "1234",
                Version = 1.0,
                UserId = "TestUser",
            };
            GetContactTypeLookupResponse response = ngManager.GetContactTypeLookup(request);
            
            Assert.IsNotNull(response);
        }
              
    }
} 

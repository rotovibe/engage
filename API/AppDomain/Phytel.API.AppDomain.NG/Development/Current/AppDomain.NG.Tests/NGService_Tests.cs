using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Service.Tests.Factories;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Service.Tests
{
    [TestClass()]
    public class NGService_Tests
    {
        [TestMethod()]
        public void Post_Test()
        {
            ISecurityManager ism = SecurityManagerFactory.Get();
            INGManager ingm = NGManagerFactory.Get();

            NGService ngs = new NGService{ Security = ism, NGManager = ingm };

            GetPatientRequest request = new GetPatientRequest { ContractNumber="NG", PatientID="", Token="dsafgsdfgdafg", UserId="", Version= 1.0 };
            //((ServiceStack.ServiceInterface.Service)ngs).
            GetPatientResponse response = ngs.Get(request);

            Assert.Fail();
        }
    }
}

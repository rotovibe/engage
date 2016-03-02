using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Engage.Population;
using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DataDomainClient.Fakes;
using AppDomain.Engage.Population.DTO.Context;
using Microsoft.QualityTools.Testing.Fakes.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Tests
{
    [TestClass()]
    public class DemographicsManagerTests
    {
        [TestMethod()]
        public void DoSomethingTest()
        {
            const string contract = "TestContract001";
            IServiceContext context = new ServiceContext {Contract = contract, Tag = null, Token = "Token", UserId = "userid", Version = 1};
            IPatientDataDomainClient client = new StubPatientDataDomainClient("http://localhost:888/Population", new Helpers(), new JsonServiceClient(), context);
            DemographicsManager manager = new DemographicsManager(context, client);
            var result = manager.DoSomething();
            Assert.AreEqual("TestContract001", contract);
        }
    }
}

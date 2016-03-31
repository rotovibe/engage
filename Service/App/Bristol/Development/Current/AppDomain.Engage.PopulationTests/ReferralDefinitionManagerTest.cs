﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Engage.Population;
using AppDomain.Engage.Population.DataDomainClient;
using AppDomain.Engage.Population.DTO.Context;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.Common;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.PopulationTests
{
    [TestClass]
    public class ReferralDefinitionManagerTest
    {
        // Test stub
        [TestMethod()]
        public void DoSomethingTest()
        {
            const string contract = "TestContract001";
            IServiceContext context = new ServiceContext { Contract = contract, Tag = null, Token = "Token", UserId = "userid", Version = 1 };
            IPatientDataDomainClient client = new PatientDataDomainClient(Mapper.Engine, "http://localhost:888/ReferralDefinition", new Helpers(), new JsonServiceClient(), context);
            DemographicsManager manager = new DemographicsManager(context, client);
            var result = manager.DoSomething();
            Assert.AreEqual("TestContract001", contract);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.QueueProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ASE.DTO.Message;

namespace Phytel.Engage.Integrations.ProcessTests
{
    [TestClass()]
    public class IntegrationProcessTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            const string body = @"<RegistryComplete contractid=""465"" contractdatabase=""ORLANDOHEALTH001"" runtype=""Daily"" reportdate=""09/11/1012""/>";
            var queMessage = new QueueMessage {Body = body};

            var proc = new IntegrationProcess();
            proc.Execute(queMessage);
        }
    }
}

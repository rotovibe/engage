using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.Engage.Integrations.QueueProcess;

namespace Phytel.Engage.Integrations.ProcessTests
{
    [TestClass()]
    public class IntegrationProcessTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            //<RegistryComplete contractid="465" contractdatabase="ORLANDOHEALTH001" runtype="Daily" reportdate="09/11/1012"/>
            var watch = Stopwatch.StartNew();
            const string body = @"<RegistryComplete contractid=""465"" contractdatabase=""ORLANDOHEALTH001"" runtype=""Daily"" reportdate=""09/11/1012""/>";
            var queMessage = new QueueMessage {Body = body};

            var proc = new IntegrationProcess();
            proc.Execute(queMessage);

            watch.Stop();
            var elapsed = TimeSpan.FromMilliseconds( watch.ElapsedMilliseconds).TotalMinutes;
        }
    }
}
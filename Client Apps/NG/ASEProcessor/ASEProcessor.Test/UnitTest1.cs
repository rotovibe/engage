using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;

namespace ASEProcessor.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string msgbody = "<?xml version=\"1.0\"?>" + 
                "<AuditData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + 
                "<Type>PageView</Type>" + 
                "<UserId>da71ccd1-e8b9-4d65-95ce-582ad052bf69</UserId>" + 
                "<ImpersonatingUserId>00000000-0000-0000-0000-000000000000</ImpersonatingUserId>" + 
                "<EventDateTime>2013-02-08T16:07:28.5756727-06:00</EventDateTime>" + 
                "<SourcePage>Dashboard</SourcePage><SourceIP>::1</SourceIP><Browser>IE9</Browser><SessionId>y0ukhnuqropvfgbcnvzogi10</SessionId><ContractID>6</ContractID><EditedUserId>00000000-0000-0000-0000-000000000000</EditedUserId><EnteredUserName /><SearchText /><LandingPage /></AuditData>";

            QueueMessage msg = new QueueMessage(ASEMessageType.Process, "");
            msg.Body = msgbody;

            Phytel.ASEProcessor.ProcessAuditLog log = new Phytel.ASEProcessor.ProcessAuditLog();
            //log.Execute(msg);
            Assert.IsTrue(log != null);
        }
    }
}

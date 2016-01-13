using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Data.ETLASEProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ASE.DTO.Message;

namespace Phytel.Data.ETLASEProcess.Tests
{
    [TestClass()]
    public class ETLProcessTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            var etl = new ETLProcess();
            var body = "<Contract>InHealthEngage001</Contract>";
            var message = new QueueMessage {Body = body};
            etl.Execute(message);
        }
    }
}

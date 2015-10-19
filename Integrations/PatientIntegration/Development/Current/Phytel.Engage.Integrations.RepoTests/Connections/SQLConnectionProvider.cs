using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.Repo.Connections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Phytel.Engage.Integrations.Repo.Connections.Tests
{
    [TestClass()]
    public class SQLConnectionProvider_Test
    {
        [TestMethod()]
        public void GetConnectionString_Test()
        {
            var pvd = new SQLConnectionProvider();
            var context = "ORLANDOHEALTH001";
            var connection = pvd.GetConnectionStringEF(context);
        }
    }
}

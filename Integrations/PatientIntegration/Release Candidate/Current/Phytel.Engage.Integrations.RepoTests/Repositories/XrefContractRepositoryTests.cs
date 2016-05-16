using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Engage.Integrations.Repo.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.Engage.Integrations.Repo.Bridge;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTO;

namespace Phytel.Engage.Integrations.Repo.Repositories.Tests
{
    [TestClass()]
    public class XrefContractRepositoryTests
    {
        [TestMethod()]
        public void SelectAllTest()
        {
            var cr = new XrefContractRepository("ORLANDOHEALTH001", new SQLConnectionProvider(), new ORLANDOHEALTH001Implementation());
            var result = cr.SelectAll();
            Assert.AreEqual(typeof(List<PatientXref>), result.GetType());
        }
    }
}

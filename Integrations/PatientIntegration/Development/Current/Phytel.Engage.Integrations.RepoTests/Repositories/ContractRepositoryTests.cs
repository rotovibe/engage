using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.Repositories;

namespace Phytel.Engage.Integrations.Repo.RepositoriesTests
{
    [TestClass()]
    public class ContractRepositoryTests
    {
        [TestMethod()]
        public void Select()
        {
            var cr = new PatientsContractRepository("ORLANDOHEALTH001", new SQLConnectionProvider()) { ConnStr = new SQLConnectionProvider() };
            cr.SelectAll(); 
        }
    }

}

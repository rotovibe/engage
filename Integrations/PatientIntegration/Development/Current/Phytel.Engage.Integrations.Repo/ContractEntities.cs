using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Phytel.Engage.Integrations.Repo
{

    public partial class ContractEntities : DbContext
    {
        public ContractEntities(string connectionString) : base(connectionString)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 120;
        }
    }
}
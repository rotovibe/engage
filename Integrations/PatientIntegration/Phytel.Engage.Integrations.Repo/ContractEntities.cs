using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Phytel.Engage.Integrations.Repo
{

    public partial class ContractEntities : DbContext
    {
        public ContractEntities(string connectionString) : base(connectionString)
        {
            this.Database.CommandTimeout = 0;
        }
    }
}
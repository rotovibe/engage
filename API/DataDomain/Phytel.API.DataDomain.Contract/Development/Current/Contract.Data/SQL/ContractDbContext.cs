using Phytel.API.DataDomain.Contract.Data;
using Phytel.API.DataDomain.Contract.Data.Map;
using System.Data.Entity;

namespace Phytel.API.DataDomain.Contract.Data
{
    public class ContractDbContext : DbContext
    {
        static ContractDbContext()
        {
            Database.SetInitializer<ContractDbContext>(null);
        }

        public ContractDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Contract> Contracts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContractMap());
        }
    }
}

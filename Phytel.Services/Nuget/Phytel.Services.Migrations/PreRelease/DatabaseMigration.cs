using Phytel.Services.Versions;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public abstract class DatabaseMigration : Phytel.Services.Migrations.IDatabaseMigration
    {
        protected readonly Funq.Container _container;
        protected readonly string _product;
        protected readonly SemanticVersion _version;

        public DatabaseMigration(string product, int major, int minor, int patch, Funq.Container container)
            : this(product, new SemanticVersion(major, minor, patch), container)
        {
        }

        public DatabaseMigration(string product, SemanticVersion version, Funq.Container container)
        {
            _product = product;
            _version = version;
            _container = container;
        }

        public string Product
        {
            get { return _product; }
        }

        public SemanticVersion Version
        {
            get { return _version; }
        }

        public abstract void Down(Connection database);

        public abstract void Up(Connection database);
    }
}
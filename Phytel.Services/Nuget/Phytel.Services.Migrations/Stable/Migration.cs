using Phytel.Services.Versions;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Migrations
{
    public abstract class Migration : IMigration
    {
        protected readonly SemanticVersion _version;
        protected readonly string _product;
        protected readonly Funq.Container _container;

        public Migration(string product, int major, int minor, int patch, Funq.Container container)
            : this(product, new SemanticVersion(major, minor, patch), container)
        {
        }

        public Migration(string product, SemanticVersion version, Funq.Container container)
        {
            _product = product;
            _version = version;
            _container = container;
        }

        public SemanticVersion Version
        {
            get { return _version; }
        }

        public string Product
        {
            get { return _product; }
        }

        public abstract void Up();

        public abstract void Down();
    }
}
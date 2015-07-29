using Phytel.Services.Versions;

namespace Phytel.Services.Migrations
{
    public abstract class Migration : IMigration
    {
        protected readonly Funq.Container _container;
        protected readonly string _product;
        protected readonly SemanticVersion _version;

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

        public string Product
        {
            get { return _product; }
        }

        public SemanticVersion Version
        {
            get { return _version; }
        }

        public abstract void Down();

        public abstract void Up();
    }
}
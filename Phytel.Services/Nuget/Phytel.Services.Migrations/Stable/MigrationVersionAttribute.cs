using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations
{
    public class MigrationVersionAttribute : Attribute
    {
        protected readonly string _product;
        protected readonly SemanticVersion _version;

        public MigrationVersionAttribute(string product, int major, int minor, int patch)
        {
            _version = new SemanticVersion(major, minor, patch);
            _product = product;
        }

        public string Product { get; set; }

        public SemanticVersion Version { get { return _version; } }
    }
}
using Phytel.Services.Versions;

namespace Phytel.Services.Migrations
{
    public interface IMigration
    {
        string Product { get; }

        SemanticVersion Version { get; }

        void Down();

        void Up();
    }
}
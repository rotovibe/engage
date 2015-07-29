using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations
{
    public interface IMigration
    {
        SemanticVersion Version { get; }

        string Product { get; }

        void Up();

        void Down();
    }
}
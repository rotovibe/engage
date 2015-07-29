using ServiceStack.ServiceHost;

namespace Phytel.Services.Migrations
{
    [Route("/{Version}/Migrations/{Product}/{Major}/{Minor}/{Patch}", "POST")]
    public class PostMigration : IReturn<MigrationResponse>
    {
        public double Version { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Patch { get; set; }

        public string Product { get; set; }
    }
}
using ServiceStack.ServiceHost;

namespace Phytel.Services.Migrations
{
    [Route("/{Version}/Migrations", "GET")]
    public class GetMigrations : IReturn<MigrationsResponse>
    {
        public string Product { get; set; }

        public double Version { get; set; }
    }
}
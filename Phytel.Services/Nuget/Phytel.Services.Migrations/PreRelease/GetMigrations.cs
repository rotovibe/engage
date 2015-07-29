using Phytel.Services.API.DTO;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    [Route("/{Version}/Migrations", "GET")]
    public class GetMigrations : IReturn<MigrationsResponse>, IVersionRequest, IJournaledAsChildRequest
    {
        public string Product { get; set; }

        public double Version { get; set; }

        public List<Connection> Connections { get; set; }

        public string ParentActionId { get; set; }

        public string ActionId { get; set; }
    }
}
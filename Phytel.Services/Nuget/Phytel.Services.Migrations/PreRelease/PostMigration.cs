using Phytel.Services.API.DTO;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    [Route("/{Version}/Migrations/{Product}/{Major}/{Minor}/{Patch}", "POST")]
    public class PostMigration : IReturn<MigrationResponse>, IVersionRequest, IJournaledAsChildRequest
    {
        public double Version { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Patch { get; set; }

        public string Product { get; set; }

        public string ParentActionId { get; set; }

        public string ActionId { get; set; }

        public List<Connection> Connections { get; set; }
    }
}
using Phytel.Services.SQLServer;
using System.Data.Entity.Core.EntityClient;

namespace Phytel.Engage.Integrations.Repo.Connections
{
    public class SQLConnectionProvider : ISQLConnectionProvider
    {
        private string _connString;

        public string GetConnectionString(string context)
        {
            _connString = SQLDataService.Instance.GetConnectionString("Phytel", context, true, "Contract");
            var entStringBuilder = new EntityConnectionStringBuilder();
            entStringBuilder.ProviderConnectionString = _connString;
            entStringBuilder.Provider = "System.Data.SqlClient";
            entStringBuilder.Metadata = "res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl";
            return entStringBuilder.ConnectionString;
        }
    }
}

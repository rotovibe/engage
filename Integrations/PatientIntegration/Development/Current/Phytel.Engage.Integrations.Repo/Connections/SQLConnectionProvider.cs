using Phytel.Services.SQLServer;

namespace Phytel.Engage.Integrations.Repo.Connections
{
    public class SQLConnectionProvider : ISQLConnectionProvider
    {
        private string _connString;

        public string GetConnectionString(string context)
        {
            //_connString = SQLDataService.Instance.GetConnectionString(context, context, true, "REPORT");
            _connString = ""; //SQLDataService.Instance.GetConnectionString(context, context, true, "REPORT");
            return "";
        }
    }
}

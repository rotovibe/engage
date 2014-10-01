using Phytel.Services.SQLServer;
using System.Collections.Specialized;

namespace Phytel.Services.Web.Security
{
    public class SqlProfileProvider : System.Web.Profile.SqlProfileProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            string dbConnName = config["connectionStringName"];
            config.Add("connectionString", SQLDataService.Instance.GetConnectionString(dbConnName, false));

            base.Initialize(name, config);
        }
    }
}

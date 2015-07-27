using Phytel.Services.SQLServer;

namespace Phytel.Services.Web.Security
{
    public class SqlMembershipProvider : System.Web.Security.SqlMembershipProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            string dbConnName = config["connectionStringName"];
            config.Add("connectionString", SQLDataService.Instance.GetConnectionString(dbConnName, false));

            base.Initialize(name, config);
        }
    }
}

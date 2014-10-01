using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Phytel.Web.Security
{
    public class SqlMembershipProvider : System.Web.Security.SqlMembershipProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            string dbConnName = config["connectionStringName"];
            config.Add("connectionString", Phytel.Services.SQLDataService.Instance.GetConnectionString(dbConnName, false));

            base.Initialize(name, config);
        }
    }
}

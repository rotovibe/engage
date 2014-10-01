using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;

namespace Phytel.Web.Security
{
    public class SqlRoleProvider : System.Web.Security.SqlRoleProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            string dbConnName = config["connectionStringName"];
            config.Add("connectionString", Phytel.Services.SQLDataService.Instance.GetConnectionString(dbConnName, false));

            base.Initialize(name, config);
        }
    }
}

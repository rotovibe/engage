using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;

namespace Phytel.Web.Security
{
    public class SqlProfileProvider : System.Web.Profile.SqlProfileProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            string dbConnName = config["connectionStringName"];
            config.Add("connectionString", Phytel.Services.SQLDataService.Instance.GetConnectionString(dbConnName, false));

            base.Initialize(name, config);
        }
    }
}

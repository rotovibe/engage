using System;
using System.Configuration;
using Phytel.Services.SQLServer;
using System.Data.Entity.Core.EntityClient;
using Phytel.Engage.Integrations.DTO;

namespace Phytel.Engage.Integrations.Repo.Connections
{
    public class SQLConnectionProvider : ISQLConnectionProvider
    {
        private string _connString;

        public string GetConnectionStringEF(string context)
        {
            try
            {
                _connString = SQLDataService.Instance.GetConnectionString(ConfigurationManager.AppSettings["PhytelServicesConnName"], context, true, "Contract");
                
                //// temporary fix
                //_connString = "server=" + ConfigurationManager.AppSettings["ConDb"] + 
                //    ";database=Hillcrest001;user id=\"" + ConfigurationManager.AppSettings["ConUserName"] + 
                //    "\";password=" + ConfigurationManager.AppSettings["ConPassword"] + ";"; 

                var entStringBuilder = new EntityConnectionStringBuilder
                {
                    ProviderConnectionString = _connString,
                    Provider = "System.Data.SqlClient",
                    Metadata = "res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl"
                };

                return entStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetConnectionString(string context)
        {
            _connString = SQLDataService.Instance.GetConnectionString(ConfigurationManager.AppSettings["PhytelServicesConnName"], context, true, "Contract");

            return _connString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using Phytel.Framework.SQL.Data;

namespace C3.Business
{
    public class TOSService : SqlDataAccessor
    {
        private string _dbConnName = "Phytel";

        public TOSService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public TermsOfService GetLatestTOS()
        {
            TermsOfService tos = Query<TermsOfService>(null, _dbConnName, StoredProcedure.GetLatestTOS, TermsOfService.Build);
            return tos;
        }
    }
}

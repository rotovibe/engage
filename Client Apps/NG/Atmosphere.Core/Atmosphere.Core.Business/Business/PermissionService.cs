using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using C3.Data;
using Phytel.Framework.SQL.Data;
using System.Configuration;

namespace C3.Business
{
    public class PermissionService : SqlDataAccessor
    {
        private string _dbConnName = "Phytel";

        public PermissionService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public List<Permission> GetAll()
        {
            return QueryAll<Permission>(null, _dbConnName, StoredProcedure.GetAllPermissions, Permission.Build);
        }

        public List<Permission> GetByUser(Guid userId, int contractId)
        {
            return QueryAll<Permission>(null, _dbConnName, StoredProcedure.GetUserPermissions, Permission.Build, userId, contractId);
        }
    }
}

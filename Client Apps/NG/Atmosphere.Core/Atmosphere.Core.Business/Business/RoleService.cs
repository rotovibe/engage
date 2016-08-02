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
    public class RoleService : SqlDataAccessor
    {
        private static volatile RoleService _instance;
        private static object _syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public RoleService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public static RoleService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if ( _instance == null)
                            _instance = new RoleService();
                    }
                }
                return _instance;
            }
        }


        public List<Role> GetContractRoles(int contractId)
        {
            return QueryAll<Role>(null, _dbConnName, StoredProcedure.GetContractRoles, Role.Build, contractId);
        }
        
        public List<RolePermission> GetRolePermissions(Guid roleId)
        {
            return QueryAll<RolePermission>(null, _dbConnName, StoredProcedure.GetRolePermissions, RolePermission.Build, roleId);
        }

        public Role GetRoleByName(string name)
        {
            return Query<Role>(null, _dbConnName, StoredProcedure.GetRoleByName, Role.Build, name);
        }
    }
}

using Phytel.Framework.SQL.Data;
using System;
using System.Runtime.Serialization;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public int PermissionId { get; set; }

        public static RolePermission Build(ITypeReader reader)
        {
            RolePermission rolePermission = new RolePermission();

            rolePermission.RoleId = reader.GetGuid("RoleId");
            rolePermission.PermissionId = reader.GetInt("PermissionId");

            return rolePermission;
        }
    }
}

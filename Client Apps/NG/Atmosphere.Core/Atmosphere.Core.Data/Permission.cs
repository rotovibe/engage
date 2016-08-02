using Phytel.Framework.SQL.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Permission //: ITimeStamp
    {
        [DataMember]
        public int PermissionId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int PermissionTypeId { get; set; }

        public static Permission Build(ITypeReader reader)
        {
            Permission permission = new Permission();

            permission.PermissionId = reader.GetInt("PermissionId");
            permission.Name = reader.GetString("Name");
            permission.Description = reader.GetString("Description");
            permission.PermissionTypeId = reader.GetInt("PermissionTypeId");

            return permission;
        }
    }

    [Serializable]
    public class PermissionList : List<Permission>
    {
        public PermissionList()
        {
        }
    }
}

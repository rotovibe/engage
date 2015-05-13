using System;
using System.Data;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;
using Phytel.Framework.SQL;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Role
    {
        [DataMember]
        public Guid RoleId { get; set;  }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string LoweredRoleName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int UserTypeId { get; set; }
        [DataMember]
        public int ContractId { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public bool IsSelected { get; set; }

        public static Role Build(ITypeReader reader)
        {
            Role role = new Role();

            role.RoleId = reader.GetGuid("RoleId");
            role.RoleName = reader.GetString("RoleName");
            role.LoweredRoleName = reader.GetString("LoweredRoleName");
            role.Description = reader.GetString("Description");
            role.UserTypeId = reader.GetInt("UserTypeId");
            role.ContractId = reader.GetInt("ContractId");
            role.Level = reader.GetInt("Level");

            return role;
        }

        //FF: This was done for Impersonation User
        public static Role BuildLight(DataRow row)
        {
            Role role = new Role();

            role.RoleId = Converter.ToGuid(row["RoleId"].ToString());
            role.RoleName = row["RoleName"].ToString();

            return role;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using C3.Data.Enum;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    public class UserGroup
    {
        public Guid UserId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }

        public static UserGroup Build(ITypeReader reader)
        {
            UserGroup group = new UserGroup();

            group.UserId = reader.GetGuid("UserId");
            group.GroupId = reader.GetInt("GroupId");
            group.Name = reader.GetString("Name");

            return group;
        }
    }
}

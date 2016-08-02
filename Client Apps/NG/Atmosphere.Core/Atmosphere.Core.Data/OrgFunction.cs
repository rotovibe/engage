using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    /// <summary>
    /// The Organization Function is assigned to a user to describe what his/her function is within an organization.
    /// Unlike Roles, which determine what a user may or may not do in the system from a security standpoint, an organization function will only
    /// impact business logic concerning what data is presented.
    /// </summary>
    
    [Serializable]
    public class OrgFunction
    {
        public int OrgFunctionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        /* Base Overrides */

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            var orgFunction = obj as OrgFunction;
            if (orgFunction == null) return false;
            return this.OrgFunctionId == orgFunction.OrgFunctionId;
        }

        public override int GetHashCode()
        {
            return this.OrgFunctionId;
        }

        /* End Base Overrides */

    }

    public class OrgFunctionBuilder
    {
        public static OrgFunction Build(ITypeReader reader)
        {
            var orgFunction = new OrgFunction();
            orgFunction.OrgFunctionId = reader.GetInt("OrgFunctionId");
            orgFunction.Name = reader.GetString("OrgFunctionName");
            orgFunction.Description = reader.GetString("Description", string.Empty);
            orgFunction.IsDeleted = reader.GetBool("DeleteFlag", false);
            return orgFunction;
        }
    }

    public static class OrgFunctionExtensions
    {
        public static string ToIdXml(this List<OrgFunction> orgFunctions)
        {
            var sb = new StringBuilder();
            sb.Append("<OrgFunctionIds>");
            foreach (var o in orgFunctions)
            {
                sb.Append(String.Format("<Id>{0}</Id>", o.OrgFunctionId.ToString(CultureInfo.InvariantCulture)));
            }
            sb.Append("</OrgFunctionIds>");
            return sb.ToString();
        }
    }
}

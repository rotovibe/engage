using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Control //: ITimeStamp
    {
        [DataMember]
        public int ControlId { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool IsTab { get; set; }
        [DataMember]
        public bool IsSubTab { get; set; }
        [DataMember]
        public bool IsVisible { get; set; }
        [DataMember]
        public int ParentControlId { get; set; }
        [DataMember]
        public string ParentControlName { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Description { get; set; }

        public static Control Build(ITypeReader reader)
        {
            Control control = new Control();

            control.ControlId = reader.GetInt("ControlId");
            control.DisplayOrder = reader.GetInt("DisplayOrder");
            control.IsTab = reader.GetBool("IsTab");
            control.IsSubTab = reader.GetBool("IsSubTab");
            control.IsVisible = reader.GetBool("IsVisible");
            control.ParentControlId = reader.GetInt("ParentControlId");
            control.Name = reader.GetString("Name");
            control.Path = reader.GetString("Path");
            control.Description = reader.GetString("Description");
            control.ParentControlName = reader.GetString("ParentControlName");

            return control;
        }

        #region Lists

        public List<Permission> Permissions
        {
            get
            {
                List<Permission> _permissions = new List<Permission>();
                return _permissions;
            }
        }

        public List<Control> SubTabs { get; set; }

        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class ControlPermission //: ITimeStamp
    {
        [DataMember]
        public int ControlId { get; set; }
        [DataMember]
        public int PermissionId { get; set; }
        [DataMember]
        public int PermissionTypeId { get; set; }

        public static ControlPermission Build (ITypeReader reader)
        {
            ControlPermission controlPermissionDTO = new ControlPermission();

            controlPermissionDTO.ControlId = reader.GetInt("ControlId");
            controlPermissionDTO.PermissionId = reader.GetInt("PermissionId");
            controlPermissionDTO.PermissionTypeId = reader.GetInt("PermissionTypeId");

            return controlPermissionDTO;
        }        
    }    
}

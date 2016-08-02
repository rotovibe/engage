using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{    
    [Serializable]
    [DataContract]
    public class AuditItem
    {
        [DataMember]
        public int PatientID { get; set; }

        public static AuditItem Build(ITypeReader reader)
        {
            AuditItem _auditItem = new AuditItem();

            _auditItem.PatientID = reader.GetInt("PatientId");

            return _auditItem;
        }
    } 
   
    [Serializable]
    public class AuditItemList : List<AuditItem>
    {
        public AuditItemList()
        {
        }
    }
}

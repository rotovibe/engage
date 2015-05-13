using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "OptOutType", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class OptOutType
    {   
        #region Public Properties

        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public int ProtocolId { get; set; }
        [DataMember]
        public string ProtocolName { get; set; }


        #endregion

        #region Public Methods

        public static OptOutType Build(ITypeReader reader)
        {
            OptOutType ooType = new OptOutType();

            ooType.ProtocolId = reader.GetInt("ProtocolId");
            ooType.ProtocolName = reader.GetString("ProtocolName");

            return ooType;
        }

        public static OptOutType Build(DataRow row)
        {
            OptOutType ooType = new OptOutType();

            ooType.ProtocolId = int.Parse(row["ProtocolId"].ToString());
            ooType.ProtocolName = row["ProtocolName"].ToString();

            return ooType;
        }


        #endregion
    }
}

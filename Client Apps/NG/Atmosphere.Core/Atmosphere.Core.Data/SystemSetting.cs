using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "SystemSetting", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class SystemSetting
    {
        #region Constants
        public struct Columns
        {
            public const string KEYNAME = "KeyName";
            public const string VALUE = "Value";
           

        }
        #endregion

        #region Public Properties

        [DataMember]
        public string KeyName { get; set; }
        [DataMember]
        public string Value { get; set; }
       

        #endregion

        #region Public Methods

        public static SystemSetting Build(ITypeReader reader)
        {
            SystemSetting systemSetting = new SystemSetting();

            systemSetting.KeyName = reader.GetString(Columns.KEYNAME);
            systemSetting.Value  = reader.GetString(Columns.VALUE);
            return systemSetting;
        }

        public static SystemSetting Build(DataRow row)
        {
            SystemSetting systemSetting = new SystemSetting();
            systemSetting.KeyName = row[Columns.KEYNAME].ToString();
            systemSetting.Value = row[Columns.VALUE].ToString();
            return systemSetting;
        }


        #endregion
    }
}

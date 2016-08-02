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
    public class ApplicationSetting
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }

        public static ApplicationSetting Build(ITypeReader reader)
        {
            ApplicationSetting applicationSetting = new ApplicationSetting();
            applicationSetting.Key = reader.GetString("Key");
            applicationSetting.Value = reader.GetString("Value");
            return applicationSetting;
        }

    }
}

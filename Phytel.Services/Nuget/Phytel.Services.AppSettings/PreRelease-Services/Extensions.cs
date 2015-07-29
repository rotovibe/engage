using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Phytel.Services.AppSettings
{
    public static class Extensions
    {
        public const string AseAppSettingsXpath = "//ProcessConfiguration/AppSettings/AppSetting";
        public const string AttributeNameKey = "key";
        public const string AttributeNameValue = "value";

        public static IDictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            IDictionary<string, string> rvalues = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                rvalues.Add(k, col[k]);
            }
            return rvalues;
        }

        public static IDictionary<string, string> ToDictionary(this XmlNode configuration, string xpathToAppSettings = AseAppSettingsXpath, string attributeNameValue = AttributeNameValue, string attributeNameKey = AttributeNameKey)
        {
            Dictionary<string, string> rvalue = new Dictionary<string, string>();
            var appSettings = configuration.SelectNodes(xpathToAppSettings);
            foreach (XmlNode appSetting in appSettings)
            {
                string value = string.Empty;
                XmlAttribute valueAsXmlAttribute = appSetting.Attributes[attributeNameValue];
                if (valueAsXmlAttribute != null)
                {
                    value = valueAsXmlAttribute.Value;
                }

                string key = string.Empty;
                XmlAttribute keyAsXmlAttribute = appSetting.Attributes[attributeNameKey];
                if (keyAsXmlAttribute != null)
                {
                    key = keyAsXmlAttribute.Value;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    rvalue.Add(key, value);
                }
            }

            return rvalue;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Phytel.Services.AppSettings
{
    public class AseAppSettingsProvider : IAppSettingsProvider
    {
        protected readonly XmlNode _configuration;
        protected IDictionary<string, string> _appSettings;

        public AseAppSettingsProvider(XmlNode configuration)
        {
            _configuration = configuration;
        }

        public virtual IDictionary<string, string> AppSettings
        {
            get
            {
                if (_appSettings == null || _appSettings.Any() == false)
                {
                    _appSettings = BuildAppSettings(_configuration);
                }

                return _appSettings;
            }
        }

        public string Get(string key)
        {
            string rvalue = string.Empty;

            if (AppSettings.ContainsKey(key))
            {
                rvalue = AppSettings[key];
            }

            return rvalue;
        }

        protected virtual IDictionary<string, string> BuildAppSettings(XmlNode configuration)
        {
            Dictionary<string, string> rvalue = new Dictionary<string, string>();
            var appSettings = configuration.SelectNodes("//ProcessConfiguration/AppSettings/AppSetting");
            foreach (XmlNode appSetting in appSettings)
            {
                string value = string.Empty;
                XmlAttribute valueAsXmlAttribute = appSetting.Attributes["value"];
                if (valueAsXmlAttribute != null)
                {
                    value = valueAsXmlAttribute.Value;
                }

                string key = string.Empty;
                XmlAttribute keyAsXmlAttribute = appSetting.Attributes["key"];
                if (keyAsXmlAttribute != null)
                {
                    key = keyAsXmlAttribute.Value;
                }

                if (!String.IsNullOrEmpty(key))
                {
                    rvalue.Add(key, value);
                }
            }

            return rvalue;
        }
    }
}
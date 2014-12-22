using System.Collections.Specialized;
using System.Configuration;

namespace Phytel.Services.AppSettings
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        protected readonly NameValueCollection _appSettings;

        public AppSettingsProvider()
            : this(ConfigurationManager.AppSettings)
        {
        }

        public AppSettingsProvider(NameValueCollection appSettings)
        {
            _appSettings = appSettings;
        }

        public string Get(string key)
        {
            string rvalue = string.Empty;

            if (_appSettings.HasKeys())
            {
                rvalue = _appSettings[key];
            }

            return rvalue;
        }

        public int GetAsInt(string key)
        {
            int rvalue = default(int);

            string rvalueAsString = Get(key);

            if(!string.IsNullOrEmpty(rvalueAsString))
            {
                int.TryParse(rvalueAsString, out rvalue);
            }

            return rvalue;
        }

        public int GetAsInt(string key, int defaultValue)
        {
            int rvalue = defaultValue;

            string rvalueAsString = Get(key, defaultValue.ToString());
            if(!string.IsNullOrEmpty(rvalueAsString))
            {
                bool isInt = int.TryParse(rvalueAsString, out rvalue);
                if(!isInt)
                {
                    rvalue = defaultValue;
                }
            }

            return rvalue;
        }

        public string Get(string key, string defaultValue)
        {
            return Get(key, null, defaultValue);
        }

        public string Get(string key, string value, string defaultValue)
        {
            string rvalue = defaultValue;

            if(string.IsNullOrEmpty(value))
            {
                string valueFromAppSetting = Get(key);
                if(!string.IsNullOrEmpty(valueFromAppSetting))
                {
                    rvalue = valueFromAppSetting;
                }
            }

            return rvalue;
        }
    }
}
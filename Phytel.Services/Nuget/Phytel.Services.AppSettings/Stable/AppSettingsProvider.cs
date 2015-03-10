using System.Collections.Generic;
using System.Linq;

namespace Phytel.Services.AppSettings
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        protected readonly IDictionary<string, string> _appSettings;

        public AppSettingsProvider(IDictionary<string, string> appSettings)
        {
            _appSettings = appSettings;
        }

        public string Get(string key)
        {
            return Get(key, string.Empty);
        }

        public string Get(string key, string defaultValue)
        {
            return Get(key, null, defaultValue);
        }

        public virtual string Get(string key, string value, string defaultValue)
        {
            string rvalue = value;

            if (string.IsNullOrEmpty(rvalue))
            {
                rvalue = defaultValue;

                if (_appSettings.Any() && _appSettings.ContainsKey(key))
                {
                    string valueFromAppSettings = _appSettings[key];
                    if (!string.IsNullOrEmpty(valueFromAppSettings))
                    {
                        rvalue = valueFromAppSettings;
                    }
                }
            }

            return rvalue;
        }

        public int GetAsInt(string key)
        {
            return GetAsInt(key, default(int));
        }

        public virtual int GetAsInt(string key, int defaultValue)
        {
            int rvalue = defaultValue;

            string rvalueAsString = Get(key, defaultValue.ToString());
            if (!string.IsNullOrEmpty(rvalueAsString))
            {
                bool isInt = int.TryParse(rvalueAsString, out rvalue);
                if (!isInt)
                {
                    rvalue = defaultValue;
                }
            }

            return rvalue;
        }

        public virtual bool GetAsBoolean(string key, bool defaultValue = false)
        {
            bool rvalue = defaultValue;

            string rvalueAsString = Get(key, defaultValue.ToString());
            if(!string.IsNullOrEmpty(rvalueAsString))
            {
                bool isBool = bool.TryParse(rvalueAsString, out rvalue);
                if(!isBool)
                {
                    rvalue = defaultValue;
                }
            }

            return rvalue;
        }
    }
}
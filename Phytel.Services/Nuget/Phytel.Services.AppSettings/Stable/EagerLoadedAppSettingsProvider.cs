using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.AppSettings
{
    public class EagerLoadedAppSettingsProvider : AppSettingsProvider
    {
        protected readonly IDictionary<string, string> _appSettings;

        public EagerLoadedAppSettingsProvider(IDictionary<string, string> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override string OnGetValueFromSource(string key)
        {
            string rvalue = null;
            if (_appSettings != null && _appSettings.Any() && _appSettings.ContainsKey(key))
            {
                string valueFromAppSettings = _appSettings[key];
                if (!string.IsNullOrEmpty(valueFromAppSettings))
                {
                    rvalue = valueFromAppSettings;
                }
            }
            return rvalue;
        }
    }
}

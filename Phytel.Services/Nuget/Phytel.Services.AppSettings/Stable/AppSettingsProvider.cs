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
    }
}
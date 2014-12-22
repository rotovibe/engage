using System.Collections.Specialized;
using System.Configuration;

namespace Phytel.Services.AppSettings
{
    public class ConfigAppSettingsProvider : AppSettingsProvider
    {
        public ConfigAppSettingsProvider()
            : this(ConfigurationManager.AppSettings)
        {
        }

        public ConfigAppSettingsProvider(NameValueCollection appSettings)
            : base(appSettings.ToDictionary())
        {
        }
    }
}
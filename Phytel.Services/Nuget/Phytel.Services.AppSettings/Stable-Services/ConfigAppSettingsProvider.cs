using System.Collections.Specialized;
using System.Configuration;

namespace Phytel.Services.AppSettings
{
    public class ConfigAppSettingsProvider : EagerLoadedAppSettingsProvider
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
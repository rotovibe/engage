using System.Xml;

namespace Phytel.Services.AppSettings
{
    public class AseAppSettingsProvider : EagerLoadedAppSettingsProvider
    {
        public AseAppSettingsProvider(XmlNode configuration)
            : base(configuration.ToDictionary())
        {
        }
    }
}
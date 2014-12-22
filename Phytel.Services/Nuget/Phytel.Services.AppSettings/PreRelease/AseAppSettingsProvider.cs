using System.Xml;

namespace Phytel.Services.AppSettings
{
    public class AseAppSettingsProvider : AppSettingsProvider
    {
        public AseAppSettingsProvider(XmlNode configuration)
            : base(configuration.ToDictionary())
        {
        }
    }
}
namespace Phytel.Services.AppSettings
{
    public interface IAppSettingsProvider
    {
        string Get(string key);

        string Get(string key, string value, string defaultValue);
    }
}
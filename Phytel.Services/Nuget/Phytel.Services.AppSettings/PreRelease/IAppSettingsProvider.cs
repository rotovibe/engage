namespace Phytel.Services.AppSettings
{
    public interface IAppSettingsProvider
    {
        string Get(string key);
    }
}
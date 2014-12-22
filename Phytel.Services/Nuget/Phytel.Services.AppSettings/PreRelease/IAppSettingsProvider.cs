﻿namespace Phytel.Services.AppSettings
{
    public interface IAppSettingsProvider
    {
        string Get(string key);

        int GetAsInt(string key);

        int GetAsInt(string key, int defaultValue);

        string Get(string key, string defaultValue);

        string Get(string key, string value, string defaultValue);
    }
}
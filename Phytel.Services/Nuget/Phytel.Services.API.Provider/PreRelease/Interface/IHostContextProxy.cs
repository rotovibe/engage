using System;
namespace Phytel.Services.API.Provider
{
    public interface IHostContextProxy
    {
        T GetItem<T>(string key) where T : class;

        object GetItem(string key);

        string GetItemAsString(string key);

        double GetItemAsDouble(string key);

        void SetItem(string key, object value);
    }
}
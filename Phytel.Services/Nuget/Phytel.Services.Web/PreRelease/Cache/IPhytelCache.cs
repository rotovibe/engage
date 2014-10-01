using System;

namespace Phytel.Services.Web.Cache
{
    public interface IPhytelCache
    {
        void ClearCache(string prefix);

        T Get<T>(string prefix, string key);

        void Put<T>(string prefix, string key, T value);

        void Put<T>(string prefix, string key, T value, TimeSpan cacheTime);

        void Remove(string prefix, string key);
    }
}

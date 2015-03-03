using Phytel.Services.Dates;
using System;
using System.Collections.Generic;

namespace Phytel.Services.API.Cache
{
    public class MongoClientCache : ServiceStack.CacheAccess.ICacheClient
    {
        protected readonly IPhytelCache _cache;
        protected readonly IDateTimeProxy _dateTimeProxy;

        public MongoClientCache(IPhytelCache cache, IDateTimeProxy dateTimeProxy)
        {
            _cache = cache;
            _dateTimeProxy = dateTimeProxy;
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            bool rvalue = false;
            try
            {
                if (!_cache.Exists(Constants.ApplicationScopePrefix, key))
                {
                    _cache.Put<T>(Constants.ApplicationScopePrefix, key, value, expiresIn);
                    rvalue = true;
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            bool rvalue = false;
            try
            {
                DateTime now = _dateTimeProxy.GetCurrentDateTime();
                if (expiresAt > now)
                {
                    TimeSpan expiresIn = now.Subtract(expiresAt);
                    rvalue = Add<T>(key, value, expiresIn);
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Add<T>(string key, T value)
        {
            bool rvalue = false;
            try
            {
                if (!_cache.Exists(Constants.ApplicationScopePrefix, key))
                {
                    _cache.Put<T>(Constants.ApplicationScopePrefix, key, value);
                    rvalue = true;
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public long Decrement(string key, uint amount)
        {
            long nextVal = default(long);
            if(_cache.Exists(Constants.ApplicationScopePrefix, key))
            {
                nextVal = Get<long>(key);   
            }

            nextVal -= amount;

            _cache.Put<long>(Constants.ApplicationScopePrefix, key, nextVal);

            return nextVal;
        }

        public void Dispose()
        {
        }

        public void FlushAll()
        {
            _cache.ClearCache(Constants.ApplicationScopePrefix);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(Constants.ApplicationScopePrefix, key);
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            IDictionary<string, T> rvalues = new Dictionary<string, T>();
            foreach (string key in keys)
            {
                T value = Get<T>(key);
                KeyValuePair<string, T> rvalue = new KeyValuePair<string, T>(key, value);
                rvalues.Add(rvalue);
            }

            return rvalues;
        }

        public long Increment(string key, uint amount)
        {
            long nextVal = default(long);
            if (_cache.Exists(Constants.ApplicationScopePrefix, key))
            {
                nextVal = Get<long>(key);
            }

            nextVal += amount;

            _cache.Put<long>(Constants.ApplicationScopePrefix, key, nextVal);

            return nextVal;
        }

        public bool Remove(string key)
        {
            bool rvalue = false;

            try
            {
                _cache.Remove(Constants.ApplicationScopePrefix, key);
                rvalue = true;
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                Remove(key);
            }
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            bool rvalue = false;
            try
            {
                if (_cache.Exists(Constants.ApplicationScopePrefix, key))
                {
                    _cache.Put<T>(Constants.ApplicationScopePrefix, key, value, expiresIn);
                    rvalue = true;
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            bool rvalue = false;
            try
            {
                DateTime now = _dateTimeProxy.GetCurrentDateTime();
                if (expiresAt > now)
                {
                    TimeSpan expiresIn = now.Subtract(expiresAt);
                    rvalue = Replace<T>(key, value, expiresIn);
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Replace<T>(string key, T value)
        {
            bool rvalue = false;
            try
            {
                if (_cache.Exists(Constants.ApplicationScopePrefix, key))
                {
                    _cache.Put<T>(Constants.ApplicationScopePrefix, key, value);
                    rvalue = true;
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            bool rvalue = false;
            try
            {
                _cache.Put<T>(Constants.ApplicationScopePrefix, key, value, expiresIn);
                rvalue = true;
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            bool rvalue = false;
            try
            {
                DateTime now = _dateTimeProxy.GetCurrentDateTime();
                if (expiresAt > now)
                {
                    TimeSpan expiresIn = now.Subtract(expiresAt);
                    rvalue = Set<T>(key, value, expiresIn);
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public bool Set<T>(string key, T value)
        {
            bool rvalue = false;
            try
            {
                _cache.Put<T>(Constants.ApplicationScopePrefix, key, value);
                rvalue = true;
            }
            catch (Exception)
            {
                rvalue = false;
            }

            return rvalue;
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            foreach (var kvp in values)
            {
                Set<T>(kvp.Key, kvp.Value);
            }
        }
    }
}
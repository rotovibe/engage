using Phytel.Services.Mongo;
using Phytel.Services.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Phytel.Services.Web.Cache
{
    /// <summary>
    /// Cache with Mongo backing
    /// </summary>
    public class MongoWebCache : IPhytelCache
    {
        #region ctor and singleton

        private MongoWebCache()
        {
        }

        private static Lazy<MongoWebCache> _instance = new Lazy<MongoWebCache>(() =>
        {
            return new MongoWebCache();
        },
            true);

        public static MongoWebCache Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        public void Put<T>(string prefix, string key, T value)
        {
            Put<T>(prefix, key, value, new TimeSpan(8, 0, 0));
        }

        public void Put<T>(string prefix, string key, T value, TimeSpan cacheTime)
        {
            RetryHelper.DoWithRetry(() =>
            {
                using (ASPSessionContext context = new ASPSessionContext())
                {
                    string cacheKey = CreateKey(prefix, key);
                    MongoCacheItem item = context.Cache.FirstOrDefault(e => e.Key == cacheKey);

                    if (item == null)
                    {
                        item = new MongoCacheItem();
                        item.Key = cacheKey;
                    }

                    item.Expiry = DateTime.Now.Add(cacheTime);

                    List<string> serialized = Serialize<T>(value);

                    if (serialized.Count == 1)
                    {
                        item.Value = serialized[0];

                    }
                    else
                    {
                        PutValueChunks(context, cacheKey, serialized, item.Expiry);
                        item.Value = Serialize<CacheHeader>(new CacheHeader { Chunks = serialized.Count }).First();
                    }

                    context.Cache.Save(item);
                }
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY);
        }

        public T Get<T>(string prefix, string key)
        {
            return Get<T>(prefix, key, new TimeSpan(8, 0, 0));
        }

        public T Get<T>(string prefix, string key, TimeSpan cacheTime)
        {
            return RetryHelper.DoWithRetry<T>(() =>
            {
                using (ASPSessionContext context = new ASPSessionContext())
                {
                    string cacheKey = CreateKey(prefix, key);

                    MongoCacheItem item = context.Cache.FirstOrDefault(e => e.Key == cacheKey);

                    if (item != null)
                    {
                        item.Expiry = DateTime.Now.Add(cacheTime);
                        context.Cache.Save(item);

                        object result = Deserialize(item.Value);

                        if (result.GetType() == typeof(CacheHeader))
                        {
                            string value = GetValueFromChunks(context, cacheKey, ((CacheHeader)result).Chunks, cacheTime);
                            result = Deserialize(value);
                        }

                        return (T)result;
                    }
                    else
                        return default(T);
                }
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY);
        }

        public void Remove(string prefix, string key)
        {
            RetryHelper.DoWithRetry(() =>
            {
                using (ASPSessionContext context = new ASPSessionContext())
                {
                    string cacheKey = CreateKey(prefix, key);
                    context.Cache.Remove(e => e.Key == cacheKey);
                }
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY);
        }

        public void ClearCache(string prefix)
        {
            RetryHelper.DoWithRetry(() =>
            {
                using (ASPSessionContext context = new ASPSessionContext())
                {
                    context.Cache.Remove(e => e.Key.StartsWith(prefix + "_"));
                }
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY);
        }

        private string GetValueFromChunks(ASPSessionContext context, string key, int chunks, TimeSpan cacheTime)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < chunks; i++)
            {
                string cacheKey = string.Format("{0}_{1}", key, i);

                MongoCacheItem item = context.Cache.FirstOrDefault(e => e.Key == cacheKey);

                item.Expiry = DateTime.Now.Add(cacheTime);
                context.Cache.Save(item);

                result.Append(item.Value);
            }

            return result.ToString();
        }

        private void PutValueChunks(ASPSessionContext context, string key, List<string> serializedValues, DateTime expiry)
        {
            for (int i = 0; i < serializedValues.Count; i++)
            {
                string cacheKey = string.Format("{0}_{1}", key, i);
                MongoCacheItem item = context.Cache.FirstOrDefault(e => e.Key == cacheKey);

                if (item == null)
                {
                    item = new MongoCacheItem();
                    item.Key = cacheKey;
                }

                item.Value = serializedValues[i];
                item.Expiry = expiry;

                context.Cache.Save(item);
            }
        }

        private string CreateKey(string prefix, string clientKey)
        {
            return prefix + "_" + clientKey;
        }

        private static int _maxDocLength = 15000000;

        public static int MaxDocLength
        {
            get { return MongoWebCache._maxDocLength; }
            set { MongoWebCache._maxDocLength = value; }
        }

        private List<string> Serialize<T>(T item)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, item);

            string serializedObject = Convert.ToBase64String(stream.ToArray());

            List<string> result = new List<string>();

            if (serializedObject.Length > MaxDocLength)
            {
                do
                {
                    result.Add(serializedObject.Substring(0, serializedObject.Length > MaxDocLength ? MaxDocLength : serializedObject.Length));

                    if (serializedObject.Length > MaxDocLength)
                    {
                        serializedObject = serializedObject.Substring(MaxDocLength);
                    }
                    else
                    {
                        serializedObject = string.Empty;
                    }
                } while (serializedObject.Length > 0);
            }
            else
            {
                result.Add(serializedObject);
            }

            return result;
        }

        private object Deserialize(string item)
        {
            byte[] itemBytes = Convert.FromBase64String(item);

            MemoryStream stream = new MemoryStream();
            stream.Write(itemBytes, 0, itemBytes.Length);
            stream.Position = 0;

            BinaryFormatter formatter = new BinaryFormatter();
            object result = formatter.Deserialize(stream);

            return result;
        }

        [Serializable]
        private class CacheHeader
        {
            public int Chunks { get; set; }
        }
    }
}

using MongoDB.Bson;
using Phytel.Services.Dates;
using Phytel.Services.Mongo.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Phytel.Services.API.Cache.Mongo
{
    public class CacheRepository : IPhytelCache
    {
        protected readonly IDateTimeProxy _dateTimeProxy;
        protected readonly IRepositoryMongo _repositoryMongo;
        private static int _maxDocLength = 15000000;

        public CacheRepository(IRepositoryMongo repositoryMongo, IDateTimeProxy dateTimeProxy)
        {
            _repositoryMongo = repositoryMongo;
            _dateTimeProxy = dateTimeProxy;
        }

        public static int MaxDocLength
        {
            get { return _maxDocLength; }
            set { _maxDocLength = value; }
        }

        public void ClearCache(string prefix)
        {
            _repositoryMongo.Remove<CacheMongoEntity, ObjectId>(e => e.Key.StartsWith(prefix + "_"));
        }

        public bool Exists(string prefix, string key)
        {
            string cacheKey = CreateKey(prefix, key);
            return _repositoryMongo.Exists<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);
        }

        public T Get<T>(string prefix, string key)
        {
            string cacheKey = CreateKey(prefix, key);

            CacheMongoEntity item = _repositoryMongo.First<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);

            if (item != null)
            {
                //CZ: Do not want to keep extending the cache time each time cache is accessed...
                //item.Expiry = DateTime.Now.Add(cacheTime);
                //context.Cache.Save(item);

                object result = Deserialize(item.Value);

                if (result is CacheHeader)
                {
                    string value = GetValueFromChunks(cacheKey, ((CacheHeader)result).Chunks);
                    result = Deserialize(value);
                }

                return (T)result;
            }
            else
                return default(T);
        }

        public void Put<T>(string prefix, string key, T value)
        {
            Put<T>(prefix, key, value, new TimeSpan(8, 0, 0));
        }

        public void Put<T>(string prefix, string key, T value, TimeSpan cacheTime)
        {
            string cacheKey = CreateKey(prefix, key);
            CacheMongoEntity item = _repositoryMongo.First<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);

            if (item == null)
            {
                item = new CacheMongoEntity();
                item.Key = cacheKey;
            }

            item.Expiry = _dateTimeProxy.GetCurrentDateTime().Add(cacheTime);

            List<string> serialized = Serialize<T>(value);

            if (serialized.Count == 1)
            {
                item.Value = serialized[0];
            }
            else
            {
                PutValueChunks(cacheKey, serialized, item.Expiry);
                item.Value = Serialize<CacheHeader>(new CacheHeader { Chunks = serialized.Count }).First();
            }

            _repositoryMongo.Save<CacheMongoEntity, ObjectId>(item);
        }

        public void Remove(string prefix, string key)
        {
            string cacheKey = CreateKey(prefix, key);
            _repositoryMongo.Remove<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);
        }

        private string CreateKey(string prefix, string clientKey)
        {
            return prefix + "_" + clientKey;
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

        private string GetValueFromChunks(string key, int chunks)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < chunks; i++)
            {
                string cacheKey = string.Format("{0}_{1}", key, i);

                CacheMongoEntity item = _repositoryMongo.First<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);

                //CZ: Do not want to keep extending the cache time each time cache is accessed...
                //item.Expiry = DateTime.Now.Add(cacheTime);
                //context.Cache.Save(item);

                result.Append(item.Value);
            }

            return result.ToString();
        }

        private void PutValueChunks(string key, List<string> serializedValues, DateTime expiry)
        {
            for (int i = 0; i < serializedValues.Count; i++)
            {
                string cacheKey = string.Format("{0}_{1}", key, i);
                CacheMongoEntity item = _repositoryMongo.First<CacheMongoEntity, ObjectId>(e => e.Key == cacheKey);

                if (item == null)
                {
                    item = new CacheMongoEntity();
                    item.Key = cacheKey;
                }

                item.Value = serializedValues[i];
                item.Expiry = expiry;

                _repositoryMongo.Save<CacheMongoEntity, ObjectId>(item);
            }
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

        [Serializable]
        private class CacheHeader
        {
            public int Chunks { get; set; }
        }
    }
}
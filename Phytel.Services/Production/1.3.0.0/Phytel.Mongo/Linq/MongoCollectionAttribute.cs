using System;

namespace Phytel.Mongo.Linq
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public class MongoCollectionAttribute : Attribute
    {
        public string CollectionName { get; set; }
    }
}

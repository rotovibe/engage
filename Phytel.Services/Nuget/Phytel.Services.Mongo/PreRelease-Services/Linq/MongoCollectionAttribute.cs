using System;

namespace Phytel.Services.Mongo.Linq
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public class MongoCollectionAttribute : Attribute
    {
        public string CollectionName { get; set; }
    }
}

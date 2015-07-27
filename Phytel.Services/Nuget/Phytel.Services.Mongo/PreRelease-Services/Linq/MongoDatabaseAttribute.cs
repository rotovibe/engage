using System;

namespace Phytel.Services.Mongo.Linq
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class MongoDatabaseAttribute : Attribute
    {
        public string DatabaseName { get; set; }
    }
}

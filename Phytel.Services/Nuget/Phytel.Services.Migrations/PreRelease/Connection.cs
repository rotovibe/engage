using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Phytel.Services.Migrations
{
    public class Connection
    {
        public string Name { get; set; }

        public DatabaseTypes DatabaseType { get; set; }

        public bool IsContract { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Phytel.Services.Migrations.Mongo;
using Phytel.Services.Mongo.Linq;
using System;

namespace Phytel.Services.Migrations.Mongo
{
    public class MigrationMongoContext : MongoContext
    {
        public MigrationMongoContext(string database, bool isContract = false)
            : base(database, isContract)
        {
            MigrationLogs = new MongoSet<MigrationLogEntity, ObjectId>(this, MongoDB.Driver.WriteConcern.Acknowledged);
            MigrationDatabaseLogs = new MongoSet<MigrationDatabaseLogEntity, ObjectId>(this, MongoDB.Driver.WriteConcern.Acknowledged);         
        }

        public MongoSet<MigrationLogEntity, ObjectId> MigrationLogs { get; set; }

        public MongoSet<MigrationDatabaseLogEntity, ObjectId> MigrationDatabaseLogs { get; set; }
    }
}
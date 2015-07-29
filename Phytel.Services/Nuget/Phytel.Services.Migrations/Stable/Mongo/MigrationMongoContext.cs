using MongoDB.Bson;
using Phytel.Services.Migrations.Mongo;
using Phytel.Services.Mongo.Linq;

namespace Phytel.Services.Migrations.Mongo
{
    public class MigrationMongoContext : MongoContext
    {
        public MigrationMongoContext(string database, bool isContract = false)
            : base(database, isContract)
        {
            MigrationLogs = new MongoSet<MigrationLogEntity, ObjectId>(this, MongoDB.Driver.WriteConcern.Acknowledged);
        }

        public MongoSet<MigrationLogEntity, ObjectId> MigrationLogs { get; set; }
    }
}
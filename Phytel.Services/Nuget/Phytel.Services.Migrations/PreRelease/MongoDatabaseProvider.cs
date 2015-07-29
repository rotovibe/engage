using MongoDB.Driver;
using Phytel.Services.Mongo;

namespace Phytel.Services.Migrations
{
    public class MongoDatabaseProvider : IDatabaseProvider
    {
        public Database Get(Connection connection)
        {
            Database rvalue = new Database();
            string connectionString = MongoService.Instance.GetConnectionString(connection.Name, connection.IsContract);
            MongoUrl url = MongoUrl.Create(connectionString);
            rvalue.Name = url.DatabaseName;
            rvalue.Type = DatabaseTypes.Mongo;
            rvalue.Server = url.Server.Host;
            return rvalue;
        }
    }
}
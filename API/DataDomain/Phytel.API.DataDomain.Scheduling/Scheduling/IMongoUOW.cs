using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.ToDo
{
    public interface IMongoUOW<TContext> where TContext : MongoContext
    {
        string Db { get; }
        TContext MongoContext { get; }
    }
}
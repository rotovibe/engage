using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Search
{
    public interface IMongoUOW<TContext> where TContext : MongoContext
    {
        string Db { get; }
        TContext MongoContext { get; }
    }
}
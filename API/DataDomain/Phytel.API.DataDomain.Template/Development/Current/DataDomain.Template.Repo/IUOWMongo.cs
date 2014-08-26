using Phytel.Mongo.Linq;

namespace DataDomain.Template.Repo
{
    public interface IUOWMongo<TContext> where TContext : MongoContext
    {
        TContext MongoContext { get; }
    }
}
using Phytel.Mongo.Linq;

namespace DataDomain.Template.Repo
{
    public interface IUOWMongo<TContext> where TContext : TemplateMongoContext
    {
        TContext MongoContext { get; }
    }
}
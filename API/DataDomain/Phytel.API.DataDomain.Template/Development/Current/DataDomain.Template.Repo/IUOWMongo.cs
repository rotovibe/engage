using System;
using Phytel.Mongo.Linq;

namespace DataDomain.Template.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : TemplateMongoContext
    {
        TContext MongoContext { get; }
    }
}
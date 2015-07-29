using Phytel.Services.Mongo.Linq;
using System;

namespace Phytel.Services.Mongo.Repository
{
    public interface IUnitOfWorkMongo<TContext> : IDisposable
        where TContext : MongoContext
    {
        TContext MongoContext { get; }
    }
}
using System;

namespace DataDomain.Search.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : SearchMongoContext
    {
        TContext MongoContext { get; }
    }
}
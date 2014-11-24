using System;

namespace DataDomain.Allergy.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : AllergyMongoContext
    {
        TContext MongoContext { get; }
    }
}
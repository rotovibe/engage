using System;

namespace Phytel.API.DataDomain.PatientSystem.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : PatientSystemMongoContext
    {
        TContext MongoContext { get; }
    }
}
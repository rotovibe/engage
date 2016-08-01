using System;

namespace DataDomain.Medication.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : MedicationMongoContext
    {
        TContext MongoContext { get; }
    }
}
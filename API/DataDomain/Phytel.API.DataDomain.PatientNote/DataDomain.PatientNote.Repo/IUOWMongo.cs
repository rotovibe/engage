using System;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public interface IUOWMongo<TContext> : IDisposable where TContext : PatientNoteMongoContext
    {
        TContext MongoContext { get; }
    }
}
using System;
using System.Data.Entity;

namespace Phytel.Services.SQLServer.Repository
{
    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        TContext DbContext { get; }
    }

    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
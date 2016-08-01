using System;

namespace DataDomain.Allergy.Repo
{
    public class UOWMongo<TContext> : IUOWMongo<TContext>, IDisposable where TContext : AllergyMongoContext
    {
        protected readonly TContext _context;

        public TContext MongoContext
        {
            get { return _context; }
        }

        public UOWMongo(string database, bool isContract)
        {
            _context = (Activator.CreateInstance(typeof (TContext), new object[]{ database, isContract }) as TContext);
        }

        public UOWMongo(string database)
        {
            _context = (Activator.CreateInstance(typeof (TContext), new object[]{ database}) as TContext);
        }

        public void Dispose()
        {
            var context = _context;
            context.Dispose();
        }
    }
}

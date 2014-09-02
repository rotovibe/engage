using System;
using Phytel.Mongo.Linq;

namespace DataDomain.Template.Repo
{
    public class UOWMongo<TContext> : IUOWMongo<TContext>, IDisposable where TContext : TemplateMongoContext
    {
        protected readonly TContext _context;

        public TContext MongoContext
        {
            get { return _context; }
        }

        public UOWMongo(string database, bool isContract)
        {
            _context = (Activator.CreateInstance(typeof (TContext), new object[]
            {
                database,
                isContract
            }) as TContext);
        }

        public void Dispose()
        {
            TContext context = _context;
            context.Dispose();
        }
    }
}

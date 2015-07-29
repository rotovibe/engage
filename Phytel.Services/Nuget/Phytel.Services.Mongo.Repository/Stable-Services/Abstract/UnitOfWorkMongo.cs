
using Phytel.Services.Mongo.Linq;
namespace Phytel.Services.Mongo.Repository
{
    public class UnitOfWorkMongo<TContext> : IUnitOfWorkMongo<TContext>
        where TContext : MongoContext
    {
        protected readonly TContext _context;

        public UnitOfWorkMongo(string database, bool isContract)
        {
            _context = System.Activator.CreateInstance(typeof(TContext), database, isContract) as TContext;
        }

        public UnitOfWorkMongo(string connectionString, string databaseName)
        {
            _context = System.Activator.CreateInstance(typeof(TContext), connectionString, databaseName) as TContext;
        }

        public UnitOfWorkMongo(TContext context)
        {
            _context = context;
        }

        public TContext MongoContext
        {
            get { return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
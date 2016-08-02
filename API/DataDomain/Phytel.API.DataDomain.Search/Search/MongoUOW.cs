using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Search
{
    public class MongoUOW<TContext> :   IMongoUOW<TContext> , IDisposable where TContext : global::Phytel.Mongo.Linq.MongoContext
   {
        protected readonly TContext _context;
        protected readonly string _db;

        public string Db
        {
            get { return _db; }
        }

        public MongoUOW(string database, bool isContract)
        {
            _db = database;
        }

        public TContext MongoContext
        {
            get { return _context; }
        }

        public void Dispose()
        {
        }
   }
}

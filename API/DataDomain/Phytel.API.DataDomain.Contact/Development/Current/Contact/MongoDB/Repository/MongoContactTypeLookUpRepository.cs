using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common.Audit;
using Phytel.API.DataDomain.Contact.MongoDB;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public class MongoContactTypeLookUpRepository : IContactTypeLookUpRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        public IAuditHelpers AuditHelpers { get; set; }


        public MongoContactTypeLookUpRepository()
        {
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof (MEContactTypeLookup)) == false)
                    BsonClassMap.RegisterClassMap<MEContactTypeLookup>();
            }
            catch
            {
                
            }
        }
        public MongoContactTypeLookUpRepository(string dbname)
        {
            _dbName = dbname;
            AppHostBase.Instance.Container.AutoWire(this);
        }
        public object GetContactTypeLookUps(GroupType type)
        {
            var dataResponse = new List<MEContactTypeLookup>();
            using (var ctx = new ContactTypeLookUpMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                {
                    Query<MEContactTypeLookup>.EQ(c => c.Active, true),
                    Query<MEContactTypeLookup>.EQ(c => c.DeleteFlag, false)
                };


                if (type != GroupType.Unknown)
                {
                    var typeMongoQuery = Query<MEContactTypeLookup>.EQ(c => c.Group, type);
                    queries.Add(typeMongoQuery);

                }

                var query = Query.And(queries);
                var lookUps = ctx.ContactTypeLookUps.Collection.Find(query);
                dataResponse = lookUps.ToList();
            }

            return dataResponse;
        }

        public string UserId { get; set; }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}

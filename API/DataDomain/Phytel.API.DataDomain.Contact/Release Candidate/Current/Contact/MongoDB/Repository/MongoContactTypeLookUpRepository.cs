using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common.Audit;
using Phytel.API.DataDomain.Contact.DTO;
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

        #region IContactTypeLookUpRepository Members

        public object GetContactTypeLookUps(ContactLookUpGroupType type)
        {
            var dataResponse = new List<MEContactTypeLookup>();
            using (var ctx = new ContactTypeLookUpMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                {
                    Query<MEContactTypeLookup>.EQ(c => c.Active, true),
                    Query<MEContactTypeLookup>.EQ(c => c.DeleteFlag, false)
                };


                if (type != ContactLookUpGroupType.Unknown)
                {
                    var typeMongoQuery = Query<MEContactTypeLookup>.EQ(c => c.GroupId, type);
                    queries.Add(typeMongoQuery);

                }

                var query = Query.And(queries);
                var sortBy = new SortByBuilder<MEContactTypeLookup>().Ascending(c => c.Name);
                var lookUps = ctx.ContactTypeLookUps.Collection.Find(query);
                dataResponse = lookUps.SetSortOrder(sortBy).ToList();
            }

            return dataResponse;
        }

        public string SaveContactTypeLookUp(DTO.ContactTypeLookUpData request, string userId)
        {
            //var data = this.FindByID(request.Id);
            var id = request.Id == null ? ObjectId.GenerateNewId() : ObjectId.Parse(request.Id); 

            
            //If not exists add.
            if (string.IsNullOrEmpty(request.Id) )
            {
                var dataEntity = new MEContactTypeLookup(userId, DateTime.Now)
                {
                    Active = true,
                    DeleteFlag = false,
                    Id = id,
                    GroupId = (ContactLookUpGroupType)request.Group,
                    ParentId = request.ParentId != null ? ObjectId.Parse(request.ParentId) : ObjectId.Empty,
                    Role = request.Role,
                    Name = request.Name
                    
                };

                this.Insert(dataEntity);
            }
            else
            {
                this.Update(request);
            }

            return id.ToString();
        }

        #endregion

        #region IRepository Members

        public string UserId { get; set; }

        public object Insert(object newEntity)
        {
            var dataToInsert = (MEContactTypeLookup) newEntity;
            using (var ctx = new ContactTypeLookUpMongoContext(_dbName))
            {
                ctx.ContactTypeLookUps.Save(dataToInsert);
            }

            return dataToInsert.Id.ToString();
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
            MEContactTypeLookup data = null;
            using (var ctx = new ContactTypeLookUpMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                {
                    Query<MEContactTypeLookup>.EQ(c => c.Id, ObjectId.Parse(entityID)),
                    Query<MEContactTypeLookup>.EQ(c => c.Active, true),
                    Query<MEContactTypeLookup>.EQ(c => c.DeleteFlag, false)
                };



                var query = Query.And(queries);
                var lookUp = ctx.ContactTypeLookUps.Collection.FindOne(query);

                if (lookUp == null)
                    throw new Exception(string.Format("No Lookup Item found for Id: {0}", data.Id));

                data = lookUp;

                
            }
            return data;
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
            var data = (ContactTypeLookUpData) entity;

            using (var ctx = new ContactTypeLookUpMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                {
                    Query<MEContactTypeLookup>.EQ(c => c.Id, ObjectId.Parse(data.Id)),
                    Query<MEContactTypeLookup>.EQ(c => c.Active, true),
                    Query<MEContactTypeLookup>.EQ(c => c.DeleteFlag, false)
                };

                var query = Query.And(queries);
                var lookUp = ctx.ContactTypeLookUps.Collection.FindOne(query);

                if(lookUp == null)
                    throw new Exception(string.Format("No Lookup Item found for Id: {0}", data.Id));

                if (!string.IsNullOrEmpty(data.Name))
                    lookUp.Name = data.Name;

                if (!string.IsNullOrEmpty(data.Role))
                    lookUp.Role = data.Role;

                lookUp.LastUpdatedOn = DateTime.UtcNow;

                ctx.ContactTypeLookUps.Save(lookUp);
            }

            return true;
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

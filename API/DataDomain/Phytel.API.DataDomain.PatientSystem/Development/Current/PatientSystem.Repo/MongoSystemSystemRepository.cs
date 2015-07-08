using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.PatientSystem.Repo;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class MongoSystemSourceRepository<TContext> : IMongoPatientSystemRepository where TContext : PatientSystemMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }
        
        public MongoSystemSourceRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoSystemSourceRepository(TContext context)
        {
            Context = context;
        }

        public MongoSystemSourceRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

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


        public IEnumerable<object> Find(object entity)
        {
            GetSystemSourcesDataRequest request = (GetSystemSourcesDataRequest)entity;
            List<SystemSourceData> dataList = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MESystemSource.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MESystemSource> meSS = ctx.SystemSources.Collection.Find(mQuery).ToList();
                    if (meSS != null && meSS.Count > 0)
                    {
                        dataList = new List<SystemSourceData>();
                        meSS.ForEach(s =>
                        {
                            SystemSourceData ssData = new SystemSourceData
                            {
                                Id = s.Id.ToString(),
                                Field = s.Field,
                                Name = s.Name,
                                DisplayLabel = s.DisplayLabel,
                                Primary = s.Primary,
                                StatusId = (int)s.Status
                            };
                            dataList.Add(ssData);
                        });
                    }
                }
                return ((IEnumerable<object>)dataList);
            }
            catch (Exception) { throw; }
        }
    }
}

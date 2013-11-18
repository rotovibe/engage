using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.LookUp;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoConditionRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoConditionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public T Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public T InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            ConditionResponse conditionResponse = null;
            using (ConditionMongoContext ctx = new ConditionMongoContext(_dbName))
            {
                MEProblem meCondition = ctx.Conditions.Collection.FindOneById(ObjectId.Parse(entityID));
                if (meCondition != null)
                {
                    conditionResponse = new ConditionResponse();
                    Condition condition = new Condition { ConditionID = meCondition.Id.ToString(), Name = meCondition.Name, Active = meCondition.Active };
                    conditionResponse.Condition = condition;
                }
            }
            return conditionResponse;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            IQueryable<T> query = null;
            List<Condition> conditionList = null;
            using (ConditionMongoContext ctx = new ConditionMongoContext(_dbName))
            {
                List<MEProblem> meConditions = ctx.Conditions.Collection.FindAll().ToList();
                if (meConditions != null)
                {
                    conditionList = new List<Condition>();
                    foreach (MEProblem m in meConditions)
                    {
                        Condition condition = new Condition { ConditionID = m.Id.ToString(), Name = m.Name, Active = m.Active };
                        conditionList.Add(condition);
                    }
                }
            }
            query = ((IEnumerable<T>)conditionList).AsQueryable<T>();
            
            return query;
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}

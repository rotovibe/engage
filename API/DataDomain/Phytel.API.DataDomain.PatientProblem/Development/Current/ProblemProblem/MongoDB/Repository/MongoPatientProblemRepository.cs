using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.AppDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;

namespace Phytel.API.DataDomain.PatientProblem
{
    public class MongoPatientProblemRepository<T> : IPatientProblemRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProblemRepository(string contractDBName)
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
            throw new NotImplementedException();
        }

        public Tuple<int, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            Tuple<int, IQueryable<T>> query = null;
            //List<Problem> problemList = null;
            //using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
            //{
            //    List<MECondition> meConditions = ctx.Conditions.Collection.FindAll().ToList();
            //    if (meConditions != null)
            //    {
            //        conditionList = new List<Condition>();
            //        foreach (MECondition m in meConditions)
            //        {
            //            Condition condition = new Condition { ConditionID = m.Id.ToString(), DisplayName = m.DisplayName, IsActive = m.IsActive };
            //            conditionList.Add(condition);
            //        }
            //    }
            //}
            //query = ((IEnumerable<T>)conditionList).AsQueryable<T>();

            return query;
        }

        public IQueryable<T> SelectAll()
        {
            throw new NotImplementedException();
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

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.AppDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.Interface;

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

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            IQueryable<T> returnQuery = null;
            IMongoQuery mQuery = null;

            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if(selectExpressions.Count > 0)
            {
                IList<IMongoQuery> queries = new List<IMongoQuery>();
                for (int i = 0; i < selectExpressions.Count; i++)
                {

                    groupType = selectExpressions[0].NextExpressionType;
                    
                    Type t = selectExpressions[i].Value.GetType();
                    
                    if (selectExpressions[i].Type == SelectExpressionType.EQ)
                        queries.Add(Query.EQ(selectExpressions[i].FieldName, BsonValue.Create(convertToAppropriateType(selectExpressions[i].Value)))); 
                }

                if(groupType.Equals(SelectExpressionGroupType.AND))
                {
                    mQuery = Query.And(queries);
                }
                else if (groupType.Equals(SelectExpressionGroupType.OR))
                {
                    mQuery = Query.Or(queries);
                }

                List<Problem> patientProblemList = null;
                using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
                {
                    List<MEPatientProblem> mePatientProblems = ctx.PatientProblems.Collection.Find(mQuery).ToList();
                    if (mePatientProblems != null)
                    { 
                        patientProblemList = new List<Problem>();
                        foreach (MEPatientProblem p in mePatientProblems)
                        {
                            Problem condition = new Problem { 
                                ConditionID = p.ConditionID,
                                PatientID = p.PatientID,
                                ProblemID = p.Id.ToString()
                            };
                            patientProblemList.Add(condition);
                        }
                    }
                }
                returnQuery = ((IEnumerable<T>)patientProblemList).AsQueryable<T>();
            }

            return new Tuple<string, IQueryable<T>>(expression.ExpressionID, returnQuery);
        }

        private object convertToAppropriateType(object p)
        {
            string type = p.GetType().ToString();
            switch (type)
            {
                case "System.Int32":
                    return Convert.ToInt32(p);
                case "System.String":
                    return p.ToString();
                case "System.Int16":
                    return Convert.ToInt16(p);
                case "System.Int64":
                    return Convert.ToInt64(p);
                default:
                    return  p.ToString();
            }
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

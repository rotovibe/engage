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

                    IMongoQuery query = applyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = buildQuery(groupType, queries);

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

        /// <summary>
        /// Applies appropriate query operators on fied name and value.
        /// </summary>
        /// <param name="type">Type can be EQ, NOTEQ, LIKE, NOTLIKE, STARTWITH</param>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value of field</param>
        /// <returns>IMongoQuery object</returns>
        private static IMongoQuery applyQueryOperators(SelectExpressionType type, string fieldName, object value)
        {
            IMongoQuery query = null;
            switch (type)
            {
                case SelectExpressionType.EQ:
                    query = Query.EQ(fieldName, BsonValue.Create(convertToAppropriateType(value)));
                    break;
                case SelectExpressionType.NOTEQ:
                    query = Query.NE(fieldName, BsonValue.Create(convertToAppropriateType(value)));
                    break;
            }
            return query;
        }

        /// <summary>
        /// Builds a single mongo query from list of queries depending on AND or OR clause.
        /// </summary>
        /// <param name="groupType">AND or OR</param>
        /// <param name="queries">List of queries</param>
        /// <returns>A single mongo query</returns>
        private static IMongoQuery buildQuery(SelectExpressionGroupType groupType, IList<IMongoQuery> queries)
        {
            IMongoQuery query = null;
            if (groupType.Equals(SelectExpressionGroupType.AND))
            {
                query = Query.And(queries);
            }
            else if (groupType.Equals(SelectExpressionGroupType.OR))
            {
                query = Query.Or(queries);
            }
            return query;
        }

        /// <summary>
        /// Converts the object to an appropriate type.
        /// </summary>
        /// <param name="p">object to be converted</param>
        /// <returns>converted object.</returns>
        private static object convertToAppropriateType(object p)
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
                case "System.Boolean":
                    return Convert.ToBoolean(p);
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

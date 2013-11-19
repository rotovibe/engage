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

                    IMongoQuery query = SelectExpressionHelper.ApplyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = SelectExpressionHelper.BuildQuery(groupType, queries);

                List<Phytel.API.DataDomain.PatientProblem.DTO.PProb> patientProblemList = null;
                using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
                {
                    List<MEPatientProblem> mePatientProblems = ctx.PatientProblems.Collection.Find(mQuery).ToList();
                    if (mePatientProblems != null)
                    {
                        patientProblemList = new List<PProb>();
                        foreach (MEPatientProblem p in mePatientProblems)
                        {
                            PProb problem = new PProb
                            { 
                                ProblemID = p.ProblemID.ToString(),
                                PatientID = p.PatientID.ToString(),
                                ID = p.Id.ToString()
                            };
                            patientProblemList.Add(problem);
                        }
                    }
                }
                returnQuery = ((IEnumerable<T>)patientProblemList).AsQueryable<T>();
            }

            return new Tuple<string, IQueryable<T>>(expression.ExpressionID, returnQuery);
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

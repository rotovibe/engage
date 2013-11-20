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
    public class MongoProblemRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoProblemRepository(string contractDBName)
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
            GetProblemResponse problemResponse = null;
            using (ProblemMongoContext ctx = new ProblemMongoContext(_dbName))
            {
                MEProblem meProblem = ctx.Problems.Collection.Find(Query.EQ(MEProblem.IdProperty, ObjectId.Parse(entityID))).FirstOrDefault();
                if (meProblem != null)
                {
                    problemResponse = new GetProblemResponse();
                    Problem problem = new Problem { ProblemID = meProblem.Id.ToString(), Name = meProblem.Name, Active = meProblem.Active };
                    problemResponse.Problem = problem;
                }
            }
            return problemResponse;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            IQueryable<T> returnQuery = null;
            IMongoQuery mQuery = null;

            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if (selectExpressions.Count > 0)
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

                List<Problem> patientProblemList = null;
                using (ProblemMongoContext ctx = new ProblemMongoContext(_dbName))
                {
                    List<MEProblem> meProblems = ctx.Problems.Collection.Find(mQuery).ToList();

                    if (meProblems != null)
                    {
                        patientProblemList = new List<Problem>();
                        foreach (MEProblem p in meProblems)
                        {
                            Problem problem = new Problem
                            {
                                ProblemID = p.Id.ToString(),
                                Name = p.Name,
                                Active = p.Active,
                                Type = p.Type
                                
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
            IQueryable<T> query = null;
            List<Problem> problemList = null;
            using (ProblemMongoContext ctx = new ProblemMongoContext(_dbName))
            {
                List<MEProblem> meProblems = ctx.Problems.Collection.FindAll().ToList();
                if (meProblems != null)
                {
                    problemList = new List<Problem>();
                    foreach (MEProblem m in meProblems)
                    {
                        Problem problem = new Problem { ProblemID = m.Id.ToString(), Name = m.Name, Active = m.Active };
                        problemList.Add(problem);
                    }
                }
            }
            query = ((IEnumerable<T>)problemList).AsQueryable<T>();
            
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

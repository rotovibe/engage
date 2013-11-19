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
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEProblem.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEProblem.ActiveProperty, true));
                MEProblem meProblem = ctx.Problems.Collection.Find(Query.And(queries)).FirstOrDefault();
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
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            IQueryable<T> query = null;
            List<Problem> problemList = null;
            using (ProblemMongoContext ctx = new ProblemMongoContext(_dbName))
            {
                List<MEProblem> meProblems = ctx.Problems.Collection.Find(Query.EQ(MEProblem.ActiveProperty, true)).ToList();
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

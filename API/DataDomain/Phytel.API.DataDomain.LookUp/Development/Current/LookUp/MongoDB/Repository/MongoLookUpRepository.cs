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
using DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoLookUpRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoLookUpRepository(string contractDBName)
        {
            _dbName = contractDBName;
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEProblem)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEProblem>();
        }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEObjective)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEObjective>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MECategory)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MECategory>();
            }
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

        public object FindProblemByID(string entityID)
        {
            GetProblemDataResponse problemResponse = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if(meLookup.Data.Count > 0)
                    {
                    problemResponse = new GetProblemDataResponse();
                        MEProblem meProblem = (MEProblem)meLookup.Data.Where(a => a.DataID == ObjectId.Parse(entityID)).FirstOrDefault();
                        if(meProblem != null)
                        {
                            ProblemData problemData = new ProblemData { ProblemID = meProblem.DataID.ToString(), Name = meProblem.Name, Active = meProblem.Active };
                            problemResponse.Problem = problemData;
                }
            }
                }
            }
            return problemResponse;
        }

        public IQueryable<T> SearchProblem(SearchProblemsDataRequest request)
        {
            IQueryable<T> query = null;
            List<ProblemData> problemList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        problemList = new List<ProblemData>();
                        List<MEProblem> meproblems = new List<MEProblem>();
                        if (string.IsNullOrEmpty(request.Type))
                    {
                            meproblems = ((IEnumerable<MEProblem>)meLookup.Data).Where(a => a.Active == request.Active).ToList();
                }
                        else
                {
                            meproblems = ((IEnumerable<MEProblem>)meLookup.Data).Where(a => a.Type == request.Type && a.Active == request.Active).ToList();
                        }

                        foreach (MEProblem m in meproblems)
                            {
                            ProblemData problem = new ProblemData { ProblemID = m.DataID.ToString(), Name = m.Name, Active = m.Active };
                            problemList.Add(problem);
                        }
                    }

                }
            }
            query = ((IEnumerable<T>)problemList).AsQueryable<T>();

            return query;
        }

        public IQueryable<T> GetAllProblems()
        {
            IQueryable<T> query = null;
            List<ProblemData> problemList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
            {
                    if (meLookup.Data != null)
                {
                    problemList = new List<ProblemData>();
                        foreach (MEProblem m in meLookup.Data)
                    {
                            ProblemData problem = new ProblemData { ProblemID = m.DataID.ToString(), Name = m.Name, Active = m.Active };
                        problemList.Add(problem);
                    }
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
    
        public object FindByID(string entityID)
        {
 	        throw new NotImplementedException();
        }

        public Tuple<string,IQueryable<T>> Select(APIExpression expression)
        {
 	        throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
 	        throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Cohort.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Cohort
{
    public class MongoCohortRepository<T> : ICohortRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoCohortRepository(string contractDBName)
        {
            _dbName = contractDBName;
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
            GetCohortDataResponse cohortResponse = null;
            using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MECohort.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MECohort.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MECohort meCohort = ctx.Cohorts.Collection.Find(mQuery).FirstOrDefault();
                if (meCohort != null)
                {
                    cohortResponse = new GetCohortDataResponse();
                    API.DataDomain.Cohort.DTO.CohortData cohort = new API.DataDomain.Cohort.DTO.CohortData { 
                            ID = meCohort.Id.ToString(), 
                            SName = meCohort.ShortName, 
                            Query = meCohort.Query, 
                            QueryWithFilter = meCohort.QueryWithFilter,
                            Sort = meCohort.Sort };
                    cohortResponse.Cohort = cohort;
                }
            }
            return cohortResponse;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            //throw new NotImplementedException();

            using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
            {
                BsonDocument query = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>("{'k':'G','t':'Demo','v':'M','a':1}");
                
                QueryDocument queryDoc = new QueryDocument(query);

                SortByBuilder builder = new SortByBuilder();
                builder.Ascending(new string[] { "ln" });

                List<BsonDocument> responses = ctx.GetDatabase().GetCollection("CohortPatientView").FindAs<BsonDocument>(queryDoc).SetSortOrder(builder).SetSkip(100).SetLimit(10).ToList(); // (SortBy.Ascending(new string[] { "sort field" })).SetSkip(100).SetLimit(50);

                int count = responses.Count;
            }
            return new Tuple<string, IEnumerable<object>>(string.Empty, null);
        }

        public IEnumerable<object> SelectAll()
        {
            IEnumerable<object> query = null;
            List<API.DataDomain.Cohort.DTO.CohortData> cohorts = null;
            using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
            {
                List <MECohort> meCohorts = ctx.Cohorts.Collection.Find(Query.EQ(MECohort.DeleteFlagProperty, false)).ToList();
                if (meCohorts != null)
                {
                    cohorts = new List<API.DataDomain.Cohort.DTO.CohortData>();
                    foreach (MECohort m in meCohorts)
                    {
                        API.DataDomain.Cohort.DTO.CohortData cohort = new API.DataDomain.Cohort.DTO.CohortData { ID = m.Id.ToString(), SName = m.ShortName, Query = m.Query, Sort = m.Sort };
                        cohorts.Add(cohort);
                    }
                }
            }
            query = ((IEnumerable<object>)cohorts); //.AsEnumerable<object>();

            return query;
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}

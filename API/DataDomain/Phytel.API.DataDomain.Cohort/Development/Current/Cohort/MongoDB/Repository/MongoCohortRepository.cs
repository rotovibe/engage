using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Cohort;

namespace Phytel.API.DataDomain.Cohort
{
    public class MongoCohortRepository<T> : ICohortRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoCohortRepository(string contractDBName)
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
            CohortResponse cohortResponse = null;
            using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MECohort.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MECohort.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MECohort meCohort = ctx.Cohorts.Collection.Find(mQuery).FirstOrDefault();
                if (meCohort != null)
                {
                    cohortResponse = new CohortResponse();
                    API.DataDomain.Cohort.DTO.Cohort cohort = new API.DataDomain.Cohort.DTO.Cohort { ID = meCohort.Id.ToString(), SName = meCohort.ShortName };
                    cohortResponse.Cohort = cohort;
                }
            }
            return cohortResponse;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            IQueryable<T> query = null;
            List<API.DataDomain.Cohort.DTO.Cohort> cohorts = null;
            using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
            {
                List <MECohort> meCohorts = ctx.Cohorts.Collection.Find(Query.EQ(MECohort.DeleteFlagProperty, false)).ToList();
                if (meCohorts != null)
                {
                    cohorts = new List<API.DataDomain.Cohort.DTO.Cohort>();
                    foreach (MECohort m in meCohorts)
                    {
                        API.DataDomain.Cohort.DTO.Cohort cohort = new API.DataDomain.Cohort.DTO.Cohort { ID = m.Id.ToString(), SName = m.ShortName };
                        cohorts.Add(cohort);
                    }
                }
            }
            query = ((IEnumerable<T>)cohorts).AsQueryable<T>();

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

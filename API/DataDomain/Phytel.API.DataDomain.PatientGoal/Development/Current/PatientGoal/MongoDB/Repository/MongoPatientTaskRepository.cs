using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.PatientGoal;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientTaskRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientTaskRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PatientTaskData pt = (PatientTaskData)newEntity;
            try
            {
                MEPatientTask pa = new MEPatientTask
                {
                    Id = ObjectId.GenerateNewId(),
                    TTLDate = pt.TTLDate
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientTasks.Collection.Insert(pa);
                    if (wcr.Ok)
                    {
                        pt.Id = pa.Id.ToString();
                    }
                }
                return pt as object;
            }
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByID(string entityID)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                IMongoQuery mQuery = null;
                List<object> PatientGoalItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, PatientGoalItems);
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public string Initialize(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }
    }
}

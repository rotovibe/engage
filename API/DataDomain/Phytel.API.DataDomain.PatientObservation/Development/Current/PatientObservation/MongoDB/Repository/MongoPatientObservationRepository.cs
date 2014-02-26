using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation;
using MB = MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class MongoPatientObservationRepository<T> : IPatientObservationRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);


        public MongoPatientObservationRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
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
                List<object> PatientObservationItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, PatientObservationItems);
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

        object IRepository<T>.Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        object IRepository<T>.InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.Delete(object entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        object IRepository<T>.FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        Tuple<string, IEnumerable<object>> IRepository<T>.Select(APIExpression expression)
        {
            throw new NotImplementedException();
        }

        IEnumerable<object> IRepository<T>.SelectAll()
        {
            throw new NotImplementedException();
        }

        object IRepository<T>.Update(object entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializeObservationDataRequest request = (PutInitializeObservationDataRequest)newEntity;
            PatientObservationData patientObservationData = null;
            
            try
            {
                MEPatientObservation mePg = new MEPatientObservation
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    UpdatedBy = request.UserId,
                    LastUpdatedOn = DateTime.UtcNow,
                    ObservationId = ObjectId.Parse(request.ObservationId)
                };

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientObservations.Collection.Insert(mePg);
                    if (wcr.Ok)
                    {
                        patientObservationData = new PatientObservationData
                        {
                            Id = mePg.Id.ToString()
                        };
                    }
                }
                return patientObservationData;
            }
            catch (Exception ex) { throw ex; }
        }


        public object GetObservationsByType(object newEntity, bool standard)
        {
            throw new NotImplementedException();
        }
    }
}

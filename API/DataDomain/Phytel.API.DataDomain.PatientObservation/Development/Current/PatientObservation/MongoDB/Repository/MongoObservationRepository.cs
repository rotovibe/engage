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

namespace Phytel.API.DataDomain.PatientObservation
{
    public class MongoObservationRepository<T> : IPatientObservationRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoObservationRepository(string contractDBName)
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
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object FindByID(string entityID)
        {
            ObservationData odL = null;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEObservation.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);

                    MEObservation o = ctx.Observations.Collection.Find(mQuery).FirstOrDefault();

                    if (o != null)
                    {
                        odL = new ObservationData
                        {
                            CodingSystem = o.CodingSystemId.ToString(),
                            CodingSystemCode = o.Code,
                            DeleteFlag = o.DeleteFlag,
                            Description = o.Description,
                            ExtraElements = o.ExtraElements,
                            GroupId = o.GroupId != null ? o.GroupId.ToString() : null,
                            LowValue = o.LowValue,
                            HighValue = o.HighValue,
                            Id = o.Id.ToString(),
                            LastUpdatedOn = o.LastUpdatedOn,
                            ObservationType = o.ObservationTypeId.ToString(),
                            Order = o.Order,
                            Source = o.Source,
                            Standard = o.Standard,
                            Status = (int)o.Status,
                            TTLDate = o.TTLDate,
                            Units = o.Units,
                            UpdatedBy = o.UpdatedBy.ToString(),
                            Version = o.Version,
                            CommonName = o.CommonName
                        };
                    }
                }
                return odL;
            }
            catch (Exception) { throw; }
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
            catch (Exception) { throw; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
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
            throw new NotImplementedException();
        }
        
        public object GetObservationsByType(object type, bool standard)
        {
            List<ObservationData> odL = new List<ObservationData>();
            try
            {
                if (type != null)
                {
                    using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        queries.Add(Query.EQ(MEObservation.ObservationTypeProperty, ObjectId.Parse(type as string)));
                        queries.Add(Query.EQ(MEObservation.StandardProperty, standard));
                        queries.Add(Query.EQ(MEObservation.StatusProperty, 1)); // active
                        IMongoQuery mQuery = Query.And(queries);

                        List<MEObservation> meObs = ctx.Observations.Collection.Find(mQuery).ToList();

                        if (meObs != null && meObs.Count > 0)
                        {
                            meObs.ForEach(o =>
                            {
                                odL.Add(new ObservationData
                                {
                                    CodingSystem = o.CodingSystemId.ToString(),
                                    CodingSystemCode = o.Code,
                                    DeleteFlag = o.DeleteFlag,
                                    Description = o.Description,
                                    ExtraElements = o.ExtraElements,
                                    GroupId = o.GroupId != null ? o.GroupId.ToString() : null,
                                    LowValue = o.LowValue,
                                    HighValue = o.HighValue,
                                    Id = o.Id.ToString(),
                                    LastUpdatedOn = o.LastUpdatedOn,
                                    ObservationType = o.ObservationTypeId.ToString(),
                                    Order = o.Order,
                                    Source = o.Source,
                                    Standard = o.Standard,
                                    Status = (int)o.Status,
                                    TTLDate = o.TTLDate,
                                    Units = o.Units,
                                    UpdatedBy = o.UpdatedBy.ToString(),
                                    Version = o.Version,
                                    CommonName = o.CommonName
                                });
                            });
                        }
                    }
                }
                return odL;
            }
            catch (Exception ex)
            {
                throw new Exception("PatientObservationDD:GetStandardObservationsByType()::" + ex.Message, ex.InnerException);
            }
        }

        public IEnumerable<object> FindObservationIdByPatientId(string Id)
        {
            throw new NotImplementedException();
        }
        
        public object FindRecentObservationValue(string observationTypeId, string patientId)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
    }
}

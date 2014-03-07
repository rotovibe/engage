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
            DeletePatientObservationRequest request = (DeletePatientObservationRequest)entity;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientObservation>.EQ(b => b.Id, ObjectId.Parse(request.PatientObservationId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientObservation.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientObservation.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientObservation.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientObservations.Collection.Update(q, update);
                }
            }
            catch (Exception ex) { throw; }
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
            bool result = false;
            PutUpdateObservationDataRequest odr = (PutUpdateObservationDataRequest)entity;
            PatientObservationRecordData pord = odr.PatientObservationData;

            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientObservation>.EQ(b => b.Id, ObjectId.Parse(pord.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientObservation.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, odr.UserId));
                    uv.Add(MB.Update.Set(MEPatientObservation.LastUpdatedOnProperty, System.DateTime.UtcNow));

                    if (pord.Source != null) uv.Add(MB.Update.Set(MEPatientObservation.SourceProperty, pord.Source));
                    if (pord.NonNumericValue != null) uv.Add(MB.Update.Set(MEPatientObservation.NonNumericValueProperty, pord.NonNumericValue));
                    if (pord.Value != null) uv.Add(MB.Update.Set(MEPatientObservation.NumericValueProperty, BsonDouble.Create(pord.Value)));
                    if (pord.Units != null) uv.Add(MB.Update.Set(MEPatientObservation.UnitsProperty, pord.Units));
                    if (pord.EndDate != null) uv.Add(MB.Update.Set(MEPatientObservation.EndDateProperty, pord.EndDate));
                    if (pord.StartDate != null) uv.Add(MB.Update.Set(MEPatientObservation.StartDateProperty, pord.StartDate));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientObservations.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("DD:MongoPatientBarrierRepository:Update()" + ex.Message, ex.InnerException); }
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
                    ObservationId = ObjectId.Parse(request.ObservationId),
                    DeleteFlag = true
                };

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientObservations.Collection.Insert(mePg);
                    if (wcr.Ok)
                    {
                        patientObservationData = new PatientObservationData
                        {
                            Id = mePg.Id.ToString(),
                            PatientId = mePg.PatientId.ToString()
                        };
                    }
                }
                return patientObservationData;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> FindObservationIdByPatientId(string Id)
        {
            try
            {
                List<PatientObservationData> observationsDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(Id)));
                IMongoQuery mQuery = Query.And(queries);

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<MEPatientObservation> meObservations = ctx.PatientObservations.Collection.Find(mQuery).ToList();
                    if (meObservations != null)
                    {
                        observationsDataList = new List<PatientObservationData>();
                        foreach (MEPatientObservation b in meObservations)
                        {
                            PatientObservationData observationData = new PatientObservationData
                            {
                                Id = b.Id.ToString(),
                                PatientId = b.PatientId.ToString()
                            };
                            observationsDataList.Add(observationData);
                        }
                    }
                }
                return observationsDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindRecentObservationValue(string observationTypeId, string patientId)
        {
            try
            {
                PatientObservationData observationData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(patientId)));
                queries.Add(Query.EQ(MEPatientObservation.ObservationIdProperty, ObjectId.Parse(observationTypeId)));
                queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);

                MEPatientObservation meObservation = null;

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    meObservation = ctx.PatientObservations.Collection.Find(mQuery).SetSortOrder(SortBy.Descending(MEPatientObservation.StartDateProperty).Descending(MEPatientObservation.LastUpdatedOnProperty)).FirstOrDefault();

                    if (meObservation != null)
                    {
                        observationData = new PatientObservationData
                        {
                            Id = meObservation.Id.ToString(),
                            PatientId = meObservation.PatientId.ToString(),
                            Values = GetValueList(meObservation.NumericValue, meObservation.NonNumericValue),
                            Source = meObservation.Source != null ? meObservation.Source : null,
                            StartDate = meObservation.StartDate,
                            EndDate = meObservation.EndDate,
                            Units = meObservation.Units != null ? meObservation.Units : null
                        };
                    }
                }
                return observationData as object;
            }
            catch (Exception ex) { throw ex; }
        }

        private List<ObservationValueData> GetValueList(double? numericVal, string nonNumericVal)
        {
            List<ObservationValueData> ovdl = new List<ObservationValueData>();
            try
            {
                if (numericVal != null)
                {
                    ovdl.Add(new ObservationValueData { Value = numericVal.ToString() });
                }
                else if (nonNumericVal != null)
                {
                    ovdl.Add(new ObservationValueData { Value = nonNumericVal });
                }
                return ovdl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetObservationsByType(object newEntity, bool standard)
        {
            throw new NotImplementedException();
        }
    }
}

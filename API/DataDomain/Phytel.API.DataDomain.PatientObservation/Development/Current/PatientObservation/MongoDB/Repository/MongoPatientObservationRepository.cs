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
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using MongoDB.Bson.Serialization;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class MongoPatientObservationRepository<T> : IPatientObservationRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientObservationRepository()
        {
            #region Register ClassMap
            try
            {
            if (BsonClassMap.IsClassMapRegistered(typeof(ObservationValue)) == false)
                BsonClassMap.RegisterClassMap<ObservationValue>();
            }
            catch { }

            try
            {
            if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientObservation)) == false)
                BsonClassMap.RegisterClassMap<MEPatientObservation>();
            }
            catch { }
            #endregion
        }

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
            try
            {
                throw new NotImplementedException();
                // code here //
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
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientObservations.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientObservation.ToString(), 
                                            request.PatientObservationId, 
                                            Common.DataAuditType.Delete, 
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
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
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientObservation.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientObservation.VersionProperty, odr.Version));

                    if (pord.Source != null) uv.Add(MB.Update.Set(MEPatientObservation.SourceProperty, pord.Source));
                    if (pord.NonNumericValue != null) uv.Add(MB.Update.Set(MEPatientObservation.NonNumericValueProperty, pord.NonNumericValue));
                    if (pord.Value != null) uv.Add(MB.Update.Set(MEPatientObservation.NumericValueProperty, BsonDouble.Create(pord.Value)));
                    if (pord.Units != null) uv.Add(MB.Update.Set(MEPatientObservation.UnitsProperty, pord.Units));
                    if (pord.EndDate != null) uv.Add(MB.Update.Set(MEPatientObservation.EndDateProperty, pord.EndDate));
                    if (pord.StartDate != null) uv.Add(MB.Update.Set(MEPatientObservation.StartDateProperty, pord.StartDate));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientObservations.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientObservation.ToString(), 
                                            pord.Id, 
                                            Common.DataAuditType.Update, 
                                            odr.ContractNumber);

                    result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("PatientObservationDD:MongoPatientBarrierRepository:Update()::" + ex.Message, ex.InnerException); }
        }

        void IRepository<T>.CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializeObservationDataRequest request = (PutInitializeObservationDataRequest)newEntity;
            PatientObservationData patientObservationData = null;
            MEPatientObservation mePg = null;

            try
            {
                mePg = new MEPatientObservation(this.UserId)
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    //LastUpdatedOn = DateTime.UtcNow,
                    ObservationId = ObjectId.Parse(request.ObservationId),
                    DeleteFlag = false,
                    Version = request.Version
                    //,UpdatedBy = ObjectId.Parse(this.UserId)
                };

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    ctx.PatientObservations.Collection.Insert(mePg);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientObservation.ToString(), 
                                            mePg.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            request.ContractNumber);

                    patientObservationData = new PatientObservationData
                    {
                        Id = mePg.Id.ToString(),
                        PatientId = mePg.PatientId.ToString()
                    };
                }
                return patientObservationData;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindObservationIdByPatientId(string Id)
        {
            try
            {
                List<PatientObservationData> observationsDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
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
            catch (Exception) { throw; }
        }

        public object GetAllPatientProblems(GetPatientProblemsSummaryRequest request, List<string> oidlist)
        {
            try
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                List<int> odl = new List<int>() { (int)ObservationDisplay.Primary, (int)ObservationDisplay.Secondary };
                List<ObjectId> oidls = oidlist.Select(r => ObjectId.Parse(r)).ToList<ObjectId>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(request.PatientId)));
                queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                queries.Add(Query.EQ(MEPatientObservation.ObservationStateProperty, 2));
                queries.Add(Query.In(MEPatientObservation.DisplayProperty, new BsonArray(odl)));
                queries.Add(Query.In(MEPatientObservation.ObservationIdProperty, new BsonArray(oidls)));
                IMongoQuery mQuery = Query.And(queries);

                List<MEPatientObservation> meObservation = null;
                List<PatientObservationData> observationDataL = new List<PatientObservationData>();

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    meObservation = ctx.PatientObservations.Collection.Find(mQuery).ToList();

                    if (meObservation != null && meObservation.Count > 0)
                    {
                        meObservation.ForEach(o =>
                        {
                            observationDataL.Add(
                               new PatientObservationData
                               {
                                   Id = o.Id.ToString(),
                                   ObservationId = o.ObservationId.ToString(),
                                   DisplayId = (int)o.Display,
                                   Name = GetObservationName(o.ObservationId),
                                   StateId = (int)o.State,
                                   PatientId = request.PatientId,
                                   StartDate = o.StartDate,
                                   EndDate = o.EndDate,
                                   Source = o.Source
                               });
                        });
                    }
                }
                return observationDataL as object;
            }
            catch (Exception ex) { throw new Exception("PatientObservationDD:MongoPatientBarrierRepository:GetAllPatientProblems()::" + ex.Message, ex.InnerException); }
        }

        private string GetObservationName(ObjectId objectId)
        {
            try
            {
                string val = string.Empty;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEObservation.IdProperty, objectId));
                IMongoQuery mQuery = Query.And(queries);

                MEObservation meObservation = null;

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    meObservation = ctx.Observations.Collection.Find(mQuery).FirstOrDefault();

                    if (meObservation != null)
                    {
                        val = meObservation.Description;
                    }
                }
                return val;
            }
            catch (Exception ex) { throw new Exception("PatientObservationDD:MongoPatientBarrierRepository:GetObservationName()::" + ex.Message, ex.InnerException); }
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
                queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
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
            catch (Exception) { throw; }
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

        public List<IdNamePair> GetAllowedObservationStates(object entity)
        {
            throw new NotImplementedException();
        }

        public object GetObservationsByType(object newEntity, bool? standard, bool? status)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
    }
}

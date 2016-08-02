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
    public class MongoPatientObservationRepository : IPatientObservationRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);
        private const string CareManager = "CM";

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
            return FindByID(entityID, false);
        }

        public object FindByID(string entityID, bool includeDeletedObservations)
        {
            try
            {
                PatientObservationData poData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientObservation.IdProperty, ObjectId.Parse(entityID)));
                if (!includeDeletedObservations)
                {
                    queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                }
                IMongoQuery mQuery = Query.And(queries);

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    MEPatientObservation mePO = ctx.PatientObservations.Collection.Find(mQuery).FirstOrDefault();
                    if (mePO != null)
                    {
                        poData = new PatientObservationData
                        {
                            Id = mePO.Id.ToString(),
                            PatientId = mePO.PatientId.ToString(),
                            EndDate = mePO.EndDate,
                            ObservationId = mePO.ObservationId.ToString(),
                            Source = mePO.Source,
                            StartDate = mePO.StartDate,
                            StateId = (int)mePO.State,
                            Units = mePO.Units,
                            Values = GetValueList(mePO.NumericValue, mePO.NonNumericValue),
                            LastUpdatedOn = mePO.LastUpdatedOn,
                            DisplayId = (int)mePO.Display,
                            DeleteFlag = mePO.DeleteFlag,
                            DataSource = mePO.DataSource,
                            ExternalRecordId = mePO.ExternalRecordId
                        };
                    }
                }
                return poData;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
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

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        object IRepository.Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        object IRepository.InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        void IRepository.Delete(object entity)
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

        void IRepository.DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByObservationID(string entityId, string patientId)
        {
            PatientObservationData odL = null;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(patientId)));
                    queries.Add(Query.EQ(MEPatientObservation.ObservationIdProperty, ObjectId.Parse(entityId)));
                    queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientObservation.ObservationStateProperty, 2));
                    queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);

                    MEPatientObservation o = ctx.PatientObservations.Collection.Find(mQuery).FirstOrDefault();

                    if (o != null)
                    {
                        odL = new PatientObservationData
                        {
                            Id = o.Id.ToString(),
                            StateId = (int)o.State
                        };
                    }
                }
                return odL;
            }
            catch (Exception) { throw; }
        }

        Tuple<string, IEnumerable<object>> IRepository.Select(APIExpression expression)
        {
            throw new NotImplementedException();
        }

        IEnumerable<object> IRepository.SelectAll()
        {
            throw new NotImplementedException();
        }

        object IRepository.Update(object entity)
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
                    uv.Add(MB.Update.Set(MEPatientObservation.DeleteFlagProperty, pord.DeleteFlag));
                    if (pord.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientObservation.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientObservation.VersionProperty, odr.Version));

                    if (pord.NonNumericValue != null) uv.Add(MB.Update.Set(MEPatientObservation.NonNumericValueProperty, pord.NonNumericValue));
                    if (pord.Value != null) uv.Add(MB.Update.Set(MEPatientObservation.NumericValueProperty, BsonDouble.Create(pord.Value)));
                    if (pord.Units != null) uv.Add(MB.Update.Set(MEPatientObservation.UnitsProperty, pord.Units));
                    if (pord.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.StartDateProperty, pord.StartDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.StartDateProperty, BsonNull.Value));
                    }
                    if (pord.EndDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.EndDateProperty, pord.EndDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.EndDateProperty, BsonNull.Value));
                    }
                    if (pord.DisplayId != 0) uv.Add(MB.Update.Set(MEPatientObservation.DisplayProperty, pord.DisplayId));
                    if (pord.StateId != 0) uv.Add(MB.Update.Set(MEPatientObservation.ObservationStateProperty, pord.StateId));
                    if (pord.Source != null) uv.Add(MB.Update.Set(MEPatientObservation.SourceProperty, pord.Source));

                    if (pord.DataSource != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.DataSourceProperty, pord.DataSource));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.DataSourceProperty, BsonNull.Value));
                    }

                    if (pord.ExternalRecordId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.ExternalRecordIdProperty, pord.ExternalRecordId));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientObservation.ExternalRecordIdProperty, BsonNull.Value));
                    }

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

        void IRepository.CacheByID(List<string> entityIDs)
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
                    ObservationId = ObjectId.Parse(request.ObservationId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    DeleteFlag = false,
                    Source = CareManager,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    State = ObservationState.Complete,
                    Version = request.Version
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

        public object InitializeProblem(object newEntity)
        {
            GetInitializeProblemDataRequest request = (GetInitializeProblemDataRequest)newEntity;
            PatientObservationData patientObservationData = null;
            MEPatientObservation mePg = null;

            try
            {
                mePg = new MEPatientObservation(this.UserId)
                {
                    PatientId = ObjectId.Parse(request.PatientId),
                    ObservationId = ObjectId.Parse(request.ObservationId),
                    DeleteFlag = false,
                    TTLDate = GetTTLDate(request.Initial),
                    Display = ObservationDisplay.Primary,
                    State = ObservationState.Active,
                    Source = CareManager,
                    Version = request.Version
                };

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    ctx.PatientObservations.Collection.Insert(mePg);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientObservation.ToString(),
                                            mePg.Id.ToString(),
                                            Common.DataAuditType.Insert,
                                            request.ContractNumber);

                    IdNamePair oDetails = GetObservationDetails(mePg.ObservationId);
                    patientObservationData = new PatientObservationData
                    {
                        Id = mePg.Id.ToString(),
                        PatientId = mePg.PatientId.ToString(),
                        ObservationId = mePg.ObservationId.ToString(),
                        DeleteFlag = mePg.DeleteFlag,
                        StartDate = mePg.StartDate,
                        EndDate = mePg.EndDate,
                        DisplayId = (int)mePg.Display,
                        StateId = (int)mePg.State,
                        Name = oDetails == null ? null : oDetails.Name,
                        TypeId = oDetails == null ? null : oDetails.Id,
                    };
                }
                return patientObservationData;
            }
            catch (Exception) { throw; }
        }

        private DateTime? GetTTLDate(string p)
        {
            DateTime? date = null;
            if (string.IsNullOrEmpty(p))
            {
                date = System.DateTime.UtcNow.AddDays(_initializeDays);
            }
            return date;
        }

        public IEnumerable<object> FindObservationIdByPatientId(string Id)
        {
            try
            {
                List<PatientObservationData> poDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    List<MEPatientObservation> mePO = ctx.PatientObservations.Collection.Find(mQuery).ToList();
                    if (mePO != null && mePO.Count > 0)
                    {
                        poDataList = new List<PatientObservationData>();
                        foreach (MEPatientObservation b in mePO)
                        {
                            PatientObservationData poData = new PatientObservationData
                            {
                                Id = b.Id.ToString(),
                                PatientId = b.PatientId.ToString(),
                                EndDate = b.EndDate,
                                ObservationId = b.ObservationId.ToString(),
                                Source = b.Source,
                                StartDate = b.StartDate,
                                StateId = (int)b.State,
                                Units = b.Units,
                                Values = GetValueList(b.NumericValue, b.NonNumericValue),
                                LastUpdatedOn = b.LastUpdatedOn,
                                DisplayId = (int)b.Display,
                                DataSource = b.DataSource,
                                ExternalRecordId = b.ExternalRecordId
                            };
                            poDataList.Add(poData);
                        }
                    }
                }
                return poDataList;
            }
            catch (Exception) { throw; }
        }

        public object GetAllPatientProblems(GetPatientProblemsSummaryRequest request, List<string> oidlist)
        {
            try
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                List<ObjectId> oidls = oidlist.Select(r => ObjectId.Parse(r)).ToList<ObjectId>();
                queries.Add(Query.EQ(MEPatientObservation.PatientIdProperty, ObjectId.Parse(request.PatientId)));
                queries.Add(Query.EQ(MEPatientObservation.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                queries.Add(Query.EQ(MEPatientObservation.ObservationStateProperty, (int)ObservationState.Active));
                queries.Add(Query.In(MEPatientObservation.ObservationIdProperty, new BsonArray(oidls)));
                IMongoQuery mQuery = Query.And(queries);

                List<MEPatientObservation> meObservations = null;
                List<PatientObservationData> observationDataL = new List<PatientObservationData>();

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    meObservations = ctx.PatientObservations.Collection.Find(mQuery).ToList();

                    if (meObservations != null && meObservations.Count > 0)
                    {
                        foreach (MEPatientObservation mePO in meObservations)
                        {
                            IdNamePair oDetails = GetObservationDetails(mePO.ObservationId);
                            PatientObservationData data = new PatientObservationData
                            {
                                Id = mePO.Id.ToString(),
                                ObservationId = mePO.ObservationId.ToString(),
                                DisplayId = (int)mePO.Display,
                                Name = oDetails == null ? null : oDetails.Name,
                                TypeId = oDetails == null ? null : oDetails.Id,
                                StateId = (int)mePO.State,
                                PatientId = request.PatientId,
                                StartDate = mePO.StartDate,
                                EndDate = mePO.EndDate,
                                Source = mePO.Source,
                                DataSource = mePO.DataSource,
                                ExternalRecordId = mePO.ExternalRecordId
                            };
                            observationDataL.Add(data);
                        }
                        observationDataL = observationDataL.OrderBy(o => o.Name).ToList();

                    }
                }
                return observationDataL as object;
            }
            catch (Exception ex) { throw new Exception("PatientObservationDD:MongoPatientBarrierRepository:GetAllPatientProblems()::" + ex.Message, ex.InnerException); }
        }

        private IdNamePair GetObservationDetails(ObjectId objectId)
        {
            try
            {
                IdNamePair result = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEObservation.IdProperty, objectId));
                queries.Add(Query.EQ(MEObservation.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);

                MEObservation meO = null;

                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    meO = ctx.Observations.Collection.Find(mQuery).FirstOrDefault();

                    if (meO != null)
                    {
                        result = new IdNamePair { Id = meO.ObservationTypeId.ToString(), Name = meO.CommonName != null ? meO.CommonName : meO.Description };
                    }
                }
                return result;
            }
            catch (Exception ex) { throw new Exception("PatientObservationDD:MongoPatientBarrierRepository:GetObservationDetails()::" + ex.Message, ex.InnerException); }
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
                            Units = meObservation.Units != null ? meObservation.Units : null,
                            DataSource = meObservation.DataSource,
                            ExternalRecordId = meObservation.ExternalRecordId
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

        public List<ObservationStateData> GetAllowedObservationStates()
        {
            throw new NotImplementedException();
        }

        public object GetObservationsByType(object newEntity, bool? standard, bool? status)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetActiveObservations()
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientObservationsDataRequest request = (UndoDeletePatientObservationsDataRequest)entity;
            try
            {
                using (PatientObservationMongoContext ctx = new PatientObservationMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientObservation>.EQ(b => b.Id, ObjectId.Parse(request.PatientObservationId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientObservation.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientObservation.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientObservation.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientObservation.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientObservations.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientObservation.ToString(),
                                            request.PatientObservationId,
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; };
        }
    }
}

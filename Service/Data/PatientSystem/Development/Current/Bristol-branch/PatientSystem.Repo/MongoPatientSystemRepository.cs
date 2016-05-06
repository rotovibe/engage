using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.PatientSystem.Repo;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.PatientSystem
{
    
    public class MongoPatientSystemRepository<TContext> : IMongoPatientSystemRepository where TContext : PatientSystemMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }
        
        public MongoPatientSystemRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientSystemRepository(TContext context)
        {
            Context = context;
        }

        public MongoPatientSystemRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            InsertPatientSystemDataRequest request = newEntity as InsertPatientSystemDataRequest;
            PatientSystemData data = request.PatientSystemsData;
            string Id = null;
            try
            {
                if(data != null)
                {
                    if (string.IsNullOrEmpty(data.Value) || string.IsNullOrWhiteSpace(data.Value))
                        throw new ArgumentException("Patient System value is missing");
                    if (!Enum.IsDefined(typeof(Status), data.StatusId))
                        throw new ArgumentException("Patient System status is invalid");

                    using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                    {
                        MEPatientSystem mePS = new MEPatientSystem(this.UserId, data.CreatedOn)
                        {
                            PatientId = ObjectId.Parse(data.PatientId),
                            Value = Helper.TrimAndLimit(data.Value, 100),
                            Status = (Status)data.StatusId,
                            Primary = data.Primary,
                            SystemId = ObjectId.Parse(data.SystemId),
                            SystemName = data.SystemName,
                            DataSource = Helper.TrimAndLimit(data.DataSource, 50),
                            DeleteFlag = false,
                            TTLDate = null,
                            ExternalRecordId = data.ExternalRecordId,
                            LastUpdatedOn = data.UpdatedOn,
                            Version = request.Version
                            };
                        ctx.PatientSystems.Collection.Insert(mePS);
                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientSystem.ToString(), 
                                                mePS.Id.ToString(), 
                                                DataAuditType.Insert, 
                                                request.ContractNumber);
                        Id = mePS.Id.ToString();
                    }
                }
                return Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object InsertAll(List<object> entities)
        {
            BulkInsertResult result = new BulkInsertResult();
            List<string> insertedIds = new List<string>();
            List<string> errorMessages = new List<string>();
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    var bulk = ctx.PatientSystems.Collection.InitializeUnorderedBulkOperation();
                    foreach (PatientSystemData data in entities)
                    {
                        MEPatientSystem mePS = new MEPatientSystem(this.UserId, data.CreatedOn)
                        {
                            PatientId = ObjectId.Parse(data.PatientId),
                            Value = Helper.TrimAndLimit(data.Value, 100),
                            Status = (Status)data.StatusId,
                            Primary = data.Primary,
                            SystemId = ObjectId.Parse(data.SystemId),
                            SystemName = data.SystemName,
                            DataSource = Helper.TrimAndLimit(data.DataSource, 50),
                            DeleteFlag = false,
                            TTLDate = null,
                            ExternalRecordId = data.ExternalRecordId,
                            LastUpdatedOn = data.UpdatedOn,
                            UpdatedBy = ParseObjectId(data.UpdatedById)
                        };
                        bulk.Insert(mePS.ToBsonDocument());
                        insertedIds.Add(mePS.Id.ToString());
                    }
                    BulkWriteResult bwr = bulk.Execute();
                }
                // TODO: Auditing.
            }
            catch (BulkWriteException bwEx)
            {
                // Get the error messages for the ones that failed.
                foreach (BulkWriteError er in bwEx.WriteErrors)
                {
                    errorMessages.Add(er.Message);
                }
            }
            catch (Exception ex)
            {
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            result.ProcessedIds = insertedIds;
            result.ErrorMessages = errorMessages;
            return result;
        }

        private ObjectId? ParseObjectId(string p)
        {
            try
            {
                if (string.IsNullOrEmpty(p)) return null;
                var val = ObjectId.Parse(p);
                return val;
            }
            catch (Exception ex)
            {
                var aseProcessId = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessId), ex);
                throw ex;
            }
        }

        public void Delete(object entity)
        {
            DeletePatientSystemByPatientIdDataRequest request = (DeletePatientSystemByPatientIdDataRequest)entity;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    var query = MB.Query<MEPatientSystem>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientSystem.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MEPatientSystem.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MEPatientSystem.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientSystem.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientSystems.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientSystem.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            PatientSystemData data = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientSystem.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MEPatientSystem.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientSystem mePS = ctx.PatientSystems.Collection.Find(mQuery).FirstOrDefault();
                    if (mePS != null)
                    {
                        data = new PatientSystemData 
                        {
                            Id = mePS.Id.ToString(),
                            PatientId = mePS.PatientId.ToString(),
                            Value = mePS.Value,
                            StatusId = (int)mePS.Status,
                            SystemName = mePS.SystemName,
                            Primary = mePS.Primary,
                            ExternalRecordId = mePS.ExternalRecordId,
                            SystemId = mePS.SystemId.ToString(),
                            DataSource = mePS.DataSource,
                            CreatedById = mePS.RecordCreatedBy.ToString(),
                            CreatedOn = mePS.RecordCreatedOn,
                            UpdatedById = mePS.UpdatedBy == null ? null : mePS.UpdatedBy.ToString(),
                            UpdatedOn = mePS.LastUpdatedOn
                        };
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientSystem.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);

                List<MEPatientSystem> mePatSys = ctx.PatientSystems.Collection.Find(mQuery).ToList();

                List<PatientSystemOldData> patSys = null;
                if (mePatSys != null && mePatSys.Count > 0)
                {
                    patSys = mePatSys.Select(a => Mapper.Map<PatientSystemOldData>(a)).ToList();
                }

                return patSys;
            }            
        }

        public object Update(object entity)
        {
            UpdatePatientSystemDataRequest request = (UpdatePatientSystemDataRequest)entity;
            PatientSystemData data = request.PatientSystemsData;
            bool result = false;
            try
            {
                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.Value) || string.IsNullOrWhiteSpace(data.Value))
                        throw new ArgumentException("Patient System value is missing");
                    if (data.StatusId == 0)
                        throw new ArgumentException("Patient System status is missing");
                    using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                    {
                        var q = MB.Query<MEPatientSystem>.EQ(b => b.Id, ObjectId.Parse(data.Id));
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(MEPatientSystem.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        uv.Add(MB.Update.Set(MEPatientSystem.VersionProperty, request.Version));
                        uv.Add(MB.Update.Set(MEPatientSystem.LastUpdatedOnProperty, System.DateTime.UtcNow));
                        if (!string.IsNullOrEmpty(data.PatientId)) uv.Add(MB.Update.Set(MEPatientSystem.PatientIdProperty, ObjectId.Parse(data.PatientId)));
                        uv.Add(MB.Update.Set(MEPatientSystem.ValueProperty, Helper.TrimAndLimit(data.Value, 100)));

                        if (!string.IsNullOrEmpty(data.DataSource))
                            uv.Add(MB.Update.Set(MEPatientSystem.DataSourceProperty, Helper.TrimAndLimit(data.DataSource, 50)));
                        else
                            uv.Add(MB.Update.Set(MEPatientSystem.DataSourceProperty, BsonNull.Value));

                        if (data.StatusId != 0) uv.Add(MB.Update.Set(MEPatientSystem.StatusProperty, data.StatusId));
                        uv.Add(MB.Update.Set(MEPatientSystem.PrimaryProperty, data.Primary));
                        if (!string.IsNullOrEmpty(data.SystemId)) uv.Add(MB.Update.Set(MEPatientSystem.SystemIdProperty, ObjectId.Parse(data.SystemId)));
                        if (!string.IsNullOrEmpty(data.SystemName)) uv.Add(MB.Update.Set(MEPatientSystem.SystemNameProperty, ObjectId.Parse(data.SystemName)));
                        
                        IMongoUpdate update = MB.Update.Combine(uv);
                        WriteConcernResult res = ctx.PatientSystems.Collection.Update(q, update);
                        if (res.Ok == false)
                            throw new Exception("Failed to update a patient system: " + res.ErrorMessage);
                        else
                            AuditHelper.LogDataAudit(this.UserId,
                                                    MongoCollectionName.PatientSystem.ToString(),
                                                    data.Id,
                                                    DataAuditType.Update,
                                                    request.ContractNumber);

                        result = true;
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            List<PatientSystemData> dataList = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientSystem.PatientIdProperty, ObjectId.Parse(patientId)));
                    queries.Add(Query.EQ(MEPatientSystem.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientSystem> mePatientSystems = ctx.PatientSystems.Collection.Find(mQuery).ToList();
                    if (mePatientSystems != null && mePatientSystems.Count > 0)
                    {
                        dataList = new List<PatientSystemData>();
                        foreach (MEPatientSystem mePS in mePatientSystems)
                        {
                            dataList.Add(new PatientSystemData
                            {
                                Id = mePS.Id.ToString(),
                                PatientId = mePS.PatientId.ToString(),
                                Value = mePS.Value,
                                StatusId = (int)mePS.Status,
                                Primary = mePS.Primary,
                                ExternalRecordId = mePS.ExternalRecordId,
                                SystemId = mePS.SystemId.ToString(),
                                SystemName = mePS.SystemName,
                                DataSource = mePS.DataSource,
                                CreatedById = mePS.RecordCreatedBy.ToString(),
                                CreatedOn = mePS.RecordCreatedOn,
                                UpdatedById = mePS.UpdatedBy == null ? null : mePS.UpdatedBy.ToString(),
                                UpdatedOn = mePS.LastUpdatedOn
                            });
                        }
                    }
                }
                return dataList;
            }
            catch (Exception) { throw; }
        }

        public void UndoDelete(object entity)
        {
            UndoDeletePatientSystemsDataRequest request = (UndoDeletePatientSystemsDataRequest)entity;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    var query = MB.Query<MEPatientSystem>.EQ(b => b.Id, ObjectId.Parse(request.PatientSystemId));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEPatientSystem.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEPatientSystem.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientSystem.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientSystems.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientSystem.ToString(),
                                            request.PatientSystemId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }


        public IEnumerable<object> Find(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            PatientSystemData data = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientSystem.ExternalRecordIdProperty, externalRecordId));
                    queries.Add(Query.EQ(MEPatientSystem.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientSystem mePS = ctx.PatientSystems.Collection.Find(mQuery).FirstOrDefault();
                    if (mePS != null)
                    {
                        data = new PatientSystemData
                        {
                            Id = mePS.Id.ToString(),
                        };
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public List<PatientSystemData> Select(List<string> Ids)
        {
            List<PatientSystemData> psList = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(MEPatientSystem.IdProperty, new BsonArray(Helper.ConvertToObjectIdList(Ids))));
                    queries.Add(Query.EQ(MEPatientSystem.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientSystem> mePatients = ctx.PatientSystems.Collection.Find(mQuery).ToList();
                    if (mePatients != null && mePatients.Count > 0)
                    {
                        psList = new List<PatientSystemData>();
                        foreach (MEPatientSystem mePS in mePatients)
                        {
                            PatientSystemData data = new PatientSystemData
                            {
                                Id = mePS.Id.ToString(),
                                PatientId = mePS.PatientId.ToString(),
                                Value = mePS.Value,
                                StatusId = (int)mePS.Status,
                                Primary = mePS.Primary,
                                ExternalRecordId = mePS.ExternalRecordId,
                                SystemId = mePS.SystemId.ToString(),
                                SystemName = mePS.SystemName,
                                DataSource = mePS.DataSource,
                                CreatedById = mePS.RecordCreatedBy.ToString(),
                                CreatedOn = mePS.RecordCreatedOn,
                                UpdatedById = mePS.UpdatedBy == null ? null : mePS.UpdatedBy.ToString(),
                                UpdatedOn = mePS.LastUpdatedOn
                            };
                            psList.Add(data);
                        }
                    }
                }
                return psList;
            }
            catch (Exception) { throw; }
        }
    }
}

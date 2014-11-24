using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataAudit;
using Phytel.API.Common;
using MongoDB.Bson.Serialization;
using MB = MongoDB.Driver.Builders;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class MongoPatientSystemRepository : IPatientSystemRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoPatientSystemRepository()
        {
            try 
            {
                #region Register ClassMap
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientSystem)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientSystem>();
                #endregion
            }
            catch { }
        }

        public MongoPatientSystemRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutPatientSystemDataRequest request = newEntity as PutPatientSystemDataRequest;
            string patientSystemId = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
                {
                    IMongoQuery query = Query.And(
                                    Query.EQ(MEPatientSystem.PatientIDProperty, ObjectId.Parse(request.PatientID)));

                    MEPatientSystem patientSystem = ctx.PatientSystems.Collection.FindOneAs<MEPatientSystem>(query);
                    if (patientSystem == null)
                    {
                        patientSystem = new MEPatientSystem(this.UserId)
                        {
                            PatientID = ObjectId.Parse(request.PatientID),
                            SystemID = request.SystemID,
                            DisplayLabel = request.DisplayLabel,
                            SystemName = request.SystemName,
                            TTLDate = null,
                            DeleteFlag = false
                        };
                        ctx.PatientSystems.Collection.Insert(patientSystem);
                        patientSystemId = patientSystem.Id.ToString();
                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientSystem.ToString(), 
                                                patientSystem.Id.ToString(), 
                                                Common.DataAuditType.Insert, 
                                                request.ContractNumber);

                    }
                    return patientSystemId;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeletePatientSystemByPatientIdDataRequest request = (DeletePatientSystemByPatientIdDataRequest)entity;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
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
            DTO.PatientSystemData patientSystemData = null;
            using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
            {
                patientSystemData = (from p in ctx.PatientSystems
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.PatientSystemData
                           {
                            PatientId = p.PatientID.ToString(),
                            SystemId = p.SystemID,
                            SystemName = p.SystemName,
                            DisplayLabel = p.DisplayLabel
                           }).FirstOrDefault();
            }
            return patientSystemData;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            PutUpdatePatientSystemDataRequest request = (PutUpdatePatientSystemDataRequest)entity;
            bool result = false;
            try
            {
                if (request.Id != null)
                {
                    using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
                    {
                        var q = MB.Query<MEPatientSystem>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(MEPatientSystem.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        uv.Add(MB.Update.Set(MEPatientSystem.VersionProperty, request.Version));
                        uv.Add(MB.Update.Set(MEPatientSystem.LastUpdatedOnProperty, System.DateTime.UtcNow));
                        if (!string.IsNullOrEmpty(request.SystemID))
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.SystemIDProperty, request.SystemID));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.SystemIDProperty, BsonNull.Value));
                        }
                        if (!string.IsNullOrEmpty(request.SystemName))
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.SystemNameProperty, request.SystemName));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.SystemNameProperty, BsonNull.Value));
                        }
                        if (!string.IsNullOrEmpty(request.DisplayLabel))
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.DisplayLabelProperty, request.DisplayLabel));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.DisplayLabelProperty, BsonNull.Value));
                        }
                        uv.Add(MB.Update.Set(MEPatientSystem.DeleteFlagProperty, request.DeleteFlag));
                        DataAuditType type;
                        if (request.DeleteFlag)
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                            type = Common.DataAuditType.Delete;
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEPatientSystem.TTLDateProperty, BsonNull.Value));
                            type = Common.DataAuditType.Update;
                        }
                        IMongoUpdate update = MB.Update.Combine(uv);
                        WriteConcernResult res = ctx.PatientSystems.Collection.Update(q, update);
                        if (res.Ok == false)
                            throw new Exception("Failed to update a patient system: " + res.ErrorMessage);
                        else
                            AuditHelper.LogDataAudit(this.UserId,
                                                    MongoCollectionName.PatientSystem.ToString(),
                                                    request.Id,
                                                    type,
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
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientSystem.PatientIDProperty, ObjectId.Parse(patientId)));
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
                                PatientId = mePS.PatientID.ToString(),
                                SystemId = mePS.SystemID,
                                DisplayLabel = mePS.DisplayLabel,
                                SystemName =  mePS.SystemName,
                                DeleteFlag = mePS.DeleteFlag
                            });
                        }
                    }
                }
                return dataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientSystemsDataRequest request = (UndoDeletePatientSystemsDataRequest)entity;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
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
    }
}

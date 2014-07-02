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
    public class MongoPatientSystemRepository<T> : IPatientSystemRepository<T>
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
            PutPatientSystemDataRequest systemRequest = newEntity as PutPatientSystemDataRequest;
            MEPatientSystem patientSystem = null;
            try
            {
                using (PatientSystemMongoContext ctx = new PatientSystemMongoContext(_dbName))
                {
                    IMongoQuery query = Query.And(
                                    Query.EQ(MEPatientSystem.PatientIDProperty, ObjectId.Parse(systemRequest.PatientID)));

                    patientSystem = ctx.PatientSystems.Collection.FindOneAs<MEPatientSystem>(query);
                    if (patientSystem == null)
                    {
                        patientSystem = new MEPatientSystem(this.UserId)
                        {
                            Id = ObjectId.GenerateNewId(),
                            PatientID = ObjectId.Parse(systemRequest.PatientID),
                            SystemID = systemRequest.SystemID,
                            DisplayLabel = systemRequest.DisplayLabel,
                            SystemName = systemRequest.SystemName,
                            Version = systemRequest.Version,
                            TTLDate = null,
                            DeleteFlag = false,
                            LastUpdatedOn = System.DateTime.UtcNow,
                            UpdatedBy = ObjectId.Parse(this.UserId)
                        };
                        ctx.PatientSystems.Collection.Insert(patientSystem);

                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientSystem.ToString(), 
                                                patientSystem.Id.ToString(), 
                                                Common.DataAuditType.Insert, 
                                                systemRequest.ContractNumber);

                    }
                }

                return new PutPatientSystemDataResponse
                {
                    PatientSystemId = patientSystem.Id.ToString()
                };
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

                    IMongoUpdate patientUserUpdate = MB.Update.Combine(builder);
                    ctx.PatientSystems.Collection.Update(query, patientUserUpdate);

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
                            PatientID = p.PatientID.ToString(),
                            SystemID = p.SystemID,
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
            throw new NotImplementedException();
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
                                PatientID = mePS.PatientID.ToString(),
                                SystemID = mePS.SystemID,
                                DisplayLabel = mePS.DisplayLabel,
                                SystemName =  mePS.SystemName
                            });
                        }
                    }
                }
                return dataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }
    }
}

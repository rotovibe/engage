
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
using Phytel.API.DataDomain.Allergy.DTO;
using DTO = Phytel.API.DataDomain.Allergy.DTO;
using MB = MongoDB.Driver.Builders;

namespace DataDomain.Allergy.Repo
{
    public class MongoPatientAllergyRepository<TContext> : IMongoPatientAllergyRepository where TContext : AllergyMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoPatientAllergyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientAllergyRepository(TContext context)
        {
            Context = context;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            PatientAllergyData data = null;
            try
            {
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientAllergy.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientAllergy mePA = ctx.PatientAllergies.Collection.Find(mQuery).FirstOrDefault();
                    if (mePA != null)
                    {
                        data = AutoMapper.Mapper.Map<PatientAllergyData>(mePA);
                        // get corresponding allergy name and type.
                        if(data != null)
                        {
                            getAllergyDetails(data, ctx, mePA.AllergyId);
                        }
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        private static void getAllergyDetails(PatientAllergyData data, AllergyMongoContext ctx, ObjectId aid)
        {
            MEAllergy meA = ctx.Allergies.Collection.Find(Query.EQ(MEAllergy.IdProperty, aid)).FirstOrDefault();
            if (meA != null)
            {
                data.AllergyName = meA.Description;
                data.AllergyTypeId = Helper.ConvertToStringList(meA.SubType);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            bool result = false;
            PutPatientAllergyDataRequest pa = (PutPatientAllergyDataRequest)entity;
            PatientAllergyData pt = pa.PatientAllergyData;
            try
            {
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientAllergy>.EQ(b => b.Id, ObjectId.Parse(pt.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientAllergy.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientAllergy.VersionProperty, pa.Version));
                    uv.Add(MB.Update.Set(MEPatientAllergy.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pt.PatientId != null) uv.Add(MB.Update.Set(MEPatientAllergy.PatientIdProperty, ObjectId.Parse(pt.PatientId)));
                    if (pt.AllergyId != null) uv.Add(MB.Update.Set(MEPatientAllergy.AllergyIdProperty, ObjectId.Parse(pt.AllergyId)));
                    if (pt.SeverityId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.SeverityIdProperty, ObjectId.Parse(pt.SeverityId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.SeverityIdProperty, BsonNull.Value));
                    }
                    if (pt.ReactionIds != null)
                    {
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientAllergy.ReactionIdsProperty, Helper.ConvertToObjectIdList(pt.ReactionIds)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.ReactionIdsProperty, BsonNull.Value));
                    }
                    if (pt.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.StartDateProperty, pt.StartDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.StartDateProperty, BsonNull.Value));
                    }
                    if (pt.EndDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.EndDateProperty, pt.EndDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.EndDateProperty, BsonNull.Value));
                    }
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientAllergy.StatusProperty, pt.StatusId));
                    if (pt.SourceId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.SourceIdProperty, ObjectId.Parse(pt.SourceId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.SourceIdProperty, BsonNull.Value));
                    }
                    if (pt.Notes != null) uv.Add(MB.Update.Set(MEPatientAllergy.NotesProperty, pt.Notes));
                    if (pt.SystemName != null) uv.Add(MB.Update.Set(MEPatientAllergy.SystemProperty, pt.SystemName));
                    uv.Add(MB.Update.Set(MEPatientAllergy.DeleteFlagProperty, pt.DeleteFlag));
                    DataAuditType type;
                    if (pt.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientAllergy.TTLDateProperty, BsonNull.Value));
                        type = DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientAllergies.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientAllergy.ToString(),
                                            pt.Id,
                                            type,
                                            pa.ContractNumber);

                    result = true;
                }
                return result as object;
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializePatientAllergyDataRequest request = (PutInitializePatientAllergyDataRequest)newEntity;
            PatientAllergyData data = null;
            try
            {
                MEPatientAllergy mePA = new MEPatientAllergy(this.UserId)
                {
                    PatientId = ObjectId.Parse(request.PatientId),
                    AllergyId = ObjectId.Parse(request.AllergyId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    DeleteFlag = false
                };

                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    ctx.PatientAllergies.Collection.Insert(mePA);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientAllergy.ToString(),
                                            mePA.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);

                    data = new PatientAllergyData
                    {
                        Id = mePA.Id.ToString(),
                        PatientId = mePA.PatientId.ToString(),
                        AllergyId = mePA.AllergyId.ToString()
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public object FindByPatientId(string entityID)
        {
            List<PatientAllergyData> list = null;
            try
            {
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientAllergy.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientAllergy.TTLDateProperty, BsonNull.Value));
                    queries.Add(Query.EQ(MEPatientAllergy.PatientIdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientAllergy> mePAs = ctx.PatientAllergies.Collection.Find(mQuery).ToList();
                    if (mePAs != null && mePAs.Count > 0)
                    {
                        list = new List<PatientAllergyData>();
                        mePAs.ForEach(p =>
                            { 
                                PatientAllergyData data = AutoMapper.Mapper.Map<PatientAllergyData>(p);
                                // get corresponding allergy name and type.
                                if (data != null)
                                {
                                    getAllergyDetails(data, ctx, p.AllergyId);
                                }
                                list.Add(data);
                            });
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }
    }
}

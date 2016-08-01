using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.CareMember.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.CareMember
{
    public class MongoCareMemberRepository : ICareMemberRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoCareMemberRepository()
        {
            try 
            {
                #region Register ClassMap
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MECareMember)) == false)
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MECareMember>();
                #endregion
            }
            catch { }
        }
        public MongoCareMemberRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutCareMemberDataRequest request = (PutCareMemberDataRequest)newEntity;
            CareMemberData careMemberData = request.CareMember;
            MECareMember meCM = null;

            try
            {
                if (careMemberData != null)
                {
                    meCM = new MECareMember(this.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        PatientId = ObjectId.Parse(careMemberData.PatientId),
                        ContactId = ObjectId.Parse(careMemberData.ContactId),
                        Primary = careMemberData.Primary,
                        TypeId = ObjectId.Parse(careMemberData.TypeId),
                        Version = request.Version
                    };

                    using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                    {
                        WriteConcernResult wcr = ctx.CareMembers.Collection.Insert(meCM);
                        if (wcr.Ok == false)
                            throw new Exception("Care Member failed to insert: " + wcr.ErrorMessage);
                        else
                            AuditHelper.LogDataAudit(this.UserId, 
                                                        MongoCollectionName.CareMember.ToString(), 
                                                        meCM.Id.ToString(), 
                                                        Common.DataAuditType.Insert, 
                                                        request.ContractNumber);
                    }
                }
                return meCM.Id.ToString();
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
            DeleteCareMemberByPatientIdDataRequest request = (DeleteCareMemberByPatientIdDataRequest)entity;
            try
            {
                using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                {
                    var query = MB.Query<MECareMember>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MECareMember.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MECareMember.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MECareMember.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MECareMember.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.CareMembers.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CareMember.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }
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
            CareMemberData careMemberData = null;
            try
            {
                using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MECareMember.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MECareMember.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    MECareMember meCM = ctx.CareMembers.Collection.Find(mQuery).FirstOrDefault();
                    if (meCM != null)
                    {
                        careMemberData = new CareMemberData
                        {
                            Id = meCM.Id.ToString(),
                            PatientId = meCM.PatientId.ToString(),
                            ContactId = meCM.ContactId.ToString(),
                            Primary = meCM.Primary,
                            TypeId = meCM.TypeId.ToString()
                        };
                    }
                }
                return careMemberData;
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

        public object Update(object entity)
        {
            PutUpdateCareMemberDataRequest request = (PutUpdateCareMemberDataRequest)entity;
            CareMemberData careMemberData = request.CareMember;
            bool result = false;
            try
            {
                if(careMemberData != null)
                {
                    using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                    {
                        var q = MB.Query<MECareMember>.EQ(b => b.Id, ObjectId.Parse(careMemberData.Id));
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(MECareMember.TTLDateProperty, BsonNull.Value));
                        uv.Add(MB.Update.Set(MECareMember.DeleteFlagProperty, false));
                        uv.Add(MB.Update.Set(MECareMember.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        uv.Add(MB.Update.Set(MECareMember.VersionProperty, request.Version));
                        uv.Add(MB.Update.Set(MECareMember.LastUpdatedOnProperty, System.DateTime.UtcNow));
                        uv.Add(MB.Update.Set(MECareMember.PrimaryProperty, careMemberData.Primary));
                        if (careMemberData.PatientId != null) uv.Add(MB.Update.Set(MECareMember.PatientIdProperty, ObjectId.Parse(careMemberData.PatientId)));
                        if (careMemberData.ContactId != null) uv.Add(MB.Update.Set(MECareMember.ContactIdProperty, ObjectId.Parse(careMemberData.ContactId)));
                        if (careMemberData.TypeId != null) uv.Add(MB.Update.Set(MECareMember.TypeProperty, ObjectId.Parse(careMemberData.TypeId)));

                        IMongoUpdate update = MB.Update.Combine(uv);
                        WriteConcernResult res = ctx.CareMembers.Collection.Update(q, update);
                        if (res.Ok == false)
                            throw new Exception("Failed to update Care Member: " + res.ErrorMessage);
                        else
                            AuditHelper.LogDataAudit(this.UserId, 
                                                    MongoCollectionName.CareMember.ToString(), 
                                                    careMemberData.Id, 
                                                    Common.DataAuditType.Update, 
                                                    request.ContractNumber);

                        result = true;        
                    }
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("CareMemberDD:MongoCareMemberRepository:Update()" + ex.Message, ex.InnerException); }
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

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            List<CareMemberData> careMembersDataList = null;
            try
            {
                using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MECareMember.PatientIdProperty, ObjectId.Parse(patientId)));
                    queries.Add(Query.EQ(MECareMember.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MECareMember> meCareMembers = ctx.CareMembers.Collection.Find(mQuery).ToList();
                    if (meCareMembers != null && meCareMembers.Count > 0)
                    {
                        careMembersDataList = new List<CareMemberData>();
                        foreach (MECareMember meCM in meCareMembers)
                        {
                            careMembersDataList.Add(new CareMemberData
                            {
                                Id = meCM.Id.ToString(),
                                PatientId = meCM.PatientId.ToString(),
                                ContactId = meCM.ContactId.ToString(),
                                Primary = meCM.Primary,
                                TypeId = meCM.TypeId.ToString()
                            });
                        }
                    }
                }
                return careMembersDataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeleteCareMembersDataRequest request = (UndoDeleteCareMembersDataRequest)entity;
            try
            {
                using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                {
                    var query = MB.Query<MECareMember>.EQ(b => b.Id, ObjectId.Parse(request.CareMemberId));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MECareMember.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MECareMember.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MECareMember.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MECareMember.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.CareMembers.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CareMember.ToString(),
                                            request.CareMemberId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }
    }
}

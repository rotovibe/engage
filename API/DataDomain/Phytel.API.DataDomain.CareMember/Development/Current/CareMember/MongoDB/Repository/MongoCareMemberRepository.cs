using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using System.Configuration;

namespace Phytel.API.DataDomain.CareMember
{
    public class MongoCareMemberRepository<T> : ICareMemberRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        public MongoCareMemberRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutCareMemberDataRequest request = (PutCareMemberDataRequest)newEntity;
            CareMemberData careMemberData = request.CareMember;
            string careMemberId = string.Empty;
            try
            {
                if (careMemberData != null)
                {
                    MECareMember meCM = new MECareMember
                    {
                        Id = ObjectId.GenerateNewId(),
                        PatientId = ObjectId.Parse(careMemberData.PatientId),
                        ContactId = ObjectId.Parse(careMemberData.ContactId),
                        Primary = careMemberData.Primary,
                        Type = ObjectId.Parse(careMemberData.TypeId),
                        Version = request.Version,
                        UpdatedBy = request.UserId,
                        LastUpdatedOn = DateTime.UtcNow
                    };

                    using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                    {
                        WriteConcernResult wcr = ctx.CareMembers.Collection.Insert(meCM);
                        if (wcr.Ok)
                        {
                            careMemberId = meCM.Id.ToString();
                        }
                    }
                }
                return careMemberId;
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
                            TypeId = meCM.Type.ToString()
                        };
                    }
                }
                return careMemberData;
            }
            catch (Exception ex) { throw ex; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                IMongoQuery mQuery = null;
                List<object> CareMemberItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, CareMemberItems);
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
                        uv.Add(MB.Update.Set(MECareMember.UpdatedByProperty, request.UserId));
                        uv.Add(MB.Update.Set(MECareMember.VersionProperty, request.Version));
                        uv.Add(MB.Update.Set(MECareMember.LastUpdatedOnProperty, System.DateTime.UtcNow));
                        if (careMemberData.PatientId != null) uv.Add(MB.Update.Set(MECareMember.PatientIdProperty, careMemberData.PatientId));
                        if (careMemberData.ContactId != null) uv.Add(MB.Update.Set(MECareMember.ContactIdProperty, ObjectId.Parse(careMemberData.ContactId)));
                        if (careMemberData.TypeId != null) uv.Add(MB.Update.Set(MECareMember.TypeProperty, careMemberData.TypeId));
                        if (careMemberData.Primary != null) uv.Add(MB.Update.Set(MECareMember.PrimaryProperty, careMemberData.Primary));

                        IMongoUpdate update = MB.Update.Combine(uv);
                        WriteConcernResult res = ctx.CareMembers.Collection.Update(q, update);
                        if (res.Ok)
                            result = true;          
                    }
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("DD:MongoCareMemberRepository:Update()" + ex.Message, ex.InnerException); }
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

        public IEnumerable<object> FindByPatientId(object request)
        {
            List<CareMemberData> careMembersDataList = null;
            GetAllCareMembersDataRequest dataRequest = (GetAllCareMembersDataRequest)request;
            try
            {
                using (CareMemberMongoContext ctx = new CareMemberMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MECareMember.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
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
                                TypeId = meCM.Type.ToString()
                            });
                        }
                    }
                }
                return careMembersDataList;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}

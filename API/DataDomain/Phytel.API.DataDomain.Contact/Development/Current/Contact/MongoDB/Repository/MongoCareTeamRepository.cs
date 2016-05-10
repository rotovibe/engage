using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.DataDomain.Contact.MongoDB;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Contact.CareTeam
{
    public class MongoCareTeamRepository : ICareTeamRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        public IAuditHelpers AuditHelpers { get; set; }

        public MongoCareTeamRepository()
        {
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEContactCareTeam)) == false)
                    BsonClassMap.RegisterClassMap<MEContactCareTeam>();
            }
            catch
            {

            }
        }

        public MongoCareTeamRepository(string dbname)
        {
            _dbName = dbname;           
        }

        public string UserId { get; set; }

        public object Insert(object newEntity)
        {
            string response = string.Empty;
            var data = newEntity as SaveCareTeamDataRequest;
            if (data != null)
            {
                try
                {
                    using (var ctx = new ContactCareTeamMongoContext(_dbName))
                    {

                        var queries = new List<IMongoQuery>
                            {
                                MB.Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(data.ContactId)),
                                MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                            };

                        var query = MB.Query.And(queries);
                        var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);

                        if (contactCareTeam == null)
                        {
                            //Add 
                            var meCareTeam = new MEContactCareTeam(this.UserId, DateTime.UtcNow)
                            {

                                ContactId = ObjectId.Parse(data.ContactId),
                                MeCareTeamMembers = BuildMECareTeamMembers(data.CareTeamData.Members, this.UserId),
                                DeleteFlag = false
                            };

                            ctx.CareTeam.Collection.Save(meCareTeam);
                            response = meCareTeam.Id.ToString();
                            AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.CareTeam.ToString(),
                                            meCareTeam.Id.ToString(),
                                           DataAuditType.Insert,
                                           data.ContractNumber);
                        }
                        else
                        {
                            //Update
                            contactCareTeam.MeCareTeamMembers = BuildMECareTeamMembers(data.CareTeamData.Members, this.UserId);
                            contactCareTeam.UpdatedBy = ObjectId.Parse(this.UserId);
                            contactCareTeam.LastUpdatedOn = DateTime.UtcNow;

                            ctx.CareTeam.Collection.Save(contactCareTeam);
                            response = contactCareTeam.Id.ToString();

                            AuditHelper.LogDataAudit(this.UserId,
                                          MongoCollectionName.CareTeam.ToString(),
                                           contactCareTeam.Id.ToString(),
                                          DataAuditType.Update,
                                          data.ContractNumber);
                        }
                    }
                  
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

       
        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Determines if a Care Team Exist
        /// </summary>
        /// <param name="careTeamId"></param>
        /// <returns></returns>
        public bool CareTeamExist(string careTeamId)
        {
            var res = false;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    ObjectId cid;
                    if (ObjectId.TryParse(careTeamId, out cid))
                    {
                        var queries = new List<IMongoQuery>
                        {

                            MB.Query<MEContactCareTeam>.EQ(c => c.Id, cid),
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                        var query = MB.Query.And(queries);
                        var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);
                        res = contactCareTeam != null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

       

        /// <summary>
        /// Determines if a Care Team Exist
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public bool ContactCareTeamExist(string contactId)
        {
            var res = false;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    ObjectId cid;
                    if (ObjectId.TryParse(contactId, out cid))
                    {
                        var queries = new List<IMongoQuery>
                        {

                            MB.Query<MEContactCareTeam>.EQ(c => c.ContactId, cid),
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                        var query = MB.Query.And(queries);
                        var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);
                        res = contactCareTeam != null;
                    }                                        
                }
            }
            catch (Exception)
            {                    
                throw;
            }
            return res;
        }

        public bool CareTeamMemberExist(string careTeamId, string memberId)
        {
            var res = false;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    ObjectId cid;
                    ObjectId mid;

                    if (ObjectId.TryParse(careTeamId, out cid) && ObjectId.TryParse(memberId, out mid))
                    {
                        var careTeam = GetCareTeam(careTeamId);
                        if (careTeam!=null)
                        {
                            var currentMeCareTeamMember = careTeam.MeCareTeamMembers.FirstOrDefault(
                                x => x.Id == mid);
                            res = currentMeCareTeamMember != null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
        
        private MEContactCareTeam GetContactCareTeam(string contactId)
        {
            MEContactCareTeam res = null;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    ObjectId cid;
                    if (ObjectId.TryParse(contactId, out cid))
                    {
                        var queries = new List<IMongoQuery>
                        {

                            MB.Query<MEContactCareTeam>.EQ(c => c.ContactId, cid),
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                        var query = MB.Query.And(queries);
                        var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);
                        res = contactCareTeam;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        private MEContactCareTeam GetCareTeam(string careTeamId)
        {
            MEContactCareTeam res = null;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    ObjectId cid;
                    if (ObjectId.TryParse(careTeamId, out cid))
                    {
                        var queries = new List<IMongoQuery>
                        {

                            MB.Query<MEContactCareTeam>.EQ(c => c.Id, cid),
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                        var query = MB.Query.And(queries);
                        var careTeam = ctx.CareTeam.Collection.FindOne(query);
                        res = careTeam;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }


        public bool UpdateCareTeamMember(object entity)
        {
            UpdateCareTeamMemberDataRequest request = (UpdateCareTeamMemberDataRequest)entity;
            CareTeamMemberData careTeamMemberData = request.CareTeamMemberData;
            bool result = false;
            try
            {
                if (careTeamMemberData != null)
                {
                    using (ContactCareTeamMongoContext ctx = new ContactCareTeamMongoContext(_dbName))
                    {

                        var contactCareTeam = GetContactCareTeam(request.ContactId);

                        if (contactCareTeam == null)
                            throw new ApplicationException("UpdateCareTeamMember: The referenced contact doesn't have a care team");

                        if (contactCareTeam.Id != ObjectId.Parse(request.CareTeamId))
                            throw new ApplicationException("UpdateCareTeamMember: The referenced Care Team doesn't exist or is not assigned to the referenced contact");

                        var currentMeCareTeamMember = 
                            contactCareTeam.MeCareTeamMembers.FirstOrDefault(
                                x => x.Id == ObjectId.Parse(careTeamMemberData.Id));
                        
                        if (currentMeCareTeamMember == null)
                            throw new ApplicationException("UpdateCareTeamMember: The referenced care team member doesn't exist");

                        var memberIndex = contactCareTeam.MeCareTeamMembers.FindIndex(
                                x => x.Id == ObjectId.Parse(careTeamMemberData.Id));

                        var updatedMecareMemberTeam = BuildMECareTeamMember(this.UserId, careTeamMemberData);

                        updatedMecareMemberTeam.RecordCreatedOn = currentMeCareTeamMember.RecordCreatedOn;
                        updatedMecareMemberTeam.RecordCreatedBy = currentMeCareTeamMember.RecordCreatedBy;
                       
                        contactCareTeam.MeCareTeamMembers[memberIndex] = updatedMecareMemberTeam;

                        contactCareTeam.LastUpdatedOn = DateTime.UtcNow;
                        contactCareTeam.UpdatedBy = ObjectId.Parse(this.UserId);

                        ctx.CareTeam.Collection.Save(contactCareTeam);

                        AuditHelper.LogDataAudit(this.UserId,
                                          MongoCollectionName.CareTeam.ToString(),
                                           contactCareTeam.Id.ToString(),
                                          DataAuditType.Update,
                                          request.ContractNumber);


                        result = true;
                    }
                }
                return result;
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex) { throw new Exception("CareMemberDD:MongoCareMemberRepository:Update()" + ex.Message, ex.InnerException); }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
        
        public object GetCareTeamByContactId(string contactId)
        {
            CareTeamData careTeam = null;
            using (var ctx = new ContactCareTeamMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                {
                    MB.Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(contactId)),
                    MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                };

                var query = MB.Query.And(queries);
                var meCareTeam = ctx.CareTeam.Collection.FindOne(query);
                if (meCareTeam != null)
                {
                    careTeam = BuildCareTeamData(meCareTeam);
                }
            }
            return careTeam;
        }

        private CareTeamData BuildCareTeamData(MEContactCareTeam meCareTeam)
        {
            CareTeamData team = null;
            if (meCareTeam != null)
            {
                team = new CareTeamData
                {
                    Id = meCareTeam.Id.ToString(),
                    ContactId = meCareTeam.ContactId.ToString(),
                    Members = BuildCareTeamMembers(meCareTeam.MeCareTeamMembers),
                    CreatedOn = meCareTeam.RecordCreatedOn,
                    CreatedById = meCareTeam.RecordCreatedBy.ToString(),
                    UpdatedById = meCareTeam.UpdatedBy == null ? null : meCareTeam.UpdatedBy.ToString(),
                    UpdatedOn = meCareTeam.LastUpdatedOn
                };
            }
            return team;
        }

        private List<MECareTeamMember> BuildMECareTeamMembers(List<CareTeamMemberData> members, string userId)
        {
            var result = new List<MECareTeamMember>();

            foreach (var member in members)
            {
                var meMember = BuildMECareTeamMember(userId, member);
                result.Add(meMember);
            }

            return result;
        }

        private MECareTeamMember BuildMECareTeamMember(string userId, CareTeamMemberData member)
        {
            MECareTeamMember meMember = null;
            if (member != null)
            {
                meMember = new MECareTeamMember
                {
                    Id = string.IsNullOrEmpty(member.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(member.Id),
                    ContactId = ObjectId.Parse(member.ContactId),
                    Core = member.Core,
                    RoleId = string.IsNullOrEmpty(member.RoleId) ? (ObjectId?)BsonNull.Value : ObjectId.Parse(member.RoleId),
                    CustomRoleName = member.CustomRoleName,
                    StartDate = member.StartDate,
                    EndDate = member.EndDate,
                    Frequency =
                        string.IsNullOrEmpty(member.FrequencyId) ? (ObjectId?)BsonNull.Value : ObjectId.Parse(member.FrequencyId),
                    Distance = member.Distance ?? member.Distance,
                    DistanceUnit = member.DistanceUnit,
                    ExternalRecordId = member.ExternalRecordId,
                    Notes = member.Notes,
                    DataSource = member.DataSource,
                    Status = (CareTeamMemberStatus)member.StatusId,
                };
                if (string.IsNullOrEmpty(member.Id))
                {
                    //it is an insert
                    meMember.RecordCreatedBy = ObjectId.Parse(userId);
                    meMember.RecordCreatedOn = DateTime.UtcNow;
                }
                else
                {
                    //it is an update
                    meMember.UpdatedBy = ObjectId.Parse(userId);
                    meMember.LastUpdatedOn = DateTime.UtcNow;
                }
            }
            return meMember;
        }

        private List<CareTeamMemberData> BuildCareTeamMembers(List<MECareTeamMember> members)
        {
            List<CareTeamMemberData> result = null;
            if (members != null && members.Count != 0)
            {
                result = new List<CareTeamMemberData>();
                foreach (var member in members)
                {
                    var meMember = BuildCareTeamMember(member);
                    result.Add(meMember);
                }
            }
            return result;
        }

        private CareTeamMemberData BuildCareTeamMember(MECareTeamMember member)
        {
            CareTeamMemberData meMember = null;
            if (member != null)
            {
                meMember = new CareTeamMemberData
                {
                    Id = member.Id.ToString(),
                    ContactId = member.ContactId.ToString(),
                    Core = member.Core,
                    RoleId = (member.RoleId == null || member.RoleId == ObjectId.Empty) ? null : member.RoleId.ToString(),
                    CustomRoleName = member.CustomRoleName,
                    StartDate = member.StartDate,
                    EndDate = member.EndDate,
                    FrequencyId = (member.Frequency == null || member.Frequency == ObjectId.Empty) ? null : member.Frequency.ToString(),
                    Distance = member.Distance ?? member.Distance,
                    DistanceUnit = member.DistanceUnit,
                    ExternalRecordId = member.ExternalRecordId,
                    Notes = member.Notes,
                    DataSource = member.DataSource,
                    StatusId = (int)member.Status,
                    CreatedOn = member.RecordCreatedOn,
                    CreatedById = member.RecordCreatedBy.ToString(),
                    UpdatedById = member.UpdatedBy == null ? null : member.UpdatedBy.ToString(),
                    UpdatedOn = member.LastUpdatedOn
                };
            }
            return meMember;
        }

        public void DeleteCareTeamMember(object entity)
        {
            var request = (DeleteCareTeamMemberDataRequest)entity;

            using (var ctx = new ContactCareTeamMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                        {
                            MB.Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(request.ContactId)),
                            MB.Query<MEContactCareTeam>.EQ(c => c.Id, ObjectId.Parse(request.CareTeamId)),
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                var query = MB.Query.And(queries);
                var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);
                var members = contactCareTeam.MeCareTeamMembers;
                var memberToRemove = members.FirstOrDefault(m => m.Id == ObjectId.Parse(request.MemberId));

                if (memberToRemove == null) return;

                members.Remove(memberToRemove);

                contactCareTeam.MeCareTeamMembers = members;
                contactCareTeam.LastUpdatedOn = DateTime.UtcNow;
                contactCareTeam.UpdatedBy = ObjectId.Parse(this.UserId);

                var saveResult = ctx.CareTeam.Collection.Save(contactCareTeam);

                AuditHelper.LogDataAudit(this.UserId,
                    MongoCollectionName.CareTeam.ToString(),
                    contactCareTeam.Id.ToString(),
                    DataAuditType.Update,
                    request.ContractNumber);
            }
            
        }

        public void Delete(object entity)
        {
            var request = (DeleteCareTeamDataRequest)entity;

            using (var ctx = new ContactCareTeamMongoContext(_dbName))
            {
                var queries = new List<IMongoQuery>
                        {
                            MB.Query<MEContactCareTeam>.EQ(c => c.Id, ObjectId.Parse(request.Id)),                            
                            MB.Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                        };

                var query = MB.Query.And(queries);
                var contactCareTeam = ctx.CareTeam.Collection.FindOne(query);
                var builder = new List<MB.UpdateBuilder>();
                builder.Add(MB.Update.Set(MEContactCareTeam.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                builder.Add(MB.Update.Set(MEContactCareTeam.DeleteFlagProperty, true));
                builder.Add(MB.Update.Set(MEContactCareTeam.LastUpdatedOnProperty, DateTime.UtcNow));
                builder.Add(MB.Update.Set(MEContactCareTeam.LastUpdatedByProperty, ObjectId.Parse(this.UserId)));

                IMongoUpdate update = MB.Update.Combine(builder);
                ctx.CareTeam.Collection.Update(query, update);                

                AuditHelper.LogDataAudit(this.UserId,
                    MongoCollectionName.CareTeam.ToString(),
                    contactCareTeam.Id.ToString(),
                    DataAuditType.Update,
                    request.ContractNumber);
            }
        }

        public void UndoDelete(object entity)
        {
            UndoDeleteCareTeamDataRequest request = (UndoDeleteCareTeamDataRequest)entity;
            try
            {
                using (var ctx = new ContactCareTeamMongoContext(_dbName))
                {
                    var query = MB.Query<MEContactCareTeam>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEContactCareTeam.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEContactCareTeam.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEContactCareTeam.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEContactCareTeam.LastUpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.CareTeam.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CareMember.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }
    }
}

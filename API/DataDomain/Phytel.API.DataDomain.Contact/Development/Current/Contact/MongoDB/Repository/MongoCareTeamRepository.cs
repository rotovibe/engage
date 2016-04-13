using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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
            AppHostBase.Instance.Container.AutoWire(this);
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
                                Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(data.CareTeamData.ContactId)),
                                Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                            };

                        var query = Query.And(queries);
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

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
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
                    Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(contactId)),
                    Query<MEContactCareTeam>.EQ(c => c.DeleteFlag, false)
                };

                var query = Query.And(queries);
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
            var result = new List<CareTeamMemberData>();

            foreach (var member in members)
            {
                var meMember = BuildCareTeamMember(member);
                result.Add(meMember);
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
    }
}

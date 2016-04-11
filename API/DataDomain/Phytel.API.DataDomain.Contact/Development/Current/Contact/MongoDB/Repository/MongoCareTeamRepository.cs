﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common.Audit;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.DataDomain.Contact.MongoDB;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using ServiceStack.Common;
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
            var data = newEntity as CareTeamData;
            if (data != null)
            {
                try
                {
                    using (var ctx = new ContactCareTeamMongoContext(_dbName))
                    {

                        var queries = new List<IMongoQuery>
                            {
                                Query<MEContactCareTeam>.EQ(c => c.ContactId, ObjectId.Parse(data.ContactId)),
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
                                Members = BuildMECareTeamMembers(data.Members),
                                DeleteFlag = false
                            };

                            ctx.CareTeam.Collection.Save(meCareTeam);
                            response = meCareTeam.Id.ToString();
                        }
                        else
                        {
                            //Update
                            contactCareTeam.Members = BuildMECareTeamMembers(data.Members);
                            contactCareTeam.UpdatedBy = ObjectId.Parse(this.UserId);
                            contactCareTeam.LastUpdatedOn = DateTime.UtcNow;

                            ctx.CareTeam.Collection.Save(contactCareTeam);
                            response = contactCareTeam.Id.ToString();
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

        private List<CareTeamMember> BuildMECareTeamMembers(List<CareMemberData> members)
        {
            var result = new List<CareTeamMember>();

            foreach (var member in members)
            {
                var meMember = new CareTeamMember
                {
                    Id = string.IsNullOrEmpty(member.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(member.Id),
                    ContactId = ObjectId.Parse(member.ContactId),
                    Core = member.Core,
                    RoleId = string.IsNullOrEmpty(member.RoleId) ? ObjectId.Empty : ObjectId.Parse(member.RoleId),
                    CustomRoleName = member.CustomRoleName,
                    StartDate = member.StartDate,
                    EndDate = member.EndDate,
                    Frequency =
                        string.IsNullOrEmpty(member.Frequency) ? ObjectId.Empty : ObjectId.Parse(member.Frequency),
                    Distance = member.Distance ?? member.Distance,
                    ExternalRecordId = member.ExternalRecordId,
                    Notes = member.Notes,
                    DataSource = member.DataSource
                };

                result.Add(meMember);
            }

            return result;
        }

        private List<CareMemberData> BuildMECareTeamMemberData(List<CareTeamMember> members)
        {
            var result = new List<CareMemberData>();

            foreach (var member in members)
            {
                var meMember = new CareMemberData
                {
                    Id = member.ToString(),
                    ContactId = member.ContactId.ToString(),
                    Core = member.Core,
                    RoleId = member.RoleId.ToString(),
                    CustomRoleName = member.CustomRoleName,
                    StartDate = member.StartDate,
                    EndDate = member.EndDate,
                    Frequency = member.Frequency.ToString(),
                    Distance = member.Distance ?? member.Distance,
                    ExternalRecordId = member.ExternalRecordId,
                    Notes = member.Notes,
                    DataSource = member.DataSource
                };

                result.Add(meMember);
            }

            return result;
        } 
    }
}

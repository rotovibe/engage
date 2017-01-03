using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Services;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientUserRepository : IPatientRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoPatientUserRepository()
        {

            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientUser)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientUser>();
            }
            catch { }

            #endregion

        }

        public MongoPatientUserRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }


        public List<PatientData> Select(List<string> patientIds)
        {
            throw new NotImplementedException();
        }

        public List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        public PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string patientId, string userId)
        {
            throw new NotImplementedException();
        }

        public object Update(PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object GetSSN(string patientId)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }

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
            DeletePatientUserByPatientIdDataRequest request = (DeletePatientUserByPatientIdDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var patientUserQuery = MB.Query<MEPatientUser>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var patientUserBuilder = new List<MB.UpdateBuilder>();
                    patientUserBuilder.Add(MB.Update.Set(MEPatientUser.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    patientUserBuilder.Add(MB.Update.Set(MEPatientUser.DeleteFlagProperty, true));
                    patientUserBuilder.Add(MB.Update.Set(MEPatientUser.LastUpdatedOnProperty, DateTime.UtcNow));
                    patientUserBuilder.Add(MB.Update.Set(MEPatientUser.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate patientUserUpdate = MB.Update.Combine(patientUserBuilder);
                    ctx.PatientUsers.Collection.Update(patientUserQuery, patientUserUpdate);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientUser.ToString(),
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

        public List<PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            List<PatientUserData> list = null;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientUser.PatientIdProperty, ObjectId.Parse(patientId)));
                    queries.Add(Query.EQ(MEPatientUser.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientUser> mePatientUsers = ctx.PatientUsers.Collection.Find(mQuery).ToList();
                    if (mePatientUsers != null && mePatientUsers.Count > 0)
                    {
                        list = new List<PatientUserData>();
                        foreach (MEPatientUser mePU in mePatientUsers)
                        {
                            list.Add(new PatientUserData
                            {
                                Id = mePU.Id.ToString(),
                                PatientId = mePU.PatientId.ToString(),
                                ContactId = mePU.ContactId.ToString(),
                                Flag = mePU.Flagged
                            });
                        }
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }


        public CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientUsersDataRequest request = (UndoDeletePatientUsersDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatient>.EQ(b => b.Id, ObjectId.Parse(request.PatientUserId));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientUser.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEPatientUser.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEPatientUser.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientUser.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientUsers.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientUser.ToString(),
                                            request.PatientUserId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }


        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindDuplicatePatient(PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }


        public PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool SyncPatient(SyncPatientInfoDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool AddPCMToPatientCohortView(AddPCMToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool RemovePCMFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool AddContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindByNameDOB(string firstName, string lastName, string dob)
        {
            throw new NotImplementedException();
        }
    }
}

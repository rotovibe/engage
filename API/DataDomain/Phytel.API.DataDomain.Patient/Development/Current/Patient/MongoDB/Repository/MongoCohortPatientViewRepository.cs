using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Patient.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MB = MongoDB.Driver.Builders;
using System.Configuration;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoCohortPatientViewRepository : IPatientRepository
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        public IDTOUtils Utils { get; set; }

        private string _dbName = string.Empty;
        static MongoCohortPatientViewRepository()
        {
            #region Register ClassMap           
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MECohortPatientView)) == false)
                    BsonClassMap.RegisterClassMap<MECohortPatientView>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(SearchField)) == false)
                    BsonClassMap.RegisterClassMap<SearchField>();
            }
            catch { }
            #endregion
        }

        public MongoCohortPatientViewRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutCohortPatientViewDataRequest cohortRequest = newEntity as PutCohortPatientViewDataRequest;
            MECohortPatientView patientView = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                //Does the patient exist in cohortpatientview?
                IMongoQuery query = Query.And(
                                Query.EQ(MECohortPatientView.PatientIDProperty, ObjectId.Parse(cohortRequest.PatientID)),
                                Query.EQ(MECohortPatientView.LastNameProperty, cohortRequest.LastName));
                patientView = ctx.CohortPatientViews.Collection.FindOneAs<MECohortPatientView>(query);
                if (patientView == null)
                {
                    patientView = new MECohortPatientView(this.UserId)
                    {
                        PatientID = ObjectId.Parse(cohortRequest.PatientID),
                        LastName = cohortRequest.LastName,
                        Version = cohortRequest.Version,
                        DeleteFlag = false
                    };

                    if (cohortRequest.SearchFields != null && cohortRequest.SearchFields.Count > 0)
                    {
                        List<SearchField> fields = new List<SearchField>();
                        foreach (SearchFieldData c in cohortRequest.SearchFields)
                        {
                            fields.Add(new SearchField { Active = c.Active, FieldName = c.FieldName, Value = c.Value });
                        }
                        patientView.SearchFields = fields;
                    }

                    ctx.CohortPatientViews.Collection.Insert(patientView);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CohortPatientView.ToString(),
                                            patientView.Id.ToString(),
                                            Common.DataAuditType.Insert,
                                            cohortRequest.ContractNumber);
                }
            }

            return new PutCohortPatientViewDataResponse
            {
                Id = patientView.Id.ToString()
            };
        }

        public object InsertAll(List<object> entities)
        {
            List<string> insertedIds = new List<string>();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var bulk = ctx.CohortPatientViews.Collection.InitializeUnorderedBulkOperation();
                    foreach (CohortPatientViewData cpvData in entities)
                    {
                        MECohortPatientView meCPV = new MECohortPatientView(this.UserId)
                        {
                            PatientID = ObjectId.Parse(cpvData.PatientID),
                            LastName = cpvData.LastName,
                            DeleteFlag = false,
                            AssignedToContactIds = cpvData.AssignedToContactIds
                        };
                        if (cpvData.SearchFields != null && cpvData.SearchFields.Count > 0)
                        {
                            List<SearchField> fields = new List<SearchField>();
                            foreach (SearchFieldData c in cpvData.SearchFields)
                            {
                                fields.Add(new SearchField { Active = c.Active, FieldName = c.FieldName, Value = c.Value });
                            }
                            meCPV.SearchFields = fields;
                        }
                        bulk.Insert(meCPV.ToBsonDocument());
                        insertedIds.Add(meCPV.Id.ToString());
                    }
                    BulkWriteResult bwr = bulk.Execute();
                }
                AuditHelper.LogDataAudit(this.UserId, MongoCollectionName.CohortPatientView.ToString(), insertedIds, Common.DataAuditType.Insert, _dbName);
            }
            catch (Exception ex)
            {
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return true;
        }

        public void Delete(object entity)
        {
            DeleteCohortPatientViewDataRequest request = (DeleteCohortPatientViewDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var ub = new List<MB.UpdateBuilder>();
                    ub.Add(MB.Update.Set(MECohortPatientView.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    ub.Add(MB.Update.Set(MECohortPatientView.DeleteFlagProperty, true));
                    ub.Add(MB.Update.Set(MECohortPatientView.LastUpdatedOnProperty, DateTime.UtcNow));
                    ub.Add(MB.Update.Set(MECohortPatientView.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(ub);
                    ctx.CohortPatientViews.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CohortPatientView.ToString(),
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
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.CohortPatientViews
                           where p.Id == ObjectId.Parse(entityID) && p.DeleteFlag == false
                           select new DTO.PatientData
                            {
                            }).FirstOrDefault();
            }
            return patient;
        }

        public object Update(object entity)
        {
            PutUpdateCohortPatientViewRequest p = (PutUpdateCohortPatientViewRequest)entity;
            PutUpdateCohortPatientViewResponse resp = new PutUpdateCohortPatientViewResponse();

            try
            {
                CohortPatientViewData cpvd = p.CohortPatientView;
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.Id, ObjectId.Parse(p.CohortPatientView.Id));
                    List<SearchField> sfds = Utils.CloneAppDomainCohortPatientViews(p.CohortPatientView.SearchFields);

                    var uv = new List<MB.UpdateBuilder>();
                    if (!String.IsNullOrEmpty(cpvd.LastName)) uv.Add(MB.Update.Set(MECohortPatientView.LastNameProperty, cpvd.LastName));
                    if (!String.IsNullOrEmpty(cpvd.PatientID)) uv.Add(MB.Update.Set(MECohortPatientView.PatientIDProperty, ObjectId.Parse(cpvd.PatientID)));
                    uv.Add(MB.Update.Set(MECohortPatientView.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MECohortPatientView.LastUpdatedOnProperty, DateTime.UtcNow ));
                   // uv.Add(MB.Update.Set(MECohortPatientView.AssignedToProperty, p.CohortPatientView.AssignedToContactIds.Select(c => BsonValue.Create(c))));
                    
                    if (p.CohortPatientView != null) { uv.Add(MB.Update.SetWrapped<List<SearchField>>(MECohortPatientView.SearchFieldsProperty, sfds)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.CohortPatientViews.Collection.Update(q, update);
                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.CohortPatientView.ToString(),
                        p.CohortPatientView.Id.ToString(),
                        Common.DataAuditType.Update,
                        p.ContractNumber);
                }
                resp.CohortPatientViewId = p.CohortPatientView.Id;
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("CohortPatientDD:Update()::" + ex.Message, ex.InnerException);
            }
        }

        public List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            #region commented code
            /* Query without filter:
             *  { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}
             * 
             * Query with single field filter:
             *  { $and : [ 
             *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}, 
             *      { $or : [ 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'FN'}}}, 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'LN'}}}, 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'PN'}}} 
             *          ] 
             *      } 
             *   ] 
             * }
             * 
             * Query with double field filter:
             *  { $and : [ 
             *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}, 
             *      { $and : [ 
             *          { $or : [ 
             *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fldn':'FN'}}}, 
             *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fldn':'PN'}}}
             *            ]
             *          },	
             *          { sf: { $elemMatch : {'val':/^<Second Field Text Here>/i, 'fldn':'LN'}}}
             *        ] 
             *      } 
             *    ] 
             *  }
             * 
            */
            
            #endregion

            try
            {
                string jsonQuery = string.Empty;
                string queryName = "TBD"; //Pass this into the method call
                string redisKey = string.Empty;

                if (filterData[0].Trim() == string.Empty)
                {
                    jsonQuery = query;
                    redisKey = string.Format("{0}{1}{2}", queryName, skip, take);
                }
                else
                {
                    if (filterData[1].Trim() == string.Empty)
                    {
                        redisKey = string.Format("{0}{1}{2}{3}", queryName, filterData[0].Trim(), skip, take);

                        jsonQuery = "{ $and : [ ";
                        jsonQuery += string.Format("{0},  ", query);
                        jsonQuery += "{ $or : [  ";
                        jsonQuery += "{ sf: { $elemMatch : {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'FN'}}},  ";
                        jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'LN'}}},  ";
                        jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'PN'}}}]}]}";
                    }
                    else
                    {
                        redisKey = string.Format("{0}{1}{2}{3}{4}", queryName, filterData[0].Trim(), filterData[1].Trim(), skip, take);

                        jsonQuery = "{ $and : [ ";
                        jsonQuery += string.Format("{0},  ", query);
                        jsonQuery += "{ $and : [  ";
                        jsonQuery += "{ $or : [  ";
                        jsonQuery += "{ sf : { $elemMatch: {'" + SearchField.ValueProperty + "':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'FN'}}},  ";
                        jsonQuery += "{ sf : { $elemMatch: {'" + SearchField.ValueProperty + "':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'PN'}}} ";
                        jsonQuery += "]}, ";
                        jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[1].Trim());
                        jsonQuery += "'" + SearchField.FieldNameProperty + "':'LN'}}}]}]}}";
                    }
                }

                string redisClientIPAddress = System.Configuration.ConfigurationManager.AppSettings.Get("RedisClientIPAddress");

                List<PatientData> cohortPatientList = new List<PatientData>();
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if(string.IsNullOrEmpty(redisClientIPAddress) == false)
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get cohortPatientList from Redis using the redisKey now
                    cohortPatientList = client.Get<List<PatientData>>(redisKey);
                }
                else
                {
                    using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                    {
                        BsonDocument searchQuery = BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
                        QueryDocument queryDoc = new QueryDocument(searchQuery);
                        SortByBuilder builder = PatientsUtils.BuildSortByBuilder(querySort);

                        List<MECohortPatientView> meCohortPatients = ctx.CohortPatientViews.Collection.Find(queryDoc)
                            .SetSortOrder(builder).SetSkip(skip).SetLimit(take).Distinct().ToList();

                        if (meCohortPatients != null && meCohortPatients.Count > 0)
                        {
                            meCohortPatients.ForEach(delegate(MECohortPatientView pat)
                            {
                                if(!pat.DeleteFlag)
                                {   
                                    PatientData cohortPatient = new PatientData();
                                    cohortPatient.Id = pat.PatientID.ToString();

                                    foreach (SearchField sf in pat.SearchFields)
                                    {
                                        cohortPatient.FirstName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.FN).FirstOrDefault()).Value;
                                        cohortPatient.LastName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.LN).FirstOrDefault()).Value;
                                        cohortPatient.Gender = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.G).FirstOrDefault()).Value;
                                        cohortPatient.DOB = CommonFormatter.FormatDateOfBirth(((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.DOB).FirstOrDefault()).Value);
                                        cohortPatient.MiddleName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.MN).FirstOrDefault()).Value;
                                        cohortPatient.Suffix = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.SFX).FirstOrDefault()).Value;
                                        cohortPatient.PreferredName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == Constants.PN).FirstOrDefault()).Value;
                                    }
                                    cohortPatientList.Add(cohortPatient);
                                }

                            });
                        }
                    }
                    //put cohortPatientList into cache using redisKey now
                    if (client != null)
                        client.Set<List<PatientData>>(redisKey, cohortPatientList);
                }
                return cohortPatientList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<object> patViews = new List<object>();

            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if (selectExpressions.Count > 0)
            {
                IList<IMongoQuery> queries = new List<IMongoQuery>();
                for (int i = 0; i < selectExpressions.Count; i++)
                {
                    groupType = selectExpressions[0].NextExpressionType;

                    IMongoQuery query = SelectExpressionHelper.ApplyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = SelectExpressionHelper.BuildQuery(groupType, queries);
            }

            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                List<MECohortPatientView> cpvs = ctx.CohortPatientViews.Collection.Find(mQuery).ToList();

                if (cpvs != null)
                {
                    cpvs.ForEach(cpv =>
                    {
                        patViews.Add(new CohortPatientViewData
                        {
                            Id = cpv.Id.ToString(),
                            LastName = cpv.LastName,
                            PatientID = cpv.PatientID.ToString(),
                            SearchFields = PatientsUtils.GetSearchFields(cpv.SearchFields),
                            Version = cpv.Version
                        });
                    });
                }
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, patViews);
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }

        public CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            CohortPatientViewData data = null;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MECohortPatientView.PatientIDProperty, ObjectId.Parse(patientId)));
                    //queries.Add(Query.EQ(MECohortPatientView.DeleteFlagProperty, false)); Commented out this line as there are few records in Prod that do not contain basic fields like del, ttl, uon, etc.
                    IMongoQuery mQuery = Query.And(queries);
                    MECohortPatientView meCPV = ctx.CohortPatientViews.Collection.Find(mQuery).FirstOrDefault();
                    if (meCPV != null)
                    {
                        data = new CohortPatientViewData
                        {
                            Id = meCPV.Id.ToString(),
                            LastName = meCPV.LastName,
                            PatientID = meCPV.PatientID.ToString(),
                            AssignedToContactIds = meCPV.AssignedToContactIds
                        };
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        #region needs to be taken out of IPatientRepository . In place right now to accomidate the interface
        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public List<PatientData> Select(List<string> patientIds)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientPriorityResponse UpdatePriority(DTO.PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientFlaggedResponse UpdateFlagged(DTO.PutPatientFlaggedRequest request)
        {
            throw new NotImplementedException();
        }
        public object FindByID(string patientId, string userId)
        {
            throw new NotImplementedException();
        }

        public object Update(DTO.PutUpdatePatientDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object GetSSN(string patientId)
        {
            throw new NotImplementedException();
        }
        #endregion


        public List<PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            UndoDeleteCohortPatientViewDataRequest request = (UndoDeleteCohortPatientViewDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var ub = new List<MB.UpdateBuilder>();
                    ub.Add(MB.Update.Set(MECohortPatientView.TTLDateProperty, BsonNull.Value));
                    ub.Add(MB.Update.Set(MECohortPatientView.DeleteFlagProperty, false));
                    ub.Add(MB.Update.Set(MECohortPatientView.LastUpdatedOnProperty, DateTime.UtcNow));
                    ub.Add(MB.Update.Set(MECohortPatientView.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(ub);
                    ctx.CohortPatientViews.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CohortPatientView.ToString(),
                                            request.Id.ToString(),
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
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.Id));

                    var cohort = ctx.CohortPatientViews.Collection.FindOne(q);

                    if (cohort != null)
                    {
                        var fields = cohort.SearchFields;
                        var PcmField = fields.FirstOrDefault(f => f.FieldName == Constants.PCM);

                        if (PcmField == null)
                        {
                            var searchField = new SearchField
                            {
                                FieldName = Constants.PCM,
                                Value = request.ContactIdToAdd,
                                Active = true
                            };

                            fields.Add(searchField);

                        }
                        else
                        {
                            PcmField.Value = request.ContactIdToAdd;
                            PcmField.Active = true;

                        }

                        ctx.CohortPatientViews.Collection.Save(cohort);

                        cohort.LastUpdatedOn = DateTime.UtcNow;
                        cohort.UpdatedBy = ObjectId.Parse(request.UserId);
                    }

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.CohortPatientView.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    return true;
                }
            }
            catch (Exception) { throw; }
        }

        public bool RemovePCMFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request)
        {
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.Id));

                    var cohort = ctx.CohortPatientViews.Collection.FindOne(q);

                    if (cohort != null)
                    {
                        var fields = cohort.SearchFields;
                        var PcmField = fields.FirstOrDefault(f => f.FieldName == Constants.PCM);

                        if (PcmField != null)
                        
                        {
                            PcmField.Value = string.Empty;
                            PcmField.Active = true;
                            ctx.CohortPatientViews.Collection.Save(cohort);

                            AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.CohortPatientView.ToString(),
                                           request.Id.ToString(),
                                           Common.DataAuditType.Update,
                                           request.ContractNumber);

                        }

                        AuditHelper.LogDataAudit(this.UserId,
                                      MongoCollectionName.CohortPatientView.ToString(),
                                      request.Id.ToString(),
                                      Common.DataAuditType.Update,
                                      request.ContractNumber);

                        cohort.LastUpdatedOn = DateTime.UtcNow;
                        cohort.UpdatedBy = ObjectId.Parse(request.UserId);

                     

                    }

                   

                    return true;
                }
            }
            catch (Exception) { throw; }
        }

        public bool AddContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request)
        {
            try
            {
                using (var ctx = new PatientMongoContext(_dbName))
                {
                    var q = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.Id));

                    var cohort = ctx.CohortPatientViews.Collection.FindOne(q);

                    if (cohort != null)
                    {
                      var contactsToAssign = new List<string>();

                        if (request.ContactIdsToAssign != null)
                            contactsToAssign = request.ContactIdsToAssign;

                        cohort.AssignedToContactIds = contactsToAssign;
                        cohort.LastUpdatedOn = DateTime.UtcNow;
                        cohort.UpdatedBy = ObjectId.Parse(request.UserId);

                        //Update the Cohort.
                        ctx.CohortPatientViews.Collection.Save(cohort);

                        AuditHelper.LogDataAudit(this.UserId,
                                       MongoCollectionName.CohortPatientView.ToString(),
                                       request.Id.ToString(),
                                       Common.DataAuditType.Update,
                                       request.ContractNumber);
                    }



                    return true;
                }
            }
            catch (Exception) { throw; }
        }
    }
}

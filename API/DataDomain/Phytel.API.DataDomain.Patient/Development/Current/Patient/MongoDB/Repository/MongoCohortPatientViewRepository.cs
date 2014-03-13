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

namespace Phytel.API.DataDomain.Patient
{
    public class MongoCohortPatientViewRepository<T> : IPatientRepository<T>
    {
        private string _dbName = string.Empty;

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
                //Does the patient exist?
                IMongoQuery query = Query.And(
                                Query.EQ(MECohortPatientView.PatientIDProperty, ObjectId.Parse(cohortRequest.PatientID)),
                                Query.EQ(MECohortPatientView.LastNameProperty, cohortRequest.LastName));
                patientView = ctx.CohortPatientViews.Collection.FindOneAs<MECohortPatientView>(query);
                if (patientView == null)
                {
                    patientView = new MECohortPatientView
                    {
                        PatientID = ObjectId.Parse(cohortRequest.PatientID),
                        LastName = cohortRequest.LastName,
                        Version = cohortRequest.Version
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
                }
            }

            return new PutCohortPatientViewDataResponse
            {
                PatientID = patientView.Id.ToString()
            };
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
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.CohortPatientViews
                           where p.Id == ObjectId.Parse(entityID)
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
                    List<SearchField> sfds = DTOUtils.CloneAppDomainCohortPatientViews(p.CohortPatientView.SearchFields);

                    var uv = new List<MB.UpdateBuilder>();
                    if (!String.IsNullOrEmpty(cpvd.LastName)) uv.Add(MB.Update.Set(MECohortPatientView.LastNameProperty, cpvd.LastName));
                    if (!String.IsNullOrEmpty(cpvd.PatientID)) uv.Add(MB.Update.Set(MECohortPatientView.PatientIDProperty, ObjectId.Parse(cpvd.PatientID)));
                    
                    if (p.CohortPatientView != null) { uv.Add(MB.Update.SetWrapped<List<SearchField>>(MECohortPatientView.SearchFieldsProperty, sfds)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.CohortPatientViews.Collection.Update(q, update);
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
                                PatientData cohortPatient = new PatientData();
                                cohortPatient.ID = pat.PatientID.ToString();

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

        #region needs to be taken out of IPatientRepository . In place right now to accomidate the interface
        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public DTO.GetPatientsDataResponse Select(string[] patientIds)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientPriorityResponse UpdatePriority(DTO.PutPatientPriorityRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutPatientBackgroundDataResponse UpdateBackground(DTO.PutPatientBackgroundDataRequest request)
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
    }
}

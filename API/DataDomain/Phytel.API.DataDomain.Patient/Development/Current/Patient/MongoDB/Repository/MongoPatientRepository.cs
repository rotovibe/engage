using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Patient.DTO;
using MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientRepository<T> : IPatientRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutPatientDataRequest request = newEntity as PutPatientDataRequest;
            MEPatient patient = new MEPatient
            {
                Id = ObjectId.GenerateNewId(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Suffix = request.Suffix,
                PreferredName = request.PreferredName,
                Gender = request.Gender,
                DOB = request.DOB,
                Version = request.Version,
                //UpdatedBy = security token user id,
                //DisplayPatientSystemID
                TTLDate = null,
                DeleteFlag = false,
                LastUpdatedOn = System.DateTime.Now
            };

            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                ctx.Patients.Collection.Insert(patient);
            }

            return new PutPatientDataResponse
            {
                Id = patient.Id.ToString()
            };


        }

        public object InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.PatientData
                            {
                                ID = p.Id.ToString(),
                                DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                                FirstName = p.FirstName,
                                Gender = p.Gender,
                                LastName = p.LastName,
                                PreferredName = p.PreferredName,
                                MiddleName = p.MiddleName,
                                Suffix = p.Suffix,
                                Priority = (DTO.Priority)((int)p.Priority),
                                DisplayPatientSystemID = p.DisplayPatientSystemID.ToString()
                            }).FirstOrDefault();
            }
            return patient;
        }

        public object FindByID(string entityId, string userId)
        {
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityId)
                           select new DTO.PatientData
                           {
                               ID = p.Id.ToString(),
                               DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                               FirstName = p.FirstName,
                               Gender = p.Gender,
                               LastName = p.LastName,
                               PreferredName = p.PreferredName,
                               MiddleName = p.MiddleName,
                               Suffix = p.Suffix,
                               Priority = (DTO.Priority)((int)p.Priority),
                               DisplayPatientSystemID = p.DisplayPatientSystemID.ToString(),
                               Flagged = GetFlaggedStatus(entityId, userId)
                           }).FirstOrDefault();
            }
            return patient;
        }

        private bool GetFlaggedStatus(string entityId, string userId)
        {
            bool result = false;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                var patientUsr = FindPatientUser(entityId, userId, ctx);

                if (patientUsr != null)
                {
                    result = patientUsr.Flagged;
                }
            }
            return result;
        }

        private static MEPatientUser FindPatientUser(string entityId, string userId, PatientMongoContext ctx)
        {
            var findQ = MB.Query.And(
                MB.Query<MEPatientUser>.EQ(b => b.PatientId, ObjectId.Parse(entityId)),
                MB.Query<MEPatientUser>.EQ(b => b.UserId, userId)
            );

            var patientUsr = ctx.PatientUsers.Collection.Find(findQ).FirstOrDefault();
            return patientUsr;
        }

        public List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            /* Query without filter:
             *  { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fld':'Problem', 'act':true}}}
             * 
             * Query with single field filter:
             *  { $and : [ 
             *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fld':'Problem', 'act':true}}}, 
             *      { $or : [ 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fld':'FN'}}}, 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fld':'LN'}}}, 
             *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fld':'PN'}}} 
             *          ] 
             *      } 
             *   ] 
             * }
             * 
             * Query with double field filter:
             *  { $and : [ 
             *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fld':'Problem', 'act':true}}}, 
             *      { $and : [ 
             *          { $or : [ 
             *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fld':'FN'}}}, 
             *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fld':'PN'}}}
             *            ]
             *          },	
             *          { sf: { $elemMatch : {'val':/^<Second Field Text Here>/i, 'fld':'LN'}}}
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
                        jsonQuery += "'fld':'FN'}}},  ";
                        jsonQuery += "{ sf: { $elemMatch : {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'fld':'LN'}}},  ";
                        jsonQuery += "{ sf: { $elemMatch : {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'fld':'PN'}}}]}]}";
                    }
                    else
                    {
                        redisKey = string.Format("{0}{1}{2}{3}{4}", queryName, filterData[0].Trim(), filterData[1].Trim(), skip, take);

                        jsonQuery = "{ $and : [ ";
                        jsonQuery += string.Format("{0},  ", query);
                        jsonQuery += "{ $and : [  ";
                        jsonQuery += "{ $or : [  ";
                        jsonQuery += "{ sf : { $elemMatch: {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'fld':'FN'}}},  ";
                        jsonQuery += "{ sf : { $elemMatch: {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
                        jsonQuery += "'fld':'PN'}}} ";
                        jsonQuery += "]}, ";
                        jsonQuery += "{ sf: { $elemMatch : {'val':/^";
                        jsonQuery += string.Format("{0}/i, ", filterData[1].Trim());
                        jsonQuery += "'fld':'LN'}}}]}]}}";
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
                        BsonDocument searchQuery = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
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
                                    cohortPatient.FirstName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "FN").FirstOrDefault()).Value;
                                    cohortPatient.LastName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "LN").FirstOrDefault()).Value;
                                    cohortPatient.Gender = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "G").FirstOrDefault()).Value;
                                    cohortPatient.DOB = CommonFormatter.FormatDateOfBirth(((SearchField)pat.SearchFields.Where(x => x.FieldName == "DOB").FirstOrDefault()).Value);
                                    cohortPatient.MiddleName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "MN").FirstOrDefault()).Value;
                                    cohortPatient.Suffix = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "SFX").FirstOrDefault()).Value;
                                    cohortPatient.PreferredName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "PN").FirstOrDefault()).Value;
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
            throw new NotImplementedException();
            //List<IMongoQuery> queries = new List<IMongoQuery>();

            //queries.Add(Query.EQ(MEPatient.FirstNameProperty, "Greg"));
            //queries.Add(Query.EQ(MEPatient.LastNameProperty, "Tony"));

            //IMongoQuery query2 = Query.And(queries);

            //IMongoQuery query = Query.Or(
            //    Query.EQ(MEPatient.FirstNameProperty, "Greg"),
            //    Query.EQ(MEPatient.LastNameProperty, "Tony"));
        }

        public GetPatientsDataResponse Select(string[] patientIds)
        {
            BsonValue[] bsv = new BsonValue[patientIds.Length];
            for (int i = 0; i < patientIds.Length; i++)
            {
                bsv[i] = ObjectId.Parse(patientIds[i]);
            }

            IMongoQuery query = MB.Query.In("_id", bsv);
            List<MEPatient> pr = null;
            List<DTO.PatientData> pResp = new List<DTO.PatientData>();
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                pr = ctx.Patients.Collection.Find(query).ToList();
                // convert to a PatientDetailsResponse
                foreach (MEPatient mp in pr)
                {
                    pResp.Add(new DTO.PatientData
                    {
                        ID = mp.Id.ToString(),
                        PreferredName = mp.PreferredName,
                        DOB = mp.DOB,
                        FirstName = mp.FirstName,
                        Gender = mp.Gender,
                        LastName = mp.LastName,
                        MiddleName = mp.MiddleName,
                        Suffix = mp.Suffix,
                        Version = mp.Version,
                        Priority = (DTO.Priority)((int)mp.Priority),
                        DisplayPatientSystemID = mp.DisplayPatientSystemID.ToString()
                    });
                }
            }

            GetPatientsDataResponse pdResponse = new GetPatientsDataResponse();
            pdResponse.Patients = pResp;

            return pdResponse;
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), MB.SortBy.Null,
                                                new MB.UpdateBuilder().Set(MEPatient.PriorityProperty, (MEPriority)request.Priority).Set(MEPatient.UpdatedByProperty, request.UserId));
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {

                    var patientUsr = FindPatientUser(request.PatientId, request.UserId, ctx);

                    if (patientUsr == null)
                    {
                        ctx.PatientUsers.Collection.Insert(new MEPatientUser
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            UserId = request.UserId,
                            Flagged = Convert.ToBoolean(request.Flagged),
                            Version = "v1",
                            LastUpdatedOn = System.DateTime.UtcNow,
                            DeleteFlag = false,
                             UpdatedBy = request.UserId
                        });
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                    else
                    {

                        var pUQuery = new QueryDocument(MEPatientUser.IdProperty, patientUsr.Id);
                        var sortBy = new MB.SortByBuilder().Ascending("_id");
                        MB.UpdateBuilder updt = new MB.UpdateBuilder().Set(MEPatientUser.FlaggedProperty, Convert.ToBoolean(request.Flagged))
                            .Set(MEPatientUser.UpdatedByProperty, request.UserId);
                        var pt = ctx.PatientUsers.Collection.FindAndModify(pUQuery, sortBy, updt, true);
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                if (request.Priority == null)
                    throw new ArgumentException("Priority is missing from the DataDomain request.");

                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEPatient.IdProperty, ObjectId.Parse(request.Id));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.FirstName != null)
                    {
                        if (request.FirstName == "\"\"" || (request.FirstName == "\'\'"))
                            updt.Set(MEPatient.FirstNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.FirstNameProperty, request.FirstName);
                    }
                    if (request.LastName != null)
                    {
                        if (request.LastName == "\"\"" || (request.LastName == "\'\'"))
                            updt.Set(MEPatient.LastNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.LastNameProperty, request.LastName);
                    }
                    if (request.MiddleName != null)
                    {
                        if (request.MiddleName == "\"\"" || (request.MiddleName == "\'\'"))
                            updt.Set(MEPatient.MiddleNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.MiddleNameProperty, request.MiddleName);
                    }
                    if (request.Suffix != null)
                    {
                        if (request.Suffix == "\"\"" || (request.Suffix == "\'\'"))
                            updt.Set(MEPatient.SuffixProperty, string.Empty);
                        else
                            updt.Set(MEPatient.SuffixProperty, request.Suffix);
                    }
                    if (request.PreferredName != null)
                    {
                        if (request.PreferredName == "\"\"" || (request.PreferredName == "\'\'"))
                            updt.Set(MEPatient.PreferredProperty, string.Empty);
                        else
                            updt.Set(MEPatient.PreferredProperty, request.PreferredName);
                    }
                    if (request.Gender != null)
                    {
                        if (request.Gender == "\"\"" || (request.Gender == "\'\'"))
                            updt.Set(MEPatient.GenderProperty, string.Empty);
                        else
                            updt.Set(MEPatient.GenderProperty, request.Gender);
                    }
                    if (request.DOB != null)
                    {
                        if (request.DOB == "\"\"" || (request.DOB == "\'\'"))
                            updt.Set(MEPatient.DOBProperty, string.Empty);
                        else
                            updt.Set(MEPatient.DOBProperty, request.DOB);
                    }
                    if (request.Version != null)
                    {
                        if ((request.Version == "\"\"") || (request.Version == "\'\'"))
                            updt.Set(MEPatient.VersionProperty, string.Empty);
                        else
                            updt.Set(MEPatient.VersionProperty, request.Version);
                    }
                    updt.Set("uon", System.DateTime.UtcNow);
                    updt.Set("pri", request.Priority);
                    updt.Set("uby", request.UserId);

                    var sortBy = new MB.SortByBuilder().Ascending("_id");
                    var pt = ctx.Patients.Collection.FindAndModify(pUQuery, sortBy, updt, true);

                    response.Id = request.Id;

                    // save to cohortuser collection
                    var findQ = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.Id));
                    MECohortPatientView cPV = ctx.CohortPatientViews.Collection.Find(findQ).FirstOrDefault();
                    cPV.SearchFields.ForEach(s => UpdateProperty(request, s));
                    List<SearchField> sfs = cPV.SearchFields.ToList<SearchField>();

                    ctx.CohortPatientViews.Collection.Update(findQ, MB.Update.SetWrapped<List<SearchField>>("sf", sfs).Set(MECohortPatientView.LastNameProperty, request.LastName));
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateProperty(PutUpdatePatientDataRequest request, SearchField s)
        {
            if (s.FieldName.Equals("PN"))
            {
                if (request.PreferredName != null)
                {
                    if (request.PreferredName == "\"\"" || (request.PreferredName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PreferredName;
                }
            }

            if (s.FieldName.Equals("SFX"))
            {
                if (request.Suffix != null)
                {
                    if (request.Suffix == "\"\"" || (request.Suffix == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.Suffix;
                }
            }

            if (s.FieldName.Equals("MN"))
            {
                if (request.MiddleName != null)
                {
                    if (request.MiddleName == "\"\"" || (request.MiddleName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.MiddleName;
                }
            }

            if (s.FieldName.Equals("DOB"))
            {
                if (request.DOB != null)
                {
                    if (request.DOB == "\"\"" || (request.DOB == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.DOB;
                }
            }

            if (s.FieldName.Equals("G"))
            {
                if (request.Gender != null)
                {
                    if (request.Gender == "\"\"" || (request.Gender == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.Gender;
                }
            }

            if (s.FieldName.Equals("LN"))
            {
                if (request.LastName != null)
                {
                    if (request.LastName == "\"\"" || (request.LastName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.LastName;
                }
            }
            if (s.FieldName.Equals("FN"))
            {
                if (request.FirstName != null)
                {
                    if (request.FirstName == "\"\"" || (request.FirstName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.FirstName;
                }
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public object Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}

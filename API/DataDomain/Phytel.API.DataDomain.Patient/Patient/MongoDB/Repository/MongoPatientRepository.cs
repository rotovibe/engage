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
using System.Net;
using System.Text.RegularExpressions;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientRepository : IPatientRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientRepository()
        {             
            
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatient)) == false)
                    BsonClassMap.RegisterClassMap<MEPatient>();
            }
            catch { }

            try 
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientUser)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientUser>();
            }
            catch { }
            
            #endregion
        
        }
        public MongoPatientRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                //Patient
                PutPatientDataRequest request = newEntity as PutPatientDataRequest;
                PatientData pd = request.Patient;
                MEPatient patient = null;
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    //Does the patient exist?
                    string searchQuery = string.Empty;
                    searchQuery = string.Format("{0} : /^{1}$/i, {2} : /^{3}$/i, {4} : '{5}', {6} : false, {7} : null",
                        MEPatient.FirstNameProperty,
                        pd.FirstName,
                        MEPatient.LastNameProperty, pd.LastName,
                        MEPatient.DOBProperty, pd.DOB,
                        MEPatient.DeleteFlagProperty,
                        MEPatient.TTLDateProperty);

                    var jsonQuery = "{ ";
                    jsonQuery += searchQuery;
                    jsonQuery += " }";
                    QueryDocument query = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
                    patient = ctx.Patients.Collection.FindOneAs<MEPatient>(query);

                    MongoCohortPatientViewRepository repo = new MongoCohortPatientViewRepository(_dbName);
                    repo.UserId = this.UserId;

                    if (patient == null)
                    {
                        patient = new MEPatient(this.UserId, pd.RecordCreatedOn)
                        {
                            FirstName = pd.FirstName,
                            LastName = pd.LastName,
                            MiddleName = pd.MiddleName,
                            Suffix = pd.Suffix,
                            PreferredName = pd.PreferredName,
                            Gender = pd.Gender,
                            DOB = pd.DOB,
                            Background = pd.Background,
                            ClinicalBackground = pd.ClinicalBackground,
                            Version = request.Version,
                            TTLDate = null,
                            DeleteFlag = false,
                            DataSource = Helper.TrimAndLimit(pd.DataSource, 50),
                            Status = (Status)pd.StatusId,
                            StatusDataSource  = Helper.TrimAndLimit(pd.StatusDataSource, 50),
                            Protected = pd.Protected,
                            Deceased = (Deceased)pd.DeceasedId,
                            LastUpdatedOn = pd.LastUpdatedOn,
                            ExternalRecordId = pd.ExternalRecordId,
                            Prefix = pd.Prefix
                        };
                        if(!string.IsNullOrEmpty(pd.ReasonId))
                        {
                            patient.ReasonId = ObjectId.Parse(pd.ReasonId);
                        }
                        if (!string.IsNullOrEmpty(pd.MaritalStatusId))
                        {
                            patient.MaritalStatusId = ObjectId.Parse(pd.MaritalStatusId);
                        }
                        ctx.Patients.Collection.Insert(patient);

                        List<SearchFieldData> data = new List<SearchFieldData>();
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.FN, Value = patient.FirstName });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.LN, Value = patient.LastName });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.G, Value = patient.Gender.ToUpper() });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.DOB, Value = patient.DOB });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.MN, Value = patient.MiddleName });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.SFX, Value = patient.Suffix });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.PN, Value = patient.PreferredName });
                        data.Add(new SearchFieldData { Active = true, FieldName = Constants.PCM }); //value left null on purpose

                        PutCohortPatientViewDataRequest cohortPatientRequest = new PutCohortPatientViewDataRequest
                        {
                            PatientID = patient.Id.ToString(),
                            LastName = patient.LastName,
                            SearchFields = data,
                            Version = patient.Version,
                            Context = request.Context,
                            ContractNumber = request.ContractNumber
                        };
                        repo.Insert(cohortPatientRequest);
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("A patient by firstname: {0}, lastname: {1} and DOB: {2} already exists.", patient.FirstName, patient.LastName, patient.DOB ));
                    }

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.Patient.ToString(), 
                                            patient.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            request.ContractNumber);

                    return new PutPatientDataResponse
                    {
                        Id = patient.Id.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string formatSystem(string p)
        {
            var val = p;
            if (!string.IsNullOrEmpty(p) && p.Length > 50)
            {
                val = p.Substring(0, 50);
            }
            return val;
        }

        public object InsertAll(List<object> entities)
        {
            BulkInsertResult result = new BulkInsertResult();
            List<string> insertedIds = new List<string>();
            List<string> errorMessages = new List<string>();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var bulk = ctx.Patients.Collection.InitializeUnorderedBulkOperation();
                    foreach (PatientData pd in entities)
                    {
                        MEPatient meP = new MEPatient(this.UserId, pd.RecordCreatedOn)
                        {
                            //Id = ObjectId.Parse("561ea745d43325133cf09a6d"),
                            FirstName = pd.FirstName,
                            LastName = pd.LastName,
                            MiddleName = pd.MiddleName,
                            Suffix = pd.Suffix,
                            PreferredName = pd.PreferredName,
                            Gender = pd.Gender,
                            DOB = pd.DOB,
                            Background = pd.Background,
                            ClinicalBackground = pd.ClinicalBackground,
                            TTLDate = null,
                            DeleteFlag = false,
                            DataSource = Helper.TrimAndLimit(pd.DataSource, 50),
                            Status = (Status)pd.StatusId,
                            StatusDataSource = Helper.TrimAndLimit(pd.StatusDataSource, 50),
                            Protected = pd.Protected,
                            Deceased = (Deceased)pd.DeceasedId,
                            FullSSN = pd.FullSSN,
                            LastFourSSN = pd.LastFourSSN,
                            LastUpdatedOn = pd.LastUpdatedOn,
                            ExternalRecordId = pd.ExternalRecordId,
                            Prefix = pd.Prefix
                        };
                        if (!string.IsNullOrEmpty(pd.ReasonId))
                        {
                            meP.ReasonId = ObjectId.Parse(pd.ReasonId);
                        }
                        if (!string.IsNullOrEmpty(pd.MaritalStatusId))
                        {
                            meP.MaritalStatusId = ObjectId.Parse(pd.MaritalStatusId);
                        }
                        if (!string.IsNullOrEmpty(pd.UpdatedByProperty))
                        {
                            meP.UpdatedBy = ObjectId.Parse(pd.UpdatedByProperty);
                        }
                        bulk.Insert(meP.ToBsonDocument());
                        insertedIds.Add(meP.Id.ToString());
                    }
                    BulkWriteResult bwr = bulk.Execute();
                }
                // TODO: Auditing.
            }
            catch (BulkWriteException bwEx)
            {
                // Get the error messages for the ones that failed.
                foreach (BulkWriteError er in bwEx.WriteErrors)
                {
                    errorMessages.Add(er.Message);
                }
            }
            catch (Exception ex)
            {
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            result.ProcessedIds = insertedIds;
            result.ErrorMessages = errorMessages;
            return result;
        }

        public void Delete(object entity)
        {
            DeletePatientDataRequest request = (DeletePatientDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var patientQuery = MB.Query<MEPatient>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var patientBuilder = new List<MB.UpdateBuilder>();
                    patientBuilder.Add(MB.Update.Set(MEPatient.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    patientBuilder.Add(MB.Update.Set(MEPatient.DeleteFlagProperty, true));
                    patientBuilder.Add(MB.Update.Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow));
                    patientBuilder.Add(MB.Update.Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate patientUpdate = MB.Update.Combine(patientBuilder);
                    ctx.Patients.Collection.Update(patientQuery, patientUpdate);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Patient.ToString(),
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
            PatientData patientData = null;
            var patient = FindByID(entityID, string.Empty);
            if (patient != null)
            {
                patientData = (PatientData)patient;
            }
            return patientData;
        }

        public object FindByNameDOB(string firstName, string lastName, string dob)
        {
            PatientData patientData = null;
            using (PatientMongoContext context = new PatientMongoContext(_dbName))
            {
                MEPatient mePatient = null;
                string searchQuery = string.Empty;
                searchQuery = string.Format("{0} : /^{1}$/i, {2} : /^{3}$/i, {4} : '{5}', {6} : false, {7} : null",
                    MEPatient.FirstNameProperty,firstName,
                    MEPatient.LastNameProperty, lastName,
                    MEPatient.DOBProperty, dob,
                    MEPatient.DeleteFlagProperty,
                    MEPatient.TTLDateProperty);

                var jsonQuery = "{ ";
                jsonQuery += searchQuery;
                jsonQuery += " }";
                QueryDocument query = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
                mePatient = context.Patients.Collection.FindOneAs<MEPatient>(query);

                if (mePatient != null)
                {
                    patientData = new PatientData
                    {
                        Id = mePatient.Id.ToString(),
                        DOB = CommonFormatter.FormatDateOfBirth(mePatient.DOB),
                        FirstName = mePatient.FirstName,
                        Gender = mePatient.Gender,
                        LastName = mePatient.LastName,
                        PreferredName = mePatient.PreferredName,
                        MiddleName = mePatient.MiddleName,
                        Suffix = mePatient.Suffix,
                        PriorityData = (int)mePatient.Priority,
                        DisplayPatientSystemId = mePatient.DisplayPatientSystemId.ToString(),
                        Background = mePatient.Background,
                        ClinicalBackground = mePatient.ClinicalBackground,
                        LastFourSSN = mePatient.LastFourSSN,
                        DataSource = formatSystem(mePatient.DataSource),
                        StatusDataSource = mePatient.StatusDataSource,
                        ReasonId = mePatient.ReasonId == null ? null : mePatient.ReasonId.ToString(),
                        MaritalStatusId = mePatient.MaritalStatusId == null ? null : mePatient.MaritalStatusId.ToString(),
                        Protected = mePatient.Protected,
                        DeceasedId = (int)mePatient.Deceased,
                        StatusId = (int)mePatient.Status,
                        ExternalRecordId = mePatient.ExternalRecordId,
                        Prefix = mePatient.Prefix
                    };                    
                }
            }
            return patientData;
        }

        public object FindByID(string entityId, string userId)
        {
            PatientData patientData = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEPatient.IdProperty, ObjectId.Parse(entityId)),
                                Query.EQ(MEPatient.DeleteFlagProperty, false),
                                Query.EQ(MEPatient.TTLDateProperty, BsonNull.Value));

                var mePatient = ctx.Patients.Collection.Find(query).FirstOrDefault();
                if (mePatient != null)
                {
                    patientData = new PatientData
                    {
                        Id = mePatient.Id.ToString(),
                        DOB = CommonFormatter.FormatDateOfBirth(mePatient.DOB),
                        FirstName = mePatient.FirstName,
                        Gender = mePatient.Gender,
                        LastName = mePatient.LastName,
                        PreferredName = mePatient.PreferredName,
                        MiddleName = mePatient.MiddleName,
                        Suffix = mePatient.Suffix,
                        PriorityData = (int)mePatient.Priority,
                        DisplayPatientSystemId = mePatient.DisplayPatientSystemId.ToString(),
                        Background = mePatient.Background,
                        ClinicalBackground = mePatient.ClinicalBackground,
                        LastFourSSN = mePatient.LastFourSSN,
                        DataSource = formatSystem(mePatient.DataSource),
                        StatusDataSource = mePatient.StatusDataSource,
                        ReasonId = mePatient.ReasonId == null ? null : mePatient.ReasonId.ToString(),
                        MaritalStatusId = mePatient.MaritalStatusId == null ? null : mePatient.MaritalStatusId.ToString(),
                        Protected = mePatient.Protected,
                        DeceasedId = (int)mePatient.Deceased,
                        StatusId = (int)mePatient.Status,
                        ExternalRecordId = mePatient.ExternalRecordId,
                        Prefix = mePatient.Prefix
                    };
                    if (!string.IsNullOrEmpty(userId))
                    {
                        patientData.Flagged = GetFlaggedStatus(entityId, userId);
                    }
                }
            }
            return patientData;
        }

        public object FindDuplicatePatient(PutUpdatePatientDataRequest request)
        {
            MEPatient mePatient = null;
            try
            {
                if (request.PatientData != null)
                {
                    using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                    {
                        string searchQuery = string.Empty;
                        if (request.PatientData.DOB == null)
                        {
                            searchQuery = string.Format("{0} : /^{1}$/i, {2} : /^{3}$/i, {4} : null, {5} : false, {6} : null", MEPatient.FirstNameProperty, request.PatientData.FirstName,
                              MEPatient.LastNameProperty, request.PatientData.LastName,
                              MEPatient.DOBProperty,
                              MEPatient.DeleteFlagProperty,
                              MEPatient.TTLDateProperty);
                        }
                        else
                        {
                            searchQuery = string.Format("{0} : /^{1}$/i, {2} : /^{3}$/i, {4} : '{5}', {6} : false, {7} : null", MEPatient.FirstNameProperty, request.PatientData.FirstName,
                              MEPatient.LastNameProperty, request.PatientData.LastName,
                              MEPatient.DOBProperty, request.PatientData.DOB,
                              MEPatient.DeleteFlagProperty,
                              MEPatient.TTLDateProperty);
                        }

                        string jsonQuery = "{ ";
                        jsonQuery += searchQuery;
                        jsonQuery += " }";
                        QueryDocument query = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
                        mePatient = ctx.Patients.Collection.FindOneAs<MEPatient>(query);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mePatient;
        }

        public object GetSSN(string patientId)
        {
            string ssn = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEPatient.IdProperty, ObjectId.Parse(patientId)),
                                Query.EQ(MEPatient.DeleteFlagProperty, false),
                                Query.EQ(MEPatient.TTLDateProperty, BsonNull.Value));

                var mePatient = ctx.Patients.Collection.Find(query).SetFields(MEPatient.FullSSNProperty).FirstOrDefault();
                if (mePatient != null)
                {
                    DataProtector protector = new DataProtector(Services.DataProtector.Store.USE_SIMPLE_STORE);
                    if(!string.IsNullOrEmpty(mePatient.FullSSN))
                        ssn = protector.Decrypt(mePatient.FullSSN);  
                }
            }
            return ssn;
        }

        private bool GetFlaggedStatus(string patientId, string userId)
        {
            bool result = false;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                var patientUsr = FindPatientUser(patientId, userId, ctx);

                if (patientUsr != null)
                {
                    result = patientUsr.Flagged;
                }
            }
            return result;
        }

        private static MEPatientUser FindPatientUser(string patientId, string userId, PatientMongoContext ctx)
        {
            var findQ = MB.Query.And(
                MB.Query<MEPatientUser>.EQ(b => b.PatientId, ObjectId.Parse(patientId)),
                MB.Query<MEPatientUser>.EQ(b => b.ContactId, ObjectId.Parse(userId)),
                MB.Query<MEPatientUser>.EQ(b => b.DeleteFlag, false)
            );

            var patientUsr = ctx.PatientUsers.Collection.Find(findQ).FirstOrDefault();
            return patientUsr;
        }

        public List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public List<PatientData> Select(List<string> patientIds)
        {
            List<PatientData> pList = null;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(MEPatient.IdProperty, new BsonArray(Helper.ConvertToObjectIdList(patientIds))));
                    queries.Add(Query.EQ(MEPatient.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatient.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatient> mePatients = ctx.Patients.Collection.Find(mQuery).ToList();
                    if(mePatients != null && mePatients.Count > 0)
                    {
                        pList = new List<PatientData>();
                        foreach (MEPatient meP in mePatients)
                        {
                            PatientData data = new PatientData
                            {
                                Id = meP.Id.ToString(),
                                PreferredName = meP.PreferredName,
                                DOB = meP.DOB,
                                FirstName = meP.FirstName,
                                Gender = meP.Gender,
                                LastName = meP.LastName,
                                MiddleName = meP.MiddleName,
                                Suffix = meP.Suffix,
                                Version = meP.Version,
                                PriorityData = (int) meP.Priority,
                                DisplayPatientSystemId = meP.DisplayPatientSystemId.ToString(),
                                Background = meP.Background,
                                ClinicalBackground = meP.ClinicalBackground,
                                LastFourSSN = meP.LastFourSSN,
                                DataSource = formatSystem(meP.DataSource),
                                StatusDataSource = meP.StatusDataSource,
                                ReasonId = meP.ReasonId == null ? null : meP.ReasonId.ToString(),
                                StatusId = (int)meP.Status,
                                MaritalStatusId = meP.MaritalStatusId == null ? null : meP.MaritalStatusId.ToString(),
                                Protected = meP.Protected,
                                DeceasedId = (int)meP.Deceased,
                                ExternalRecordId = meP.ExternalRecordId,
                                Prefix = meP.Prefix
                            };
                            pList.Add(data);
                        }
                    }   
                }
                return pList;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> SelectAll()
        {
            List<PatientData> patients = null;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatient.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatient.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatient> mePatients = ctx.Patients.Collection.Find(mQuery).ToList();
                    if (mePatients != null && mePatients.Count > 0)
                    {
                        patients = new List<PatientData>();
                        foreach (MEPatient meP in mePatients)
                        {
                            PatientData data = new PatientData
                            {
                                Id = meP.Id.ToString(),
                            };
                            patients.Add(data);
                        }
                    }
                }
                return patients;
            }
            catch (Exception) { throw; }
        }

        public PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), MB.SortBy.Null,
                                                new MB.UpdateBuilder()
                                                .Set(MEPatient.PriorityProperty, (PriorityData)request.Priority)
                                                .Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow)
                                                .Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                }

                AuditHelper.LogDataAudit(this.UserId, 
                                        MongoCollectionName.Patient.ToString(), 
                                        request.PatientId, 
                                        Common.DataAuditType.Update, 
                                        request.ContractNumber);

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
            response.Success = false;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {

                    var patientUsr = FindPatientUser(request.PatientId, request.UserId, ctx);

                    if (patientUsr == null)
                    {
                        MEPatientUser pu = new MEPatientUser(this.UserId)
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            ContactId = ObjectId.Parse(request.UserId),
                            Flagged = Convert.ToBoolean(request.Flagged),
                            Version = 1,
                            DeleteFlag = false
                            //,LastUpdatedOn = System.DateTime.UtcNow,
                            //UpdatedBy = ObjectId.Parse(this.UserId)
                        };
                        ctx.PatientUsers.Collection.Insert(pu);

                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientUser.ToString(), 
                                                pu.Id.ToString(), 
                                                Common.DataAuditType.Insert, 
                                                request.ContractNumber);
                    }
                    else
                    {

                        var pUQuery = new QueryDocument(MEPatientUser.IdProperty, patientUsr.Id);
                        
                        MB.UpdateBuilder updt = new MB.UpdateBuilder()
                            .Set(MEPatientUser.FlaggedProperty, Convert.ToBoolean(request.Flagged))
                            .Set(MEPatientUser.LastUpdatedOnProperty, DateTime.UtcNow)
                            .Set(MEPatientUser.UpdatedByProperty, ObjectId.Parse(request.UserId));
                        var pt = ctx.PatientUsers.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientUser.ToString(), 
                                                patientUsr.Id.ToString(), 
                                                Common.DataAuditType.Update, 
                                                request.ContractNumber);
                    }
                    response.Success = true;
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
                if (request.PatientData.PriorityData == null)
                    throw new ArgumentException("Priority is missing from the DataDomain request.");

                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEPatient.IdProperty, ObjectId.Parse(request.PatientData.Id));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.PatientData.FirstName != null)
                    {
                        if (request.PatientData.FirstName == "\"\"" || (request.PatientData.FirstName == "\'\'"))
                            updt.Set(MEPatient.FirstNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.FirstNameProperty, request.PatientData.FirstName);
                    }
                    if (request.PatientData.LastName != null)
                    {
                        if (request.PatientData.LastName == "\"\"" || (request.PatientData.LastName == "\'\'"))
                            updt.Set(MEPatient.LastNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.LastNameProperty, request.PatientData.LastName);
                    }
                    if (request.PatientData.MiddleName != null)
                    {
                        if (request.PatientData.MiddleName == "\"\"" || (request.PatientData.MiddleName == "\'\'"))
                            updt.Set(MEPatient.MiddleNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.MiddleNameProperty, request.PatientData.MiddleName);
                    }
                    if (request.PatientData.Suffix != null)
                    {
                        if (request.PatientData.Suffix == "\"\"" || (request.PatientData.Suffix == "\'\'"))
                            updt.Set(MEPatient.SuffixProperty, string.Empty);
                        else
                            updt.Set(MEPatient.SuffixProperty, request.PatientData.Suffix);
                    }

                    if (request.PatientData.Prefix != null)
                    {
                        if (request.PatientData.Prefix == "\"\"" || (request.PatientData.Prefix == "\'\'"))
                            updt.Set(MEPatient.PrefixProperty, string.Empty);
                        else
                            updt.Set(MEPatient.PrefixProperty, Helper.TrimAndLimit(request.PatientData.Prefix, 20));
                    }

                    if (request.PatientData.DataSource != null)
                    {
                        if (request.PatientData.DataSource == "\"\"" || (request.PatientData.DataSource == "\'\'"))
                            updt.Set(MEPatient.DataSourceProperty, string.Empty);
                        else
                            updt.Set(MEPatient.DataSourceProperty, Helper.TrimAndLimit(request.PatientData.DataSource, 50));
                    }

                    if (request.PatientData.PreferredName != null)
                    {
                        if (request.PatientData.PreferredName == "\"\"" || (request.PatientData.PreferredName == "\'\'"))
                            updt.Set(MEPatient.PreferredNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.PreferredNameProperty, request.PatientData.PreferredName);
                    }
                    if (request.PatientData.Gender != null)
                    {
                        if (request.PatientData.Gender == "\"\"" || (request.PatientData.Gender == "\'\'"))
                            updt.Set(MEPatient.GenderProperty, string.Empty);
                        else
                            updt.Set(MEPatient.GenderProperty, request.PatientData.Gender);
                    }
                    if (request.PatientData.DOB != null)
                    {
                        if (request.PatientData.DOB == "\"\"" || (request.PatientData.DOB == "\'\'"))
                            updt.Set(MEPatient.DOBProperty, string.Empty);
                        else
                            updt.Set(MEPatient.DOBProperty, request.PatientData.DOB);
                    }

                    if (request.PatientData.DisplayPatientSystemId != null)
                    {
                        if (ObjectId.Parse(request.PatientData.DisplayPatientSystemId) != null)
                            updt.Set(MEPatient.DisplayPatientSystemIdProperty, ObjectId.Parse(request.PatientData.DisplayPatientSystemId));
                    }

                    if (request.PatientData.FullSSN != null)
                    {
                        string fullSSN = request.PatientData.FullSSN.Trim().Replace("-", string.Empty);
                        if (fullSSN == "\"\"" || (fullSSN == "\'\'"))
                        {
                            updt.Set(MEPatient.FullSSNProperty, string.Empty);
                            updt.Set(MEPatient.LastFourSSNProperty, BsonNull.Value);
                        }
                        else
                        {
                            int SSNinInt;
                            if (int.TryParse(fullSSN, out SSNinInt))
                            {
                                if (fullSSN.Length == 9)
                                {
                                    // Save the last 4 digits in LastFourSSN field.
                                    string lastFourSSN = fullSSN.Substring(5, 4);
                                    updt.Set(MEPatient.LastFourSSNProperty, lastFourSSN);
                                    // Save the full SSN in the encrypted form.
                                    DataProtector protector = new DataProtector(Services.DataProtector.Store.USE_SIMPLE_STORE);
                                    string encryptedSSN = protector.Encrypt(fullSSN);
                                    updt.Set(MEPatient.FullSSNProperty, encryptedSSN);
                                }
                                else
                                {
                                    throw new ArgumentException("Incorrect SSN length - It does not contain all 9 digits.");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Incorrect SSN format- It does not contain digits.");
                            }
                        }
                    }

                    updt.Set(MEPatient.TTLDateProperty, BsonNull.Value);
                    updt.Set(MEPatient.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEPatient.PriorityProperty, request.PatientData.PriorityData);
                    if (request.PatientData.Background != null)
                    {
                        updt.Set(MEPatient.BackgroundProperty, request.PatientData.Background);
                    }
                    if (request.PatientData.ClinicalBackground != null)
                    {
                        updt.Set(MEPatient.ClinicalBackgroundProperty, request.PatientData.ClinicalBackground);
                    }
                    if (!string.IsNullOrEmpty(request.PatientData.ReasonId))
                    {
                        updt.Set(MEPatient.ReasonProperty, ObjectId.Parse(request.PatientData.ReasonId));
                    }
                    else
                    {
                        updt.Set(MEPatient.ReasonProperty, BsonNull.Value);
                    }
                    if (!string.IsNullOrEmpty(request.PatientData.MaritalStatusId))
                    {
                        updt.Set(MEPatient.MaritalStatusProperty, ObjectId.Parse(request.PatientData.MaritalStatusId));
                    }
                    else
                    {
                        updt.Set(MEPatient.MaritalStatusProperty, BsonNull.Value);
                    }
                    updt.Set(MEPatient.ProtectedProperty, request.PatientData.Protected);
                    updt.Set(MEPatient.DeceasedProperty, request.PatientData.DeceasedId);
                    updt.Set(MEPatient.StatusProperty, request.PatientData.StatusId);
                    if (request.PatientData.StatusDataSource != null)
                    {
                        updt.Set(MEPatient.StatusDataSourceProperty, Helper.TrimAndLimit(request.PatientData.StatusDataSource, 50));
                    }
                    updt.Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEPatient.VersionProperty, request.Version);

                    var pt = ctx.Patients.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.Patient.ToString(),
                                            request.PatientData.Id.ToString(), 
                                            Common.DataAuditType.Update, 
                                            request.ContractNumber);

                    response.Id = request.PatientData.Id;

                    // save to cohortuser collection
                    var findQ = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.PatientData.Id));
                    MECohortPatientView cPV = ctx.CohortPatientViews.Collection.Find(findQ).FirstOrDefault();
                    if (cPV == null)
                    {
                        //If the Update call is for an Initialized Patient, CohortPatientView doesnot have a record for this patient, hence insert one.
                        insertCohortPatientView(request);
                    }
                    else 
                    {
                        cPV.SearchFields.ForEach(s => UpdateProperty(request, s));
                        List<SearchField> sfs = cPV.SearchFields.ToList<SearchField>();
                        ctx.CohortPatientViews.Collection.Update(findQ, MB.Update.SetWrapped<List<SearchField>>(MECohortPatientView.SearchFieldsProperty, sfs).Set(MECohortPatientView.LastNameProperty, request.PatientData.LastName));

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.CohortPatientView.ToString(),
                                                cPV.Id.ToString(),
                                                DataAuditType.Update,
                                                request.ContractNumber);
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void insertCohortPatientView(PutUpdatePatientDataRequest request) 
        {
            MongoCohortPatientViewRepository repo = new MongoCohortPatientViewRepository(_dbName);
            repo.UserId = request.UserId;
            List<SearchFieldData> data = new List<SearchFieldData>();
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.FN, Value = request.PatientData.FirstName });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.LN, Value = request.PatientData.LastName });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.G, Value = request.PatientData.Gender == null ? null : request.PatientData.Gender.ToUpper()});
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.DOB, Value = request.PatientData.DOB });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.MN, Value = request.PatientData.MiddleName });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.SFX, Value = request.PatientData.Suffix });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.PN, Value = request.PatientData.PreferredName });
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.PCM }); //value left null on purpose
            PutCohortPatientViewDataRequest cpVRequest = new PutCohortPatientViewDataRequest
            {
                Context = request.Context,
                ContractNumber = request.ContractNumber,
                LastName = request.PatientData.LastName,
                PatientID = request.PatientData.Id,
                SearchFields = data,
                UserId = request.UserId,
                Version = request.Version
            };
            repo.Insert(cpVRequest);
        }

        private void UpdateProperty(PutUpdatePatientDataRequest request, SearchField s)
        {
            if (s.FieldName.Equals(Constants.PN))
            {
                if (request.PatientData.PreferredName != null)
                {
                    if (request.PatientData.PreferredName == "\"\"" || (request.PatientData.PreferredName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.PreferredName;
                }
            }

            if (s.FieldName.Equals(Constants.SFX))
            {
                if (request.PatientData.Suffix != null)
                {
                    if (request.PatientData.Suffix == "\"\"" || (request.PatientData.Suffix == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.Suffix;
                }
            }

            if (s.FieldName.Equals(Constants.MN))
            {
                if (request.PatientData.MiddleName != null)
                {
                    if (request.PatientData.MiddleName == "\"\"" || (request.PatientData.MiddleName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.MiddleName;
                }
            }

            if (s.FieldName.Equals(Constants.DOB))
            {
                if (request.PatientData.DOB != null)
                {
                    if (request.PatientData.DOB == "\"\"" || (request.PatientData.DOB == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.DOB;
                }
            }

            if (s.FieldName.Equals(Constants.G))
            {
                if (request.PatientData.Gender != null)
                {
                    if (request.PatientData.Gender == "\"\"" || (request.PatientData.Gender == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.Gender;
                }
            }

            if (s.FieldName.Equals(Constants.LN))
            {
                if (request.PatientData.LastName != null)
                {
                    if (request.PatientData.LastName == "\"\"" || (request.PatientData.LastName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.LastName;
                }
            }
            if (s.FieldName.Equals(Constants.FN))
            {
                if (request.PatientData.FirstName != null)
                {
                    if (request.PatientData.FirstName == "\"\"" || (request.PatientData.FirstName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PatientData.FirstName;
                }
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }


        public List<PatientUserData> FindPatientUsersByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public CohortPatientViewData FindCohortPatientViewByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientDataRequest request = (UndoDeletePatientDataRequest)entity;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatient>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatient.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEPatient.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.Patients.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Patient.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public object Initialize(object newEntity)
        {
            PutInitializePatientDataRequest request = (PutInitializePatientDataRequest)newEntity;
            PatientData patientData = null;
            try
            {
                MEPatient meP = new MEPatient(this.UserId, null)
                {
                    Id = ObjectId.GenerateNewId(),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays)
                };

                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    ctx.Patients.Collection.Insert(meP);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Patient.ToString(),
                                            meP.Id.ToString(),
                                            Common.DataAuditType.Insert,
                                            request.ContractNumber);

                    patientData = new PatientData
                    {
                        Id = meP.Id.ToString()
                    };
                }
                return patientData;
            }
            catch (Exception) { throw; }
        }


        public PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request)
        {
            PutPatientSystemIdDataResponse response = new PutPatientSystemIdDataResponse();
            response.Success = false;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    if (!string.IsNullOrEmpty(request.PatientSystemId))
                    {
                        FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), MB.SortBy.Null,
                            new MB.UpdateBuilder()
                            .Set(MEPatient.DisplayPatientSystemIdProperty, ObjectId.Parse(request.PatientSystemId))
                            .Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId))
                            .Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow));

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.Patient.ToString(),
                                                request.PatientId,
                                                Common.DataAuditType.Update,
                                                request.ContractNumber);

                        response.Success = true;
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool SyncPatient(SyncPatientInfoDataRequest request)
        {
            var response = new SyncPatientInfoDataResponse();
            try
            {
                using (var ctx = new PatientMongoContext(_dbName))
                {
                    var data = request.PatientInfo;
                    var queries = new List<IMongoQuery>
                    {
                        MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)),
                        MB.Query.EQ(MEPatient.DeleteFlagProperty, false)
                    };

                    var query = MB.Query.And(queries);
                    var mc = ctx.Patients.Collection.Find(query).FirstOrDefault();
                    if (mc != null)
                    {
                        var uv = new List<MB.UpdateBuilder>();
                        if(string.IsNullOrEmpty(data.FirstName))
                            uv.Add(MB.Update.Set(MEPatient.FirstNameProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEPatient.FirstNameProperty, data.FirstName));
                        if(string.IsNullOrEmpty(data.LastName))
                            uv.Add(MB.Update.Set(MEPatient.LastNameProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.LastNameProperty, data.LastName));
                        if(string.IsNullOrEmpty(data.MiddleName))
                            uv.Add(MB.Update.Set(MEPatient.MiddleNameProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.MiddleNameProperty, data.MiddleName));
                        if(string.IsNullOrEmpty(data.PreferredName))
                            uv.Add(MB.Update.Set(MEPatient.PreferredNameProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.PreferredNameProperty, data.PreferredName));
                        if(string.IsNullOrEmpty(data.Gender))
                            uv.Add(MB.Update.Set(MEPatient.GenderProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.GenderProperty, data.Gender));
                        if(string.IsNullOrEmpty(data.Suffix))
                            uv.Add(MB.Update.Set(MEPatient.SuffixProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.SuffixProperty, data.Suffix));
                        if(string.IsNullOrEmpty(data.Prefix))
                            uv.Add(MB.Update.Set(MEPatient.PrefixProperty, BsonNull.Value));
                        else 
                            uv.Add(MB.Update.Set(MEPatient.PrefixProperty, data.Prefix));
                        uv.Add(MB.Update.Set(MEPatient.DeceasedProperty, data.DeceasedId));
                        //uv.Add(MB.Update.Set(MEPatient.StatusProperty, data.StatusId));
                        uv.Add(MB.Update.Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow));
                        uv.Add(MB.Update.Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId)));


                        var update = MB.Update.Combine(uv);

                        ctx.Patients.Collection.Update(query, update);

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.Patient.ToString(),
                                                request.PatientId,
                                                DataAuditType.Update,
                                                request.ContractNumber);

                        response.IsSuccessful = true;

                        // save to cohortuser collection
                        var findQ = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.PatientId));
                        var cPV = ctx.CohortPatientViews.Collection.Find(findQ).FirstOrDefault();

                        var putUpdaterequest = new PutUpdatePatientDataRequest
                        {
                            ContractNumber = request.ContractNumber,
                            Context = request.Context,
                            PatientData = new PatientData
                            {
                                FirstName = request.PatientInfo.FirstName,
                                LastName = request.PatientInfo.LastName,
                                MiddleName = request.PatientInfo.MiddleName,
                                PreferredName = request.PatientInfo.PreferredName,
                                Gender = request.PatientInfo.Gender,
                                Suffix = request.PatientInfo.Suffix,
                                Prefix = request.PatientInfo.Prefix,
                                Id = request.PatientId,
                                //StatusId = request.PatientInfo.StatusId,
                                DeceasedId = request.PatientInfo.DeceasedId
                            }
                        };

                        cPV.SearchFields.ForEach(s => UpdateProperty(putUpdaterequest, s));
                        var sfs = cPV.SearchFields.ToList<SearchField>();
                        ctx.CohortPatientViews.Collection.Update(findQ, MB.Update.SetWrapped<List<SearchField>>(MECohortPatientView.SearchFieldsProperty, sfs).Set(MECohortPatientView.LastNameProperty, request.PatientInfo.LastName));

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.CohortPatientView.ToString(),
                                                cPV.Id.ToString(),
                                                DataAuditType.Update,
                                                request.ContractNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                throw ex;
            }

            return response.IsSuccessful;
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
    }
}

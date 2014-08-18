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
                MEPatient patient = null;
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    //Does the patient exist?
                    IMongoQuery query = Query.And(
                                    Query.EQ(MEPatient.FirstNameProperty, request.FirstName),
                                    Query.EQ(MEPatient.LastNameProperty, request.LastName),
                                    Query.EQ(MEPatient.DOBProperty, request.DOB));

                    patient = ctx.Patients.Collection.FindOneAs<MEPatient>(query);
                    MongoCohortPatientViewRepository repo = new MongoCohortPatientViewRepository(_dbName);
                    repo.UserId = this.UserId;

                    if (patient == null)
                    {
                        patient = new MEPatient(this.UserId)
                        {
                            Id = ObjectId.GenerateNewId(),
                            FirstName = request.FirstName,
                            LastName = request.LastName,
                            MiddleName = request.MiddleName,
                            Suffix = request.Suffix,
                            PreferredName = request.PreferredName,
                            Gender = request.Gender,
                            DOB = request.DOB,
                            Background = request.Background,
                            Version = request.Version,
                            UpdatedBy = ObjectId.Parse(this.UserId),
                            TTLDate = null,
                            DeleteFlag = false,
                            LastUpdatedOn = System.DateTime.UtcNow
                        };
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

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
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
                        LastFourSSN = mePatient.LastFourSSN
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
            if(request.PatientData != null)
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    IMongoQuery query = Query.And(
                                    Query.EQ(MEPatient.FirstNameProperty, request.PatientData.FirstName),
                                    Query.EQ(MEPatient.LastNameProperty, request.PatientData.LastName),
                                    Query.EQ(MEPatient.DOBProperty, request.PatientData.DOB),
                                    Query.EQ(MEPatient.DeleteFlagProperty, false),
                                    Query.EQ(MEPatient.TTLDateProperty, BsonNull.Value));

                    mePatient = ctx.Patients.Collection.FindOneAs<MEPatient>(query);
                }
            }
            return mePatient;
        }

        public object GetSSN(string patientId)
        {
            string ssn = string.Empty;
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

        public GetPatientsDataResponse Select(string[] patientIds)
        {
            GetPatientsDataResponse pdResponse = new GetPatientsDataResponse();
            try
            {
                BsonValue[] bsv = new BsonValue[patientIds.Length];
                if(patientIds != null && patientIds.Length > 0)
                {
                    for (int i = 0; i < patientIds.Length; i++)
                    {
                        bsv[i] = ObjectId.Parse(patientIds[i]);
                    }

                    var query = MB.Query.And(
                        MB.Query<MEPatientUser>.In(b => b.Id, bsv),
                        MB.Query<MEPatientUser>.EQ(b => b.DeleteFlag, false));

                    List<PatientData> pResp = null;
                    using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                    {
                        List<MEPatient> pr = ctx.Patients.Collection.Find(query).ToList();
                        if(pr != null && pr.Count > 0)
                        {
                            pResp = new List<PatientData>();
                            foreach (MEPatient mp in pr)
                            {
                                pResp.Add(new DTO.PatientData
                                {
                                    Id = mp.Id.ToString(),
                                    PreferredName = mp.PreferredName,
                                    DOB = mp.DOB,
                                    FirstName = mp.FirstName,
                                    Gender = mp.Gender,
                                    LastName = mp.LastName,
                                    MiddleName = mp.MiddleName,
                                    Suffix = mp.Suffix,
                                    Version = mp.Version,
                                    PriorityData = (int)mp.Priority,
                                    DisplayPatientSystemId = mp.DisplayPatientSystemId.ToString(),
                                    Background = mp.Background,
                                    LastFourSSN = mp.LastFourSSN
                                });
                            }
                        }
                        
                    }
                    pdResponse.Patients = pResp;      
                }

                return pdResponse;
            }
            catch (Exception)
            {
                throw;
            }
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

        public PutPatientBackgroundDataResponse UpdateBackground(PutPatientBackgroundDataRequest request)
        {
            PutPatientBackgroundDataResponse response = new PutPatientBackgroundDataResponse();
            response.Success = false;
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    string background = string.Empty;
                    if (!string.IsNullOrEmpty(request.Background))
                    {
                        background = request.Background;
                    }
                    
                    FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), MB.SortBy.Null,
                                                new MB.UpdateBuilder()
                                                .Set(MEPatient.BackgroundProperty, background)
                                                .Set(MEPatient.UpdatedByProperty, ObjectId.Parse(this.UserId))
                                                .Set(MEPatient.LastUpdatedOnProperty, DateTime.UtcNow));

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.Patient.ToString(), 
                                            request.PatientId, 
                                            Common.DataAuditType.Update, 
                                            request.ContractNumber);

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
                                    int lastFourSSN;
                                    if (int.TryParse(fullSSN.Substring(5, 4), out lastFourSSN))
                                    {
                                        updt.Set(MEPatient.LastFourSSNProperty, lastFourSSN);
                                    }

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
            data.Add(new SearchFieldData { Active = true, FieldName = Constants.G, Value = request.PatientData.Gender.ToUpper() });
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
                MEPatient meP = new MEPatient(this.UserId)
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
    }
}

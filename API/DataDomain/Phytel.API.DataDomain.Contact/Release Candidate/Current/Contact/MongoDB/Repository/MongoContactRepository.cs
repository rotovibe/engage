using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.MongoDB.Repository;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.Contact
{
    public class MongoContactRepository : IContactRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        public IAuditHelpers AuditHelpers { get; set; }

        static MongoContactRepository() 
        {

                #region Register ClassMap
                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MEContact)) == false)
                        BsonClassMap.RegisterClassMap<MEContact>();
                }
                catch { }

                try
                {
                if (BsonClassMap.IsClassMapRegistered(typeof(Address)) == false)
                    BsonClassMap.RegisterClassMap<Address>();
                }
                catch { }

                try
                {
                if (BsonClassMap.IsClassMapRegistered(typeof(CommMode)) == false)
                    BsonClassMap.RegisterClassMap<CommMode>();
                }
                catch { }

                try
                {
                if (BsonClassMap.IsClassMapRegistered(typeof(Email)) == false)
                    BsonClassMap.RegisterClassMap<Email>();
                }
                catch { }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Language)) == false)
                        BsonClassMap.RegisterClassMap<Language>();
                }
                catch { }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Phone)) == false)
                        BsonClassMap.RegisterClassMap<Phone>();
                }
                catch { }

                #endregion
        }

        public MongoContactRepository(string dbname)
        {
            _dbName = dbname;
            AppHostBase.Instance.Container.AutoWire(this);
        }

        /// <summary>
        /// Inserts a contact object.
        /// </summary>
        /// <param name="newEntity">PutContactDataRequest object</param>
        /// <returns>Id of the newly inserted contact.</returns>
        public object Insert(object newEntity)
        {
            string id = null;
            PutContactDataRequest request = newEntity as PutContactDataRequest;
            ContactData data = request.ContactData;
            MEContact meContact = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    if (data.PatientId != null)
                    {
                        queries.Add(MB.Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(data.PatientId)));
                        queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                        IMongoQuery query = MB.Query.And(queries);
                        MEContact mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();
                        if (mc != null)
                        {
                            throw new ApplicationException("A contact record already exists for the patient.");
                        }
                    }
                    meContact = new MEContact(this.UserId, data.CreatedOn)
                    {
                        Id = ObjectId.GenerateNewId(),
                        FirstName = data.FirstName,
                        LastName = data.LastName, 
                        PreferredName = data.PreferredName,
                        Gender = data.Gender,
                        ResourceId = data.UserId,
                        Version = request.Version,
                        LastUpdatedOn = DateTime.UtcNow,
                        UpdatedBy = ObjectId.Parse(this.UserId),
                        DeleteFlag = false
                    };
                    
                    //PatientId
                    if (data.PatientId != null)
                    {
                        meContact.PatientId = ObjectId.Parse(data.PatientId);
                    }
                    //Timezone
                    if (data.TimeZoneId != null)
                    {
                        meContact.TimeZoneId = ObjectId.Parse(data.TimeZoneId);
                    }
                    //Modes
                    if (data.Modes != null && data.Modes.Count > 0)
                    {
                        List<CommMode> commModes = new List<CommMode>();
                        foreach (CommModeData c in data.Modes)
                        {
                            commModes.Add(new CommMode { ModeId = ObjectId.Parse(c.ModeId), OptOut = c.OptOut, Preferred = c.Preferred });
                        }
                        meContact.Modes = commModes;
                    }

                    //Weekdays
                    if (data.WeekDays != null && data.WeekDays.Count > 0)
                    {
                        meContact.WeekDays = data.WeekDays;
                    }

                    //TimesOfDays
                    if (data.TimesOfDaysId != null && data.TimesOfDaysId.Count > 0)
                    {
                        List<ObjectId> ids = new List<ObjectId>();
                        foreach (string s in data.TimesOfDaysId)
                        {
                            ids.Add(ObjectId.Parse(s));
                        }
                        meContact.TimesOfDays = ids;
                    }

                    //Languages
                    if (data.Languages != null && data.Languages.Count > 0)
                    {
                        List<Language> languages = new List<Language>();
                        foreach (LanguageData c in data.Languages)
                        {
                            languages.Add(new Language { LookUpLanguageId = ObjectId.Parse(c.LookUpLanguageId), Preferred = c.Preferred });
                        }
                        meContact.Languages = languages;
                    }

                    //Addresses
                    if (data.Addresses != null && data.Addresses.Count > 0)
                    {
                        List<Address> meAddresses = new List<Address>();
                        List<AddressData> addressData = data.Addresses;
                        foreach (AddressData p in addressData)
                        {
                            Address me = new Address
                            {
                                Id = ObjectId.GenerateNewId(),
                                TypeId = ObjectId.Parse(p.TypeId),
                                Line1 = p.Line1,
                                Line2 = p.Line2,
                                Line3 = p.Line3,
                                City = p.City,
                                StateId = ObjectId.Parse(p.StateId),
                                PostalCode = p.PostalCode,
                                Preferred = p.Preferred,
                                OptOut = p.OptOut,
                                DeleteFlag = false
                            };
                            meAddresses.Add(me);
                        }
                        meContact.Addresses = meAddresses;
                    }

                    //Phones
                    if (data.Phones != null && data.Phones.Count > 0)
                    {
                        PhoneVisitor.GetContactPhones(data.Phones, ref meContact);
                    }

                    //Emails
                    if (data.Emails != null && data.Emails.Count > 0)
                    {
                        List<Email> meEmails = new List<Email>();
                        List<EmailData> emailData = data.Emails;
                        foreach (EmailData p in emailData)
                        {
                            Email me = new Email
                            {
                                Id = ObjectId.GenerateNewId(),
                                Text = p.Text,
                                Preferred = p.Preferred,
                                TypeId = ObjectId.Parse(p.TypeId),
                                OptOut = p.OptOut,
                                DeleteFlag = false
                            };
                            meEmails.Add(me);
                        }
                        meContact.Emails = meEmails;
                    }

                    ctx.Contacts.Collection.Insert(meContact);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.Contact.ToString(), 
                                            meContact.Id.ToString(), 
                                            DataAuditType.Insert, 
                                            request.ContractNumber);

                    id = meContact.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }

        public object InsertAll(List<object> entities)
        {
            BulkInsertResult result = new BulkInsertResult();
            List<string> insertedIds = new List<string>();
            List<string> errorMessages = new List<string>();
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    var bulk = ctx.Contacts.Collection.InitializeUnorderedBulkOperation();
                    foreach (ContactData data in entities)
                    {
                        MEContact meContact = new MEContact(this.UserId, data.CreatedOn)
                        {
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            PreferredName = data.PreferredName,
                            Gender = data.Gender,
                            ResourceId = data.UserId,
                            Version = 1.0,
                            LastUpdatedOn = DateTime.UtcNow,
                            UpdatedBy = ObjectId.Parse(this.UserId),
                            DeleteFlag = false
                        };

                        //PatientId
                        if (data.PatientId != null)
                        {
                            meContact.PatientId = ObjectId.Parse(data.PatientId);
                        }
                        //Timezone
                        if (data.TimeZoneId != null)
                        {
                            meContact.TimeZoneId = ObjectId.Parse(data.TimeZoneId);
                        }
                        //Modes
                        if (data.Modes != null && data.Modes.Count > 0)
                        {
                            List<CommMode> commModes = new List<CommMode>();
                            foreach (CommModeData c in data.Modes)
                            {
                                commModes.Add(new CommMode { ModeId = ObjectId.Parse(c.ModeId), OptOut = c.OptOut, Preferred = c.Preferred });
                            }
                            meContact.Modes = commModes;
                        }

                        //Weekdays
                        if (data.WeekDays != null && data.WeekDays.Count > 0)
                        {
                            meContact.WeekDays = data.WeekDays;
                        }

                        //TimesOfDays
                        if (data.TimesOfDaysId != null && data.TimesOfDaysId.Count > 0)
                        {
                            List<ObjectId> ids = new List<ObjectId>();
                            foreach (string s in data.TimesOfDaysId)
                            {
                                ids.Add(ObjectId.Parse(s));
                            }
                            meContact.TimesOfDays = ids;
                        }

                        //Languages
                        if (data.Languages != null && data.Languages.Count > 0)
                        {
                            List<Language> languages = new List<Language>();
                            foreach (LanguageData c in data.Languages)
                            {
                                languages.Add(new Language { LookUpLanguageId = ObjectId.Parse(c.LookUpLanguageId), Preferred = c.Preferred });
                            }
                            meContact.Languages = languages;
                        }

                        //Addresses
                        if (data.Addresses != null && data.Addresses.Count > 0)
                        {
                            List<Address> meAddresses = new List<Address>();
                            List<AddressData> addressData = data.Addresses;
                            foreach (AddressData p in addressData)
                            {
                                Address me = new Address
                                {
                                    Id = ObjectId.GenerateNewId(),
                                    TypeId = ObjectId.Parse(p.TypeId),
                                    Line1 = p.Line1,
                                    Line2 = p.Line2,
                                    Line3 = p.Line3,
                                    City = p.City,
                                    StateId = ObjectId.Parse(p.StateId),
                                    PostalCode = p.PostalCode,
                                    Preferred = p.Preferred,
                                    OptOut = p.OptOut,
                                    DeleteFlag = false
                                };
                                meAddresses.Add(me);
                            }
                            meContact.Addresses = meAddresses;
                        }

                        //Phones
                        if (data.Phones != null && data.Phones.Count > 0)
                        {
                            PhoneVisitor.GetContactPhones(data.Phones, ref meContact);
                        }

                        //Emails
                        if (data.Emails != null && data.Emails.Count > 0)
                        {
                            List<Email> meEmails = new List<Email>();
                            List<EmailData> emailData = data.Emails;
                            foreach (EmailData p in emailData)
                            {
                                Email me = new Email
                                {
                                    Id = ObjectId.GenerateNewId(),
                                    Text = p.Text,
                                    Preferred = p.Preferred,
                                    TypeId = ObjectId.Parse(p.TypeId),
                                    OptOut = p.OptOut,
                                    DeleteFlag = false
                                };
                                meEmails.Add(me);
                            }
                            meContact.Emails = meEmails;
                        }
                        bulk.Insert(meContact.ToBsonDocument());
                        insertedIds.Add(meContact.Id.ToString());
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
            DeleteContactByPatientIdDataRequest request = (DeleteContactByPatientIdDataRequest)entity;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    var query = MB.Query<MEContact>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEContact.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MEContact.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEContact.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.Contacts.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Contact.ToString(),
                                            request.Id.ToString(),
                                            DataAuditType.Delete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
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
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(MB.Query.EQ(MEContact.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = MB.Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData
                    {
                        Id = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        UserId = (string.IsNullOrEmpty(mc.ResourceId)) ? string.Empty : mc.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                        FirstName = mc.FirstName,
                        MiddleName = mc.MiddleName,
                        LastName = mc.LastName,
                        PreferredName = mc.PreferredName,
                        Gender = mc.Gender,
                        RecentsList = mc.RecentList != null ? mc.RecentList.ConvertAll(r => r.ToString()) : null
                    };
                }
            }
            return contactData;
        }

        public Tuple<string, IEnumerable<object>> Select(APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
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

        /// <summary>
        /// Updates a contact object in the database.
        /// </summary>
        /// <param name="entity">PutContactDataRequest object to be updated.</param>
        /// <returns>Boolean field indicating if the update was successful or not.</returns>
        public object Update(object entity)
        {
            PutUpdateContactDataResponse response = new PutUpdateContactDataResponse();
            response.SuccessData = false;
            List<CleanupIdData> updatedPhones = new List<CleanupIdData>();
            List<CleanupIdData> updatedEmails = new List<CleanupIdData>();
            List<CleanupIdData> updatedAddresses = new List<CleanupIdData>();
            try
            {
                PutUpdateContactDataRequest request = entity as PutUpdateContactDataRequest;
                ContactData data = request.ContactData;
                if (data.Id == null)
                    throw new ArgumentException("ContactId is missing from the DataDomain request.");

                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(MB.Query.EQ(MEContact.IdProperty, ObjectId.Parse(data.Id)));
                    queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery query = MB.Query.And(queries);
                    MEContact mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();
                    if(mc != null)
                    {
                        var uv = new List<MB.UpdateBuilder>();

                        #region Modes
                        if (data.Modes != null)
                        {
                            List<CommMode> meModes = null;
                            if (data.Modes.Count != 0)
                            {
                                meModes = new List<CommMode>();
                                List<CommModeData> modeData = data.Modes;
                                foreach (CommModeData m in modeData)
                                {
                                    CommMode meM = new CommMode
                                    {
                                        ModeId = ObjectId.Parse(m.ModeId),
                                        OptOut = m.OptOut,
                                        Preferred = m.Preferred
                                    };
                                    meModes.Add(meM);
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<CommMode>>(MEContact.ModesProperty, meModes));

                        } 
                        #endregion

                        #region WeekDays
                        if (data.WeekDays != null)
                        {
                            List<int> weekDays = null;
                            if (data.WeekDays.Count != 0)
                            {
                                weekDays = new List<int>();
                                weekDays = data.WeekDays;
                            }
                            uv.Add(MB.Update.SetWrapped<List<int>>(MEContact.WeekDaysProperty, weekDays));

                        } 
                        #endregion

                        #region TimesOfDays
                        if (data.TimesOfDaysId != null)
                        {
                            List<ObjectId> timesOfDays = null;
                            // if nothing is selected
                            if (data.TimesOfDaysId.Count != 0)
                            {
                                timesOfDays = new List<ObjectId>();
                                List<string> times = data.TimesOfDaysId;
                                foreach (string s in times)
                                {
                                    timesOfDays.Add(ObjectId.Parse(s));
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEContact.TimesOfDaysProperty, timesOfDays));
                        } 
                        #endregion

                        #region Languages
                        if (data.Languages != null)
                        {
                            List<Language> meLanguages = null;
                            if (data.Languages.Count != 0)
                            {
                                meLanguages = new List<Language>();
                                List<LanguageData> languageData = data.Languages;
                                foreach (LanguageData l in languageData)
                                {
                                    Language meL = new Language
                                    {
                                        LookUpLanguageId = ObjectId.Parse(l.LookUpLanguageId),
                                        Preferred = l.Preferred
                                    };
                                    meLanguages.Add(meL);
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<Language>>(MEContact.LanguagesProperty, meLanguages));

                        } 
                        #endregion
                        
                        #region Phone&Text(softdeletes)
                        if (data.Phones != null)
                        {
                            List<Phone> mePhones = null;
                            List<Phone> existingPhones = mc.Phones;
                            if (existingPhones == null)
                            {
                                // Add all the new phones that are sent in the request with the newly generated ObjectId.
                                if (data.Phones.Count != 0)
                                {
                                    mePhones = new List<Phone>();
                                    List<PhoneData> phoneData = data.Phones;
                                    foreach (PhoneData p in phoneData)
                                    {
                                        Phone mePh = new Phone
                                        {
                                            Id = ObjectId.GenerateNewId(),
                                            Number = p.Number,
                                            ExtNumber = p.ExtNumber,
                                            IsText = p.IsText,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            PreferredPhone = p.PhonePreferred,
                                            PreferredText = p.TextPreferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            DataSource = Helper.TrimAndLimit(p.DataSource, 50)
                                        };
                                        mePhones.Add(mePh);
                                        updatedPhones.Add(new CleanupIdData { OldId = p.Id, NewId = mePh.Id.ToString() });
                                    }
                                }
                            }
                            else 
                            {
                                mePhones = new List<Phone>();
                                // Set deleteflag == true for the existing 
                                if (data.Phones.Count == 0)
                                {
                                    foreach (Phone mePh in existingPhones)
                                    {
                                        mePh.DeleteFlag = true;
                                        mePhones.Add(mePh);
                                    }
                                }
                                else
                                {
                                    List<PhoneData> phoneData = data.Phones;
                                    foreach(PhoneData p in phoneData )
                                    {
                                        // Check if it was a new insert.
                                        ObjectId result;
                                        ObjectId id;
                                        if (ObjectId.TryParse(p.Id, out result))
                                        {
                                            // this is an update to an existing.
                                            id = result;
                                        }
                                        else
                                        {
                                            // this is a new insert
                                            id = ObjectId.GenerateNewId();
                                            updatedPhones.Add(new CleanupIdData { OldId = p.Id, NewId = id.ToString() });
                                        }
                                        Phone mePh = new Phone
                                        {
                                            Id = id,
                                            Number = p.Number,
                                            ExtNumber = p.ExtNumber,
                                            IsText = p.IsText,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            PreferredPhone = p.PhonePreferred,
                                            PreferredText = p.TextPreferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            DataSource = Helper.TrimAndLimit(p.DataSource, 50),
                                            ExternalRecordId = p.ExternalRecordId
                                        };
                                        mePhones.Add(mePh);
                                    } 
                                    // Addd the ones that are alreaddy soft deleted in DB or deleted in the UI and not sent back from the UI.
                                    foreach (var e in existingPhones)
                                    {
                                        if (e.DeleteFlag == true)
                                        {
                                            mePhones.Add(e);
                                        }
                                        else
                                        {
                                            if(phoneData.Where(a => a.Id == e.Id.ToString()).FirstOrDefault() == null)
                                            {
                                                e.DeleteFlag = true;
                                                mePhones.Add(e);
                                            }
                                        }
                                    }
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<Phone>>(MEContact.PhonesProperty, mePhones));
                        }
                        #endregion

                        #region Emails(softdeletes)
                        if (data.Emails != null)
                        {
                            List<Email> meEmails = null;
                            List<Email> existingEmails = mc.Emails;
                            if (existingEmails == null)
                            {
                                // Add all the new emails that are sent in the request with the newly generated ObjectId.
                                if (data.Emails.Count != 0)
                                {
                                    meEmails = new List<Email>();
                                    List<EmailData> emailData = data.Emails;
                                    foreach (EmailData p in emailData)
                                    {
                                        Email me = new Email
                                        {
                                            Id = ObjectId.GenerateNewId(),
                                            Text = p.Text,
                                            Preferred = p.Preferred,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            DataSource = Helper.TrimAndLimit(p.DataSource, 50),
                                            ExternalRecordId = p.ExternalRecordId
                                        };
                                        meEmails.Add(me);
                                        updatedEmails.Add(new CleanupIdData { OldId = p.Id, NewId = me.Id.ToString() });
                                    }
                                }
                            }
                            else
                            {
                                meEmails = new List<Email>();
                                // Set deleteflag == true for the existing 
                                if (data.Emails.Count == 0)
                                {
                                    foreach (Email me in existingEmails)
                                    {
                                        me.DeleteFlag = true;
                                        meEmails.Add(me);
                                    }
                                }
                                else
                                {
                                    List<EmailData> emailData = data.Emails;
                                    foreach (EmailData p in emailData)
                                    {
                                        // Check if it was a new insert.
                                        ObjectId result;
                                        ObjectId id;
                                        if (ObjectId.TryParse(p.Id, out result))
                                        {
                                            // this is an update to an existing.
                                            id = result;
                                        }
                                        else
                                        {
                                            // this is a new insert
                                            id = ObjectId.GenerateNewId();
                                            updatedEmails.Add(new CleanupIdData { OldId = p.Id, NewId = id.ToString() });
                                        }
                                        Email mePh = new Email
                                        {
                                            Id = id,
                                            Text = p.Text,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            Preferred = p.Preferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            DataSource = Helper.TrimAndLimit(p.DataSource, 50),
                                            ExternalRecordId = p.ExternalRecordId
                                        };
                                        meEmails.Add(mePh);
                                    }
                                    // Addd the ones that are alreaddy soft deleted in DB or deleted in the UI and not sent back from the UI.
                                    foreach (var e in existingEmails)
                                    {
                                        if (e.DeleteFlag == true)
                                        {
                                           meEmails.Add(e);
                                        }
                                        else
                                        {
                                            if (emailData.Where(a => a.Id == e.Id.ToString()).FirstOrDefault() == null)
                                            {
                                                e.DeleteFlag = true;
                                                meEmails.Add(e);
                                            }
                                        }
                                    }
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<Email>>(MEContact.EmailsProperty, meEmails));
                        }
                        #endregion

                        #region Addresses(softdeletes)
                        if (data.Addresses != null)
                        {
                            List<Address> meAddresses = null;
                            List<Address> existingAddresses = mc.Addresses;
                            if (existingAddresses == null)
                            {
                                // Add all the new addresses that are sent in the request with the newly generated ObjectId.
                                if (data.Addresses.Count != 0)
                                {
                                    meAddresses = new List<Address>();
                                    List<AddressData> addressData = data.Addresses;
                                    foreach (AddressData p in addressData)
                                    {
                                        Address me = new Address
                                        {
                                            Id = ObjectId.GenerateNewId(),
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            Line1 = p.Line1,
                                            Line2 = p.Line2,
                                            Line3 = p.Line3,
                                            City = p.City,
                                            StateId = ObjectId.Parse(p.StateId),
                                            PostalCode = p.PostalCode,
                                            Preferred = p.Preferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            ExternalRecordId = p.ExternalRecordId,
                                            DataSource = p.DataSource
                                        };
                                        meAddresses.Add(me);
                                        updatedAddresses.Add(new CleanupIdData { OldId = p.Id, NewId = me.Id.ToString() });
                                    }
                                }
                            }
                            else
                            {
                                meAddresses = new List<Address>();
                                // Set deleteflag == true for the existing 
                                if (data.Addresses.Count == 0)
                                {
                                    foreach (Address me in existingAddresses)
                                    {
                                        me.DeleteFlag = true;
                                        meAddresses.Add(me);
                                    }
                                }
                                else
                                {
                                    List<AddressData> addressData = data.Addresses;
                                    foreach (AddressData p in addressData)
                                    {
                                        // Check if it was a new insert.
                                        ObjectId result;
                                        ObjectId id;
                                        if (ObjectId.TryParse(p.Id, out result))
                                        {
                                            // this is an update to an existing.
                                            id = result;
                                        }
                                        else
                                        {
                                            // this is a new insert
                                            id = ObjectId.GenerateNewId();
                                            updatedAddresses.Add(new CleanupIdData { OldId = p.Id, NewId = id.ToString() });
                                        }
                                        Address me = new Address
                                        {
                                            Id = id,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            Line1 = p.Line1,
                                            Line2 = p.Line2,
                                            Line3 = p.Line3,
                                            City = p.City,
                                            StateId = ObjectId.Parse(p.StateId),
                                            PostalCode = p.PostalCode,
                                            Preferred = p.Preferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false,
                                            ExternalRecordId = p.ExternalRecordId,
                                            DataSource = p.DataSource

                                        };
                                        meAddresses.Add(me);
                                    }
                                    // Addd the ones that are alreaddy soft deleted in DB or deleted in the UI and not sent back from the UI.
                                    foreach (var e in existingAddresses)
                                    {
                                        if (e.DeleteFlag == true)
                                        {
                                            meAddresses.Add(e);
                                        }
                                        else
                                        {
                                            if (addressData.Where(a => a.Id == e.Id.ToString()).FirstOrDefault() == null)
                                            {
                                                e.DeleteFlag = true;
                                                meAddresses.Add(e);
                                            }
                                        }
                                    }
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<Address>>(MEContact.AddressessProperty, meAddresses));
                        }
                        #endregion

                        // TimeZone
                        if (!string.IsNullOrEmpty(data.TimeZoneId))
                        {
                            uv.Add(MB.Update.Set(MEContact.TimeZoneProperty, ObjectId.Parse(data.TimeZoneId)));
                        } 

                        // LastUpdatedOn
                        uv.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty,DateTime.UtcNow));

                        //UpdatedBy
                        uv.Add(MB.Update.Set(MEContact.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                        IMongoUpdate update = MB.Update.Combine(uv);
                        ctx.Contacts.Collection.Update(query, update);

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.Contact.ToString(),
                                                data.Id,
                                                DataAuditType.Update,
                                                request.ContractNumber);

                        //set the response
                        response.SuccessData = true;
                        response.UpdatedPhoneData = updatedPhones;
                        response.UpdatedEmailData = updatedEmails;
                        response.UpdatedAddressData = updatedAddresses;
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
            return response;
        }

        public bool UpdateRecentList(PutRecentPatientRequest request, List<string> recents)
        {
            try
            {
                bool result = false;
                List<ObjectId> lsO = recents.ConvertAll(r => ObjectId.Parse(r));

                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(MB.Query.EQ(MEContact.IdProperty, ObjectId.Parse(request.ContactId)));
                    queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery query = MB.Query.And(queries);
                    MEContact mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();

                    if (mc != null)
                    {
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEContact.RecentListProperty, lsO));

                        // LastUpdatedOn
                        uv.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty, DateTime.UtcNow));
                        //UpdatedBy
                        uv.Add(MB.Update.Set(MEContact.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                        IMongoUpdate update = MB.Update.Combine(uv);
                        ctx.Contacts.Collection.Update(query, update);

                        AuditHelpers.LogDataAudit(this.UserId,
                                                MongoCollectionName.Contact.ToString(),
                                                request.ContactId,
                                                DataAuditType.Update,
                                                request.ContractNumber);
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        /// <summary>
        /// Get a contact for patient
        /// </summary>
        /// <param name="request">GetContactDataRequest object</param>
        /// <returns>ContactData object</returns>
        public object FindContactByPatientId(GetContactByPatientIdDataRequest request)
        {
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(MB.Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(request.PatientId)));
                queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = MB.Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData { 
                        Id = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        UserId = (string.IsNullOrEmpty(mc.ResourceId)) ? string.Empty : mc.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                        FirstName = mc.FirstName,
                        MiddleName = mc.MiddleName,
                        LastName = mc.LastName,
                        PreferredName = mc.PreferredName,
                        Gender = mc.Gender,
                        TimeZoneId = mc.TimeZoneId == null ? null : mc.TimeZoneId.ToString(),
                        WeekDays = mc.WeekDays,
                        TimesOfDaysId = Helper.ConvertToStringList(mc.TimesOfDays)
                    };

                    //Modes
                    List<CommMode> meCommModes = mc.Modes;
                    if(meCommModes != null && meCommModes.Count > 0 )
                    {
                        List<CommModeData> modes = new List<CommModeData>();
                        foreach(CommMode cm in meCommModes)
                        {
                            CommModeData commMode = new CommModeData { ModeId = cm.ModeId.ToString() , OptOut = cm.OptOut, Preferred = cm.Preferred };
                            modes.Add(commMode);
                        }
                        contactData.Modes = modes;
                    }

                    //Phones
                    List<Phone> mePhones = mc.Phones;
                    if (mePhones != null && mePhones.Count > 0)
                    {
                        List<PhoneData> phones = new List<PhoneData>();
                        foreach (Phone mePh in mePhones)
                        {
                            // Get the ones that are not deleted.
                            if(!mePh.DeleteFlag)
                            {
                                PhoneData phone = new PhoneData
                                {
                                    Id = mePh.Id.ToString(),
                                    IsText = mePh.IsText,
                                    Number = mePh.Number,
                                    ExtNumber = mePh.ExtNumber,
                                    OptOut = mePh.OptOut,
                                    PhonePreferred = mePh.PreferredPhone,
                                    TextPreferred = mePh.PreferredText,
                                    TypeId = mePh.TypeId.ToString(),
                                    DataSource = Helper.TrimAndLimit(mePh.DataSource, 50),
                                    ExternalRecordId = mePh.ExternalRecordId
                                };
                                phones.Add(phone);
                            }
                        }
                        contactData.Phones = phones;
                    }

                    //Emails
                    List<Email> meEmails = mc.Emails;
                    if (meEmails != null && meEmails.Count > 0)
                    {
                        List<EmailData> emails = new List<EmailData>();
                        foreach (Email meE in meEmails)
                        {
                            // Get the ones that are not deleted.
                            if (!meE.DeleteFlag)
                            {
                                EmailData email = new EmailData { Id = meE.Id.ToString(), OptOut = meE.OptOut, Preferred = meE.Preferred, Text = meE.Text, TypeId = meE.TypeId.ToString(), DataSource = Helper.TrimAndLimit(meE.DataSource, 50),ExternalRecordId = meE.ExternalRecordId};
                                emails.Add(email);
                            }
                        }
                        contactData.Emails = emails;
                    }

                    //Addresses
                    List<Address> meAddresses = mc.Addresses;
                    if (meAddresses != null && meAddresses.Count > 0)
                    {
                        List<AddressData> addresses = new List<AddressData>();
                        foreach (Address meAdd in meAddresses)
                        {
                            // Get the ones that are not deleted.
                            if (!meAdd.DeleteFlag)
                            {
                                AddressData address = new AddressData { Id = meAdd.Id.ToString(), Line1 = meAdd.Line1, Line2 = meAdd.Line2, Line3 = meAdd.Line3, City = meAdd.City, StateId = meAdd.StateId.ToString(), PostalCode = meAdd.PostalCode, TypeId = meAdd.TypeId.ToString(), OptOut = meAdd.OptOut, Preferred = meAdd.Preferred,DataSource = meAdd.DataSource,ExternalRecordId = meAdd.ExternalRecordId};
                                addresses.Add(address);
                            }
                        }
                        contactData.Addresses = addresses;
                    }

                    //Languages
                    List<Language> meLanguages = mc.Languages;
                    if (meLanguages != null && meLanguages.Count > 0)
                    {
                        List<LanguageData> languages = new List<LanguageData>();
                        foreach (Language meLang in meLanguages)
                        {
                            LanguageData langugage = new LanguageData { LookUpLanguageId = meLang.LookUpLanguageId.ToString() ,Preferred = meLang.Preferred };
                            languages.Add(langugage);
                        }
                        contactData.Languages = languages;
                    }
                }
            }
            return contactData;
        }

        public object FindContactByUserId(GetContactByUserIdDataRequest request)
        {
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(MB.Query.EQ(MEContact.ResourceIdProperty, request.SQLUserId));
                queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = MB.Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData
                    {
                        Id = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        UserId = (string.IsNullOrEmpty(mc.ResourceId)) ? string.Empty : mc.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                        FirstName = mc.FirstName,
                        MiddleName = mc.MiddleName,
                        LastName = mc.LastName,
                        PreferredName = mc.PreferredName,
                        Gender = mc.Gender
                    };
                }

            }
            return contactData;
        }

        /// <summary>
        /// This method get minimal information of contact. For full information use FindContactByPatientId method.
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>ContactData</returns>
        public object GetContactByPatientId(string patientId)
        {
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(MB.Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(patientId)));
                queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = MB.Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData
                    {
                        Id = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        FirstName = mc.FirstName,
                        LastName = mc.LastName
                    };
                }

            }
            return contactData;
        }


        public IEnumerable<object> FindCareManagers()
        { 
            List<ContactData> contactDataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    //queries.Add(Query.NE(MEContact.ResourceIdProperty, BsonNull.Value)); commenting this out, so that System is returned.
                    queries.Add(MB.Query.EQ(MEContact.PatientIdProperty, BsonNull.Value));
                    queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery mQuery = MB.Query.And(queries);
                    List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                    if (meContacts != null)
                    {
                        contactDataList = new List<ContactData>();
                        foreach (MEContact c in meContacts)
                        {
                            ContactData contactData = new ContactData
                            {
                               Id = c.Id.ToString(),
                               UserId = (string.IsNullOrEmpty(c.ResourceId)) ? string.Empty : c.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                               PreferredName = c.PreferredName,
                               FirstName = c.FirstName,
                               LastName = c.LastName
                            };
                            contactDataList.Add(contactData);
                        }

                    }
                }
                return contactDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SearchContacts(SearchContactsDataRequest request)
        { 
            List<ContactData> contactDataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    List<BsonValue> bsonList = Helper.ConvertToBsonValueList(request.ContactIds);
                    if (bsonList != null)
                    {
                        queries.Add(MB.Query.In(MEContact.IdProperty, bsonList));
                        queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                        IMongoQuery mQuery = MB.Query.And(queries);
                        List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                        if (meContacts != null)
                        {
                            contactDataList = new List<ContactData>();
                            foreach (MEContact c in meContacts)
                            {
                                ContactData contactData = new ContactData
                                {
                                    Id = c.Id.ToString(),
                                    Gender = c.Gender,
                                    PreferredName = c.PreferredName
                                };
                                contactDataList.Add(contactData);
                            }

                        }
                    }
                }
                return contactDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> FindContactsWithAPatientInRecentList(string entityId)
        {
            List<ContactData> contactDataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(MB.Query.In(MEContact.RecentListProperty, new List<BsonValue> { BsonValue.Create(ObjectId.Parse(entityId))}));
                    queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery mQuery = MB.Query.And(queries);
                    List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                    if (meContacts != null)
                    {
                        contactDataList = new List<ContactData>();
                        foreach (MEContact c in meContacts)
                        {
                            ContactData contactData = new ContactData
                            {
                                Id = c.Id.ToString(),
                                PatientId = c.PatientId.ToString(),
                                RecentsList = c.RecentList != null ? c.RecentList.ConvertAll(r => r.ToString()) : null,
                                FirstName = c.FirstName,
                                LastName = c.LastName
                            };
                            contactDataList.Add(contactData);
                        }

                    }
                }
                return contactDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public string UserId { get; set; }

        public void UndoDelete(object entity)
        {
            UndoDeleteContactDataRequest request = (UndoDeleteContactDataRequest)entity;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    var query = MB.Query<MEContact>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEContact.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEContact.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEContact.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.Contacts.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Contact.ToString(),
                                            request.Id.ToString(),
                                            DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Select(List<string> ids)
        {
            List<ContactData> dataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(MEContact.IdProperty, new BsonArray(Helper.ConvertToObjectIdList(ids))));
                    queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEContact.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                    if (meContacts != null && meContacts.Count > 0)
                    {
                        dataList = new List<ContactData>();
                        foreach (MEContact mc in meContacts)
                        {
                            dataList.Add(new ContactData
                            {
                                Id = mc.Id.ToString(),
                                PatientId = mc.PatientId.ToString(),
                                UserId = (string.IsNullOrEmpty(mc.ResourceId)) ? string.Empty : mc.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                                FirstName = mc.FirstName,
                                MiddleName = mc.MiddleName,
                                LastName = mc.LastName,
                                PreferredName = mc.PreferredName,
                                Gender = mc.Gender
                            });
                        }
                    }
                }
                return dataList as IEnumerable<object>;
            }
            catch (Exception) { throw; }
        }
    }
}

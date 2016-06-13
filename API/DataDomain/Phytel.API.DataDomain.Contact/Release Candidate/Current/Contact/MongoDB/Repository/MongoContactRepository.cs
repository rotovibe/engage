using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
using AutoMapper;
using Phytel.API.Common.Extensions;

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
        /// <param name="newEntity">InsertContactDataRequest object</param>
        /// <returns>Id of the newly inserted contact.</returns>
        public object Insert(object newEntity)
        {
            string id = null;
            InsertContactDataRequest request = newEntity as InsertContactDataRequest;
            if (request != null)
            {
                ContactData data = request.ContactData;
                try
                {
                    using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        // Following logic for patient check should not be in Repository class, Should be in Manager class. 
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
                        MEContact meContact = BuildMEContact(data, request.Version);
                        if (meContact != null)
                        {
                            ctx.Contacts.Collection.Insert(meContact);
                            AuditHelper.LogDataAudit(this.UserId,
                                MongoCollectionName.Contact.ToString(),
                                meContact.Id.ToString(),
                                DataAuditType.Insert,
                                request.ContractNumber);

                            id = meContact.Id.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                        MEContact meContact = BuildMEContact(data, 1.0);
                        if (meContact != null)
                        {
                            bulk.Insert(meContact.ToBsonDocument());
                            insertedIds.Add(meContact.Id.ToString());
                        }
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
                    contactData = BuildContactData(mc);
                    contactData.RecentsList = mc.RecentList != null ? mc.RecentList.ConvertAll(r => r.ToString()) : null;
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
            UpdateContactDataResponse response = new UpdateContactDataResponse();
            response.SuccessData = false;
            List<CleanupIdData> updatedPhones = new List<CleanupIdData>();
            List<CleanupIdData> updatedEmails = new List<CleanupIdData>();
            List<CleanupIdData> updatedAddresses = new List<CleanupIdData>();
            try
            {
                UpdateContactDataRequest request = entity as UpdateContactDataRequest;
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
                        if (string.IsNullOrEmpty(data.FirstName))
                        {
                            uv.Add(MB.Update.Set(MEContact.FirstNameProperty, BsonNull.Value));
                            uv.Add(MB.Update.Set(MEContact.LoweredFirstNameProperty, BsonNull.Value));
                        }

                        else
                        {
                            uv.Add(MB.Update.Set(MEContact.FirstNameProperty, data.FirstName));
                            uv.Add(MB.Update.Set(MEContact.LoweredFirstNameProperty, data.FirstName.ToLower()));
                        }

                        if (string.IsNullOrEmpty(data.LastName))
                        {
                            uv.Add(MB.Update.Set(MEContact.LastNameProperty, BsonNull.Value));
                            uv.Add(MB.Update.Set(MEContact.LoweredLastNameProperty, BsonNull.Value));
                        }

                        else
                        {
                            uv.Add(MB.Update.Set(MEContact.LastNameProperty, data.LastName));
                            uv.Add(MB.Update.Set(MEContact.LoweredLastNameProperty,data.LastName.ToLower()));
                        }
                          
                        if(string.IsNullOrEmpty(data.MiddleName))
                            uv.Add(MB.Update.Set(MEContact.MiddleNameProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.MiddleNameProperty, data.MiddleName));
                        if(string.IsNullOrEmpty(data.PreferredName))
                            uv.Add(MB.Update.Set(MEContact.PreferredNameProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.PreferredNameProperty, data.PreferredName));
                        if(string.IsNullOrEmpty(data.Gender))
                            uv.Add(MB.Update.Set(MEContact.GenderProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.GenderProperty, data.Gender));
                        if(string.IsNullOrEmpty(data.Suffix))
                            uv.Add(MB.Update.Set(MEContact.SuffixProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.SuffixProperty, data.Suffix));
                        if(string.IsNullOrEmpty(data.Prefix))
                            uv.Add(MB.Update.Set(MEContact.PrefixProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.PrefixProperty, data.Prefix));
                        if(string.IsNullOrEmpty(data.DataSource))
                            uv.Add(MB.Update.Set(MEContact.DataSourceProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.DataSourceProperty, data.DataSource));
                        if(string.IsNullOrEmpty(data.ExternalRecordId))
                            uv.Add(MB.Update.Set(MEContact.ExternalRecordIdProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.ExternalRecordIdProperty, data.ExternalRecordId));
                        uv.Add(MB.Update.Set(MEContact.DeceasedProperty, data.DeceasedId));
                        uv.Add(MB.Update.Set(MEContact.StatusProperty, data.StatusId));
                        if (!string.IsNullOrEmpty(data.ContactTypeId))
                            uv.Add(MB.Update.Set(MEContact.ContactTypeIdProperty, ObjectId.Parse(data.ContactTypeId)));
                        uv.Add(MB.Update.SetWrapped<List<MEContactSubType>>(MEContact.ContactSubTypesProperty, BuildMEContactSubTypes(data.ContactSubTypesData)));
                        #region Communication
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
                                    foreach (PhoneData p in phoneData)
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
                                            if (phoneData.Where(a => a.Id == e.Id.ToString()).FirstOrDefault() == null)
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
                            uv.Add(MB.Update.Set(MEContact.TimeZoneProperty,  ObjectId.Parse(data.TimeZoneId)));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEContact.TimeZoneProperty, BsonNull.Value));
                        }
                        #endregion

                        uv.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty,DateTime.UtcNow));
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
                    contactData = BuildContactData(mc);
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
                    var mongoQuery = new List<IMongoQuery>();
                    var resourceNullQuery = MB.Query.NE("rid", BsonNull.Value);
                    var fnQuery = MB.Query<MEContact>.EQ(c => c.FirstName, "System");
                    var delQuery = MB.Query<MEContact>.EQ(c => c.DeleteFlag, false);
                    mongoQuery.Add(resourceNullQuery);
                    mongoQuery.Add(fnQuery);
                    var orQuery = MB.Query.Or(mongoQuery);
                    var finalQuery = MB.Query.And(orQuery, delQuery);
                    List<MEContact> meContacts = ctx.Contacts.Collection.Find(finalQuery).ToList();
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

        public IEnumerable<object> GetContactsByContactIds(GetContactsByContactIdsDataRequest request)
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
                        if (meContacts != null && meContacts.Count > 0)
                        {
                            contactDataList = new List<ContactData>();
                            foreach (MEContact c in meContacts)
                            {
                                ContactData contactData = BuildContactData(c);
                                if(contactData != null)
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

        public IEnumerable<object> SearchContacts(SearchContactsDataRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            List<ContactData> dataList = null;

            using (var ctx = new ContactMongoContext(_dbName))
            {
                var query = BuildSearchContactsMongoQuery(request);
                var results = new List<MEContact>();

                var cursor = ctx.Contacts.Collection.Find(query);

                 var sortByBuilder = new SortByBuilder();
                sortByBuilder.Ascending(MEContact.LoweredLastNameProperty, MEContact.LoweredFirstNameProperty, MEContact.IdProperty);

                cursor.SetSortOrder(sortByBuilder);
                cursor.SetSkip(request.Skip);

                if (request.Take.HasValue)
                    cursor.SetLimit(request.Take.Value);

                results = cursor.ToList();

                if (!results.IsNullOrEmpty())
                {
                    dataList = new List<ContactData>();
                    foreach (var item in results)
                    {
                        var contactData = BuildContactData(item);
                        dataList.Add(contactData);
                    }
                }
            }

            return dataList;
        }

        public long GetSearchContactsCount(SearchContactsDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            long searchTotalCount = 0;

            using (var ctx = new ContactMongoContext(_dbName))
            {
                var query = BuildSearchContactsMongoQuery(request);
                searchTotalCount =  ctx.Contacts.Collection.Count(query);

            }

            return searchTotalCount;
        }

        public bool SyncContact(SyncContactInfoDataRequest request)
        {
            var response = new SyncContactInfoDataResponse();
            try
            {
                using (var ctx = new ContactMongoContext(_dbName))
                {
                    var data = request.ContactInfo;
                    var queries = new List<IMongoQuery>
                    {
                        MB.Query.EQ(MEContact.IdProperty, ObjectId.Parse(request.ContactId)),
                        MB.Query.EQ(MEContact.DeleteFlagProperty, false)
                    };

                    var query = MB.Query.And(queries);
                    var mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();
                    if (mc != null)
                    {
                        var uv = new List<MB.UpdateBuilder>();
                        if (string.IsNullOrEmpty(data.FirstName))
                        {
                            uv.Add(MB.Update.Set(MEContact.FirstNameProperty, BsonNull.Value));
                            uv.Add(MB.Update.Set(MEContact.LoweredFirstNameProperty, BsonNull.Value));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(MEContact.FirstNameProperty, data.FirstName));
                            uv.Add(MB.Update.Set(MEContact.LoweredFirstNameProperty, data.FirstName.ToLower()));
                        }
                        if (string.IsNullOrEmpty(data.LastName))
                        {
                            uv.Add(MB.Update.Set(MEContact.LastNameProperty, BsonNull.Value));
                            uv.Add(MB.Update.Set(MEContact.LoweredLastNameProperty, BsonNull.Value));
                        }

                        else
                        {
                            uv.Add(MB.Update.Set(MEContact.LastNameProperty, data.LastName));
                            uv.Add(MB.Update.Set(MEContact.LoweredLastNameProperty, data.LastName.ToLower()));
                        }
                            
                        if(string.IsNullOrEmpty(data.MiddleName))
                            uv.Add(MB.Update.Set(MEContact.MiddleNameProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.MiddleNameProperty, data.MiddleName));
                        if(string.IsNullOrEmpty(data.PreferredName))
                            uv.Add(MB.Update.Set(MEContact.PreferredNameProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.PreferredNameProperty, data.PreferredName));
                        if(string.IsNullOrEmpty(data.Gender))
                            uv.Add(MB.Update.Set(MEContact.GenderProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.GenderProperty, data.Gender));
                        if(string.IsNullOrEmpty(data.Suffix))
                            uv.Add(MB.Update.Set(MEContact.SuffixProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.SuffixProperty, data.Suffix));
                        if(string.IsNullOrEmpty(data.Prefix))
                            uv.Add(MB.Update.Set(MEContact.PrefixProperty, BsonNull.Value));
                        else
                            uv.Add(MB.Update.Set(MEContact.PrefixProperty, data.Prefix));
                        uv.Add(MB.Update.Set(MEContact.DeceasedProperty, data.DeceasedId));
                        //uv.Add(MB.Update.Set(MEContact.StatusProperty, data.StatusId));
                        uv.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty, DateTime.UtcNow));
                        uv.Add(MB.Update.Set(MEContact.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                   

                    var update = MB.Update.Combine(uv);

                    ctx.Contacts.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Contact.ToString(),
                                            request.ContactId,
                                            DataAuditType.Update,
                                            request.ContractNumber);

                        response.IsSuccessful = true;
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

        public bool DereferencePatient(DereferencePatientDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var isSuccessful = false;

            try
            {
                using (var ctx = new ContactMongoContext(_dbName))
                {
                    var query = Query.And(MB.Query<MEContact>.EQ(b => b.PatientId, ObjectId.Parse(request.PatientId)),
                                          MB.Query<MEContact>.EQ(b => b.DeleteFlag, false));

                    var meContact = ctx.Contacts.Collection.FindOne(query);

                    meContact.PatientId = null;
                    meContact.UpdatedBy = ObjectId.Parse(request.UserId);
                    meContact.LastUpdatedOn = DateTime.UtcNow;

                    ctx.Contacts.Collection.Save(meContact);

                    isSuccessful = true;

                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.Contact.ToString(),
                        meContact.Id.ToString(),
                        DataAuditType.Update,
                        request.ContractNumber);
                }
            }
            catch (Exception)
            {
                isSuccessful = false;
                throw;
            }

            return isSuccessful;
        }

        public bool UnDereferencePatient(UndoDereferencePatientDataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var isSuccessful = false;

            try
            {
                using (var ctx = new ContactMongoContext(_dbName))
                {
                    var query = Query.And(MB.Query<MEContact>.EQ(b => b.PatientId, ObjectId.Parse(request.PatientId)),
                                          MB.Query<MEContact>.EQ(b => b.DeleteFlag, false));

                    var meContact = ctx.Contacts.Collection.FindOne(query);

                    meContact.PatientId = ObjectId.Parse(request.PatientId);
                    meContact.UpdatedBy = ObjectId.Parse(request.UserId);
                    meContact.LastUpdatedOn = DateTime.UtcNow;

                    ctx.Contacts.Collection.Save(meContact);
                    isSuccessful = true;

                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.Contact.ToString(),
                        meContact.Id.ToString(),
                        DataAuditType.Update,
                        request.ContractNumber);
                }
            }
            catch (Exception)
            {
                isSuccessful = false;
                throw;
            }

            return isSuccessful;
        }

        public IEnumerable<ContactData> GetContactsByPatientIds(List<string> patientIds)
        {
            List<ContactData> contactDataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    List<BsonValue> bsonList = Helper.ConvertToBsonValueList(patientIds);
                    if (bsonList != null)
                    {
                        queries.Add(MB.Query.In(MEContact.PatientIdProperty, bsonList));
                        queries.Add(MB.Query.EQ(MEContact.DeleteFlagProperty, false));
                        IMongoQuery mQuery = MB.Query.And(queries);
                        List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                        if (meContacts != null && meContacts.Count > 0)
                        {
                            contactDataList = new List<ContactData>();
                            foreach (MEContact c in meContacts)
                            {
                                ContactData contactData = BuildContactData(c);
                                if (contactData != null)
                                    contactDataList.Add(contactData);
                            }
                        }
                    }
                }
                return contactDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        #region Private Methods

        private ContactData BuildContactData(MEContact contactEntity)
        {
            ContactData contactData = null;
            if (contactEntity != null)
            {
                contactData = new ContactData
                {
                    Id = contactEntity.Id.ToString(),
                    PatientId = contactEntity.PatientId.ToString(),
                    UserId =
                        (string.IsNullOrEmpty(contactEntity.ResourceId))
                            ? string.Empty
                            : contactEntity.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                    FirstName = contactEntity.FirstName,
                    MiddleName = contactEntity.MiddleName,
                    LastName = contactEntity.LastName,
                    PreferredName = contactEntity.PreferredName,
                    Gender = contactEntity.Gender,
                    TimeZoneId = contactEntity.TimeZoneId == null ? null : contactEntity.TimeZoneId.ToString(),
                    WeekDays = contactEntity.WeekDays,
                    TimesOfDaysId = Helper.ConvertToStringList(contactEntity.TimesOfDays),
                    Modes = BuildCommunicationModes(contactEntity.Modes),
                    Phones = BuildPhoneData(contactEntity.Phones),
                    Emails = BuildEmailData(contactEntity.Emails),
                    Addresses = BuildAddressData(contactEntity.Addresses),
                    Languages = BuildLanguageData(contactEntity.Languages),
                    ContactTypeId = contactEntity.ContactTypeId.ToString(),
                    ContactSubTypesData = BuildContactTypesData(contactEntity.ContactSubTypes),
                    StatusId = (int) contactEntity.Status,
                    Prefix = contactEntity.Prefix,
                    Suffix = contactEntity.Suffix,
                    DeceasedId = (int) contactEntity.Deceased,
                    DataSource = contactEntity.DataSource,
                    ExternalRecordId = contactEntity.ExternalRecordId,
                    CreatedById = contactEntity.RecordCreatedBy.ToString(),
                    CreatedOn = contactEntity.RecordCreatedOn,
                    UpdatedById = contactEntity.UpdatedBy == null ? null : contactEntity.UpdatedBy.ToString(),
                    UpdatedOn = contactEntity.LastUpdatedOn
                };
            }
            return contactData;
        }

        private List<CommModeData> BuildCommunicationModes(List<DTO.CommMode> meCommModes )
        {

            var modes = new List<CommModeData>();
            if (meCommModes != null && meCommModes.Count > 0)
            {
                
                foreach (CommMode cm in meCommModes)
                {
                    CommModeData commMode = new CommModeData { ModeId = cm.ModeId.ToString(), OptOut = cm.OptOut, Preferred = cm.Preferred };
                    modes.Add(commMode);
                }
                
            }

            return modes;
        }

        private List<PhoneData> BuildPhoneData(List<Phone> mePhones)
        {
            var phones = new List<PhoneData>();
            if (mePhones != null && mePhones.Count > 0)
            {
              
                foreach (Phone mePh in mePhones)
                {
                    // Get the ones that are not deleted.
                    if (!mePh.DeleteFlag)
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
               
            }
            return phones;
        }

        private List<EmailData> BuildEmailData(List<Email> meEmails)
        {
            var emails = new List<EmailData>();
            if (meEmails != null && meEmails.Count > 0)
            {
                
                foreach (Email meE in meEmails)
                {
                    // Get the ones that are not deleted.
                    if (!meE.DeleteFlag)
                    {
                        EmailData email = new EmailData { Id = meE.Id.ToString(), OptOut = meE.OptOut, Preferred = meE.Preferred, Text = meE.Text, TypeId = meE.TypeId.ToString(), DataSource = Helper.TrimAndLimit(meE.DataSource, 50), ExternalRecordId = meE.ExternalRecordId };
                        emails.Add(email);
                    }
                }
                
            }
            return emails;
        }

        private List<AddressData> BuildAddressData(List<Address> meAddresses)
        {
            var addresses = new List<AddressData>();
            if (meAddresses != null && meAddresses.Count > 0)
            {
                foreach (Address meAdd in meAddresses)
                {
                    // Get the ones that are not deleted.
                    if (!meAdd.DeleteFlag)
                    {
                        AddressData address = new AddressData { Id = meAdd.Id.ToString(), Line1 = meAdd.Line1, Line2 = meAdd.Line2, Line3 = meAdd.Line3, City = meAdd.City, StateId = meAdd.StateId.ToString(), PostalCode = meAdd.PostalCode, TypeId = meAdd.TypeId.ToString(), OptOut = meAdd.OptOut, Preferred = meAdd.Preferred, DataSource = meAdd.DataSource, ExternalRecordId = meAdd.ExternalRecordId };
                        addresses.Add(address);
                    }
                }
               
            }
            return addresses;
        }

        private List<LanguageData> BuildLanguageData(List<Language> meLanguages)
        {
            var languages = new List<LanguageData>();
            if (meLanguages != null && meLanguages.Count > 0)
            {
                foreach (Language meLang in meLanguages)
                {
                    LanguageData langugage = new LanguageData { LookUpLanguageId = meLang.LookUpLanguageId.ToString(), Preferred = meLang.Preferred };
                    languages.Add(langugage);
                }
                
            }
            return languages;
        }

        private List<ContactSubTypeData> BuildContactTypesData(List<MEContactSubType> mecontactSubType)
        {
            var mappedContactTypeData = Mapper.Map<List<ContactSubTypeData>>(mecontactSubType);

            return mappedContactTypeData;
        }

        private MEContact BuildMEContact(ContactData data, double version)
        {
            MEContact meContact = null;
            if (data != null)
            {
                meContact = new MEContact(this.UserId, data.CreatedOn)
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    PreferredName = data.PreferredName,
                    MiddleName = data.MiddleName,
                    Gender = data.Gender,
                    ResourceId = data.UserId,
                    Status = (Status)data.StatusId,
                    Deceased = (Deceased)data.DeceasedId,
                    Suffix = data.Suffix,
                    Prefix = data.Prefix,
                    DataSource = data.DataSource,
                    ExternalRecordId = data.ExternalRecordId,
                    Version = version,
                    DeleteFlag = false,
                    LoweredLastName =  string.IsNullOrEmpty(data.LastName) ? null : data.LastName.ToLower(),
                    LoweredFirstName = string.IsNullOrEmpty(data.FirstName)? null : data.FirstName.ToLower()
                };
                //ContactTypeId
                if (data.ContactTypeId != null)
                {
                    meContact.ContactTypeId = ObjectId.Parse(data.ContactTypeId);
                }
                //ContactSubTypes
                meContact.ContactSubTypes = BuildMEContactSubTypes(data.ContactSubTypesData);
                //PatientId
                if (data.PatientId != null)
                {
                    meContact.PatientId = ObjectId.Parse(data.PatientId);
                }

                #region Communication fields

                //Timezone
                if (!string.IsNullOrEmpty(data.TimeZoneId))
                {
                    meContact.TimeZoneId = ObjectId.Parse(data.TimeZoneId);
                }
                //Modes
                if (data.Modes != null && data.Modes.Count > 0)
                {
                    List<CommMode> commModes = new List<CommMode>();
                    foreach (CommModeData c in data.Modes)
                    {
                        commModes.Add(new CommMode
                        {
                            ModeId = ObjectId.Parse(c.ModeId),
                            OptOut = c.OptOut,
                            Preferred = c.Preferred
                        });
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
                        languages.Add(new Language
                        {
                            LookUpLanguageId = ObjectId.Parse(c.LookUpLanguageId),
                            Preferred = c.Preferred
                        });
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

                #endregion
            }
            return meContact;
        }

        private List<MEContactSubType> BuildMEContactSubTypes(List<ContactSubTypeData> data )
        {
            List<MEContactSubType> list = new List<MEContactSubType>();
            if (data != null && data.Count > 0)
            {
                list = new List<MEContactSubType>();
                foreach (var d in data)
                {
                    MEContactSubType subtype = new MEContactSubType();
                    if (!string.IsNullOrEmpty(d.SubTypeId))
                        subtype.SubTypeId = ObjectId.Parse(d.SubTypeId);
                    if (!string.IsNullOrEmpty(d.SpecialtyId))
                        subtype.SpecialtyId = ObjectId.Parse(d.SpecialtyId);
                    subtype.SubSpecialtyIds = Helper.ConvertToObjectIdList(d.SubSpecialtyIds);
                    list.Add(subtype);
                }
            }
            return list;
        }

        private IMongoQuery BuildSearchContactsMongoQuery(SearchContactsDataRequest request)
        {
            var mongoQuery = new List<IMongoQuery>();

            //Build Non Deleted Entries query
            var activeQuery = Query<MEContact>.EQ(c => c.DeleteFlag, false);
            mongoQuery.Add(activeQuery);

            //Build Contact Type Search.
            if (!request.ContactTypeIds.IsNullOrEmpty())
            {
                var contactTypesToSearch = request.ContactTypeIds;
                var contactTypesQuery = Query<MEContact>.In(ct => ct.ContactTypeId,
                    contactTypesToSearch.Select(ct => ObjectId.Parse(ct)).ToList());

                mongoQuery.Add(contactTypesQuery);
            }

            //Build Contact Statuses.
            if (!request.ContactStatuses.IsNullOrEmpty())
            {
                var contactStatusesQuery = Query<MEContact>.In(ct => ct.Status, request.ContactStatuses.ToList());
                mongoQuery.Add(contactStatusesQuery);
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                //var firstNameQuery = Query<MEContact>.Matches(c => c.FirstName,
                //    new BsonRegularExpression(new Regex("^" + request.FirstName, RegexOptions.IgnoreCase)));
                var firstNameQuery = BuildFirstNameFilterQuery(request.FirstName, request.FilterType);

                mongoQuery.Add(firstNameQuery);
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                //var lastNameQuery = Query<MEContact>.Matches(c => c.LastName,
                //    new BsonRegularExpression(new Regex("^" + request.LastName, RegexOptions.IgnoreCase)));

                var lastNameQuery = BuildLastNameFilterQuery(request.LastName, request.FilterType);
               
                mongoQuery.Add(lastNameQuery);
            }

            if (!request.ContactSubTypeIds.IsNullOrEmpty())
            {
                var inQuery = Query<MEContactSubType>.In(cs => cs.SubTypeId, 
                                                           request.ContactSubTypeIds.Select(c => BsonValue.Create(ObjectId.Parse(c))));

                var contactSubTypeQuery = Query.ElemMatch(MEContact.ContactSubTypesProperty,inQuery);

                mongoQuery.Add(contactSubTypeQuery);
            }


            var query = Query.And(mongoQuery);

            return query;


        }

        private IMongoQuery BuildLastNameFilterQuery(string name, FilterType filterType)
        {
            IMongoQuery filterQuery = null;
            //name = name.ToLower();
            name = string.IsNullOrEmpty(name) ? name : name.Trim();
            switch (filterType)
            {
                case FilterType.ExactMatch:

                    filterQuery = Query<MEContact>.Matches(c => c.LastName, new BsonRegularExpression("^" + name + "$", "i"));
                    break;
                case FilterType.StartsWith:

                    filterQuery = Query<MEContact>.Matches(c => c.LastName,
                               new BsonRegularExpression(new Regex("^" + name, RegexOptions.IgnoreCase)));
                    break;
                case FilterType.Contains:

                    filterQuery = Query<MEContact>.Matches(c => c.LastName,
                               new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase)));
                    break;
                default:
                    //StartsWith 
                    filterQuery = Query<MEContact>.Matches(c => c.LastName,
                               new BsonRegularExpression(new Regex("^" + name, RegexOptions.IgnoreCase)));

                    break;

            }

            return filterQuery;
        }

        private IMongoQuery BuildFirstNameFilterQuery(string name, FilterType filterType)
        {
            IMongoQuery filterQuery = null;
            //name = name.ToLower();
            name = string.IsNullOrEmpty(name) ? name : name.Trim();
            switch (filterType)
            {
                case FilterType.ExactMatch:

                    filterQuery = Query<MEContact>.Matches(c => c.FirstName,
                        new BsonRegularExpression("^" + name + "$", "i"));

                    break;
                case FilterType.StartsWith:

                    filterQuery = Query<MEContact>.Matches(c => c.FirstName,
                               new BsonRegularExpression(new Regex("^" + name, RegexOptions.IgnoreCase)));
                    break;
                case FilterType.Contains:

                    filterQuery = Query<MEContact>.Matches(c => c.FirstName,
                               new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase)));
                    break;
                default:
                    //starts with 
                    filterQuery = Query<MEContact>.Matches(c => c.FirstName,
                               new BsonRegularExpression(new Regex("^" + name, RegexOptions.IgnoreCase)));

                    break;

            }

            return filterQuery;
        }
         
        #endregion


       
    }
}

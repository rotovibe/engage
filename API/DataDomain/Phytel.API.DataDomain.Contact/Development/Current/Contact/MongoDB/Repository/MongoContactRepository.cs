using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Contact;
using MongoDB.Driver.Builders;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Contact
{
    public class MongoContactRepository<T> : IContactRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoContactRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        /// <summary>
        /// Inserts a contact object.
        /// </summary>
        /// <param name="newEntity">PutContactDataRequest object</param>
        /// <returns>Id of the newly inserted contact.</returns>
        public object Insert(object newEntity)
        {
            PutContactDataResponse response = null;
            PutContactDataRequest request = newEntity as PutContactDataRequest;
            try
            {
                
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    if (request.PatientId != null)
                    {
                        queries.Add(Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(request.PatientId)));
                        queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                        IMongoQuery query = Query.And(queries);
                        MEContact mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();
                        if (mc != null)
                        {
                            throw new ApplicationException("A contact record already exists for the patient.");
                        }
                    }
                    MEContact meContact = new MEContact
                    {
                        Id = ObjectId.GenerateNewId(),
                        FirstName = request.FirstName,
                        LastName = request.LastName, 
                        PreferredName = request.PreferredName,
                        Gender = request.Gender,
                        ResourceId = request.ResourceId,
                        Version = request.Version,
                        LastUpdatedOn = DateTime.UtcNow,
                        DeleteFlag = false
                    };

                    //UpdatedBy
                    if (!string.IsNullOrEmpty(request.UserId))
                    {
                        meContact.UpdatedBy = ObjectId.Parse(request.UserId);
                    }

                    //PatientId
                    if (request.PatientId != null)
                    {
                        meContact.PatientId = ObjectId.Parse(request.PatientId);
                    }
                    //Timezone
                    if (request.TimeZoneId != null)
                    {
                        meContact.TimeZone = ObjectId.Parse(request.TimeZoneId);
                    }
                    //Modes
                    if (request.Modes != null && request.Modes.Count > 0)
                    {
                        List<CommMode> commModes = new List<CommMode>();
                        foreach (CommModeData c in request.Modes)
                        {
                            commModes.Add(new CommMode { ModeId = ObjectId.Parse(c.ModeId), OptOut = c.OptOut, Preferred = c.Preferred });
                        }
                        meContact.Modes = commModes;
                    }

                    //Weekdays
                    if (request.WeekDays != null && request.WeekDays.Count > 0)
                    {
                        meContact.WeekDays = request.WeekDays;
                    }

                    //TimesOfDays
                    if (request.TimesOfDaysId != null && request.TimesOfDaysId.Count > 0)
                    {
                        List<ObjectId> ids = new List<ObjectId>();
                        foreach (string s in request.TimesOfDaysId)
                        {
                            ids.Add(ObjectId.Parse(s));
                        }
                        meContact.TimesOfDays = ids;
                    }

                    //Languages
                    if (request.Languages != null && request.Languages.Count > 0)
                    {
                        List<Language> languages = new List<Language>();
                        foreach (LanguageData c in request.Languages)
                        {
                            languages.Add(new Language { LookUpLanguageId = ObjectId.Parse(c.LookUpLanguageId), Preferred = c.Preferred });
                        }
                        meContact.Languages = languages;
                    }

                    //Addresses
                    if (request.Addresses != null && request.Addresses.Count > 0)
                    {
                        List<Address> meAddresses = new List<Address>();
                        List<AddressData> addressData = request.Addresses;
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
                    if (request.Phones != null && request.Phones.Count > 0)
                    {
                        List<Phone> mePhones = new List<Phone>();
                        List<PhoneData> phoneData = request.Phones;
                        foreach (PhoneData p in phoneData)
                        {
                            Phone mePh = new Phone
                            {
                                Id = ObjectId.GenerateNewId(),
                                Number = p.Number,
                                IsText = p.IsText,
                                TypeId = ObjectId.Parse(p.TypeId),
                                PreferredPhone = p.PhonePreferred,
                                PreferredText = p.TextPreferred,
                                OptOut = p.OptOut,
                                DeleteFlag = false
                            };
                            mePhones.Add(mePh);
                        }
                        meContact.Phones = mePhones;
                    }

                    //Emails
                    if (request.Emails != null && request.Emails.Count > 0)
                    {
                        List<Email> meEmails = new List<Email>();
                        List<EmailData> emailData = request.Emails;
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
                    //Send back the newly inserted object.
                    response = new PutContactDataResponse();
                    response.ContactId = meContact.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                queries.Add(Query.EQ(MEContact.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData { 
                        ContactId = mc.Id.ToString(),
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

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
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
                if (request.ContactId == null)
                    throw new ArgumentException("ContactId is missing from the DataDomain request.");

                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEContact.IdProperty, ObjectId.Parse(request.ContactId)));
                    queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery query = Query.And(queries);
                    MEContact mc = ctx.Contacts.Collection.Find(query).FirstOrDefault();
                    if(mc != null)
                    {
                        var uv = new List<UpdateBuilder>();

                        #region Modes
                        if (request.Modes != null)
                        {
                            List<CommMode> meModes = null;
                            if (request.Modes.Count != 0)
                            {
                                meModes = new List<CommMode>();
                                List<CommModeData> modeData = request.Modes;
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
                        if (request.WeekDays != null)
                        {
                            List<int> weekDays = null;
                            if (request.WeekDays.Count != 0)
                            {
                                weekDays = new List<int>();
                                weekDays = request.WeekDays;
                            }
                            uv.Add(MB.Update.SetWrapped<List<int>>(MEContact.WeekDaysProperty, weekDays));

                        } 
                        #endregion

                        #region TimesOfDays
                        if (request.TimesOfDaysId != null)
                        {
                            List<ObjectId> timesOfDays = null;
                            // if nothing is selected
                            if (request.TimesOfDaysId.Count != 0)
                            {
                                timesOfDays = new List<ObjectId>();
                                List<string> times = request.TimesOfDaysId;
                                foreach (string s in times)
                                {
                                    timesOfDays.Add(ObjectId.Parse(s));
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEContact.TimesOfDaysProperty, timesOfDays));
                        } 
                        #endregion

                        #region Languages
                        if (request.Languages != null)
                        {
                            List<Language> meLanguages = null;
                            if (request.Languages.Count != 0)
                            {
                                meLanguages = new List<Language>();
                                List<LanguageData> languageData = request.Languages;
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
                        if (request.Phones != null)
                        {
                            List<Phone> mePhones = null;
                            List<Phone> existingPhones = mc.Phones;
                            if (existingPhones == null)
                            {
                                // Add all the new phones that are sent in the request with the newly generated ObjectId.
                                if (request.Phones.Count != 0)
                                {
                                    mePhones = new List<Phone>();
                                    List<PhoneData> phoneData = request.Phones;
                                    foreach (PhoneData p in phoneData)
                                    {
                                        Phone mePh = new Phone
                                        {
                                            Id = ObjectId.GenerateNewId(),
                                            Number = p.Number,
                                            IsText = p.IsText,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            PreferredPhone = p.PhonePreferred,
                                            PreferredText = p.TextPreferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false
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
                                if (request.Phones.Count == 0)
                                {
                                    foreach (Phone mePh in existingPhones)
                                    {
                                        mePh.DeleteFlag = true;
                                        mePhones.Add(mePh);
                                    }
                                }
                                else
                                {
                                    List<PhoneData> phoneData = request.Phones;
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
                                            IsText = p.IsText,
                                            TypeId = ObjectId.Parse(p.TypeId),
                                            PreferredPhone = p.PhonePreferred,
                                            PreferredText = p.TextPreferred,
                                            OptOut = p.OptOut,
                                            DeleteFlag = false
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
                        if (request.Emails != null)
                        {
                            List<Email> meEmails = null;
                            List<Email> existingEmails = mc.Emails;
                            if (existingEmails == null)
                            {
                                // Add all the new emails that are sent in the request with the newly generated ObjectId.
                                if (request.Emails.Count != 0)
                                {
                                    meEmails = new List<Email>();
                                    List<EmailData> emailData = request.Emails;
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
                                        updatedEmails.Add(new CleanupIdData { OldId = p.Id, NewId = me.Id.ToString() });
                                    }
                                }
                            }
                            else
                            {
                                meEmails = new List<Email>();
                                // Set deleteflag == true for the existing 
                                if (request.Emails.Count == 0)
                                {
                                    foreach (Email me in existingEmails)
                                    {
                                        me.DeleteFlag = true;
                                        meEmails.Add(me);
                                    }
                                }
                                else
                                {
                                    List<EmailData> emailData = request.Emails;
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
                                            DeleteFlag = false
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
                        if (request.Addresses != null)
                        {
                            List<Address> meAddresses = null;
                            List<Address> existingAddresses = mc.Addresses;
                            if (existingAddresses == null)
                            {
                                // Add all the new addresses that are sent in the request with the newly generated ObjectId.
                                if (request.Addresses.Count != 0)
                                {
                                    meAddresses = new List<Address>();
                                    List<AddressData> addressData = request.Addresses;
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
                                        updatedAddresses.Add(new CleanupIdData { OldId = p.Id, NewId = me.Id.ToString() });
                                    }
                                }
                            }
                            else
                            {
                                meAddresses = new List<Address>();
                                // Set deleteflag == true for the existing 
                                if (request.Addresses.Count == 0)
                                {
                                    foreach (Address me in existingAddresses)
                                    {
                                        me.DeleteFlag = true;
                                        meAddresses.Add(me);
                                    }
                                }
                                else
                                {
                                    List<AddressData> addressData = request.Addresses;
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
                                            DeleteFlag = false
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
                        if (!string.IsNullOrEmpty(request.TimeZoneId))
                        {
                            uv.Add(MB.Update.Set(MEContact.TimeZoneProperty, ObjectId.Parse(request.TimeZoneId)));
                        } 

                        // LastUpdatedOn
                        uv.Add(MB.Update.Set(MEContact.LastUpdatedOnProperty,DateTime.UtcNow));

                        //UpdatedBy
                        uv.Add(MB.Update.Set(MEContact.UpdatedByProperty, request.UserId));

                        IMongoUpdate update = MB.Update.Combine(uv);
                        ctx.Contacts.Collection.Update(query, update);

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
        public object FindContactByPatientId(GetContactDataRequest request)
        {
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(request.PatientId)));
                queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData { 
                        ContactId = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        UserId = (string.IsNullOrEmpty(mc.ResourceId)) ? string.Empty : mc.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                        FirstName = mc.FirstName,
                        MiddleName = mc.MiddleName,
                        LastName = mc.LastName,
                        PreferredName = mc.PreferredName,
                        Gender = mc.Gender,
                        TimeZoneId = mc.TimeZone == null ? null : mc.TimeZone.ToString(),
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
                                PhoneData phone = new PhoneData { Id = mePh.Id.ToString(), IsText = mePh.IsText, Number = mePh.Number, OptOut = mePh.OptOut, PhonePreferred = mePh.PreferredPhone, TextPreferred = mePh.PreferredText, TypeId = mePh.TypeId.ToString()};
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
                                EmailData email = new EmailData { Id = meE.Id.ToString(), OptOut = meE.OptOut, Preferred = meE.Preferred, Text = meE.Text, TypeId = meE.TypeId.ToString() };
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
                                AddressData address = new AddressData { Id = meAdd.Id.ToString(), Line1 = meAdd.Line1, Line2 = meAdd.Line2, Line3 = meAdd.Line3, City = meAdd.City, StateId = meAdd.StateId.ToString(), PostalCode = meAdd.PostalCode, TypeId = meAdd.TypeId.ToString(), OptOut = meAdd.OptOut, Preferred = meAdd.Preferred };
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
                queries.Add(Query.EQ(MEContact.ResourceIdProperty, request.UserId));
                queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData
                    {
                        ContactId = mc.Id.ToString(),
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

        public IEnumerable<object> FindCareManagers()
        { 
            List<ContactData> contactDataList = null;
            try
            {
                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.NE(MEContact.ResourceIdProperty, BsonNull.Value));
                    queries.Add(Query.EQ(MEContact.PatientIdProperty, BsonNull.Value));
                    queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                    if (meContacts != null)
                    {
                        contactDataList = new List<ContactData>();
                        foreach (MEContact c in meContacts)
                        {
                            ContactData contactData = new ContactData
                            {
                               ContactId = c.Id.ToString(),
                               UserId = (string.IsNullOrEmpty(c.ResourceId)) ? string.Empty : c.ResourceId.ToString().Replace("-", string.Empty).ToLower(),
                               PreferredName = c.PreferredName
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
                        queries.Add(Query.In(MEContact.IdProperty, bsonList));
                        queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                        IMongoQuery mQuery = Query.And(queries);
                        List<MEContact> meContacts = ctx.Contacts.Collection.Find(mQuery).ToList();
                        if (meContacts != null)
                        {
                            contactDataList = new List<ContactData>();
                            foreach (MEContact c in meContacts)
                            {
                                ContactData contactData = new ContactData
                                {
                                    ContactId = c.Id.ToString(),
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
    }
}

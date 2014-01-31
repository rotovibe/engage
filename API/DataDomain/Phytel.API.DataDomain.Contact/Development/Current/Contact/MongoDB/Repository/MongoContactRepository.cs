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
using Phytel.API.AppDomain.Contact;
using MongoDB.Driver.Builders;

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
                MEContact meContact = new MEContact
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    TimeZone = ObjectId.Parse(request.TimeZoneId),
                    Version = request.Version,
                    LastUpdatedOn = DateTime.UtcNow,
                    UpdatedBy = request.UserId,
                    DeleteFlag = false
                };

                //Modes
                if (request.Modes != null && request.Modes.Count > 0)
                {
                    List<MECommMode> commModes = new List<MECommMode>();
                    foreach (CommModeData c in request.Modes)
                    {
                        commModes.Add(new MECommMode { ModeId = ObjectId.Parse(c.ModeId), OptOut = c.OptOut, Preferred = c.Preferred });
                    }
                    meContact.Modes = commModes;
                }

                using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
                {
                    ctx.Contacts.Collection.Insert(meContact);
                }
                //Send back the newly inserted object.
                response = new PutContactDataResponse();
                response.ContactId = meContact.Id.ToString();
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
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                            List<MECommMode> meModes = null;
                            if (request.Modes.Count != 0)
                            {
                                meModes = new List<MECommMode>();
                                List<CommModeData> modeData = request.Modes;
                                foreach (CommModeData m in modeData)
                                {
                                    MECommMode meM = new MECommMode
                                    {
                                        ModeId = ObjectId.Parse(m.ModeId),
                                        OptOut = m.OptOut,
                                        Preferred = m.Preferred
                                    };
                                    meModes.Add(meM);
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<MECommMode>>(MEContact.ModesProperty, meModes));

                        } 
                        #endregion

                        #region WeekDays
                        if (request.WeekDays != null)
                        {
                            List<int> weekDays = null;
                            if (request.WeekDays.Count != 0)
                            {
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
                            List<MELanguage> meLanguages = null;
                            if (request.Languages.Count != 0)
                            {
                                meLanguages = new List<MELanguage>();
                                List<LanguageData> languageData = request.Languages;
                                foreach (LanguageData l in languageData)
                                {
                                    MELanguage meL = new MELanguage
                                    {
                                        LookUpLanguageId = ObjectId.Parse(l.LookUpLanguageId),
                                        Preferred = l.Preferred
                                    };
                                    meLanguages.Add(meL);
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<MELanguage>>(MEContact.LanguagesProperty, meLanguages));

                        } 
                        #endregion
                        
                        #region Phone&Text(softdeletes)
                        if (request.Phones != null)
                        {
                            List<MEPhone> mePhones = null;
                            List<MEPhone> existingPhones = mc.Phones;
                            if (existingPhones.Count == 0)
                            {
                                // Add all the new phones that are sent in the request with the newly generated ObjectId.
                                if (request.Phones.Count != 0)
                                {
                                    mePhones = new List<MEPhone>();
                                    List<PhoneData> phoneData = request.Phones;
                                    foreach (PhoneData p in phoneData)
                                    {
                                        MEPhone mePh = new MEPhone
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
                                mePhones = new List<MEPhone>();
                                // Set deleteflag == true for the existing 
                                if (request.Phones.Count == 0)
                                {
                                    foreach (MEPhone mePh in existingPhones)
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
                                        MEPhone mePh = new MEPhone
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
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<MEPhone>>(MEContact.PhonesProperty, mePhones));
                        }
                        #endregion

                        #region Emails(softdeletes)
                        if (request.Emails != null)
                        {
                            List<MEEmail> meEmails = null;
                            List<MEEmail> existingEmails = mc.Emails;
                            if (existingEmails.Count == 0)
                            {
                                // Add all the new emails that are sent in the request with the newly generated ObjectId.
                                if (request.Emails.Count != 0)
                                {
                                    meEmails = new List<MEEmail>();
                                    List<EmailData> emailData = request.Emails;
                                    foreach (EmailData p in emailData)
                                    {
                                        MEEmail me = new MEEmail
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
                                meEmails = new List<MEEmail>();
                                // Set deleteflag == true for the existing 
                                if (request.Emails.Count == 0)
                                {
                                    foreach (MEEmail me in existingEmails)
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
                                        MEEmail mePh = new MEEmail
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
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<MEEmail>>(MEContact.EmailsProperty, meEmails));
                        }
                        #endregion

                        #region Addresses(softdeletes)
                        if (request.Addresses != null)
                        {
                            List<MEAddress> meAddresses = null;
                            List<MEAddress> existingAddresses = mc.Addresses;
                            if (existingAddresses.Count == 0)
                            {
                                // Add all the new addresses that are sent in the request with the newly generated ObjectId.
                                if (request.Addresses.Count != 0)
                                {
                                    meAddresses = new List<MEAddress>();
                                    List<AddressData> addressData = request.Addresses;
                                    foreach (AddressData p in addressData)
                                    {
                                        MEAddress me = new MEAddress
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
                                meAddresses = new List<MEAddress>();
                                // Set deleteflag == true for the existing 
                                if (request.Addresses.Count == 0)
                                {
                                    foreach (MEAddress me in existingAddresses)
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
                                        MEAddress me = new MEAddress
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
                                }
                            }
                            uv.Add(MB.Update.SetWrapped<List<MEAddress>>(MEContact.AddressessProperty, meAddresses));
                        }
                        #endregion

                        // TimeZone
                        uv.Add(MB.Update.Set(MEContact.TimeZoneProperty, ObjectId.Parse(request.TimeZoneId)));

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
                        UserId = mc.UserId == null ? null : mc.UserId.ToString(),
                        TimeZoneId = mc.TimeZone.ToString(),
                        WeekDays = mc.WeekDays,
                        TimesOfDaysId = convertToStringList(mc.TimesOfDays)
                    };

                    //Modes
                    List<MECommMode> meCommModes = mc.Modes;
                    if(meCommModes != null && meCommModes.Count > 0 )
                    {
                        List<CommModeData> modes = new List<CommModeData>();
                        foreach(MECommMode cm in meCommModes)
                        {
                            CommModeData commMode = new CommModeData { ModeId = cm.ModeId.ToString() , OptOut = cm.OptOut, Preferred = cm.Preferred };
                            modes.Add(commMode);
                        }
                        contactData.Modes = modes;
                    }

                    //Phones
                    List<MEPhone> mePhones = mc.Phones;
                    if (mePhones != null && mePhones.Count > 0)
                    {
                        List<PhoneData> phones = new List<PhoneData>();
                        foreach (MEPhone mePh in mePhones)
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
                    List<MEEmail> meEmails = mc.Emails;
                    if (meEmails != null && meEmails.Count > 0)
                    {
                        List<EmailData> emails = new List<EmailData>();
                        foreach (MEEmail meE in meEmails)
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
                    List<MEAddress> meAddresses = mc.Addresses;
                    if (meAddresses != null && meAddresses.Count > 0)
                    {
                        List<AddressData> addresses = new List<AddressData>();
                        foreach (MEAddress meAdd in meAddresses)
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
                    List<MELanguage> meLanguages = mc.Languages;
                    if (meLanguages != null && meLanguages.Count > 0)
                    {
                        List<LanguageData> languages = new List<LanguageData>();
                        foreach (MELanguage meLang in meLanguages)
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


        /// <summary>
        /// Converts a list of objectIds to list of strings.
        /// </summary>
        /// <param name="objectIds">list of objectIds</param>
        /// <returns>list of strings</returns>
        private List<string> convertToStringList(List<ObjectId> objectIds)
        {
            List<string> stringList= null;
            if (objectIds != null && objectIds.Count != 0)
            {
                stringList = new List<string>();
                foreach (ObjectId o in objectIds)
                {
                    stringList.Add(o.ToString());
                }
            }
            return stringList;
        }
    }
}

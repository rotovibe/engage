using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Contact;

namespace Phytel.API.DataDomain.Contact
{
    public class MongoContactRepository<T> : IContactRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoContactRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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

        public object FindContactByPatientId(string patientId)
        {
            ContactData contactData = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEContact.PatientIdProperty, ObjectId.Parse(patientId)));
                queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    contactData = new ContactData { 
                        ContactId = mc.Id.ToString(),
                        PatientId = mc.PatientId.ToString(),
                        UserId = mc.UserId == null ? null : mc.UserId.ToString(),
                        TimeZoneId = mc.TimeZone == null ? null : mc.TimeZone.ToString(),
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
                            CommModeData commMode = new CommModeData { Id = cm.Id.ToString(), ModeId = cm.ModeId.ToString() , OptOut = cm.OptOut, Preferred = cm.Preferred };
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
                            LanguageData langugage = new LanguageData { Id = meLang.Id.ToString(), LookUpLanguageId = meLang.LookUpLanguageId.ToString() ,Preferred = meLang.Preferred };
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

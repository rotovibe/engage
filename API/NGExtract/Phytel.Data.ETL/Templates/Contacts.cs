using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FastMember;
using MongoDB.Bson;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL.Templates
{
    public class Contacts : DocumentCollection
    {
        private void SaveContact(List<EContact> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("PatientId", "PatientId");
                    bcc.ColumnMappings.Add("MongoPatientId", "MongoPatientId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedBy", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("ResourceId", "ResourceId");
                    bcc.ColumnMappings.Add("FirstName", "FirstName");
                    bcc.ColumnMappings.Add("MiddleName", "MiddleName");
                    bcc.ColumnMappings.Add("LastName", "LastName");
                    bcc.ColumnMappings.Add("PreferredName", "PreferredName");
                    bcc.ColumnMappings.Add("Gender", "Gender");
                    bcc.ColumnMappings.Add("MongoTimeZone", "MongoTimeZone");
                    bcc.ColumnMappings.Add("TimeZone", "TimeZone");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("Delete", "Delete");
                    bcc.ColumnMappings.Add("ExtraElements", "ExtraElements");

                    bcc.DestinationTableName = "RPT_Contact";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveAddress(List<EAddress> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("TypeId", "TypeId");
                    bcc.ColumnMappings.Add("MongoCommTypeId", "MongoCommTypeId");
                    bcc.ColumnMappings.Add("StateId", "StateId");
                    bcc.ColumnMappings.Add("MongoStateId", "MongoStateId");
                    bcc.ColumnMappings.Add("Line1", "Line1");
                    bcc.ColumnMappings.Add("Line2", "Line2");
                    bcc.ColumnMappings.Add("Line3", "Line3");
                    bcc.ColumnMappings.Add("City", "City");
                    bcc.ColumnMappings.Add("PostalCode", "PostalCode");
                    bcc.ColumnMappings.Add("Preferred", "Preferred");
                    bcc.ColumnMappings.Add("OptOut", "OptOut");
                    bcc.ColumnMappings.Add("Delete", "Delete");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactAddress";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveEmail(List<EEmail> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("TypeId", "TypeId");
                    bcc.ColumnMappings.Add("MongoCommTypeId", "MongoCommTypeId");
                    bcc.ColumnMappings.Add("Text", "Text");
                    bcc.ColumnMappings.Add("Preferred", "Preferred");
                    bcc.ColumnMappings.Add("OptOut", "OptOut");
                    bcc.ColumnMappings.Add("Delete", "Delete");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactEmail";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveLanguage(List<ELanguage> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("LanguageLookUpId", "LanguageLookUpId");
                    bcc.ColumnMappings.Add("MongoLanguageLookUpId", "MongoLanguageLookUpId");
                    bcc.ColumnMappings.Add("Preferred", "Preferred");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactLanguage";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveMode(List<EMode> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;

                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("CommModeLookUpId", "CommModeLookUpId");
                    bcc.ColumnMappings.Add("MongoCommModeId", "MongoCommModeId");
                    bcc.ColumnMappings.Add("Preferred", "Preferred");
                    bcc.ColumnMappings.Add("OptOut", "OptOut");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactMode";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SavePhone(List<EPhone> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("TypeId", "TypeId");
                    bcc.ColumnMappings.Add("MongoCommTypeId", "MongoCommTypeId");
                    bcc.ColumnMappings.Add("Number", "Number");
                    bcc.ColumnMappings.Add("IsText", "IsText");
                    bcc.ColumnMappings.Add("PhonePreferred", "PhonePreferred");
                    bcc.ColumnMappings.Add("TextPreferred", "TextPreferred");
                    bcc.ColumnMappings.Add("OptOut", "OptOut");
                    bcc.ColumnMappings.Add("Delete", "Delete");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactPhone";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveTOD(List<ETimeOfDay> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("TimeOfDayLookUpId", "TimeOfDayLookUpId");
                    bcc.ColumnMappings.Add("MongoTimeOfDayId", "MongoTimeOfDayId");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactTimeOfDay";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveWeekDay(List<EWeekDay> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("WeekDay", "WeekDay");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                    bcc.DestinationTableName = "RPT_ContactWeekDay";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveRecentList(List<ERecentList> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("ContactId", "ContactId");
                    bcc.ColumnMappings.Add("MongoContactId", "MongoContactId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");

                    bcc.DestinationTableName = "RPT_ContactRecentList";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        public override void Execute()
        {
            try
            {
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading contacts.", IsError = false });
                ConcurrentBag<MEContact> contacts;
                ConcurrentBag<EContact> econtacts = new ConcurrentBag<EContact>();
                ConcurrentBag<EAddress> eAddresses = new ConcurrentBag<EAddress>();
                ConcurrentBag<EEmail> eEmails = new ConcurrentBag<EEmail>();
                ConcurrentBag<ELanguage> eLanguage = new ConcurrentBag<ELanguage>();
                ConcurrentBag<EMode> eMode = new ConcurrentBag<EMode>();
                ConcurrentBag<EPhone> ePhone = new ConcurrentBag<EPhone>();
                ConcurrentBag<ETimeOfDay> eTimeOfDays = new ConcurrentBag<ETimeOfDay>();
                ConcurrentBag<EWeekDay> eWeekDays = new ConcurrentBag<EWeekDay>();
                ConcurrentBag<ERecentList> eRecentList = new ConcurrentBag<ERecentList>();

                using (ContactMongoContext cmctx = new ContactMongoContext(Contract))
                {
                    contacts = new ConcurrentBag<MEContact>(cmctx.Contacts.Collection.FindAllAs<MEContact>().ToList());
                }

                Parallel.ForEach(contacts, contact =>
                {
                    try
                    {
                        econtacts.Add(
                            new EContact
                            {
                                MongoPatientId = contact.PatientId.ToString(),
                                MongoId = contact.Id.ToString(),
                                ResourceId = contact.ResourceId,
                                FirstName = contact.FirstName,
                                MiddleName = contact.MiddleName,
                                LastName = contact.LastName,
                                PreferredName = contact.PreferredName,
                                Gender = contact.Gender,
                                Version = Convert.ToInt32(contact.Version),
                                MongoUpdatedBy = contact.UpdatedBy.ToString(),
                                LastUpdatedOn = contact.LastUpdatedOn,
                                MongoRecordCreatedBy = contact.RecordCreatedBy.ToString(),
                                RecordCreatedOn = contact.RecordCreatedOn,
                                Delete = contact.DeleteFlag.ToString(),
                                MongoTimeZone = contact.TimeZoneId.ToString(),
                                TTLDate = contact.TTLDate
                            }
                            );

                        if (contact.Addresses != null)
                        {
                            foreach (var address in contact.Addresses)
                            {
                                eAddresses.Add(
                                    new EAddress
                                    {
                                            MongoId = (string.IsNullOrEmpty(address.Id.ToString()) ? string.Empty : address.Id.ToString()),
                                            MongoCommTypeId = (string.IsNullOrEmpty(address.TypeId.ToString()) ? string.Empty : address.TypeId.ToString()),
                                        Line1 = (string.IsNullOrEmpty(address.Line1) ? string.Empty : address.Line1),
                                        Line2 = (string.IsNullOrEmpty(address.Line2) ? string.Empty : address.Line2),
                                        Line3 = (string.IsNullOrEmpty(address.Line3) ? string.Empty : address.Line3),
                                        City = (string.IsNullOrEmpty(address.City) ? string.Empty : address.City),
                                            MongoStateId = (string.IsNullOrEmpty(address.StateId.ToString()) ? string.Empty : address.StateId.ToString()),
                                        PostalCode = (string.IsNullOrEmpty(address.PostalCode) ? string.Empty : address.PostalCode),
                                            Preferred =  (string.IsNullOrEmpty(address.Preferred.ToString()) ? string.Empty : address.Preferred.ToString()), 
                                            OptOut =  (string.IsNullOrEmpty(address.OptOut.ToString()) ? string.Empty : address.OptOut.ToString()),
                                            Delete = (string.IsNullOrEmpty(address.DeleteFlag.ToString()) ? string.Empty : address.DeleteFlag.ToString()),
                                            MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                            Version = Convert.ToInt32(contact.Version),
                                            MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                            LastUpdatedOn =  contact.LastUpdatedOn, 
                                            MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), 
                                            RecordCreatedOn = contact.RecordCreatedOn, 
                                            TTLDate = contact.TTLDate
                                    }
                                    );
                            }
                        }


                        if (contact.Emails != null)
                        {
                            foreach (var email in contact.Emails)
                            {
                                eEmails.Add(new EEmail{
                                    MongoId = (string.IsNullOrEmpty(email.Id.ToString()) ? string.Empty : email.Id.ToString()),
                                    MongoCommTypeId = (string.IsNullOrEmpty(email.TypeId.ToString()) ? string.Empty : email.TypeId.ToString()), 
                                    Text = (string.IsNullOrEmpty(email.Text) ? string.Empty : email.Text),
                                        Preferred = (string.IsNullOrEmpty(email.Preferred.ToString()) ? string.Empty : email.Preferred.ToString()),
                                    OptOut = (string.IsNullOrEmpty(email.OptOut.ToString()) ? string.Empty : email.OptOut.ToString()), 
                                    Delete = (string.IsNullOrEmpty(email.DeleteFlag.ToString()) ? string.Empty : email.DeleteFlag.ToString()),
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                    Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                    LastUpdatedOn = contact.LastUpdatedOn,
                                    MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn = contact.RecordCreatedOn,
                                    TTLDate = contact.TTLDate 
                                });
                            }
                        }

                        if (contact.Languages != null)
                        {
                            foreach (var lang in contact.Languages)
                            {
                                eLanguage.Add(new ELanguage{
                                        MongoLanguageLookUpId = (string.IsNullOrEmpty(lang.LookUpLanguageId.ToString()) ? string.Empty : lang.LookUpLanguageId.ToString()),
                                    Preferred = (string.IsNullOrEmpty(lang.Preferred.ToString()) ? string.Empty : lang.Preferred.ToString()),
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                        Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                    LastUpdatedOn = contact.LastUpdatedOn,
                                        MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn = contact.RecordCreatedOn,
                                    TTLDate = contact.TTLDate
                                });
                            }
                        }

                        if (contact.Modes != null)
                        {
                            foreach (var mode in contact.Modes)
                            {
                                eMode.Add(new EMode{
                                        MongoCommModeId = (string.IsNullOrEmpty(mode.ModeId.ToString()) ? string.Empty : mode.ModeId.ToString()),
                                    Preferred = (string.IsNullOrEmpty(mode.Preferred.ToString()) ? string.Empty : mode.Preferred.ToString()),
                                    OptOut = (string.IsNullOrEmpty(mode.OptOut.ToString()) ? string.Empty : mode.OptOut.ToString()),
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                        Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                    LastUpdatedOn = contact.LastUpdatedOn,
                                    MongoRecordCreatedBy =  (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn = contact.RecordCreatedOn,
                                    TTLDate = contact.TTLDate
                                    });
                            }
                        }

                        if (contact.Phones != null)
                        {
                            foreach (Phone phone in contact.Phones)
                            {
                                ePhone.Add(new EPhone{
                                    MongoId = (string.IsNullOrEmpty(phone.Id.ToString()) ? string.Empty : phone.Id.ToString()),
                                    MongoCommTypeId = (string.IsNullOrEmpty(phone.TypeId.ToString()) ? string.Empty : phone.TypeId.ToString()), 
                                    Number = (string.IsNullOrEmpty(phone.Number.ToString()) ? string.Empty : phone.Number.ToString()),
                                    IsText = (string.IsNullOrEmpty(phone.IsText.ToString()) ? string.Empty : phone.IsText.ToString()),
                                    PhonePreferred = (string.IsNullOrEmpty(phone.PreferredPhone.ToString()) ? string.Empty : phone.PreferredPhone.ToString()),
                                    TextPreferred = (string.IsNullOrEmpty(phone.PreferredText.ToString()) ? string.Empty : phone.PreferredText.ToString()), 
                                    OptOut = (string.IsNullOrEmpty(phone.OptOut.ToString()) ? string.Empty : phone.OptOut.ToString()),
                                    Delete = (string.IsNullOrEmpty(phone.DeleteFlag.ToString()) ? string.Empty : phone.DeleteFlag.ToString()),
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                    Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                    LastUpdatedOn = contact.LastUpdatedOn,
                                    MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn = contact.RecordCreatedOn,
                                    TTLDate = contact.TTLDate
                                });
                            }
                        }

                        if (contact.TimesOfDays != null)
                        {
                            foreach (var tod in contact.TimesOfDays)
                            {
                                eTimeOfDays.Add(new ETimeOfDay{
                                MongoTimeOfDayId = (string.IsNullOrEmpty(tod.ToString()) ? string.Empty : tod.ToString()),
                                MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                Version = Convert.ToInt32(contact.Version),
                                MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                LastUpdatedOn = contact.LastUpdatedOn,
                                MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                RecordCreatedOn = contact.RecordCreatedOn,
                                TTLDate = contact.TTLDate
                                });
                            }
                        }

                        if (contact.WeekDays != null)
                        {
                            foreach (var wd in contact.WeekDays)
                            {
                                eWeekDays.Add(new EWeekDay{
                                    WeekDay =  wd,
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                    Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), 
                                    LastUpdatedOn =  contact.LastUpdatedOn,
                                    MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn = contact.RecordCreatedOn,
                                    TTLDate = contact.TTLDate
                                });
                            }
                        }

                        if (contact.RecentList != null)
                        {
                            foreach (ObjectId rec in contact.RecentList)
                            {
                                eRecentList.Add(new ERecentList {
                                    MongoId =  rec.ToString(), 
                                    MongoContactId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
                                    Version = Convert.ToInt32(contact.Version),
                                    MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()),
                                    LastUpdatedOn = contact.LastUpdatedOn,
                                    MongoRecordCreatedBy = (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
                                    RecordCreatedOn =  contact.RecordCreatedOn
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs
                        {
                            Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace,
                            IsError = true
                        });
                    }
                });

                SaveContact(econtacts.ToList());
                SaveAddress(eAddresses.ToList());
                SaveEmail(eEmails.ToList());
                SaveLanguage(eLanguage.ToList());
                SaveMode(eMode.ToList());
                SavePhone(ePhone.ToList());
                SaveTOD(eTimeOfDays.ToList());
                SaveWeekDay(eWeekDays.ToList());
                SaveRecentList(eRecentList.ToList());
            }
            catch (Exception ex)
            {
                OnDocColEvent(new ETLEventArgs
                {
                    Message = "[" + Contract + "] LoadContacts():Error" + ex.Message + ": " + ex.StackTrace,
                    IsError = true
                });
            }
        }
    }
}

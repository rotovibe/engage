﻿using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.CareMember;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientObservation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Services;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System.Diagnostics;
using Action = Phytel.API.DataDomain.Program.MongoDB.DTO.Action;
using CommMode = Phytel.API.DataDomain.Contact.DTO.CommMode;
using Language = Phytel.API.DataDomain.Contact.DTO.Language;
using Objective = Phytel.API.DataDomain.LookUp.DTO.Objective;
using TimeZone = Phytel.API.DataDomain.LookUp.DTO.TimeZone;
using System.Windows.Forms;


namespace Phytel.Data.ETL
{
    public class ETLProcessor
    {
        private int _exponent = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelProcess"]);
        string contract = "InHealth001";
        private List<SpawnElementHash> _spawnElementDict = new List<SpawnElementHash>(); 

        public void Rebuild()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Truncate/Delete SQL databases
            SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_TruncateTables", new ParameterCollection());

            RegisterClasses();
            LoadUsers(contract);
            LoadLookUps(contract);
            LoadGoalAttributes(contract);
            LoadObservations(contract);

            LoadPatients(contract);
            LoadPatientSystems(contract);
            LoadPatientNotes(contract);
            LoadPatientProblems(contract);
            LoadPatientObservations(contract);

            LoadContacts(contract);

            LoadCareMembers(contract);
            LoadPatientUsers(contract);

            LoadPatientGoals(contract);
            LoadPatientBarriers(contract);
            LoadPatientInterventions(contract);
            LoadPatientTasks(contract);

            LoadPatientPrograms(contract);
            LoadPatientProgramResponses(contract);
            LoadPatientProgramAttributes(contract);
            ProcessSpawnElements();
            stopWatch.Stop();
            MessageBox.Show("Total Processing time : " + stopWatch.Elapsed.Minutes + " minutes.");
        }

        private void RegisterClasses()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
                {
                    BsonClassMap.RegisterClassMap<MELookup>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEProgramAttribute)) == false)
                {
                    BsonClassMap.RegisterClassMap<MEProgramAttribute>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Problem)) == false)
                {
                    BsonClassMap.RegisterClassMap<Problem>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Objective)) == false)
                {
                    BsonClassMap.RegisterClassMap<Objective>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Category)) == false)
                {
                    BsonClassMap.RegisterClassMap<Category>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Phytel.API.DataDomain.LookUp.DTO.CommMode)) == false)
                {
                    BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.CommMode>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(State)) == false)
                {
                    BsonClassMap.RegisterClassMap<State>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(TimesOfDay)) == false)
                {
                    BsonClassMap.RegisterClassMap<TimesOfDay>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(TimeZone)) == false)
                {
                    BsonClassMap.RegisterClassMap<TimeZone>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(CommType)) == false)
                {
                    BsonClassMap.RegisterClassMap<CommType>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Phytel.API.DataDomain.LookUp.DTO.Language)) == false)
                {
                    BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.Language>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(FocusArea)) == false)
                {
                    BsonClassMap.RegisterClassMap<FocusArea>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Source)) == false)
                {
                    BsonClassMap.RegisterClassMap<Source>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(BarrierCategory)) == false)
                {
                    BsonClassMap.RegisterClassMap<BarrierCategory>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(InterventionCategory)) == false)
                {
                    BsonClassMap.RegisterClassMap<InterventionCategory>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ObservationType)) == false)
                {
                    BsonClassMap.RegisterClassMap<ObservationType>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(CareMemberType)) == false)
                {
                    BsonClassMap.RegisterClassMap<CareMemberType>();
                }
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(CodingSystem)) == false)
                {
                    BsonClassMap.RegisterClassMap<CodingSystem>();
                }
            }
            catch { }
            #endregion

        }

        private void LoadCareMembers(string ctr)
        {
            using (CareMemberMongoContext cmctx = new CareMemberMongoContext(ctr))
            {
                List<MECareMember> members = cmctx.CareMembers.Collection.FindAllAs<MECareMember>().ToList();

                foreach (MECareMember mem in members)
                {
                    ParameterCollection parms = new ParameterCollection();
                    
                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(mem.Id.ToString()) ? string.Empty : mem.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(mem.PatientId.ToString()) ? string.Empty : mem.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(mem.ContactId.ToString()) ? string.Empty : mem.ContactId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TypeMongoId", (string.IsNullOrEmpty(mem.TypeId.ToString()) ? string.Empty : mem.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Primary", (string.IsNullOrEmpty(mem.Primary.ToString()) ? string.Empty : mem.Primary.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(mem.UpdatedBy.ToString()) ? string.Empty : mem.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(mem.LastUpdatedOn.ToString()) ? string.Empty : mem.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(mem.RecordCreatedBy.ToString()) ? string.Empty : mem.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(mem.RecordCreatedOn.ToString()) ? string.Empty : mem.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(mem.Version.ToString()) ? string.Empty : mem.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(mem.DeleteFlag.ToString()) ? string.Empty : mem.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(mem.TTLDate.ToString()) ? string.Empty : mem.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (mem.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", mem.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SaveCareMember", parms);
                }
            }
        }

        private void LoadContacts(string ctr)
        {
            using (ContactMongoContext cmctx = new ContactMongoContext(ctr))
            {
                List<MEContact> contacts = cmctx.Contacts.Collection.FindAllAs<MEContact>().ToList();
            
                Parallel.ForEach(contacts, new ParallelOptions{MaxDegreeOfParallelism= Environment.ProcessorCount * _exponent}, contact =>
                //foreach (MEContact contact in contacts)
                {
                    if (contact.PatientId != null)
                    {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(contact.PatientId.ToString()) ? string.Empty : contact.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ResourceId", (string.IsNullOrEmpty(contact.ResourceId) ? string.Empty : contact.ResourceId), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@FirstName", (string.IsNullOrEmpty(contact.FirstName) ? string.Empty : contact.FirstName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@MiddleName", (string.IsNullOrEmpty(contact.MiddleName) ? string.Empty : contact.MiddleName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@LastName", (string.IsNullOrEmpty(contact.LastName) ? string.Empty : contact.LastName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@PreferredName", (string.IsNullOrEmpty(contact.PreferredName) ? string.Empty : contact.PreferredName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@Gender", (string.IsNullOrEmpty(contact.Gender) ? string.Empty : contact.Gender), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(contact.LastUpdatedOn.ToString()) ? string.Empty : contact.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(contact.RecordCreatedOn.ToString()) ? string.Empty : contact.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(contact.DeleteFlag.ToString()) ? string.Empty : contact.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeZone", (string.IsNullOrEmpty(contact.TimeZoneId.ToString()) ? string.Empty : contact.TimeZoneId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TTLDate", (string.IsNullOrEmpty(contact.TTLDate.ToString()) ? string.Empty : contact.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (contact.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", contact.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContact", parms);

                        if (contact.Addresses != null)
                        {
                        foreach (Address address in contact.Addresses)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(address.Id.ToString()) ? string.Empty : address.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TypeMongoId", (string.IsNullOrEmpty(address.TypeId.ToString()) ? string.Empty : address.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Line1", (string.IsNullOrEmpty(address.Line1) ? string.Empty : address.Line1), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Line2", (string.IsNullOrEmpty(address.Line2) ? string.Empty : address.Line2), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Line3", (string.IsNullOrEmpty(address.Line3) ? string.Empty : address.Line3), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@City", (string.IsNullOrEmpty(address.City) ? string.Empty : address.City), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateMongoId", (string.IsNullOrEmpty(address.StateId.ToString()) ? string.Empty : address.StateId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PostalCode", (string.IsNullOrEmpty(address.PostalCode) ? string.Empty : address.PostalCode), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Preferred", (string.IsNullOrEmpty(address.Preferred.ToString()) ? string.Empty : address.Preferred.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OptOut", (string.IsNullOrEmpty(address.OptOut.ToString()) ? string.Empty : address.OptOut.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(address.DeleteFlag.ToString()) ? string.Empty : address.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactAddress", parms);
                            }
                        }

                        if (contact.Emails != null)
                        {
                        foreach (Email email in contact.Emails)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(email.Id.ToString()) ? string.Empty : email.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TypeMongoId", (string.IsNullOrEmpty(email.TypeId.ToString()) ? string.Empty : email.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Text", (string.IsNullOrEmpty(email.Text) ? string.Empty : email.Text), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Preferred", (string.IsNullOrEmpty(email.Preferred.ToString()) ? string.Empty : email.Preferred.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OptOut", (string.IsNullOrEmpty(email.OptOut.ToString()) ? string.Empty : email.OptOut.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(email.DeleteFlag.ToString()) ? string.Empty : email.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactEmail", parms);
                            }
                        }

                        if (contact.Languages != null)
                        {
                        foreach (Language lang in contact.Languages)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@LanguageLookUpMongoId", (string.IsNullOrEmpty(lang.LookUpLanguageId.ToString()) ? string.Empty : lang.LookUpLanguageId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Preferred", (string.IsNullOrEmpty(lang.Preferred.ToString()) ? string.Empty : lang.Preferred.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactLanguage", parms);
                            }
                        }

                        if (contact.Modes != null)
                        {
                        foreach (CommMode mode in contact.Modes)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@ModeLookUpMongoId", (string.IsNullOrEmpty(mode.ModeId.ToString()) ? string.Empty : mode.ModeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Preferred", (string.IsNullOrEmpty(mode.Preferred.ToString()) ? string.Empty : mode.Preferred.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OptOut", (string.IsNullOrEmpty(mode.OptOut.ToString()) ? string.Empty : mode.OptOut.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactMode", parms);
                            }
                        }

                        if (contact.Phones != null)
                        {
                        foreach (Phone phone in contact.Phones)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(phone.Id.ToString()) ? string.Empty : phone.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TypeMongoId", (string.IsNullOrEmpty(phone.TypeId.ToString()) ? string.Empty : phone.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Number", (string.IsNullOrEmpty(phone.Number.ToString()) ? string.Empty : phone.Number.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@IsText", (string.IsNullOrEmpty(phone.IsText.ToString()) ? string.Empty : phone.IsText.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PhonePreferred", (string.IsNullOrEmpty(phone.PreferredPhone.ToString()) ? string.Empty : phone.PreferredPhone.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TextPreferred", (string.IsNullOrEmpty(phone.PreferredText.ToString()) ? string.Empty : phone.PreferredText.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OptOut", (string.IsNullOrEmpty(phone.OptOut.ToString()) ? string.Empty : phone.OptOut.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(phone.DeleteFlag.ToString()) ? string.Empty : phone.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactPhone", parms);
                            }
                        }

                        if (contact.TimesOfDays != null)
                        {
                            foreach (ObjectId tod in contact.TimesOfDays)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@TimeOfDayLookUpMongoId", (string.IsNullOrEmpty(tod.ToString()) ? string.Empty : tod.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactTimeOfDay", parms);
                            }
                        }

                        if (contact.WeekDays != null)
                        {
                            foreach (int wd in contact.WeekDays)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@WeekDay", wd, SqlDbType.Int, ParameterDirection.Input, 8));
                           parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactWeekDay", parms);
                            }
                        }

                        if (contact.RecentList != null)
                        {
                        foreach (ObjectId rec in contact.RecentList)
                            {
                                parms.Clear();
                            parms.Add(new Parameter("@MongoID", rec, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveContactRecentList", parms);
                            }
                        }
                    }
                });
            }
        }

        private void LoadGoalAttributes(string ctr)
        {
            using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
            {
                List<MEAttributeLibrary> attributes = pgctx.AttributesLibrary.Collection.FindAllAs<MEAttributeLibrary>().ToList();
                foreach (MEAttributeLibrary att in attributes)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", att.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(att.Name) ? string.Empty : att.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(att.Type.ToString()) ? string.Empty : att.Type.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ControlType", (string.IsNullOrEmpty(att.ControlType.ToString()) ? string.Empty : att.ControlType.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Order", (string.IsNullOrEmpty(att.Order.ToString()) ? string.Empty : att.Order.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Required", (string.IsNullOrEmpty(att.Required.ToString()) ? string.Empty : att.Required.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(att.Version.ToString()) ? string.Empty : att.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(att.UpdatedBy.ToString()) ? string.Empty : att.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(att.LastUpdatedOn.ToString()) ? string.Empty : att.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(att.RecordCreatedBy.ToString()) ? string.Empty : att.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(att.RecordCreatedOn.ToString()) ? string.Empty : att.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(att.DeleteFlag.ToString()) ? string.Empty : att.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(att.TTLDate.ToString()) ? string.Empty : att.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (att.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", att.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveGoalAttribute", parms);

                    foreach (KeyValuePair<int, string> option in att.Options)
                    {
                        parms.Clear();
                        
                        parms.Add(new Parameter("@Key", option.Key, SqlDbType.Int, ParameterDirection.Input, 32));
                        parms.Add(new Parameter("@Value", option.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@GoalAttributeMongoId", att.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveGoalAttributeOption", parms);
                    }
                }
            }
        }

        private void LoadLookUps(string ctr)
        {
            using (LookUpMongoContext lmctx = new LookUpMongoContext(ctr))
            {
                List<MELookup> lookups = lmctx.LookUps.Collection.FindAll().ToList();

                Parallel.ForEach(lookups, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount*3},
                    lookup =>
                        //foreach (MELookup lookup in lookups)
                    {
                        switch (lookup.Type)
                        {
                            case LookUpType.BarrierCategory:
                                LoadBarrierCategories(lookup);
                                break;
                            case LookUpType.CareMemberType:
                                LoadCareMemberTypes(lookup);
                                break;
                            case LookUpType.Category:
                                LoadCategories(lookup);
                                break;
                            case LookUpType.CodingSystem:
                                LoadCodingSystems(lookup);
                                break;
                            case LookUpType.CommMode:
                                LoadCommModes(lookup);
                                break;
                            case LookUpType.CommType:
                                LoadCommTypes(lookup);
                                break;
                            case LookUpType.FocusArea:
                                LoadFocusAreas(lookup);
                                break;
                            case LookUpType.InterventionCategory:
                                LoadInterventionCategories(lookup);
                                break;
                            case LookUpType.Language:
                                LoadLanguages(lookup);
                                break;
                            case LookUpType.Objective:
                                LoadObjectives(lookup);
                                break;
                            case LookUpType.ObservationType:
                                LoadObservationTypes(lookup);
                                break;
                            case LookUpType.Problem:
                                LoadProblems(lookup);
                                break;
                            case LookUpType.Source:
                                LoadSources(lookup);
                                break;
                            case LookUpType.State:
                                LoadStates(lookup);
                                break;
                            case LookUpType.TimesOfDay:
                                LoadTimesOfDays(lookup);
                                break;
                            case LookUpType.TimeZone:
                                LoadTimeZones(lookup);
                                break;
                            default:
                                break;
                        }
                    });

                LinkObjectiveCategories(lookups[0]);
                LinkCommTypeCommModes(lookups[10]);

            }
        }

        #region LookUp methods
        private void LoadBarrierCategories(MELookup lookup)
        {
            foreach(LookUpBase lbase in lookup.Data)
            {
                BarrierCategory bc = (BarrierCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", bc.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", bc.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveBarrierCategoryLookUp", parms);
            }
 
        }

        private void LoadCareMemberTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CareMemberType cmt = (CareMemberType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", cmt.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmt.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCareMemberTypeLookUp", parms);
            }
        }

        private void LoadCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Category cat = (Category)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", cat.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cat.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCategoryLookUp", parms);
            }
        }

        private void LoadCodingSystems(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CodingSystem cs = (CodingSystem)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", cs.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cs.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCodingSystemLookUp", parms);
            }
        }

        private void LoadCommModes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Phytel.API.DataDomain.LookUp.DTO.CommMode cmm = (Phytel.API.DataDomain.LookUp.DTO.CommMode)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", cmm.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmm.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCommModeLookUp", parms);
            }
        }

        private void LoadCommTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CommType cmt = (CommType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", cmt.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmt.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCommTypeLookUp", parms);
             }
        }

        private void LoadFocusAreas(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                FocusArea fa = (FocusArea)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", fa.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", fa.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveFocusAreaLookUp", parms);
            }
        }

        private void LoadInterventionCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                InterventionCategory ic = (InterventionCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", ic.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", ic.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveInterventionCategoryLookUp", parms);
            }
        }

        private void LoadLanguages(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Phytel.API.DataDomain.LookUp.DTO.Language la = (Phytel.API.DataDomain.LookUp.DTO.Language)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", la.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", la.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", la.Code, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Active", la.Active, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveLanguageLookUp", parms);
            }
        }

        private void LoadObjectives(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Objective o = (Objective)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", o.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", o.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Description", o.Description, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveObjectiveLookUp", parms);
            }
        }

        private void LoadObservationTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                ObservationType ot = (ObservationType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", ot.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", ot.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveObservationTypeLookUp", parms);
            }
        }

        private void LoadProblems(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Problem prb = (Problem)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(prb.Type) ? string.Empty : prb.Type), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@CodeSystem", (string.IsNullOrEmpty(prb.CodeSystem) ? string.Empty : prb.CodeSystem), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(prb.Code) ? string.Empty : prb.Code), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Active", prb.Active, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Default", prb.DefaultFeatured, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@DefaultLevel", (string.IsNullOrEmpty(prb.DefaultLevel.ToString()) ? string.Empty : prb.DefaultLevel.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveProblemLookUp", parms);
            }
        }

        private void LoadSources(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Source src = (Source)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", src.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", src.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveSourceLookUp", parms);
            }
        }

        private void LoadStates(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                State st = (State)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", st.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", st.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", st.Code, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveStateLookUp", parms);
            }
        }

        private void LoadTimesOfDays(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                TimesOfDay tod = (TimesOfDay)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", tod.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tod.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveTimesOfDayLookUp", parms);
            }
        }

        private void LoadTimeZones(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                TimeZone tz = (TimeZone)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", tz.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tz.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Default", tz.Default, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveTimeZoneLookUp", parms);
            }
        }

        private void LinkCommTypeCommModes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CommType cmt = (CommType)lbase;
                foreach (ObjectId mode in cmt.CommModes)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@CommTypeMongoId", cmt.DataId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@CommModeMongoId", mode.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveCommTypeCommMode", parms);
                }
            }
        }

        private void LinkObjectiveCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Objective o = (Objective)lbase;
                foreach (ObjectId cat in o.CategoryIds)
                {
                    ParameterCollection parms = new ParameterCollection();
                    parms.Add(new Parameter("@ObjectiveMongoId", o.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@CategoryMongoId", cat.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveObjectiveCategory", parms);
                }
            }
        }
        #endregion

        private void LoadObservations(string ctr)
        {
            using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
            {
                List<MEObservation> observations = poctx.Observations.Collection.FindAllAs<MEObservation>().ToList();
                
                foreach (MEObservation obs in observations)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", obs.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(obs.Code) ? string.Empty : obs.Code), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@CommonName", (string.IsNullOrEmpty(obs.CommonName) ? string.Empty : obs.CommonName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@CodingSystemId", (string.IsNullOrEmpty(obs.CodingSystemId.ToString()) ? string.Empty : obs.CodingSystemId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@GroupId", (string.IsNullOrEmpty(obs.GroupId.ToString()) ? string.Empty : obs.GroupId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(obs.DeleteFlag.ToString()) ? string.Empty : obs.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Description", (string.IsNullOrEmpty(obs.Description) ? string.Empty : obs.Description), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@HighValue", (string.IsNullOrEmpty(obs.HighValue.ToString()) ? string.Empty : obs.HighValue.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(obs.LastUpdatedOn.ToString()) ? string.Empty : obs.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LowValue", (string.IsNullOrEmpty(obs.LowValue.ToString()) ? string.Empty : obs.LowValue.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ObservationTypeMongoId", (string.IsNullOrEmpty(obs.ObservationTypeId.ToString()) ? string.Empty : obs.ObservationTypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Order", (string.IsNullOrEmpty(obs.Order.ToString()) ? string.Empty : obs.Order.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(obs.Source) ? string.Empty : obs.Source), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Standard", (string.IsNullOrEmpty(obs.Standard.ToString()) ? string.Empty : obs.Standard.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(obs.Status.ToString()) ? string.Empty : obs.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(obs.TTLDate.ToString()) ? string.Empty : obs.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Units", (string.IsNullOrEmpty(obs.Units) ? string.Empty : obs.Units.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", obs.UpdatedBy == null ? string.Empty : obs.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", obs.RecordCreatedBy == null ? string.Empty : obs.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", obs.RecordCreatedOn == null ? string.Empty : obs.RecordCreatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(obs.Version.ToString()) ? string.Empty : obs.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (obs.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", obs.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveObservation", parms);
                }
            }
        }

        private void LoadPatients(string ctr)
        {
            using (PatientMongoContext pmctx = new PatientMongoContext(ctr))
            {
                List<MEPatient> patients = pmctx.Patients.Collection.FindAllAs<MEPatient>().ToList();

                //Parallel.ForEach(patients, new ParallelOptions{MaxDegreeOfParallelism= Environment.ProcessorCount * _exponent}, patient =>
                foreach (MEPatient patient in patients)
                {
                    ParameterCollection parms = new ParameterCollection();
                    
                    parms.Add(new Parameter("@MongoID", patient.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@FirstName", patient.FirstName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@MiddleName", patient.MiddleName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@LastName", patient.LastName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@PreferredName", patient.PreferredName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@Suffix", patient.Suffix, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@DateOfBirth", patient.DOB, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Gender", patient.Gender, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Priority", patient.Priority, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", patient.Version, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", patient.UpdatedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", patient.LastUpdatedOn, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", patient.RecordCreatedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", patient.RecordCreatedOn, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", patient.DeleteFlag, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(patient.Background) ? string.Empty : patient.Background), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@DisplayPatientSystemMongoId", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TTLDate", (string.IsNullOrEmpty(patient.TTLDate.ToString()) ? string.Empty : patient.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (patient.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", patient.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SavePatient", parms);
                }//);
            }
        }

        private void LoadPatientBarriers(string ctr)
        {
            using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
            {
                List<MEPatientBarrier> barriers = pgctx.PatientBarriers.Collection.FindAllAs<MEPatientBarrier>().ToList();

                Parallel.ForEach(barriers, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, bar =>
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(bar.Id.ToString()) ? string.Empty : bar.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(bar.PatientGoalId.ToString()) ? string.Empty : bar.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(bar.DeleteFlag.ToString()) ? string.Empty : bar.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(bar.LastUpdatedOn.ToString()) ? string.Empty : bar.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(bar.RecordCreatedBy.ToString()) ? string.Empty : bar.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(bar.RecordCreatedOn.ToString()) ? string.Empty : bar.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(bar.Status.ToString()) ? string.Empty : bar.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StatusDate", (string.IsNullOrEmpty(bar.StatusDate.ToString()) ? string.Empty : bar.StatusDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(bar.StartDate.ToString()) ? string.Empty : bar.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(bar.TTLDate.ToString()) ? string.Empty : bar.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(bar.UpdatedBy.ToString()) ? string.Empty : bar.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(bar.Version.ToString()) ? string.Empty : bar.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(bar.Name) ? string.Empty : bar.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientBarrier", parms);
                });
            }

        }

        private void LoadPatientGoals(string ctr)
        {
            using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
            {
                List<MEPatientGoal> goals = pgctx.PatientGoals.Collection.FindAllAs<MEPatientGoal>().ToList();

                Parallel.ForEach(goals, new ParallelOptions{MaxDegreeOfParallelism= Environment.ProcessorCount * _exponent}, goal =>
                //foreach (MEPatientGoal goal in goals)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@_id", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_patientId", (string.IsNullOrEmpty(goal.PatientId.ToString()) ? string.Empty : goal.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(goal.Name) ? string.Empty : goal.Name), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Description", goal.Description == null ? string.Empty : goal.Description, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(goal.StartDate.ToString()) ? string.Empty : goal.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@EndDate", (string.IsNullOrEmpty(goal.EndDate.ToString()) ? string.Empty : goal.EndDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(goal.Status.ToString()) ? string.Empty : goal.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@StatusDate", (string.IsNullOrEmpty(goal.StatusDate.ToString()) ? string.Empty : goal.StatusDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(goal.SourceId.ToString()) ? string.Empty : goal.SourceId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(goal.Type.ToString()) ? string.Empty : goal.Type.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TargetDate", (string.IsNullOrEmpty(goal.TargetDate.ToString()) ? string.Empty : goal.TargetDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TargetValue", goal.TargetValue == null ? string.Empty : goal.TargetValue.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn != null ? string.Empty : goal.LastUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_updatedBy", goal.UpdatedBy != null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_recordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", goal.DeleteFlag.ToString() , SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", goal.TTLDate != null ? string.Empty : goal.TTLDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (goal.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", goal.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientGoal", parms);

                    if (goal.Attributes != null)
                    {
                        foreach (MAttribute att in goal.Attributes)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@_patientGoalId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_goalAttributeId", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            
                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientGoalAttribute", parms);

                            if (att.Values != null)
                            {
                                foreach(string value in att.Values)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@GoalAttributeMongoId", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Value", (string.IsNullOrEmpty(value) ? string.Empty : value), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientGoalAttributeValue", parms);
                                }
                            }
                        }
                    }

                    if (goal.FocusAreaIds != null)
                    {
                        foreach (ObjectId foc in goal.FocusAreaIds)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@FocusAreaMongoId", (string.IsNullOrEmpty(foc.ToString()) ? string.Empty : foc.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientGoalFocusArea", parms);
                        }
                    }

                    if (goal.ProgramIds != null)
                    {
                        foreach (ObjectId pro in goal.ProgramIds)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ProgramId", (string.IsNullOrEmpty(pro.ToString()) ? string.Empty : pro.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientGoalProgram", parms);
                        }
                    }
                });
            }
        }

        private void LoadPatientInterventions(string ctr)
        {
            using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
            {
                List<MEPatientIntervention> interventions = pgctx.PatientInterventions.Collection.FindAllAs<MEPatientIntervention>().ToList();

                foreach (MEPatientIntervention intervention in interventions)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(intervention.Id.ToString()) ? string.Empty : intervention.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(intervention.PatientGoalId.ToString()) ? string.Empty : intervention.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@CategoryMongoId", (string.IsNullOrEmpty(intervention.CategoryId.ToString()) ? string.Empty : intervention.CategoryId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@AssignedTo", (string.IsNullOrEmpty(intervention.AssignedToId.ToString()) ? string.Empty : intervention.AssignedToId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(intervention.UpdatedBy.ToString()) ? string.Empty : intervention.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(intervention.LastUpdatedOn.ToString()) ? string.Empty : intervention.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(intervention.RecordCreatedBy.ToString()) ? string.Empty : intervention.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(intervention.RecordCreatedOn.ToString()) ? string.Empty : intervention.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(intervention.Version.ToString()) ? string.Empty : intervention.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(intervention.TTLDate.ToString()) ? string.Empty : intervention.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(intervention.DeleteFlag.ToString()) ? string.Empty : intervention.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(intervention.Status.ToString()) ? string.Empty : intervention.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StatusDate", (string.IsNullOrEmpty(intervention.StatusDate.ToString()) ? string.Empty : intervention.StatusDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(intervention.StartDate.ToString()) ? string.Empty : intervention.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Description", (string.IsNullOrEmpty(intervention.Description) ? string.Empty : intervention.Description), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(intervention.Name) ? string.Empty : intervention.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                    if (intervention.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", intervention.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientIntervention", parms);
                
                    if (intervention.BarrierIds != null)
                    {
                        foreach(ObjectId bar in intervention.BarrierIds)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@PatientBarrierMongoId", (string.IsNullOrEmpty(bar.ToString()) ? string.Empty : bar.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientInterventionMongoID", (string.IsNullOrEmpty(intervention.Id.ToString()) ? string.Empty : intervention.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientInterventionBarrier", parms);

                        }
                    }
                }
            }
        }

        private void LoadPatientNotes(string ctr)
        {
            using (PatientNoteMongoContext pnctx = new PatientNoteMongoContext(ctr))
            {
                List<MEPatientNote> notes = pnctx.PatientNotes.Collection.FindAllAs<MEPatientNote>().ToList();

                Parallel.ForEach(notes, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, note =>
                //foreach (MEPatientNote note in notes)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(note.PatientId.ToString()) ? string.Empty : note.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Text", (string.IsNullOrEmpty(note.Text) ? string.Empty : note.Text), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(note.UpdatedBy.ToString()) ? string.Empty : note.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(note.LastUpdatedOn.ToString()) ? string.Empty : note.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(note.RecordCreatedBy.ToString()) ? string.Empty : note.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(note.RecordCreatedOn.ToString()) ? string.Empty : note.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(note.Version.ToString()) ? string.Empty : note.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(note.DeleteFlag.ToString()) ? string.Empty : note.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TTLDate", (string.IsNullOrEmpty(note.TTLDate.ToString()) ? string.Empty : note.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientNote", parms);

                    if (note.ProgramIds != null)
                    {
                        foreach (ObjectId prg in note.ProgramIds)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(prg.ToString()) ? string.Empty : prg.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientNoteMongoId", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientNoteProgram", parms);
                        }
                    }
                });
            }
        }

        private void LoadPatientObservations(string ctr)
            {
            using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
            {
                List<MEPatientObservation> observations = poctx.PatientObservations.Collection.FindAllAs<MEPatientObservation>().ToList();

                foreach(MEPatientObservation obs in observations)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(obs.Id.ToString()) ? string.Empty : obs.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(obs.PatientId.ToString()) ? string.Empty : obs.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Display", (string.IsNullOrEmpty(obs.Display.ToString()) ? string.Empty : obs.Display.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@EndDate", (string.IsNullOrEmpty(obs.EndDate.ToString()) ? string.Empty : obs.EndDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(obs.StartDate.ToString()) ? string.Empty : obs.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(obs.UpdatedBy.ToString()) ? string.Empty : obs.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(obs.LastUpdatedOn.ToString()) ? string.Empty : obs.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(obs.RecordCreatedBy.ToString()) ? string.Empty : obs.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(obs.RecordCreatedOn.ToString()) ? string.Empty : obs.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(obs.Version.ToString()) ? string.Empty : obs.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(obs.DeleteFlag.ToString()) ? string.Empty : obs.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@NumericValue", (string.IsNullOrEmpty(obs.NumericValue.ToString()) ? string.Empty : obs.NumericValue.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ObservationMongoId", (string.IsNullOrEmpty(obs.ObservationId.ToString()) ? string.Empty : obs.ObservationId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(obs.Source) ? string.Empty : obs.Source), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@State", (string.IsNullOrEmpty(obs.State.ToString()) ? string.Empty : obs.State.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(obs.TTLDate.ToString()) ? string.Empty : obs.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Units", (string.IsNullOrEmpty(obs.Units) ? string.Empty : obs.Units), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@AdministeredBy", (string.IsNullOrEmpty(obs.AdministeredBy) ? string.Empty : obs.AdministeredBy), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@NonNumericValue", (string.IsNullOrEmpty(obs.NonNumericValue) ? string.Empty : obs.NonNumericValue), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(obs.Type) ? string.Empty : obs.Type), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (obs.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", obs.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientObservation", parms);
                }
            }
        }

        private void LoadPatientProblems(string ctr)
        {
            using (PatientProblemMongoContext ppctx = new PatientProblemMongoContext(ctr))
            {
                List<MEPatientProblem> problems = ppctx.PatientProblems.Collection.FindAllAs<MEPatientProblem>().ToList();

                Parallel.ForEach(problems, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, problem =>
                //foreach (MEPatientProblem problem in problems)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(problem.Id.ToString()) ? string.Empty : problem.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(problem.PatientID.ToString()) ? string.Empty : problem.PatientID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@ProblemMongoId", (string.IsNullOrEmpty(problem.ProblemID.ToString()) ? string.Empty : problem.ProblemID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Active", (string.IsNullOrEmpty(problem.Active.ToString()) ? string.Empty : problem.Active.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@Featured", (string.IsNullOrEmpty(problem.Featured.ToString()) ? string.Empty : problem.Featured.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@Level", (string.IsNullOrEmpty(problem.Level.ToString()) ? string.Empty : problem.Level.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(problem.UpdatedBy.ToString()) ? string.Empty : problem.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(problem.LastUpdatedOn.ToString()) ? string.Empty : problem.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(problem.RecordCreatedBy.ToString()) ? string.Empty : problem.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(problem.RecordCreatedOn.ToString()) ? string.Empty : problem.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(problem.Version.ToString()) ? string.Empty : problem.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(problem.DeleteFlag.ToString()) ? string.Empty : problem.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(problem.StartDate.ToString()) ? string.Empty : problem.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@EndDate", (string.IsNullOrEmpty(problem.EndDate.ToString()) ? string.Empty : problem.EndDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TTLDate", (string.IsNullOrEmpty(problem.TTLDate.ToString()) ? string.Empty : problem.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (problem.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", problem.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientProblem", parms);
                });
            }
        }

        private void LoadPatientPrograms(string ctr)
        {
            try
            {
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    var programs = pmctx.PatientPrograms.Collection.FindAllAs<MEPatientProgram>().ToList();

                    //foreach (MEPatientProgram prog in programs)
                    Parallel.ForEach(programs, new ParallelOptions{MaxDegreeOfParallelism= Environment.ProcessorCount * _exponent}, prog =>
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoPatientId", prog.PatientId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedBy", prog.AssignedBy == null ? string.Empty : prog.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", prog.AssignedOn == null ? string.Empty : prog.AssignedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedToId", prog.AssignedTo == null ? string.Empty : prog.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", prog.AttributeEndDate == null ? string.Empty : prog.AttributeEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", prog.AttributeStartDate == null ? string.Empty : prog.AttributeStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", prog.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContractProgramId", prog.ContractProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", prog.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityReason", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityStartDate", prog.EligibilityStartDate == null ? string.Empty : prog.EligibilityStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EndDate", prog.EndDate == null ? string.Empty : prog.EndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", prog.Name ?? null, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", prog.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ShortName", prog.ShortName ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", prog.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StartDate", prog.StartDate == null ? string.Empty : prog.StartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", prog.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", prog.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", prog.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", prog.StateUpdatedOn == null ? string.Empty : prog.StateUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_completedBy", prog.CompletedBy == null ? string.Empty: prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", prog.DateCompleted == null ? string.Empty : prog.DateCompleted.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", prog.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", prog.EligibilityEndDate == null ? string.Empty : prog.EligibilityEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoID", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_updatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn == null ? string.Empty : prog.LastUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));                            
                            parms.Add(new Parameter("@_recordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", prog.TTLDate == null ? string.Empty : prog.TTLDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            //
                            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var patientProgramId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT",
                                "spPhy_SavePatientProgram", parms);


                            if (patientProgramId != null && prog.Spawn != null && prog.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, planElementId, prog.Id.ToString(), prog.Spawn);
                                RegisterSpawnElement(prog.Spawn, prog.Id.ToString(), (int)patientProgramId);
                            }

                            LoadPatientProgramModules(ctr, patientProgramId, prog.Modules);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, prog.Id.ToString());
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadPatientProgramAttributes(string ctr)
        {
            try
            {
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    var programAttributes = pmctx.ProgramAttributes.Collection.FindAllAs<MEProgramAttribute>().ToList();

                    //foreach (MEProgramAttribute prog in programAttributes)
                    Parallel.ForEach(programAttributes, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, prog =>
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@_id", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_planElementId", prog.PlanElementId == null ? string.Empty : prog.PlanElementId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            // look into
                            parms.Add(new Parameter("@Completed", prog.Completed == null ? string.Empty : prog.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@DidNotEnrollReason", prog.DidNotEnrollReason ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligibility", prog.Eligibility == null ? string.Empty : prog.Eligibility.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enrollment", prog.Enrollment == null ? string.Empty : prog.Enrollment.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@GraduatedFlag", prog.GraduatedFlag == null ? string.Empty : prog.GraduatedFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@InelligibleReason", prog.IneligibleReason == null ? string.Empty : prog.IneligibleReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Lock", prog.Locked == null ? string.Empty : prog.Locked.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OptOut", prog.OptOut == null ? string.Empty : prog.OptOut.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@OverrideReason", prog.OverrideReason == null ? string.Empty : prog.OverrideReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Population", prog.Population == null ? string.Empty : prog.Population.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RemovedReason", prog.RemovedReason == null ? string.Empty : prog.RemovedReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", prog.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_updatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn == null ? string.Empty : prog.LastUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_recordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_completedBy", prog.CompletedBy == null ? string.Empty : prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", prog.DateCompleted == null ? string.Empty : prog.DateCompleted.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", prog.TTLDate == null ? string.Empty : prog.TTLDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));


                            var patientProgramId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT",
                                "spPhy_SavePatientProgramAttribute", parms);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, prog.Id.ToString());
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadPatientProgramModules(string ctr, object patientProgramId, List<Module> list)
        {
            try
            {
                    foreach (Module mod in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@PatientProgramId", patientProgramId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_programId", mod.ProgramId == null ? string.Empty : mod.ProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedBy", mod.AssignedBy == null ? string.Empty : mod.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", mod.AssignedOn == null ? string.Empty : mod.AssignedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedToId", mod.AssignedTo == null ? string.Empty : mod.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", mod.AttributeEndDate == null ? string.Empty : mod.AttributeEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", mod.AttributeStartDate == null ? string.Empty : mod.AttributeStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", mod.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", mod.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@_previous", mod.Previous == null ? string.Empty : mod.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_next", mod.Next == null ? string.Empty : mod.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@EligibilityStartDate", mod.EligibilityStartDate == null ? string.Empty : mod.EligibilityStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", mod.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", mod.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", mod.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", mod.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", mod.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", mod.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", mod.StateUpdatedOn == null ? string.Empty : mod.StateUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_completedBy", mod.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", mod.DateCompleted == null ? string.Empty : mod.DateCompleted.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", mod.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", mod.EligibilityEndDate == null ? string.Empty : mod.EligibilityEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_id", mod.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            //
                            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var patientProgramModuleId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientProgramModule", parms);

                            if (patientProgramModuleId != null && mod.Spawn != null && mod.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, patientProgramModuleId, mod.Id.ToString(), mod.Spawn);
                                RegisterSpawnElement(mod.Spawn, mod.Id.ToString(), (int)patientProgramModuleId);
                            }

                            LoadPatientProgramActions(ctr, patientProgramModuleId, mod.Actions);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, mod.Id.ToString());
                        }
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadPatientProgramActions(string ctr, object patientProgramModuleId, List<Action> list)
        {
            try
            {
                    foreach (Action act in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@_id", act.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_moduleId", act.ModuleId == null ? string.Empty : act.ModuleId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientProgramModuleId", patientProgramModuleId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedBy", act.AssignedBy == null ? string.Empty : act.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", act.AssignedOn == null ? string.Empty : act.AssignedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_assignedToId", act.AssignedTo == null ? string.Empty : act.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", act.AttributeEndDate == null ? string.Empty : act.AttributeEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", act.AttributeStartDate == null ? string.Empty : act.AttributeStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", act.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", act.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityStartDate", act.EligibilityStartDate == null ? string.Empty : act.EligibilityStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", act.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", act.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", act.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", act.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", act.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", act.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", act.StateUpdatedOn == null ? string.Empty : act.StateUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_completedBy", act.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", act.DateCompleted == null ? string.Empty : act.DateCompleted.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", act.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", act.EligibilityEndDate == null ? string.Empty : act.EligibilityEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_previous", act.Previous == null ? string.Empty : act.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_next", act.Next == null ? string.Empty : act.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var patientProgramActionId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientProgramModuleAction", parms);

                            if (patientProgramActionId != null && act.Spawn != null && act.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, patientProgramActionId, act.Id.ToString(), act.Spawn);
                                RegisterSpawnElement(act.Spawn, act.Id.ToString(), (int)patientProgramActionId);
                            }

                            LoadPatientProgramSteps(ctr, patientProgramActionId, act.Steps);
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, act.Id.ToString());
                        }
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadPatientProgramSteps(string ctr, object patientProgramActionId, List<Step> list)
        {
            try
            {
                    foreach (Step step in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@_id", step.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_actionId", step.ActionId == null ? string.Empty : step.ActionId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ActionId", patientProgramActionId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StepTypeId", step.StepTypeId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Header", step.Header ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 100));
                            parms.Add(new Parameter("@SelectedResponseId", step.SelectedResponseId == null ? string.Empty : step.SelectedResponseId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ControlType", step.ControlType.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SelectType", step.SelectType.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@IncludeTime", step.IncludeTime.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Question", step.Question ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@Title", step.Title ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 100));
                            parms.Add(new Parameter("@Description", step.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@Notes", step.Notes ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@Text", step.Text ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@Status", step.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            // inherited fields
                            parms.Add(new Parameter("@AttributeEndDate", step.AttributeEndDate == null ? string.Empty : step.AttributeEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", step.AttributeStartDate == null ? string.Empty : step.AttributeStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", step.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityStartDate", step.EligibilityStartDate == null ? string.Empty : step.EligibilityStartDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", step.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", step.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", step.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", step.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", step.StateUpdatedOn == null ? string.Empty : step.StateUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_completedBy", step.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", step.DateCompleted == null ? string.Empty : step.DateCompleted.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", step.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", step.EligibilityEndDate == null ? string.Empty : step.EligibilityEndDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var StepId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientProgramModuleActionStep", parms);

                            if (StepId != null && step.Spawn != null && step.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, StepId, step.Id.ToString(), step.Spawn);
                                RegisterSpawnElement(step.Spawn, step.Id.ToString(), (int)StepId);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, step.Id.ToString());
                        }
                    }
            }
            catch
            {
                throw;
            }
        }

        private void LoadPatientProgramResponses(string ctr)
        {
            try
            {
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    var responses = pmctx.PatientProgramResponses.Collection.FindAllAs<MEPatientProgramResponse>().ToList();
                    
                    Parallel.ForEach(responses, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, resp =>
                    //foreach (MEPatientProgramResponse resp in responses)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@_id", resp.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_actionId", resp.ActionId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", resp.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Text", resp.Text == null ? string.Empty : resp.Text, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_stepId", resp.StepId == null ? string.Empty : resp.StepId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Value", resp.Value == null ? string.Empty : resp.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Nominal", resp.Nominal.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Required", resp.Required.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_nextStepId", resp.NextStepId == null ? string.Empty : resp.NextStepId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Selected", resp.Selected.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_stepSourceId", resp.StepSourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@_updatedBy", resp.UpdatedBy == null ? string.Empty : resp.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@_recordCreatedBy", resp.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", resp.RecordCreatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", resp.TTLDate == null ? string.Empty : resp.TTLDate.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", resp.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", resp.LastUpdatedOn == null ? string.Empty : resp.LastUpdatedOn.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var responseId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientProgramResponse", parms);

                            if (responseId != null && resp.Spawn != null && resp.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, responseId, resp.Id.ToString(), resp.Spawn);
                                RegisterSpawnElement(resp.Spawn, resp.Id.ToString(), (int)responseId);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message, resp.Id.ToString());
                        }
                    });

        }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RegisterSpawnElement(List<SpawnElement> list, string planElementId, int sqlPlanElementId)
        {

            list.ForEach(
                r =>
                {
                    SpawnElementHash hash = new SpawnElementHash
                    {
                        PlanElementId = planElementId.ToString(),
                        SpawnElem = r,
                        SqlId = sqlPlanElementId
                    };

                    if (!_spawnElementDict.Contains(hash))
                        _spawnElementDict.Add(hash);
                });
        }

        public void ProcessSpawnElements()
        {
            Parallel.ForEach(_spawnElementDict, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, entry =>
            //foreach (var entry in _spawnElementDict)
            {
                //LoadSpawnElement(contract, entry.Value.SqlId, entry.Key, entry.Value.SpawnElem);

                try
                {
                    ParameterCollection parms = new ParameterCollection();
                    parms.Add(new Parameter("@_planElementId", entry.PlanElementId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PlanElementId", entry.SqlId, SqlDbType.Int, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_spawnId", entry.SpawnElem.SpawnId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Tag", entry.SpawnElem.Tag ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Type", (int)entry.SpawnElem.Type, SqlDbType.Int, ParameterDirection.Input, 50));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveSpawnElement", parms);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message, entry.PlanElementId);
                }
            });
        }

        //private void LoadSpawnElement(string ctr, object planElementId, string objectId , SpawnElement spawn)
        //{
        //        try
        //        {
        //            ParameterCollection parms = new ParameterCollection();
        //            parms.Add(new Parameter("@_planElementId", objectId, SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@PlanElementId", planElementId, SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@_spawnId", spawn.SpawnId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@Tag", spawn.Tag == null ? string.Empty : spawn.Tag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@Type", (int)spawn.Type, SqlDbType.Int, ParameterDirection.Input, 50));

        //            SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveSpawnElement", parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ArgumentException(ex.Message, planElementId.ToString());
        //        }
        //}

        private void LoadPatientSystems(string ctr)
        {
            using (PatientSystemMongoContext psctx = new PatientSystemMongoContext(ctr))
            {
                List<MEPatientSystem> systems = psctx.PatientSystems.Collection.FindAllAs<MEPatientSystem>().ToList();

                foreach (MEPatientSystem system in systems)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(system.Id.ToString()) ? string.Empty : system.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(system.PatientID.ToString()) ? string.Empty : system.PatientID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Label", (string.IsNullOrEmpty(system.DisplayLabel) ? string.Empty : system.DisplayLabel), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@SystemId", (string.IsNullOrEmpty(system.SystemID) ? string.Empty : system.SystemID), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@SystemName", (string.IsNullOrEmpty(system.SystemName) ? string.Empty : system.SystemName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(system.UpdatedBy.ToString()) ? string.Empty : system.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(system.LastUpdatedOn.ToString()) ? string.Empty : system.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(system.RecordCreatedBy.ToString()) ? string.Empty : system.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(system.RecordCreatedOn.ToString()) ? string.Empty : system.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(system.Version.ToString()) ? string.Empty : system.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(system.DeleteFlag.ToString()) ? string.Empty : system.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(system.TTLDate.ToString()) ? string.Empty : system.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (system.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", system.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientSystem", parms);
                }
            }
        }

        private void LoadPatientTasks(string ctr)
        {
            using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
            {
                List<MEPatientTask> tasks = pgctx.PatientTasks.Collection.FindAllAs<MEPatientTask>().ToList();

                //Parallel.ForEach(tasks, new ParallelOptions{MaxDegreeOfParallelism= Environment.ProcessorCount * _exponent}, task =>
                foreach (MEPatientTask task in tasks)
                {
                    ParameterCollection parms = new ParameterCollection();

                    parms.Add(new Parameter("@_id", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_patientGoalId", (string.IsNullOrEmpty(task.PatientGoalId.ToString()) ? string.Empty : task.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Name", task.Name == null ? string.Empty : task.Name, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Description", task.Description == null ? string.Empty : task.Description, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@StartDate", (string.IsNullOrEmpty(task.StartDate.ToString()) ? string.Empty : task.StartDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(task.Status.ToString()) ? string.Empty : task.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@StatusDate", (string.IsNullOrEmpty(task.StatusDate.ToString()) ? string.Empty : task.StatusDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TargetDate", (string.IsNullOrEmpty(task.TargetDate.ToString()) ? string.Empty : task.TargetDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TargetValue", task.TargetValue == null ? string.Empty : task.TargetValue, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_updatedBy", task.UpdatedBy == null ? string.Empty : task.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(task.LastUpdatedOn.ToString()) ? string.Empty : task.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_recordCreatedBy", (string.IsNullOrEmpty(task.RecordCreatedBy.ToString()) ? string.Empty : task.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(task.RecordCreatedOn.ToString()) ? string.Empty : task.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(task.Version.ToString()) ? string.Empty : task.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(task.DeleteFlag.ToString()) ? string.Empty : task.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TimeToLive", (string.IsNullOrEmpty(task.TTLDate.ToString()) ? string.Empty : task.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (task.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", task.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientTask", parms);

                    if (task.Attributes != null)
                    {
                        foreach (MAttribute att in task.Attributes)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@GoalAttributeMongoID", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientTaskMongoID", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            
                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientTaskAttribute", parms);

                            if (att.Values != null)
                            {
                                foreach (string val in att.Values)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@Value", (string.IsNullOrEmpty(val) ? string.Empty : val), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@PatientTaskMongoID", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@GoalAttributeMongoID", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientTaskAttributeValue", parms);
                                }
                            }
                        }
                    }

                    if (task.BarrierIds != null)
                    {
                        foreach (ObjectId bar in task.BarrierIds)
                        {
                            parms.Clear();
                            parms.Add(new Parameter("@PatientBarrierMongoId", (string.IsNullOrEmpty(bar.ToString()) ? string.Empty : bar.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientTaskMongoID", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            
                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientTaskBarrier", parms);
                        }
                    }
                }//);
            }
        }

        private void LoadPatientUsers(string ctr)
        {
            using (PatientMongoContext pctx = new PatientMongoContext(ctr))
            {
                List<MEPatientUser> users = pctx.PatientUsers.Collection.FindAllAs<MEPatientUser>().ToList();

                Parallel.ForEach(users, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, user =>
                //foreach (MEPatientUser user in users)
                {
                    ParameterCollection parms = new ParameterCollection();
                    
                    parms.Add(new Parameter("@_id", (string.IsNullOrEmpty(user.Id.ToString()) ? string.Empty : user.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_patientId", (string.IsNullOrEmpty(user.PatientId.ToString()) ? string.Empty : user.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@_contactId", (string.IsNullOrEmpty(user.ContactId.ToString()) ? string.Empty : user.ContactId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Flag", (string.IsNullOrEmpty(user.Flagged.ToString()) ? string.Empty : user.Flagged.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(user.UpdatedBy.ToString()) ? string.Empty : user.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(user.LastUpdatedOn.ToString()) ? string.Empty : user.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(user.RecordCreatedBy.ToString()) ? string.Empty : user.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(user.RecordCreatedOn.ToString()) ? string.Empty : user.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(user.Version.ToString()) ? string.Empty : user.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(user.DeleteFlag.ToString()) ? string.Empty : user.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@TTLDate", (string.IsNullOrEmpty(user.TTLDate.ToString()) ? string.Empty : user.TTLDate.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    if (user.ExtraElements != null)
                        parms.Add(new Parameter("@ExtraElements", user.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    else
                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                    
                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_SavePatientUser", parms);
                });
            }
        }

        private void LoadUsers(string ctr)
            {
            using (ContactMongoContext cctx = new ContactMongoContext(ctr))
            {
                List<MEContact> contacts = cctx.Contacts.Collection.FindAllAs<MEContact>().ToList();

                foreach (MEContact contact in contacts)
                {
                    if (contact.PatientId == null)
                    {
                        ParameterCollection parms = new ParameterCollection();

                        parms.Add(new Parameter("@MongoID", contact.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@ResourceId", (string.IsNullOrEmpty(contact.ResourceId) ? string.Empty : contact.ResourceId), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@FirstName", (string.IsNullOrEmpty(contact.FirstName) ? string.Empty : contact.FirstName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastName", (string.IsNullOrEmpty(contact.LastName) ? string.Empty : contact.LastName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@PreferredName", (string.IsNullOrEmpty(contact.PreferredName) ? string.Empty : contact.PreferredName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastUpdatedOn", (string.IsNullOrEmpty(contact.LastUpdatedOn.ToString()) ? string.Empty : contact.LastUpdatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedOn", (string.IsNullOrEmpty(contact.RecordCreatedOn.ToString()) ? string.Empty : contact.RecordCreatedOn.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(contact.DeleteFlag.ToString()) ? string.Empty : contact.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveUser", parms);

                        if (contact.RecentList != null)
                        {
                            foreach (ObjectId rec in contact.RecentList)
                            {
                                parms.Clear();
                                parms.Add(new Parameter("@MongoID", rec, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@UserMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_SaveUserRecentList", parms);
                            }
                        }
                    }
                }
            }
        }
    }
}

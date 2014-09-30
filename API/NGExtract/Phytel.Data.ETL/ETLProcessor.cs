using System.Collections.Concurrent;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FastMember;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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
//using Phytel.API.DataDomain.PatientProblem;
//using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Scheduling;
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
using Logging;
using Phytel.Data.ETL.BulkCopy;


namespace Phytel.Data.ETL
{
    public delegate void ETLEventHandler(object sender, ETLEventArgs e);

    public class ETLProcessor
    {
        public event ETLEventHandler EtlEvent;
        private int _exponent = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelProcess"]);
        string contract = "InHealth001";
        private List<SpawnElementHash> _spawnElementDict = new List<SpawnElementHash>();
        private SqlConnection _sqlConnection;
        private string connString;

        protected virtual void OnEtlEvent(ETLEventArgs e)
        {
            if (EtlEvent != null)
            {
                EtlEvent(this, e);
            }
        }

        public ETLProcessor()
        {
            OnEtlEvent(new ETLEventArgs {Message = "ETLProcessor start initialized.", IsError = false});

            connString =
                SQLDataService.Instance.GetConnectionString(
                    ConfigurationManager.AppSettings["PhytelServicesConnName"], contract, true, "REPORT");

            _sqlConnection = new SqlConnection(connString);
        }

        public void Rebuild()
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "*** ETL PROCESS START ***", IsError = false });
                OnEtlEvent(new ETLEventArgs { Message = "Truncate Tables.", IsError = false });
                //Truncate/Delete SQL databases
                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_TruncateTables", new ParameterCollection());

                RegisterClasses();
                LoadUsers(contract);
                LoadLookUps(contract);
                LoadGoalAttributes(contract);
                LoadObservations(contract);
                LoadPatients(contract);
                LoadPatientSystems(contract);

                LoadPatientNotes(contract);
                //LoadPatientProblems(contract); depricated
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
                LoadToDos(contract);
                OnEtlEvent(new ETLEventArgs { Message = "*** ETL PROCESS COMPLETED ***", IsError = false });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("Rebuild()", ex));
            }
        }

        private void RegisterClasses()
        {
            try
            {
                OnEtlEvent(new ETLEventArgs {Message = "registering classes.", IsError = false});

                #region Register ClassMap

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (MELookup)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MELookup>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (MEProgramAttribute)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MEProgramAttribute>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Problem)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Problem>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Objective)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Objective>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Category)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Category>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Phytel.API.DataDomain.LookUp.DTO.CommMode)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.CommMode>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (State)) == false)
                    {
                        BsonClassMap.RegisterClassMap<State>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (TimesOfDay)) == false)
                    {
                        BsonClassMap.RegisterClassMap<TimesOfDay>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (TimeZone)) == false)
                    {
                        BsonClassMap.RegisterClassMap<TimeZone>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (CommType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<CommType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Phytel.API.DataDomain.LookUp.DTO.Language)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.Language>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (FocusArea)) == false)
                    {
                        BsonClassMap.RegisterClassMap<FocusArea>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (Source)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Source>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (BarrierCategory)) == false)
                    {
                        BsonClassMap.RegisterClassMap<BarrierCategory>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (InterventionCategory)) == false)
                    {
                        BsonClassMap.RegisterClassMap<InterventionCategory>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (ObservationType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<ObservationType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (CareMemberType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<CareMemberType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (CodingSystem)) == false)
                    {
                        BsonClassMap.RegisterClassMap<CodingSystem>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (ToDoCategory)) == false)
                    {
                        BsonClassMap.RegisterClassMap<ToDoCategory>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (NoteMethod)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteMethod>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (NoteOutcome)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteOutcome>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (NoteWho)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteWho>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (NoteSource)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteSource>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof (NoteDuration)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteDuration>();
                    }
                }
                catch
                {
                    throw;
                }

                #endregion
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs {Message = ex.Message + ": " + ex.StackTrace, IsError = true});
            }
        }

        private void LoadCareMembers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading care members.", IsError = false });

                ConcurrentBag<MECareMember> members;
                using (CareMemberMongoContext cmctx = new CareMemberMongoContext(ctr))
                {
                    members = new ConcurrentBag<MECareMember>(cmctx.CareMembers.Collection.FindAllAs<MECareMember>().ToList());
                }

                Parallel.ForEach(members, mem =>
                    //foreach (MECareMember mem in members)//.Where(t => !t.DeleteFlag))
                {
                    try
                    {
                        ParameterCollection parms = new ParameterCollection();
                        parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(mem.Id.ToString()) ? string.Empty : mem.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@PatientMongoId",(string.IsNullOrEmpty(mem.PatientId.ToString())? string.Empty: mem.PatientId.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@ContactMongoId",(string.IsNullOrEmpty(mem.ContactId.ToString())? string.Empty: mem.ContactId.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@TypeMongoId",(string.IsNullOrEmpty(mem.TypeId.ToString()) ? string.Empty : mem.TypeId.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Primary",(string.IsNullOrEmpty(mem.Primary.ToString()) ? string.Empty : mem.Primary.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@UpdatedBy",(string.IsNullOrEmpty(mem.UpdatedBy.ToString())? string.Empty: mem.UpdatedBy.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastUpdatedOn", mem.LastUpdatedOn ?? (object) DBNull.Value,SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedBy",(string.IsNullOrEmpty(mem.RecordCreatedBy.ToString())? string.Empty: mem.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedOn", mem.RecordCreatedOn, SqlDbType.DateTime,ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Version", mem.Version, SqlDbType.Float, ParameterDirection.Input,8));
                        parms.Add(new Parameter("@Delete",(string.IsNullOrEmpty(mem.DeleteFlag.ToString())? string.Empty: mem.DeleteFlag.ToString()),SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@TimeToLive", mem.TTLDate ?? (object) DBNull.Value,SqlDbType.DateTime,ParameterDirection.Input, 50));
                        
                        if (mem.ExtraElements != null)
                            parms.Add(new Parameter("@ExtraElements", mem.ExtraElements.ToString(),SqlDbType.VarChar,ParameterDirection.Input, int.MaxValue));
                        else
                            parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar,ParameterDirection.Input, int.MaxValue));

                        SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SaveCareMember", parms);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs {Message = ex.Message + ": " + ex.StackTrace, IsError = true});
                    }
                });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "LoadCareMembers():Error" });
            }
        }

        private void LoadContacts(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading contacts.", IsError = false });

                ConcurrentBag<MEContact> contacts;
                using (ContactMongoContext cmctx = new ContactMongoContext(ctr))
                {
                    contacts = new ConcurrentBag<MEContact>(cmctx.Contacts.Collection.FindAllAs<MEContact>().ToList());
                }

                    Parallel.ForEach(contacts, contact =>
                    //foreach (MEContact contact in contacts.Where(t => !t.DeleteFlag))
                    {
                        if (contact.PatientId != null)// && !contact.DeleteFlag)
                        {
                            try
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
                                parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(contact.DeleteFlag.ToString()) ? string.Empty : contact.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@TimeZone", (string.IsNullOrEmpty(contact.TimeZoneId.ToString()) ? string.Empty : contact.TimeZoneId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                if (contact.ExtraElements != null)
                                    parms.Add(new Parameter("@ExtraElements", contact.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                                else
                                    parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContact", parms);

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
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactAddress", parms);
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
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactEmail", parms);
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
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactLanguage", parms);
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
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactMode", parms);
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
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactPhone", parms);
                                    }
                                }

                                if (contact.TimesOfDays != null)
                                {
                                    foreach (ObjectId tod in contact.TimesOfDays)
                                    {
                                        parms.Clear();
                                        parms.Add(new Parameter("@TimeOfDayLookUpMongoId", (string.IsNullOrEmpty(tod.ToString()) ? string.Empty : tod.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactTimeOfDay", parms);
                                    }
                                }

                                if (contact.WeekDays != null)
                                {
                                    foreach (int wd in contact.WeekDays)
                                    {
                                        parms.Clear();
                                        parms.Add(new Parameter("@WeekDay", wd, SqlDbType.Int, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactWeekDay", parms);
                                    }
                                }

                                if (contact.RecentList != null)
                                {
                                    foreach (ObjectId rec in contact.RecentList)
                                    {
                                        parms.Clear();
                                        parms.Add(new Parameter("@MongoID", rec, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(contact.Version.ToString()) ? string.Empty : contact.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 8));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveContactRecentList", parms);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "LoadContacts():Error" });
            }
        }

        private void LoadGoalAttributes(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading goal attributes.", IsError = false });

                ConcurrentBag<MEAttributeLibrary> attributes;
                using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
                {
                    attributes = new ConcurrentBag<MEAttributeLibrary>(pgctx.AttributesLibrary.Collection.FindAllAs<MEAttributeLibrary>().ToList());
                }
                
                    Parallel.ForEach(attributes, att =>
                    //foreach (MEAttributeLibrary att in attributes)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", att.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(att.Name) ? string.Empty : att.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                            parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(att.Type.ToString()) ? string.Empty : att.Type.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ControlType", (string.IsNullOrEmpty(att.ControlType.ToString()) ? string.Empty : att.ControlType.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", (string.IsNullOrEmpty(att.Order.ToString()) ? string.Empty : att.Order.ToString()), SqlDbType.Int, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Required", (string.IsNullOrEmpty(att.Required.ToString()) ? string.Empty : att.Required.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(att.Version.ToString()) ? string.Empty : att.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(att.UpdatedBy.ToString()) ? string.Empty : att.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", att.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(att.DeleteFlag.ToString()) ? string.Empty : att.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", att.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (att.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", att.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveGoalAttribute", parms);

                            foreach (KeyValuePair<int, string> option in att.Options)
                            {
                                parms.Clear();

                                parms.Add(new Parameter("@Key", option.Key, SqlDbType.Int, ParameterDirection.Input, 32));
                                parms.Add(new Parameter("@Value", option.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@GoalAttributeMongoId", att.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(att.Version.ToString()) ? string.Empty : att.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(att.UpdatedBy.ToString()) ? string.Empty : att.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", att.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveGoalAttributeOption", parms);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "LoadGoalAttributes():Error" });
            }
        }

        private void LoadLookUps(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading lookups.", IsError = false });

                List<MELookup> lookups;
                using (LookUpMongoContext lmctx = new LookUpMongoContext(ctr))
                {
                    lookups = lmctx.LookUps.Collection.FindAll().ToList();
                }

                        
                        foreach (MELookup lookup in lookups)
                        {
                            try
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
                                    case LookUpType.ToDoCategory:
                                        LoadToDoCategory(lookup);
                                        break;
                                    case LookUpType.NoteMethod:
                                        LoadLookUp(lookup, "spPhy_RPT_SaveNoteMethodLookUp");
                                        break;
                                    case LookUpType.NoteOutcome:
                                        LoadLookUp(lookup, "spPhy_RPT_SaveNoteOutcomeLookUp");
                                        break;
                                    case LookUpType.NoteWho:
                                        LoadLookUp(lookup, "spPhy_RPT_SaveNoteWhoLookUp");
                                        break;
                                    case LookUpType.NoteSource:
                                        LoadLookUp(lookup, "spPhy_RPT_SaveNoteSourceLookUp");
                                        break;
                                    case LookUpType.NoteDuration:
                                        LoadLookUp(lookup, "spPhy_RPT_SaveNoteDurationLookUp");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                            }
                        }

                    LinkObjectiveCategories(lookups[0]);
                    LinkCommTypeCommModes(lookups[10]);
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveBarrierCategoryLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCareMemberTypeLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCategoryLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCodingSystemLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCommModeLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCommTypeLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveFocusAreaLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveInterventionCategoryLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveLanguageLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveObjectiveLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveObservationTypeLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveProblemLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveSourceLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveStateLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveTimesOfDayLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveTimeZoneLookUp", parms);
            }
        }

        private void LoadLookUp(MELookup lookup, string sp)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var lu = (LookUpDetailsBase)lbase;

                var parms = new ParameterCollection
                {
                    new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input,int.MaxValue),
                    new Parameter("@MongoId", lu.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50),
                    new Parameter("@Name", lu.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue),
                    new Parameter("@Active", lu.Active, SqlDbType.VarChar, ParameterDirection.Input, 50),
                    new Parameter("@Default", lu.Default, SqlDbType.VarChar, ParameterDirection.Input, 50)
                };

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", sp, parms);
            }
        }

        private void LoadToDoCategory(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                ToDoCategory tz = (ToDoCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@MongoID", tz.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tz.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveToDoCategoryLookUp", parms);
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

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveCommTypeCommMode", parms);
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

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveObjectiveCategory", parms);
                }
            }
        }
        #endregion

        private void LoadObservations(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading observations.", IsError = false });

                ConcurrentBag<MEObservation> observations;
                using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
                {
                    observations = new ConcurrentBag<MEObservation>(poctx.Observations.Collection.FindAllAs<MEObservation>().ToList());
                }
                    Parallel.ForEach(observations, obs =>
                    //foreach (MEObservation obs in observations)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", obs.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(obs.Code) ? string.Empty : obs.Code), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@CommonName", (string.IsNullOrEmpty(obs.CommonName) ? string.Empty : obs.CommonName), SqlDbType.VarChar, ParameterDirection.Input, 100));
                            parms.Add(new Parameter("@CodingSystemId", (string.IsNullOrEmpty(obs.CodingSystemId.ToString()) ? string.Empty : obs.CodingSystemId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@GroupId", (string.IsNullOrEmpty(obs.GroupId.ToString()) ? string.Empty : obs.GroupId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(obs.DeleteFlag.ToString()) ? string.Empty : obs.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", (string.IsNullOrEmpty(obs.Description) ? string.Empty : obs.Description), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            parms.Add(new Parameter("@HighValue", obs.HighValue ?? -1, SqlDbType.Int, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@LastUpdatedOn", obs.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LowValue", obs.LowValue ?? -1, SqlDbType.Int, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@ObservationTypeMongoId", (string.IsNullOrEmpty(obs.ObservationTypeId.ToString()) ? string.Empty : obs.ObservationTypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", obs.Order ?? -1, SqlDbType.Int, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(obs.Source) ? string.Empty : obs.Source), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Standard", (string.IsNullOrEmpty(obs.Standard.ToString()) ? string.Empty : obs.Standard.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(obs.Status.ToString()) ? string.Empty : obs.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", obs.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Units", (string.IsNullOrEmpty(obs.Units) ? string.Empty : obs.Units.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@UpdatedBy", obs.UpdatedBy == null ? string.Empty : obs.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedBy", obs.RecordCreatedBy == null ? string.Empty : obs.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            if (obs.RecordCreatedOn.Year == 1)
                                parms.Add(new Parameter("@RecordCreatedOn", (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            else
                                parms.Add(new Parameter("@RecordCreatedOn", obs.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(obs.Version.ToString()) ? string.Empty : obs.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 50));
                            if (obs.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", obs.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));


                            SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveObservation", parms);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadObservations()", ex));
            }
        }

        private void LoadPatients(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patients.", IsError = false });

                ConcurrentBag<MEPatient> patients;
                using (PatientMongoContext pmctx = new PatientMongoContext(ctr))
                {
                    patients = new ConcurrentBag<MEPatient>(Utils.GetMongoCollectionList(pmctx.Patients.Collection, 1000));
                }

                    Parallel.ForEach(patients, patient =>
                    //foreach (MEPatient patient in patients.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            if(!patient.DeleteFlag)
                            {
                                ParameterCollection parms = new ParameterCollection();
                                parms.Add(new Parameter("@MongoID", patient.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@FirstName", patient.FirstName ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 100));
                                parms.Add(new Parameter("@MiddleName", patient.MiddleName ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 100));
                                parms.Add(new Parameter("@LastName", patient.LastName ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 100));
                                parms.Add(new Parameter("@PreferredName", patient.PreferredName ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 100));
                                parms.Add(new Parameter("@Suffix", patient.Suffix ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@DateOfBirth", patient.DOB ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Gender", patient.Gender ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Priority", patient.Priority, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LSSN", patient.LastFourSSN ?? (object)DBNull.Value, SqlDbType.Int, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@FSSN", patient.FullSSN != null ? patient.FullSSN.ToString() : (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 100));                            
                                parms.Add(new Parameter("@Version", patient.Version, SqlDbType.Float, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@UpdatedBy", patient.UpdatedBy ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", patient.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedBy", patient.RecordCreatedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", patient.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Delete", patient.DeleteFlag, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(patient.Background) ? string.Empty : patient.Background), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                                parms.Add(new Parameter("@DisplayPatientSystemMongoId", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@TTLDate", patient.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                if (patient.ExtraElements != null)
                                    parms.Add(new Parameter("@ExtraElements", patient.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                                else
                                    parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SavePatient", parms);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatients()", ex));
            }
        }

        private void LoadPatientBarriers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient barriers.", IsError = false });

                ConcurrentBag<MEPatientBarrier> barriers;
                using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
                {
                    barriers = new ConcurrentBag<MEPatientBarrier>(pgctx.PatientBarriers.Collection.FindAllAs<MEPatientBarrier>().ToList());
                }

                    Parallel.ForEach(barriers, bar =>
                    //foreach (MEPatientBarrier bar in barriers)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(bar.Id.ToString()) ? string.Empty : bar.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(bar.PatientGoalId.ToString()) ? string.Empty : bar.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoCategoryLookUpId", (string.IsNullOrEmpty(bar.CategoryId.ToString()) ? string.Empty : bar.CategoryId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(bar.DeleteFlag.ToString()) ? string.Empty : bar.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", bar.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(bar.RecordCreatedBy.ToString()) ? string.Empty : bar.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", bar.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(bar.Status.ToString()) ? string.Empty : bar.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StatusDate", bar.StatusDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StartDate", bar.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", bar.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(bar.UpdatedBy.ToString()) ? string.Empty : bar.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(bar.Version.ToString()) ? string.Empty : bar.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(bar.Name) ? string.Empty : bar.Name), SqlDbType.VarChar, ParameterDirection.Input, 500));
                            if (bar.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", bar.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientBarrier", parms);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientBarriers()", ex));
            }
        }

        private void LoadPatientGoals(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient goals.", IsError = false });

                ConcurrentBag<MEPatientGoal> goals;
                using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
                {
                    goals = new ConcurrentBag<MEPatientGoal>(pgctx.PatientGoals.Collection.FindAllAs<MEPatientGoal>().ToList());
                }

                #region old
                    Parallel.ForEach(goals, goal =>
                    //foreach (MEPatientGoal goal in goals)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoPatientId", (string.IsNullOrEmpty(goal.PatientId.ToString()) ? string.Empty : goal.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(goal.Name) ? string.Empty : goal.Name), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", goal.Description == null ? string.Empty : goal.Description, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StartDate", goal.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EndDate", goal.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(goal.Status.ToString()) ? string.Empty : goal.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            parms.Add(new Parameter("@StatusDate", goal.StatusDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(goal.SourceId.ToString()) ? string.Empty : goal.SourceId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(goal.Type.ToString()) ? string.Empty : goal.Type.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TargetDate", goal.TargetDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TargetValue", goal.TargetValue == null ? string.Empty : goal.TargetValue.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoUpdatedBy", goal.UpdatedBy == null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.Float, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Delete", goal.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", goal.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (goal.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", goal.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientGoal", parms);

                            if (goal.Attributes != null)
                            {
                                foreach (MAttribute att in goal.Attributes)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@MongoPatientGoalId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoGoalAttributeId", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoUpdatedBy", goal.UpdatedBy == null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoRecordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.Float, ParameterDirection.Input, 32));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientGoalAttribute", parms);

                                    if (att.Values != null)
                                    {
                                        foreach (string value in att.Values)
                                        {
                                            parms.Clear();
                                            parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@GoalAttributeMongoId", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@Value", (string.IsNullOrEmpty(value) ? string.Empty : value), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@MongoUpdatedBy", goal.UpdatedBy == null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@MongoRecordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.DateTime, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.Float, ParameterDirection.Input, 32));

                                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientGoalAttributeValue", parms);
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
                                    parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoUpdatedBy", goal.UpdatedBy == null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoRecordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.Float, ParameterDirection.Input, 32));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientGoalFocusArea", parms);
                                }
                            }

                            if (goal.ProgramIds != null)
                            {
                                foreach (ObjectId pro in goal.ProgramIds)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(goal.Id.ToString()) ? string.Empty : goal.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@ProgramId", (string.IsNullOrEmpty(pro.ToString()) ? string.Empty : pro.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@LastUpdatedOn", goal.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoUpdatedBy", goal.UpdatedBy == null ? string.Empty : goal.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoRecordCreatedBy", goal.RecordCreatedBy == null ? string.Empty : goal.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedOn", goal.RecordCreatedOn == null ? string.Empty : goal.RecordCreatedOn.ToString(), SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Version", goal.Version.ToString(), SqlDbType.Float, ParameterDirection.Input, 32));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientGoalProgram", parms);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
                #endregion
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientGoals()", ex));
            }
        }

        private void LoadPatientInterventions(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient interventions.", IsError = false });

                ConcurrentBag<MEPatientIntervention> interventions;                
                using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
                {
                    interventions = new ConcurrentBag<MEPatientIntervention>(pgctx.PatientInterventions.Collection.FindAllAs<MEPatientIntervention>().ToList());
                }
                    Parallel.ForEach(interventions, intervention =>
                    //foreach (MEPatientIntervention intervention in interventions)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(intervention.PatientGoalId.ToString())) 
                            { 
                                ParameterCollection parms = new ParameterCollection();
                                parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(intervention.Id.ToString()) ? string.Empty : intervention.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@PatientGoalMongoId", (string.IsNullOrEmpty(intervention.PatientGoalId.ToString()) ? string.Empty : intervention.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoCategoryLookUpId", (string.IsNullOrEmpty(intervention.CategoryId.ToString()) ? string.Empty : intervention.CategoryId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@AssignedTo", (string.IsNullOrEmpty(intervention.AssignedToId.ToString()) ? string.Empty : intervention.AssignedToId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(intervention.UpdatedBy.ToString()) ? string.Empty : intervention.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", intervention.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(intervention.RecordCreatedBy.ToString()) ? string.Empty : intervention.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                if (intervention.RecordCreatedOn.Year == 1)
                                    parms.Add(new Parameter("@RecordCreatedOn", (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                else
                                    parms.Add(new Parameter("@RecordCreatedOn", intervention.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(intervention.Version.ToString()) ? string.Empty : intervention.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));
                                parms.Add(new Parameter("@TimeToLive", intervention.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(intervention.DeleteFlag.ToString()) ? string.Empty : intervention.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(intervention.Status.ToString()) ? string.Empty : intervention.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@StatusDate", intervention.StatusDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@StartDate", intervention.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Description", (string.IsNullOrEmpty(intervention.Description) ? string.Empty : intervention.Description), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                                parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(intervention.Name) ? string.Empty : intervention.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                                if (intervention.ExtraElements != null)
                                    parms.Add(new Parameter("@ExtraElements", intervention.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                                else
                                    parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                                SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientIntervention", parms);
                            
                                if (intervention.BarrierIds != null)
                                {
                                    foreach (ObjectId bar in intervention.BarrierIds)
                                    {
                                        parms.Clear();
                                        parms.Add(new Parameter("@PatientBarrierMongoId", (string.IsNullOrEmpty(bar.ToString()) ? string.Empty : bar.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@PatientInterventionMongoID", (string.IsNullOrEmpty(intervention.Id.ToString()) ? string.Empty : intervention.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(intervention.UpdatedBy.ToString()) ? string.Empty : intervention.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", intervention.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(intervention.RecordCreatedBy.ToString()) ? string.Empty : intervention.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        if (intervention.RecordCreatedOn.Year == 1)
                                            parms.Add(new Parameter("@RecordCreatedOn", (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        else
                                            parms.Add(new Parameter("@RecordCreatedOn", intervention.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(intervention.Version.ToString()) ? string.Empty : intervention.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));

                                        SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientInterventionBarrier", parms);

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientInterventions()", ex));
            }
        }

        private void LoadPatientNotes(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading Patient Notes.", IsError = false });

                ConcurrentBag<MEPatientNote> notes;
                using (PatientNoteMongoContext pnctx = new PatientNoteMongoContext(ctr))
                {
                    notes = new ConcurrentBag<MEPatientNote>(pnctx.PatientNotes.Collection.FindAllAs<MEPatientNote>().ToList());
                }

                //var total = notes.Count(l => l.ProgramIds == null);
                //var notePrgIds = notes.Where(l => l.ProgramIds != null).SelectMany(pid => pid.ProgramIds ).ToList<ObjectId>();

                Parallel.ForEach(notes, note =>
                    //foreach (MEPatientNote note in notes)//.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(note.PatientId.ToString()) ? string.Empty : note.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Text", (string.IsNullOrEmpty(note.Text) ? string.Empty : note.Text), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            parms.Add(new Parameter("@Type", note.Type.ToString() ?? (object) DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoMethodId", (string.IsNullOrEmpty(note.MethodId.ToString()) ? string.Empty : note.MethodId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoOutcomeId", (string.IsNullOrEmpty(note.OutcomeId.ToString()) ? string.Empty : note.OutcomeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoWhoId", (string.IsNullOrEmpty(note.WhoId.ToString()) ? string.Empty : note.WhoId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoSourceId", (string.IsNullOrEmpty(note.SourceId.ToString()) ? string.Empty : note.SourceId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoDurationId", (string.IsNullOrEmpty(note.DurationId.ToString()) ? string.Empty : note.DurationId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContactedOn", note.ContactedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ValidatedIntentity", note.ValidatedIdentity.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            // standard fields
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(note.UpdatedBy.ToString()) ? string.Empty : note.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", note.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(note.RecordCreatedBy.ToString()) ? string.Empty : note.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", note.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(note.Version.ToString()) ? string.Empty : note.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(note.DeleteFlag.ToString()) ? string.Empty : note.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", note.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (note.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", note.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientNote", parms);

                            if (note.ProgramIds != null)
                            {
                                foreach (ObjectId prg in note.ProgramIds)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(prg.ToString()) ? string.Empty : prg.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@PatientNoteMongoId", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(note.UpdatedBy.ToString()) ? string.Empty : note.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@LastUpdatedOn", note.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(note.RecordCreatedBy.ToString()) ? string.Empty : note.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedOn", note.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(note.Version.ToString()) ? string.Empty : note.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientNoteProgram", parms);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientNotes()", ex));
            }
        }

        private void LoadPatientObservations(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient observations.", IsError = false });

                ConcurrentBag<MEPatientObservation> observations;                
                using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
                {
                    observations = new ConcurrentBag<MEPatientObservation>(poctx.PatientObservations.Collection.FindAllAs<MEPatientObservation>().ToList());
                }

                var rSeries = new ReadObservationsSeries().ReadEObservationSeries(observations.ToList());

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 180;
                        bcc.ColumnMappings.Add("PatientId", "PatientId");
                        bcc.ColumnMappings.Add("MongoPatientId", "MongoPatientId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("MongoObservationId", "MongoObservationId");
                        bcc.ColumnMappings.Add("ObservationId", "ObservationId");
                        bcc.ColumnMappings.Add("Display", "Display");
                        bcc.ColumnMappings.Add("StartDate", "StartDate");
                        bcc.ColumnMappings.Add("EndDate", "EndDate");
                        bcc.ColumnMappings.Add("NumericValue", "NumericValue");
                        bcc.ColumnMappings.Add("NonNumericValue", "NonNumericValue");
                        bcc.ColumnMappings.Add("Source", "Source");
                        bcc.ColumnMappings.Add("State", "State");
                        bcc.ColumnMappings.Add("Type", "Type");
                        bcc.ColumnMappings.Add("Units", "Units");
                        bcc.ColumnMappings.Add("AdministeredBy", "AdministeredBy");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedById", "UpdatedById");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedById", "RecordCreatedById");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.DestinationTableName = "RPT_PatientObservation";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs
                        {
                            Message = "SqlBulkCopy process failure: " + ex.Message + " : " + ex.InnerException,
                            IsError = true
                        });
                    }
                }

                #region old
                //Parallel.ForEach(observations, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, obs =>
                //    //foreach (MEPatientObservation obs in observations.Where(t => !t.DeleteFlag))
                //    {
                //        try
                //        {
                //            if(!obs.DeleteFlag)
                //            {
                //                ParameterCollection parms = new ParameterCollection();
                //                parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(obs.Id.ToString()) ? string.Empty : obs.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(obs.PatientId.ToString()) ? string.Empty : obs.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Display", (string.IsNullOrEmpty(obs.Display.ToString()) ? string.Empty : obs.Display.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@EndDate", obs.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@StartDate", obs.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(obs.UpdatedBy.ToString()) ? string.Empty : obs.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@LastUpdatedOn", obs.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(obs.RecordCreatedBy.ToString()) ? string.Empty : obs.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@RecordCreatedOn", obs.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Version", obs.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                //                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(obs.DeleteFlag.ToString()) ? string.Empty : obs.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@NumericValue", obs.NumericValue ?? -1, SqlDbType.Float, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@_observationId", (string.IsNullOrEmpty(obs.ObservationId.ToString()) ? string.Empty : obs.ObservationId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Source", (string.IsNullOrEmpty(obs.Source) ? string.Empty : obs.Source), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@State", (string.IsNullOrEmpty(obs.State.ToString()) ? string.Empty : obs.State.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@TimeToLive", obs.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Units", (string.IsNullOrEmpty(obs.Units) ? string.Empty : obs.Units), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@AdministeredBy", (string.IsNullOrEmpty(obs.AdministeredBy) ? string.Empty : obs.AdministeredBy), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@NonNumericValue", (string.IsNullOrEmpty(obs.NonNumericValue) ? string.Empty : obs.NonNumericValue), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(obs.Type) ? string.Empty : obs.Type), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                if (obs.ExtraElements != null)
                //                    parms.Add(new Parameter("@ExtraElements", obs.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                //                else
                //                    parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                //                SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientObservation", parms);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                //        }
                //    });
                #endregion
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientObservations()", ex));
            }
        }

        //private void LoadPatientProblems(string ctr)
        //{
        //    try
        //    {
        //        OnEtlEvent(new ETLEventArgs { Message = "Loading patient problems.", IsError = false });

        //        List<MEPatientProblem> problems;                
        //        using (PatientProblemMongoContext ppctx = new PatientProblemMongoContext(ctr))
        //        {
        //            problems =
        //                ppctx.PatientProblems.Collection.FindAllAs<MEPatientProblem>().ToList();
        //        }

        //        //Parallel.ForEach(problems, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * _exponent }, problem =>
        //            foreach (MEPatientProblem problem in problems)//.Where(t => !t.DeleteFlag))
        //            {
        //                try
        //                {
        //                    ParameterCollection parms = new ParameterCollection();
        //                    parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(problem.Id.ToString()) ? string.Empty : problem.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(problem.PatientID.ToString()) ? string.Empty : problem.PatientID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@ProblemMongoId", (string.IsNullOrEmpty(problem.ProblemID.ToString()) ? string.Empty : problem.ProblemID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@Active", (string.IsNullOrEmpty(problem.Active.ToString()) ? string.Empty : problem.Active.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
        //                    parms.Add(new Parameter("@Featured", (string.IsNullOrEmpty(problem.Featured.ToString()) ? string.Empty : problem.Featured.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
        //                    parms.Add(new Parameter("@Level", problem.Level, SqlDbType.Int, ParameterDirection.Input, 32));
        //                    parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(problem.UpdatedBy.ToString()) ? string.Empty : problem.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@LastUpdatedOn", problem.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(problem.RecordCreatedBy.ToString()) ? string.Empty : problem.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@RecordCreatedOn", problem.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@Version", problem.Version, SqlDbType.Float, ParameterDirection.Input, 32));
        //                    parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(problem.DeleteFlag.ToString()) ? string.Empty : problem.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@StartDate", problem.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@EndDate", problem.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                    parms.Add(new Parameter("@TTLDate", problem.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                    if (problem.ExtraElements != null)
        //                        parms.Add(new Parameter("@ExtraElements", problem.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
        //                    else
        //                        parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

        //                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientProblem", parms);
        //                }
        //                catch (Exception ex)
        //                {
        //                    OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
        //                }
        //            }//);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProblems()", ex));
        //    }
        //}

        private void LoadToDos(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading todos.", IsError = false });

                ConcurrentBag<METoDo> todo;
                using (SchedulingMongoContext smct = new SchedulingMongoContext(ctr))
                {
                    todo = new ConcurrentBag<METoDo>(smct.ToDos.Collection.FindAllAs<METoDo>().ToList());
                }

                    Parallel.ForEach(todo, td =>
                    //foreach(METoDo td in todo)//.Where(t => !t.DeleteFlag ))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection
                            {
                                new Parameter("@MongoId", td.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50),
                                new Parameter("@MongoSourceId", td.SourceId == null ? string.Empty : td.SourceId.ToString(),SqlDbType.VarChar, ParameterDirection.Input, 50),
                                new Parameter("@MongoPatientId",td.PatientId == null ? string.Empty : td.PatientId.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@MongoAssignedToId",td.AssignedToId == null ? string.Empty : td.AssignedToId.ToString(),SqlDbType.VarChar, ParameterDirection.Input, 50),
                                new Parameter("@ClosedDate", td.ClosedDate ?? (object) DBNull.Value, SqlDbType.DateTime,ParameterDirection.Input, 50),
                                new Parameter("@Title", td.Title ?? string.Empty, SqlDbType.VarChar,ParameterDirection.Input, 500),
                                new Parameter("@Description", td.Description ?? string.Empty, SqlDbType.VarChar,ParameterDirection.Input, 500),
                                new Parameter("@DueDate", td.DueDate ?? (object) DBNull.Value, SqlDbType.DateTime,ParameterDirection.Input, 50),
                                new Parameter("@Status", td.Status.ToString() ?? (object) DBNull.Value,SqlDbType.VarChar, ParameterDirection.Input, 50),
                                new Parameter("@MongoCategory", td.Category.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@Priority", td.Priority.ToString() ?? (object) DBNull.Value,SqlDbType.VarChar, ParameterDirection.Input, 50),
                                new Parameter("@Version", td.Version.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@MongoUpdatedBy",td.UpdatedBy == null ? string.Empty : td.UpdatedBy.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@LastUpdatedOn", td.LastUpdatedOn ?? (object) DBNull.Value,SqlDbType.DateTime, ParameterDirection.Input, 50),
                                new Parameter("@MongoRecordCreatedBy", td.RecordCreatedBy.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@RecordCreatedOn", td.RecordCreatedOn, SqlDbType.DateTime,ParameterDirection.Input, 50),
                                new Parameter("@TTLDate", td.TTLDate ?? (object) DBNull.Value, SqlDbType.DateTime,ParameterDirection.Input, 50),
                                new Parameter("@DeleteFlag", td.DeleteFlag.ToString(), SqlDbType.VarChar,ParameterDirection.Input, 50),
                                new Parameter("@ExtraElements",td.ExtraElements != null ? td.ExtraElements.ToString() : (object) DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue)
                            };

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SaveToDo", parms);


                            if (td.ProgramIds != null && td.ProgramIds.Count > 0)
                            {
                                LoadProgramIdReferences(td.Id.ToString(), td.ProgramIds);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }

        private void LoadProgramIdReferences(string todoId, List<ObjectId> list)
        {
            try
            {
                list.ForEach(pid => 
                {
                    ParameterCollection parms = new ParameterCollection
                    {
                        new Parameter("@MongoToDoId", todoId, SqlDbType.VarChar, ParameterDirection.Input, 50),
                        new Parameter("@MongoProgramId", pid.ToString() ,SqlDbType.VarChar, ParameterDirection.Input, 50)
                    };

                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SaveToDoProgram", parms);
                });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        private void LoadPatientPrograms(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs {Message = "Loading patient programs.", IsError = false});

                ConcurrentBag<MEPatientProgram> programs;
                using (ProgramMongoContext pctx = new ProgramMongoContext(ctr))
                {
                    programs = new ConcurrentBag<MEPatientProgram>(Utils.GetMongoCollectionList(pctx.PatientPrograms.Collection, 50));
                }

                    _sqlConnection.Open();
                    Parallel.ForEach(programs, prog =>
                        //foreach (MEPatientProgram prog in programs.Where(prog => !prog.DeleteFlag))
                    {
                        try
                        {
                            //if(!prog.DeleteFlag)
                            //{
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoPatientId", prog.PatientId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedBy", prog.AssignedBy == null ? string.Empty : prog.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", prog.AssignedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedToId", prog.AssignedTo == null ? string.Empty : prog.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", prog.AttributeEndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", prog.AttributeStartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", prog.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ContractProgramId", prog.ContractProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", prog.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityReason", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityStartDate", prog.EligibilityStartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EndDate", prog.EndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", prog.Name ?? null, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", prog.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ShortName", prog.ShortName ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", prog.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StartDate", prog.StartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", prog.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", prog.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", prog.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", prog.StateUpdatedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoCompletedBy", prog.CompletedBy == null ? string.Empty : prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", prog.DateCompleted ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", prog.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", prog.EligibilityEndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            //
                            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));


                            var patientProgramId = ExecuteScalarSproc(parms, _sqlConnection, "spPhy_RPT_SavePatientProgram");

                            #region -- archived 
                            // original call
                            //patientProgramId = SQLDataService.Instance.ExecuteScalar("InHealth001", true,
                            //    "REPORT",
                            //    "spPhy_RPT_SavePatientProgram", parms);

                            //var times = 0;
                            //try
                            //{
                            //    ExecuteScalarSproc(parms, out patientProgramId);
                            //}
                            //catch (Exception ex)
                            //{
                            //    if (ex != null && ex is SqlException)
                            //    {
                            //        foreach (SqlError error in (ex as SqlException).Errors)
                            //        {
                            //            switch (error.Number)
                            //            {
                            //                case 1205:
                            //                {
                            //                    System.Diagnostics.Debug.WriteLine(
                            //                        "SQL Error: Deadlock condition. Retrying...");
                            //                    //return true;
                            //                    if (times <= 5)
                            //                    {
                            //                        ExecuteScalarSproc(parms, out patientProgramId);
                            //                        times = times++;
                            //                    }
                            //                    break;
                            //                }
                            //                case -2:
                            //                    System.Diagnostics.Debug.WriteLine(
                            //                        "SQL Error: Timeout expired. Retrying...");
                            //                    //return true;
                            //                    if (times <= 5)
                            //                    {
                            //                        ExecuteScalarSproc(parms, out patientProgramId);
                            //                        times = times++;
                            //                    }
                            //                    break;
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion

                            if (patientProgramId != null && prog.Spawn != null && prog.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, planElementId, prog.Id.ToString(), prog.Spawn);
                                RegisterSpawnElement(prog.Spawn, prog.Id.ToString(), (int) patientProgramId);
                            }

                            LoadPatientProgramModules(ctr, patientProgramId, prog.Modules, prog);
                            //OnEtlEvent(new ETLEventArgs {Message = "Program:" + prog.Id.ToString() + "Loaded."});
                            //}
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs {Message = ex.Message + ": " + ex.StackTrace, IsError = true});
                        }
                    });
                    _sqlConnection.Close();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("LoadPatientPrograms() : ", ex.InnerException);
                //throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }

        private object ExecuteScalarSproc(ParameterCollection parms, SqlConnection conn, string sproc)
        {
            object result = null;
            lock (conn)
            {
                SqlCommand sqlCommand = new SqlCommand(sproc, conn, null)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                foreach (Parameter parameter in parms)
                {
                    SqlParameter sqlParameter = new SqlParameter(parameter.Name, parameter.Value)
                    {
                        SqlDbType = parameter.Type,
                        Direction = parameter.Direction,
                        Size = parameter.Size
                    };

                    sqlCommand.Parameters.Add(sqlParameter);
                }

                result = sqlCommand.ExecuteScalar();
            }
            return result;
        }

        private void LoadPatientProgramAttributes(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading program attributes.", IsError = false });
                ConcurrentBag<MEProgramAttribute> programAttributes;
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    programAttributes = new ConcurrentBag<MEProgramAttribute>(pmctx.ProgramAttributes.Collection.FindAllAs<MEProgramAttribute>().ToList());
                }

                //foreach (MEProgramAttribute prog in programAttributes.Where(t => !t.DeleteFlag))
                    Parallel.ForEach(programAttributes, prog =>
                    {
                        try
                        {
                            //if(!prog.DeleteFlag)
                            //{
                                ParameterCollection parms = new ParameterCollection();
                                parms.Add(new Parameter("@MongoId", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoPlanElementId", prog.PlanElementId == null ? string.Empty : prog.PlanElementId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

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
                                parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoCompletedBy", prog.CompletedBy == null ? string.Empty : prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@DateCompleted", prog.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));


                                var patientProgramId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT",
                                    "spPhy_RPT_SavePatientProgramAttribute", parms);

                                //OnEtlEvent(new ETLEventArgs { Message = "Program attribute:" + prog.PlanElementId.ToString() + "Loaded." });
                            //}
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
                
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramAttributes()", ex));
            }
        }

        private void LoadPatientProgramModules(string ctr, object patientProgramId, List<Module> list, MEPatientProgram prog )
        {
            try
            {
                Parallel.ForEach(list, mod =>
                    //foreach (Module mod in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@PatientProgramId", patientProgramId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoProgramId", mod.ProgramId == null ? string.Empty : mod.ProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedBy", mod.AssignedBy == null ? string.Empty : mod.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", mod.AssignedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedToId", mod.AssignedTo == null ? string.Empty : mod.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", mod.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", mod.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", mod.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", mod.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@MongoPrevious", mod.Previous == null ? string.Empty : mod.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoNext", mod.Next == null ? string.Empty : mod.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@EligibilityStartDate", mod.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", mod.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", mod.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", mod.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", mod.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", mod.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", mod.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", mod.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoCompletedBy", mod.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", mod.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", mod.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", mod.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoId", mod.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            //
                            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var patientProgramModuleId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientProgramModule", parms);

                            if (patientProgramModuleId != null && mod.Spawn != null && mod.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, patientProgramModuleId, mod.Id.ToString(), mod.Spawn);
                                RegisterSpawnElement(mod.Spawn, mod.Id.ToString(), (int)patientProgramModuleId);
                            }

                            LoadPatientProgramActions(ctr, patientProgramModuleId, mod.Actions, prog);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramModules()", ex));
            }
        }

        private void LoadPatientProgramActions(string ctr, object patientProgramModuleId, List<Action> list, MEPatientProgram prog)
        {
            try
            {
                    Parallel.ForEach(list, act =>
                    //foreach (Action act in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoId", act.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoModuleId", act.ModuleId == null ? string.Empty : act.ModuleId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientProgramModuleId", patientProgramModuleId, SqlDbType.Int, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedBy", act.AssignedBy == null ? string.Empty : act.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AssignedOn", act.AssignedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoAssignedToId", act.AssignedTo == null ? string.Empty : act.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeEndDate", act.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", act.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", act.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", act.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityStartDate", act.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", act.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", act.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", act.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", act.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", act.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", act.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", act.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoCompletedBy", act.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", act.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", act.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", act.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoPrevious", act.Previous == null ? string.Empty : act.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoNext", act.Next == null ? string.Empty : act.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Archived", prog.Archived.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@ArchivedDate", act.ArchivedDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoArchiveOriginId", act.ArchiveOriginId == null ? string.Empty : act.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var patientProgramActionId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientProgramModuleAction", parms);

                            if (patientProgramActionId != null && act.Spawn != null && act.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, patientProgramActionId, act.Id.ToString(), act.Spawn);
                                RegisterSpawnElement(act.Spawn, act.Id.ToString(), (int)patientProgramActionId);
                            }

                            LoadPatientProgramSteps(ctr, patientProgramActionId, act.Steps, prog);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramActions()", ex));
            }
        }

        private void LoadPatientProgramSteps(string ctr, object patientProgramActionId, List<Step> list, MEPatientProgram prog)
        {
            try
            {
                    Parallel.ForEach(list, step =>
                    //foreach (Step step in list)
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoId", step.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoActionId", step.ActionId == null ? string.Empty : step.ActionId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
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
                            parms.Add(new Parameter("@MongoPrevious", step.Previous == null ? string.Empty : step.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoNext", step.Next == null ? string.Empty : step.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            // inherited fields
                            parms.Add(new Parameter("@AttributeEndDate", step.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@AttributeStartDate", step.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Completed", step.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityStartDate", step.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Order", step.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SourceId", step.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@State", step.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Enabled", step.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StateUpdatedOn", step.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoCompletedBy", step.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DateCompleted", step.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@EligibilityRequirements", step.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                            parms.Add(new Parameter("@EligibilityEndDate", step.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                            var StepId = SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientProgramModuleActionStep", parms);

                            if (StepId != null && step.Spawn != null && step.Spawn.Count > 0)
                            {
                                //LoadSpawnElement(ctr, StepId, step.Id.ToString(), step.Spawn);
                                RegisterSpawnElement(step.Spawn, step.Id.ToString(), (int)StepId);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramSteps()", ex));
            }
        }

        private void LoadPatientProgramResponses(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading program responses.", IsError = false });
                
                List<MEPatientProgramResponse> responses;
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    responses = Utils.GetMongoCollectionList(pmctx.PatientProgramResponses.Collection, 50000);
                }

                // get stepidlist
                var stepIdList = Utils.GetStepIdList();
                var rSeries = new ReadResponsesSeries().ReadEStepResponseSeries(responses);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 180;
                        bcc.ColumnMappings.Add("MongoStepId", "MongoStepId");
                        bcc.ColumnMappings.Add("StepId", "StepId");
                        bcc.ColumnMappings.Add("MongoNextStepId", "MongoNextStepId");
                        bcc.ColumnMappings.Add("NextStepId", "NextStepId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("MongoActionId", "MongoActionId");
                        bcc.ColumnMappings.Add("Order", "Order");
                        bcc.ColumnMappings.Add("Text", "Text");
                        bcc.ColumnMappings.Add("Value", "Value");
                        bcc.ColumnMappings.Add("Nominal", "Nominal");
                        bcc.ColumnMappings.Add("Required", "Required");
                        bcc.ColumnMappings.Add("Selected", "Selected");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.ColumnMappings.Add("MongoStepSourceId", "MongoStepSourceId");
                        bcc.ColumnMappings.Add("StepSourceId", "StepSourceId");
                        bcc.ColumnMappings.Add("ActionId", "ActionId");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.DestinationTableName = "RPT_PatientProgramResponse";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs
                        {
                            Message = "SqlBulkCopy process failure: " + ex.Message + " : " + ex.InnerException,
                            IsError = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramResponse()", ex));
            }
        }

        public void RegisterSpawnElement(List<SpawnElement> list, string planElementId, int sqlPlanElementId)
        {
            try
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
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                //throw ex; //SimpleLog.Log(new ArgumentException("RegisterSpawnElement()", ex));
            }
        }

        public void ProcessSpawnElements()
        {
            OnEtlEvent(new ETLEventArgs { Message = "Loading SpawnElements.", IsError = false });
            Parallel.ForEach(_spawnElementDict, entry =>
            //foreach (var entry in _spawnElementDict)
            {
                //LoadSpawnElement(contract, entry.Value.SqlId, entry.Key, entry.Value.SpawnElem);

                try
                {
                    //OnEtlEvent(new ETLEventArgs { Message = "Loading spawn elements.", IsError = false });
                    ParameterCollection parms = new ParameterCollection();
                    parms.Add(new Parameter("@MongoPlanElementId", entry.PlanElementId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@PlanElementId", entry.SqlId, SqlDbType.Int, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@MongoSpawnId", entry.SpawnElem.SpawnId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Tag", entry.SpawnElem.Tag ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                    parms.Add(new Parameter("@Type", (int)entry.SpawnElem.Type, SqlDbType.Int, ParameterDirection.Input, 50));

                    SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveSpawnElement", parms);
                    //OnEtlEvent(new ETLEventArgs { Message = "Spawn Element : "+ entry.SqlId + " saved.", IsError = false });
                }
                catch (Exception ex)
                {
                    OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                }
            });
        }

        //private void LoadSpawnElement(string ctr, object planElementId, string objectId , SpawnElement spawn)
        //{
        //        try
        //        {
        //            ParameterCollection parms = new ParameterCollection();
        //            parms.Add(new Parameter("@MongoPlanElementId", objectId, SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@PlanElementId", planElementId, SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@MongoSpawnId", spawn.SpawnId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@Tag", spawn.Tag == null ? string.Empty : spawn.Tag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //            parms.Add(new Parameter("@Type", (int)spawn.Type, SqlDbType.Int, ParameterDirection.Input, 50));

        //            SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveSpawnElement", parms);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ArgumentException(ex.Message, planElementId.ToString());
        //        }
        //}

        private void LoadPatientSystems(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient system.", IsError = false });

                ConcurrentBag<MEPatientSystem> systems;                
                using (PatientSystemMongoContext psctx = new PatientSystemMongoContext(ctr))
                {
                    systems = new ConcurrentBag<MEPatientSystem>(psctx.PatientSystems.Collection.FindAllAs<MEPatientSystem>().ToList());
                }

                Parallel.ForEach(systems, system =>
                    //foreach (MEPatientSystem system in systems.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(system.Id.ToString()) ? string.Empty : system.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(system.PatientID.ToString()) ? string.Empty : system.PatientID.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Label", (string.IsNullOrEmpty(system.DisplayLabel) ? string.Empty : system.DisplayLabel), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SystemId", (string.IsNullOrEmpty(system.SystemID) ? string.Empty : system.SystemID), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@SystemName", (string.IsNullOrEmpty(system.SystemName) ? string.Empty : system.SystemName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(system.UpdatedBy.ToString()) ? string.Empty : system.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", system.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(system.RecordCreatedBy.ToString()) ? string.Empty : system.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", system.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", system.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(system.DeleteFlag.ToString()) ? string.Empty : system.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", system.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (system.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", system.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientSystem", parms);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientSystems()", ex));
            }
        }

        private void LoadPatientTasks(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient tasks.", IsError = false });

                ConcurrentBag<MEPatientTask> tasks;                
                using (PatientGoalMongoContext pgctx = new PatientGoalMongoContext(ctr))
                {
                    tasks = new ConcurrentBag<MEPatientTask>(pgctx.PatientTasks.Collection.FindAllAs<MEPatientTask>().ToList());
                }

                Parallel.ForEach(tasks, task =>
                    //foreach (MEPatientTask task in tasks.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoPatientGoalId", (string.IsNullOrEmpty(task.PatientGoalId.ToString()) ? string.Empty : task.PatientGoalId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Name", task.Name == null ? string.Empty : task.Name, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Description", task.Description == null ? string.Empty : task.Description, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@StartDate", task.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(task.Status.ToString()) ? string.Empty : task.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            parms.Add(new Parameter("@StatusDate", task.StatusDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TargetDate", task.TargetDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TargetValue", task.TargetValue == null ? string.Empty : task.TargetValue, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoUpdatedBy", task.UpdatedBy == null ? string.Empty : task.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", task.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(task.RecordCreatedBy.ToString()) ? string.Empty : task.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", task.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", task.Version, SqlDbType.Float, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(task.DeleteFlag.ToString()) ? string.Empty : task.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TimeToLive", task.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (task.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", task.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientTask", parms);

                            if (task.Attributes != null)
                            {
                                foreach (MAttribute att in task.Attributes)
                                {
                                    parms.Clear();
                                    parms.Add(new Parameter("@GoalAttributeMongoID", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@PatientTaskMongoID", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoUpdatedBy", task.UpdatedBy == null ? string.Empty : task.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@LastUpdatedOn", task.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(task.RecordCreatedBy.ToString()) ? string.Empty : task.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@RecordCreatedOn", task.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                    parms.Add(new Parameter("@Version", task.Version, SqlDbType.Float, ParameterDirection.Input, 50));

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientTaskAttribute", parms);

                                    if (att.Values != null)
                                    {
                                        foreach (string val in att.Values)
                                        {
                                            parms.Clear();
                                            parms.Add(new Parameter("@Value", (string.IsNullOrEmpty(val) ? string.Empty : val), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@PatientTaskMongoID", (string.IsNullOrEmpty(task.Id.ToString()) ? string.Empty : task.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@GoalAttributeMongoID", (string.IsNullOrEmpty(att.AttributeId.ToString()) ? string.Empty : att.AttributeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@MongoUpdatedBy", task.UpdatedBy == null ? string.Empty : task.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@LastUpdatedOn", task.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(task.RecordCreatedBy.ToString()) ? string.Empty : task.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@RecordCreatedOn", task.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                            parms.Add(new Parameter("@Version", task.Version, SqlDbType.Float, ParameterDirection.Input, 50));

                                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientTaskAttributeValue", parms);
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
                            parms.Add(new Parameter("@MongoUpdatedBy", task.UpdatedBy == null ? string.Empty : task.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", task.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(task.RecordCreatedBy.ToString()) ? string.Empty : task.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", task.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", task.Version, SqlDbType.Float, ParameterDirection.Input, 50));                            

                                    SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientTaskBarrier", parms);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientTasks()", ex));
            }
        }

        private void LoadPatientUsers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading patient users.", IsError = false });

                ConcurrentBag<MEPatientUser> users;
                using (PatientMongoContext pctx = new PatientMongoContext(ctr))
                {
                    users = new ConcurrentBag<MEPatientUser>(pctx.PatientUsers.Collection.FindAllAs<MEPatientUser>().ToList());
                }

                Parallel.ForEach(users, user =>
                    //foreach (MEPatientUser user in users.Where(t => !t.DeleteFlag))
                    {
                        try
                        {
                            ParameterCollection parms = new ParameterCollection();
                            parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(user.Id.ToString()) ? string.Empty : user.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoPatientId", (string.IsNullOrEmpty(user.PatientId.ToString()) ? string.Empty : user.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@MongoContactId", (string.IsNullOrEmpty(user.ContactId.ToString()) ? string.Empty : user.ContactId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Flag", (string.IsNullOrEmpty(user.Flagged.ToString()) ? string.Empty : user.Flagged.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(user.UpdatedBy.ToString()) ? string.Empty : user.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", user.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(user.RecordCreatedBy.ToString()) ? string.Empty : user.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@RecordCreatedOn", user.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", user.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(user.DeleteFlag.ToString()) ? string.Empty : user.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TTLDate", user.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            if (user.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", user.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                            SQLDataService.Instance.ExecuteScalar("InHealth001", true, "REPORT", "spPhy_RPT_SavePatientUser", parms);
                        }
                        catch (Exception ex)
                        {
                            OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientUsers()", ex));
            }
        }

        private void LoadUsers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "Loading users.", IsError = false });

                ConcurrentBag<MEContact> contacts;
                using (ContactMongoContext cctx = new ContactMongoContext(ctr))
                {
                    contacts = new ConcurrentBag<MEContact>(cctx.Contacts.Collection.FindAllAs<MEContact>().ToList());
                }

                    Parallel.ForEach(contacts, contact =>
                    //foreach (MEContact contact in contacts)//.Where(t => !t.DeleteFlag))
                    {
                        if (contact.PatientId == null)
                        {
                            try
                            {
                                ParameterCollection parms = new ParameterCollection();
                                parms.Add(new Parameter("@MongoID", contact.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@ResourceId", (string.IsNullOrEmpty(contact.ResourceId) ? string.Empty : contact.ResourceId), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@FirstName", (string.IsNullOrEmpty(contact.FirstName) ? string.Empty : contact.FirstName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastName", (string.IsNullOrEmpty(contact.LastName) ? string.Empty : contact.LastName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@PreferredName", (string.IsNullOrEmpty(contact.PreferredName) ? string.Empty : contact.PreferredName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", contact.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(contact.DeleteFlag.ToString()) ? string.Empty : contact.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveUser", parms);

                                if (contact.RecentList != null)
                                {
                                    foreach (ObjectId rec in contact.RecentList)
                                    {
                                        parms.Clear();
                                        parms.Add(new Parameter("@MongoID", rec, SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@UserMongoId", (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@Version", contact.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                                        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                        parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));

                                        SQLDataService.Instance.ExecuteProcedure("InHealth001", true, "REPORT", "spPhy_RPT_SaveUserRecentList", parms);
                                    }
                                }

                                //OnEtlEvent(new ETLEventArgs { Message = "User :" + contact.Id.ToString() + " Loaded." });
                            }
                            catch (Exception ex)
                            {
                                OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadUsers()", ex));
            }
        }
    }
}

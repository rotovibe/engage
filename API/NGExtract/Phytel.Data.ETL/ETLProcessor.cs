using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataDomain.Allergy.Repo;
using DataDomain.Medication.Repo;
using FastMember;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.CareMember;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.PatientObservation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Data.ETL.Templates;
using Phytel.Services.SQLServer;
using Action = Phytel.API.DataDomain.Program.MongoDB.DTO.Action;
using Category = Phytel.API.DataDomain.LookUp.DTO.Category;
using Module = Phytel.API.DataDomain.Program.MongoDB.DTO.Module;
using Objective = Phytel.API.DataDomain.LookUp.DTO.Objective;
using TimeZone = Phytel.API.DataDomain.LookUp.DTO.TimeZone;

namespace Phytel.Data.ETL
{
    public delegate void ETLEventHandler(object sender, ETLEventArgs e);

    public class ETLProcessor
    {
        public event ETLEventHandler EtlEvent;
        private int _exponent = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelProcess"]);
        private bool truncate = Convert.ToBoolean(ConfigurationManager.AppSettings["Truncate"]);
        string _contract;
        private List<SpawnElementHash> _spawnElementDict = new List<SpawnElementHash>();
        private SqlConnection _sqlConnection;
        private string connString;

        // 
        private List<MEPatientProgram> programs;
        private List<Module> modules;
        private List<Action> actions;
        private List<Step> steps;
        private PatientInfo _patientInfo;
        private PatientNotes PatientNote;
        private ToDo ToDos;
        private Contacts Contact;
        private User Users;
        private Medications Meds;
        private MedicationMap MedMap;
        private ContactTypeLookUp contactTypeLookUp;
        private CareTeam careTeam;
        private Templates.System System;
        private Templates.PatientUtilization PatientUtilization;

        void Collections_DocColEvent(object sender, ETLEventArgs e)
        {
            OnEtlEvent(e);
        }

        protected virtual void OnEtlEvent(ETLEventArgs e)
        {
            if (EtlEvent != null)
            {
                EtlEvent(this, e);
            }
        }

        public ETLProcessor(string contract)
        {
            _contract = contract;

            OnEtlEvent(new ETLEventArgs
            {
                Message = "[" + _contract + "] ETLProcessor start initialized.",
                IsError = false
            });

            connString =
                SQLDataService.Instance.GetConnectionString(
                    ConfigurationManager.AppSettings["PhytelServicesConnName"], _contract, true, "REPORT");

            _sqlConnection = new SqlConnection(connString);

            // initialize lists
            programs = new List<MEPatientProgram>();
            modules = new List<Module>();

            _patientInfo = new PatientInfo { Contract = _contract, ConnectionString = connString };
            _patientInfo.DocColEvent += Collections_DocColEvent;

            PatientNote = new PatientNotes { Contract = _contract, ConnectionString = connString };
            PatientNote.DocColEvent += Collections_DocColEvent;

            ToDos = new ToDo { Contract = _contract, ConnectionString = connString };
            ToDos.DocColEvent += Collections_DocColEvent;

            Contact = new Contacts { Contract = _contract, ConnectionString = connString };
            Contact.DocColEvent += Collections_DocColEvent;

            Users = new User { Contract = _contract, ConnectionString = connString };
            Users.DocColEvent += Collections_DocColEvent;

            Meds = new Medications { Contract = _contract, ConnectionString = connString };
            Meds.DocColEvent += Collections_DocColEvent;

            MedMap = new MedicationMap { Contract = _contract, ConnectionString = connString };
            MedMap.DocColEvent += Collections_DocColEvent;

            contactTypeLookUp = new ContactTypeLookUp { Contract = _contract, ConnectionString = connString };
            contactTypeLookUp.DocColEvent += Collections_DocColEvent;

            careTeam = new CareTeam { Contract = _contract, ConnectionString = connString };
            careTeam.DocColEvent += Collections_DocColEvent;

            System = new Templates.System { Contract = _contract, ConnectionString = connString };
            System.DocColEvent += Collections_DocColEvent;

            PatientUtilization = new Templates.PatientUtilization { Contract = _contract, ConnectionString = connString };
            PatientUtilization.DocColEvent += Collections_DocColEvent;
        }

        public void Rebuild()
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] *** ETL PROCESS START ***", IsError = false });
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Truncate Tables.", IsError = false });
                //Truncate/Delete SQL databases
                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_TruncateTables", new ParameterCollection());

                RegisterClasses();
                Users.Export();
                LoadLookUps(_contract);
                LoadGoalAttributes(_contract);
                LoadObservations(_contract);
                System.Export();
                _patientInfo.Export();

                PatientNote.Export();
                PatientUtilization.Export();
                LoadPatientObservations(_contract);

                Contact.Export();
                contactTypeLookUp.Export();
                careTeam.Export();

                //LoadCareMembers(_contract); // Commenting this out as it is replaced by CareTeam
                LoadPatientUsers(_contract);

                LoadPatientGoals(_contract);
                LoadPatientBarriers(_contract);
                LoadPatientInterventions(_contract);
                LoadPatientTasks(_contract);

                LoadAllergies(_contract);
                LoadPatientAllergies(_contract);
                LoadPatientMedSups(_contract);
                Meds.Export();
                MedMap.Export();

                LoadPatientPrograms(_contract);
                LoadPatientProgramModules(_contract);
                LoadPatientProgramActions(_contract);
                LoadPatientProgramSteps(_contract);
                LoadPatientProgramResponses(_contract);
                LoadPatientProgramAttributes(_contract);

                ProcessSpawnElements();
                ToDos.Export();

                FlattenReportSprocs();
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] *** ETL PROCESS COMPLETED ***", IsError = false });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        private void FlattenReportSprocs()
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] - Loading Flat Tables", IsError = false });
                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_Initialize_Flat_Tables", new ParameterCollection(), 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RegisterClasses()
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] registering classes.", IsError = false });

                #region Register ClassMap

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MEPatient)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MEPatient>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(MEProgramAttribute)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(Problem)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(Objective)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(Category)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(CommMode)) == false)
                    {
                        BsonClassMap.RegisterClassMap<CommMode>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(State)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(TimesOfDay)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(TimeZone)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(CommType)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(Language)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Language>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(FocusArea)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(Source)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(BarrierCategory)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(InterventionCategory)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(ObservationType)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(CareMemberType)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(CodingSystem)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(ToDoCategory)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteMethod)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteOutcome)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteWho)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteSource)) == false)
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
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteDuration)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteDuration>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(NoteType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<NoteType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(AllergyType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<AllergyType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(AllergySource)) == false)
                    {
                        BsonClassMap.RegisterClassMap<AllergySource>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Severity)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Severity>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Reaction)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Reaction>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MedSuppType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MedSuppType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(FreqHowOften)) == false)
                    {
                        BsonClassMap.RegisterClassMap<FreqHowOften>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Frequency)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Frequency>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(FreqWhen)) == false)
                    {
                        BsonClassMap.RegisterClassMap<FreqWhen>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MEMedication)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MEMedication>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MEMedicationMapping)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MEMedicationMapping>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(UtilizationLocation)) == false)
                    {
                        BsonClassMap.RegisterClassMap<UtilizationLocation>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(UtilizationSource)) == false)
                    {
                        BsonClassMap.RegisterClassMap<UtilizationSource>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(VisitType)) == false)
                    {
                        BsonClassMap.RegisterClassMap<VisitType>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Disposition)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Disposition>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Reason)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Reason>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MaritalStatus)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MaritalStatus>();
                    }
                }
                catch
                {
                    throw;
                }
                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(MedicationReview)) == false)
                    {
                        BsonClassMap.RegisterClassMap<MedicationReview>();
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Phytel.API.DataDomain.LookUp.DTO.CareTeamFrequency)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.CareTeamFrequency>();
                    }
                }
                catch
                {
                    throw;
                }


                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Phytel.API.DataDomain.LookUp.DTO.DurationUnit)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.DurationUnit>();
                    }
                }
                catch
                {
                    throw;
                }
                try
                {
                    if (BsonClassMap.IsClassMapRegistered(typeof(Phytel.API.DataDomain.LookUp.DTO.RefusalReason)) == false)
                    {
                        BsonClassMap.RegisterClassMap<Phytel.API.DataDomain.LookUp.DTO.RefusalReason>();
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
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        /// Deprecated - CareMembers is replaced by CareTeam now.
        //private void LoadCareMembers(string ctr)
        //{
        //    try
        //    {
        //        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading care members.", IsError = false });

        //        ConcurrentBag<MECareMember> members;
        //        using (CareMemberMongoContext cmctx = new CareMemberMongoContext(ctr))
        //        {
        //            members = new ConcurrentBag<MECareMember>(cmctx.CareMembers.Collection.FindAllAs<MECareMember>().ToList());
        //        }

        //        Parallel.ForEach(members, mem =>
        //        //foreach (MECareMember mem in members)//.Where(t => !t.DeleteFlag))
        //        {
        //            try
        //            {
        //                ParameterCollection parms = new ParameterCollection();
        //                parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(mem.Id.ToString()) ? string.Empty : mem.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(mem.PatientId.ToString()) ? string.Empty : mem.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@ContactMongoId", (string.IsNullOrEmpty(mem.ContactId.ToString()) ? string.Empty : mem.ContactId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@TypeMongoId", (string.IsNullOrEmpty(mem.TypeId.ToString()) ? string.Empty : mem.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@Primary", (string.IsNullOrEmpty(mem.Primary.ToString()) ? string.Empty : mem.Primary.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(mem.UpdatedBy.ToString()) ? string.Empty : mem.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@LastUpdatedOn", mem.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(mem.RecordCreatedBy.ToString()) ? string.Empty : mem.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@RecordCreatedOn", mem.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@Version", mem.Version, SqlDbType.Float, ParameterDirection.Input, 8));
        //                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(mem.DeleteFlag.ToString()) ? string.Empty : mem.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
        //                parms.Add(new Parameter("@TimeToLive", mem.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

        //                if (mem.ExtraElements != null)
        //                    parms.Add(new Parameter("@ExtraElements", mem.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
        //                else
        //                    parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

        //                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SaveCareMember", parms);
        //            }
        //            catch (Exception ex)
        //            {
        //                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] LoadCareMembers():Error" });
        //    }
        //}

        private void LoadGoalAttributes(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading goal attributes.", IsError = false });

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

                        SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveGoalAttribute", parms);

                        foreach (KeyValuePair<int, string> option in att.Options)
                        {
                            parms.Clear();

                            parms.Add(new Parameter("@Key", option.Key, SqlDbType.Int, ParameterDirection.Input, 32));
                            parms.Add(new Parameter("@Value", option.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@GoalAttributeMongoId", att.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(att.Version.ToString()) ? string.Empty : att.Version.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(att.UpdatedBy.ToString()) ? string.Empty : att.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@LastUpdatedOn", att.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                            SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveGoalAttributeOption", parms);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                });
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] LoadGoalAttributes():Error:" + ex.Message + ex.StackTrace, IsError = true });
            }
        }

        private void LoadLookUps(string ctr)
        {
            List<MELookup> lookups;
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading lookups.", IsError = false });
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
                            // AllergyType
                            case LookUpType.AllergyType:
                                LoadAllergyType(lookup);
                                break;
                            // AllergySource
                            case LookUpType.AllergySource:
                                LoadAllergySource(lookup);
                                break;
                            // Severity
                            case LookUpType.Severity:
                                LoadSeverity(lookup);
                                break;
                            // Reaction
                            case LookUpType.Reaction:
                                LoadReaction(lookup);
                                break;
                            // MedSuppType
                            case LookUpType.MedSuppType:
                                LoadMedSupType(lookup);
                                break;
                            // FreqHowOften
                            case LookUpType.FreqHowOften:
                                LoadFreqHowOften(lookup);
                                break;
                            // FreqWhen
                            case LookUpType.FreqWhen:
                                LoadFreqWhen(lookup);
                                break;
                            case LookUpType.Frequency:
                                {
                                    var pmFreq = new PatientMedicationFrequency { Contract = ctr, LookUp = lookup, ConnectionString = connString };
                                    pmFreq.Export();
                                    break;
                                }
                            case LookUpType.NoteType:
                                {
                                    var pnType = new PatientNoteType { Contract = ctr, LookUp = lookup, ConnectionString = connString };
                                    pnType.Export();
                                    break;
                                }
                            case LookUpType.MaritalStatus:
                                LoadLookUp(lookup, "spPhy_RPT_SaveMaritalStatusLookUp");
                                break;
                            case LookUpType.Reason:
                                LoadLookUp(lookup, "spPhy_RPT_SaveStatusReasonLookUp");
                                break;
                            case LookUpType.VisitType:
                                LoadLookUp(lookup, "spPhy_RPT_SaveVisitTypeLookUp");
                                break;
                            case LookUpType.UtilizationLocation:
                                LoadLookUp(lookup, "spPhy_RPT_SaveUtilizationLocationLookUp");
                                break;
                            case LookUpType.Disposition:
                                LoadLookUp(lookup, "spPhy_RPT_SaveDispositionLookUp");
                                break;
                            case LookUpType.UtilizationSource:
                                LoadLookUp(lookup, "spPhy_RPT_SaveUtilizationSourceLookUp");
                                break;
                            case LookUpType.CareTeamFrequency:
                                {
                                    var careTeamFrequency = new Templates.CareTeamFrequency { Contract = ctr, LookUp = lookup, ConnectionString = connString };
                                    careTeamFrequency.Export();
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs
                        {
                            Message =
                                "[" + _contract + "] Error loading specific lookup:" + lookup.GetType().Name + " : " + ex.Message + ": " +
                                ex.StackTrace,
                            IsError = true
                        });
                    }
                }

                LinkObjectiveCategories(lookups.Find(l => l.Data[0].GetType() == typeof(Objective)));
                LinkCommTypeCommModes(lookups.Find(l => l.Data[0].GetType() == typeof(CommType)));
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] LoadLookUps():" + ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        #region LookUp methods
        private void LoadBarrierCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                BarrierCategory bc = (BarrierCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", bc.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", bc.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveBarrierCategoryLookUp", parms);
            }

        }

        private void LoadCareMemberTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CareMemberType cmt = (CareMemberType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", cmt.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmt.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCareMemberTypeLookUp", parms);
            }
        }

        private void LoadCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Category cat = (Category)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", cat.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cat.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCategoryLookUp", parms);
            }
        }

        private void LoadCodingSystems(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CodingSystem cs = (CodingSystem)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", cs.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cs.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCodingSystemLookUp", parms);
            }
        }

        private void LoadCommModes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CommMode cmm = (CommMode)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", cmm.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmm.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCommModeLookUp", parms);
            }
        }

        private void LoadCommTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                CommType cmt = (CommType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", cmt.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", cmt.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCommTypeLookUp", parms);
            }
        }

        private void LoadFocusAreas(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                FocusArea fa = (FocusArea)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", fa.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", fa.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveFocusAreaLookUp", parms);
            }
        }

        private void LoadInterventionCategories(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                InterventionCategory ic = (InterventionCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", ic.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", ic.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveInterventionCategoryLookUp", parms);
            }
        }

        private void LoadLanguages(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Language la = (Language)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", la.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", la.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@Code", la.Code, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Active", la.Active, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveLanguageLookUp", parms);
            }
        }

        private void LoadObjectives(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Objective o = (Objective)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", o.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", o.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@Description", o.Description, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveObjectiveLookUp", parms);
            }
        }

        private void LoadObservationTypes(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                ObservationType ot = (ObservationType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", ot.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", ot.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveObservationTypeLookUp", parms);
            }
        }

        private void LoadProblems(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Problem prb = (Problem)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@Type", (string.IsNullOrEmpty(prb.Type) ? string.Empty : prb.Type), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@CodeSystem", (string.IsNullOrEmpty(prb.CodeSystem) ? string.Empty : prb.CodeSystem), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(prb.Code) ? string.Empty : prb.Code), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Active", prb.Active, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Default", prb.DefaultFeatured, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@DefaultLevel", (string.IsNullOrEmpty(prb.DefaultLevel.ToString()) ? string.Empty : prb.DefaultLevel.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveProblemLookUp", parms);
            }
        }

        // LoadFreqWhen
        private void LoadFreqWhen(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (FreqWhen)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveFreqWhenLookUp", parms);
            }
        }

        // LoadFreqHowOften
        private void LoadFreqHowOften(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (FreqHowOften)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveFreqHowOftenLookUp", parms);
            }
        }


        // LoadMedSupType
        private void LoadMedSupType(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (MedSuppType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveMedSupTypeLookUp", parms);
            }
        }

        private void LoadReaction(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (Reaction)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@CodeSystem", (string.IsNullOrEmpty(prb.CodingSystemId.ToString()) ? string.Empty : prb.CodingSystemId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(prb.CodingSystemCode) ? string.Empty : prb.CodingSystemCode), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveReactionLookUp", parms);
            }
        }

        private void LoadSeverity(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (Severity)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@CodeSystem", (string.IsNullOrEmpty(prb.CodingSystemId.ToString()) ? string.Empty : prb.CodingSystemId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(prb.CodingSystemCode) ? string.Empty : prb.CodingSystemCode), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveSeverityLookUp", parms);
            }
        }

        //LoadAllergySource
        private void LoadAllergySource(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (AllergySource)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@Active", prb.Active, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Default", prb.Default, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveAllergySourceLookUp", parms);
            }
        }

        private void LoadAllergyType(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                var prb = (AllergyType)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", prb.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", prb.Name, SqlDbType.VarChar, ParameterDirection.Input, 300));
                parms.Add(new Parameter("@CodeSystem", (string.IsNullOrEmpty(prb.CodingSystemId.ToString()) ? string.Empty : prb.CodingSystemId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", (string.IsNullOrEmpty(prb.CodingSystemCode) ? string.Empty : prb.CodingSystemCode), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveAllergyTypeLookUp", parms);
            }
        }

        private void LoadSources(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                Source src = (Source)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", src.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", src.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveSourceLookUp", parms);
            }
        }

        private void LoadStates(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                State st = (State)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", st.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", st.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Code", st.Code, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveStateLookUp", parms);
            }
        }

        private void LoadTimesOfDays(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                TimesOfDay tod = (TimesOfDay)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", tod.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tod.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveTimesOfDayLookUp", parms);
            }
        }

        private void LoadTimeZones(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                TimeZone tz = (TimeZone)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", tz.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tz.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                parms.Add(new Parameter("@Default", tz.Default, SqlDbType.VarChar, ParameterDirection.Input, 50));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveTimeZoneLookUp", parms);
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

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", sp, parms);
            }
        }

        private void LoadToDoCategory(MELookup lookup)
        {
            foreach (LookUpBase lbase in lookup.Data)
            {
                ToDoCategory tz = (ToDoCategory)lbase;

                ParameterCollection parms = new ParameterCollection();

                parms.Add(new Parameter("@LookUpType", lookup.Type.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 200));
                parms.Add(new Parameter("@MongoID", tz.DataId, SqlDbType.VarChar, ParameterDirection.Input, 50));
                parms.Add(new Parameter("@Name", tz.Name, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveToDoCategoryLookUp", parms);
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

                    SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveCommTypeCommMode", parms);
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

                    SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveObjectiveCategory", parms);
                }
            }
        }
        #endregion

        private void LoadObservations(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading observations.", IsError = false });

                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Getting MEObservations from MONGO.", IsError = false });
                ConcurrentBag<MEObservation> observations;
                using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
                {
                    observations = new ConcurrentBag<MEObservation>(poctx.Observations.Collection.FindAllAs<MEObservation>().ToList());
                }

                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Inserting observations into SQL.", IsError = false });
                //Parallel.ForEach(observations, obs =>
                foreach (MEObservation obs in observations)//.Where(t => !t.DeleteFlag))
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
                        parms.Add(new Parameter("@HighValue", obs.HighValue ?? -1, SqlDbType.Decimal, ParameterDirection.Input, 32));
                        parms.Add(new Parameter("@LastUpdatedOn", obs.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LowValue", obs.LowValue ?? -1, SqlDbType.Decimal, ParameterDirection.Input, 32));
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


                        SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveObservation", parms);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }//);
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] LoadObservations()" + ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        private void LoadPatientBarriers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient barriers.", IsError = false });

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
                        parms.Add(new Parameter("@Details", (string.IsNullOrEmpty(bar.Details) ? (object)DBNull.Value : bar.Details), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        if (bar.ExtraElements != null)
                            parms.Add(new Parameter("@ExtraElements", bar.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        else
                            parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientBarrier", parms);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
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
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient goals.", IsError = false });

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
                        parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(goal.Name) ? string.Empty : goal.Name), SqlDbType.VarChar, ParameterDirection.Input, 500));
                        parms.Add(new Parameter("@Description", goal.Description == null ? string.Empty : goal.Description, SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@StartDate", goal.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@EndDate", goal.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(goal.Status.ToString()) ? string.Empty : goal.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
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
                        parms.Add(new Parameter("@Details", (string.IsNullOrEmpty(goal.Details) ? (object)DBNull.Value : goal.Details), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        if (goal.ExtraElements != null)
                            parms.Add(new Parameter("@ExtraElements", goal.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        else
                            parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));

                        parms.Add(new Parameter("@TemplateId", (string.IsNullOrEmpty(goal.TemplateId.ToString()) ? string.Empty : goal.TemplateId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientGoal", parms);

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

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientGoalAttribute", parms);

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

                                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientGoalAttributeValue", parms);
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

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientGoalFocusArea", parms);
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

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientGoalProgram", parms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
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
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient interventions.", IsError = false });

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
                            parms.Add(new Parameter("@Description", (string.IsNullOrEmpty(intervention.Description) ? string.Empty : intervention.Description), SqlDbType.VarChar, ParameterDirection.Input, 5000));
                            parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(intervention.Name) ? string.Empty : intervention.Name), SqlDbType.VarChar, ParameterDirection.Input, 100));
                            parms.Add(new Parameter("@Details", (string.IsNullOrEmpty(intervention.Details) ? (object)DBNull.Value : intervention.Details), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                            if (intervention.ExtraElements != null)
                                parms.Add(new Parameter("@ExtraElements", intervention.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 5000));
                            else
                                parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));

                            // new fields
                            parms.Add(new Parameter("@ClosedDate", intervention.ClosedDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@TemplateId", (string.IsNullOrEmpty(intervention.TemplateId.ToString()) ? string.Empty : intervention.TemplateId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                            parms.Add(new Parameter("@DueDate", intervention.DueDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                            SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientIntervention", parms);

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

                                    SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientInterventionBarrier", parms);

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientInterventions()", ex));
            }
        }

        private void LoadPatientObservations(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient observations.", IsError = false });

                ConcurrentBag<MEPatientObservation> observations;
                using (PatientObservationMongoContext poctx = new PatientObservationMongoContext(ctr))
                {
                    observations =
                        new ConcurrentBag<MEPatientObservation>(
                            poctx.PatientObservations.Collection.FindAllAs<MEPatientObservation>().ToList());
                }

                var rSeries = new ReadObservationsSeries(_contract).ReadEObservationSeries(observations.ToList());

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
                        //  bcc.ColumnMappings.Add("Type", "Type");
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
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items =
                                (Object[])
                                    sortedColumns.GetType()
                                        .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                        .GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType()
                                .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column =
                                metadata.GetType()
                                    .GetField("column",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            var length =
                                metadata.GetType()
                                    .GetField("length",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);

                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientObservations():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
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

                //                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientObservation", parms);
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
                OnEtlEvent(new ETLEventArgs
                {
                    Message =
                        "[" + _contract + "] LoadPatientObservations():SqlBulkCopy process failure: " + ex.Message + ex.InnerException,
                    IsError = true
                });
            }
        }

        private void LoadPlanElementLists()
        {
            //programs.ForEach(m => { modules = m.Modules.ToList(); });
            modules = programs.SelectMany(m => m.Modules).ToList();
            actions = programs.SelectMany(m => m.Modules).SelectMany(a => a.Actions).ToList();
            steps = programs.SelectMany(m => m.Modules).SelectMany(a => a.Actions).SelectMany(s => s.Steps).ToList();
        }

        private void LoadPatientProgramAttributes(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading program attributes.", IsError = false });
                ConcurrentBag<MEProgramAttribute> programAttributes;
                using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
                {
                    programAttributes = new ConcurrentBag<MEProgramAttribute>(pmctx.ProgramAttributes.Collection.FindAllAs<MEProgramAttribute>().ToList());
                }

                var rSeries = new ReadPlanElementsSeries(_contract).ReadEProgramAttributeSeries(programAttributes);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 580;
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("Completed", "Completed");
                        bcc.ColumnMappings.Add("DidNotEnrollReason", "DidNotEnrollReason");
                        bcc.ColumnMappings.Add("Eligibility", "Eligibility");
                        bcc.ColumnMappings.Add("Enrollment", "Enrollment");
                        bcc.ColumnMappings.Add("GraduatedFlag", "GraduatedFlag");
                        bcc.ColumnMappings.Add("InelligibleReason", "InelligibleReason");
                        bcc.ColumnMappings.Add("Lock", "Lock");
                        bcc.ColumnMappings.Add("OptOut", "OptOut");
                        bcc.ColumnMappings.Add("OverrideReason", "OverrideReason");
                        bcc.ColumnMappings.Add("MongoPlanElementId", "MongoPlanElementId");
                        bcc.ColumnMappings.Add("PlanElementId", "PlanElementId");
                        bcc.ColumnMappings.Add("Population", "Population");
                        bcc.ColumnMappings.Add("RemovedReason", "RemovedReason");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.ColumnMappings.Add("DateCompleted", "DateCompleted");
                        bcc.ColumnMappings.Add("CompletedBy", "CompletedBy");
                        bcc.ColumnMappings.Add("MongoCompletedBy", "MongoCompletedBy");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.DestinationTableName = "RPT_PatientProgramAttribute";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items =
                                (Object[])
                                    sortedColumns.GetType()
                                        .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                        .GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType()
                                .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column =
                                metadata.GetType()
                                    .GetField("column",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            var length =
                                metadata.GetType()
                                    .GetField("length",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);

                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientProgramAttributes():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
                    }
                }

                #region old
                ////foreach (MEProgramAttribute prog in programAttributes.Where(t => !t.DeleteFlag))
                //    Parallel.ForEach(programAttributes, prog =>
                //    {
                //        try
                //        {
                //            //if(!prog.DeleteFlag)
                //            //{
                //                ParameterCollection parms = new ParameterCollection();
                //                parms.Add(new Parameter("@MongoId", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@MongoPlanElementId", prog.PlanElementId == null ? string.Empty : prog.PlanElementId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //                // look into
                //                parms.Add(new Parameter("@Completed", prog.Completed == null ? string.Empty : prog.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //                parms.Add(new Parameter("@DidNotEnrollReason", prog.DidNotEnrollReason ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Eligibility", prog.Eligibility == null ? string.Empty : prog.Eligibility.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Enrollment", prog.Enrollment == null ? string.Empty : prog.Enrollment.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@GraduatedFlag", prog.GraduatedFlag == null ? string.Empty : prog.GraduatedFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@InelligibleReason", prog.IneligibleReason == null ? string.Empty : prog.IneligibleReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Lock", prog.Locked == null ? string.Empty : prog.Locked.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@OptOut", prog.OptOut == null ? string.Empty : prog.OptOut.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@OverrideReason", prog.OverrideReason == null ? string.Empty : prog.OverrideReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Population", prog.Population == null ? string.Empty : prog.Population.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@RemovedReason", prog.RemovedReason == null ? string.Empty : prog.RemovedReason.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Status", prog.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@MongoCompletedBy", prog.CompletedBy == null ? string.Empty : prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@DateCompleted", prog.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //                parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));


                //                var patientProgramId = SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT",
                //                    "spPhy_RPT_SavePatientProgramAttribute", parms);

                //                //OnEtlEvent(new ETLEventArgs { Message = "Program attribute:" + prog.PlanElementId.ToString() + "Loaded." });
                //            //}
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
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramAttributes()", ex));
            }
        }

        private void LoadPatientPrograms(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient programs.", IsError = false });

                //ConcurrentBag<MEPatientProgram> programs;
                using (ProgramMongoContext pctx = new ProgramMongoContext(ctr))
                {
                    //programs = new List<MEPatientProgram>(Utils.GetMongoCollectionList(pctx.PatientPrograms.Collection, 300));
                    programs = new List<MEPatientProgram>(pctx.PatientPrograms.Collection.FindAllAs<MEPatientProgram>().ToList());
                }

                LoadPlanElementLists();

                var rSeries = new ReadPlanElementsSeries(_contract).ReadEProgramSeries(programs);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 180;
                        bcc.ColumnMappings.Add("PatientId", "PatientId");
                        bcc.ColumnMappings.Add("MongoPatientId", "MongoPatientId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("Name", "Name");
                        bcc.ColumnMappings.Add("ShortName", "ShortName");
                        bcc.ColumnMappings.Add("Description", "Description");
                        bcc.ColumnMappings.Add("AttributeStartDate", "AttributeStartDate");
                        bcc.ColumnMappings.Add("AttributeEndDate", "AttributeEndDate");
                        bcc.ColumnMappings.Add("SourceId", "SourceId");
                        bcc.ColumnMappings.Add("Order", "Order");
                        bcc.ColumnMappings.Add("Eligible", "Eligible");
                        bcc.ColumnMappings.Add("State", "State");
                        bcc.ColumnMappings.Add("AssignedOn", "AssignedOn");
                        bcc.ColumnMappings.Add("MongoAssignedBy", "MongoAssignedBy");
                        bcc.ColumnMappings.Add("AssignedBy", "AssignedBy");
                        bcc.ColumnMappings.Add("MongoAssignedToId", "MongoAssignedToId");
                        bcc.ColumnMappings.Add("AssignedToId", "AssignedToId");
                        bcc.ColumnMappings.Add("Completed", "Completed");
                        bcc.ColumnMappings.Add("EligibilityStartDate", "EligibilityStartDate");
                        bcc.ColumnMappings.Add("EligibilityReason", "EligibilityReason");
                        bcc.ColumnMappings.Add("StartDate", "StartDate");
                        bcc.ColumnMappings.Add("EndDate", "EndDate");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("ContractProgramId", "ContractProgramId");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.ColumnMappings.Add("Enabled", "Enabled");
                        bcc.ColumnMappings.Add("StateUpdatedOn", "StateUpdatedOn");
                        bcc.ColumnMappings.Add("CompletedBy", "CompletedBy");
                        bcc.ColumnMappings.Add("MongoCompletedBy", "MongoCompletedBy");
                        bcc.ColumnMappings.Add("DateCompleted", "DateCompleted");
                        bcc.ColumnMappings.Add("EligibilityRequirements", "EligibilityRequirements");
                        bcc.ColumnMappings.Add("EligibilityEndDate", "EligibilityEndDate");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");

                        bcc.DestinationTableName = "RPT_PatientProgram";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items =
                                (Object[])
                                    sortedColumns.GetType()
                                        .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                        .GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType()
                                .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column =
                                metadata.GetType()
                                    .GetField("column",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            var length =
                                metadata.GetType()
                                    .GetField("length",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);

                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientPrograms():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
                    }
                }

                #region -- Old --

                //_sqlConnection.Open();
                //    Parallel.ForEach(programs, prog =>
                //        //foreach (MEPatientProgram prog in programs.Where(prog => !prog.DeleteFlag))
                //    {
                //        try
                //        {
                //            //if(!prog.DeleteFlag)
                //            //{
                //            ParameterCollection parms = new ParameterCollection();
                //            parms.Add(new Parameter("@MongoID", prog.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoPatientId", prog.PatientId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoAssignedBy", prog.AssignedBy == null ? string.Empty : prog.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AssignedOn", prog.AssignedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoAssignedToId", prog.AssignedTo == null ? string.Empty : prog.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AttributeEndDate", prog.AttributeEndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AttributeStartDate", prog.AttributeStartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Completed", prog.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@ContractProgramId", prog.ContractProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Description", prog.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //            parms.Add(new Parameter("@EligibilityReason", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@EligibilityStartDate", prog.EligibilityStartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@EndDate", prog.EndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Name", prog.Name ?? null, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Order", prog.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@ShortName", prog.ShortName ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@SourceId", prog.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@StartDate", prog.StartDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@State", prog.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Status", prog.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Enabled", prog.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@StateUpdatedOn", prog.StateUpdatedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoCompletedBy", prog.CompletedBy == null ? string.Empty : prog.CompletedBy, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@DateCompleted", prog.DateCompleted ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@EligibilityRequirements", prog.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //            parms.Add(new Parameter("@EligibilityEndDate", prog.EligibilityEndDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object) DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            //
                //            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));


                //            var patientProgramId = ExecuteScalarSproc(parms, _sqlConnection, "spPhy_RPT_SavePatientProgram");

                //            #region -- archived 
                //            // original call
                //            //patientProgramId = SQLDataService.Instance.ExecuteScalar(_contract, true,
                //            //    "REPORT",
                //            //    "spPhy_RPT_SavePatientProgram", parms);

                //            //var times = 0;
                //            //try
                //            //{
                //            //    ExecuteScalarSproc(parms, out patientProgramId);
                //            //}
                //            //catch (Exception ex)
                //            //{
                //            //    if (ex != null && ex is SqlException)
                //            //    {
                //            //        foreach (SqlError error in (ex as SqlException).Errors)
                //            //        {
                //            //            switch (error.Number)
                //            //            {
                //            //                case 1205:
                //            //                {
                //            //                    System.Diagnostics.Debug.WriteLine(
                //            //                        "SQL Error: Deadlock condition. Retrying...");
                //            //                    //return true;
                //            //                    if (times <= 5)
                //            //                    {
                //            //                        ExecuteScalarSproc(parms, out patientProgramId);
                //            //                        times = times++;
                //            //                    }
                //            //                    break;
                //            //                }
                //            //                case -2:
                //            //                    System.Diagnostics.Debug.WriteLine(
                //            //                        "SQL Error: Timeout expired. Retrying...");
                //            //                    //return true;
                //            //                    if (times <= 5)
                //            //                    {
                //            //                        ExecuteScalarSproc(parms, out patientProgramId);
                //            //                        times = times++;
                //            //                    }
                //            //                    break;
                //            //            }
                //            //        }
                //            //    }
                //            //}
                //            #endregion

                //            if (patientProgramId != null && prog.Spawn != null && prog.Spawn.Count > 0)
                //            {
                //                //LoadSpawnElement(ctr, planElementId, prog.Id.ToString(), prog.Spawn);
                //                RegisterSpawnElement(prog.Spawn, prog.Id.ToString(), (int) patientProgramId);
                //            }

                //            LoadPatientProgramModules(ctr, patientProgramId, prog.Modules, prog);
                //            //OnEtlEvent(new ETLEventArgs {Message = "Program:" + prog.Id.ToString() + "Loaded."});
                //            //}
                //        }
                //        catch (Exception ex)
                //        {
                //            OnEtlEvent(new ETLEventArgs {Message = ex.Message + ": " + ex.StackTrace, IsError = true});
                //        }
                //    });
                //    _sqlConnection.Close();

                #endregion

            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                throw new ArgumentException("LoadPatientPrograms() : " + ex.Message + ex.StackTrace, ex.InnerException);
            }
        }

        private void LoadPatientProgramModules(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient modules.", IsError = false });

                var rSeries = new ReadPlanElementsSeries(_contract).ReadEModuleSeries(modules, programs);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 180;
                        bcc.ColumnMappings.Add("PatientProgramId", "PatientProgramId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("MongoProgramId", "MongoProgramId");
                        bcc.ColumnMappings.Add("AttributeStartDate", "AttributeStartDate");
                        bcc.ColumnMappings.Add("AttributeEndDate", "AttributeEndDate");
                        bcc.ColumnMappings.Add("SourceId", "SourceId");
                        bcc.ColumnMappings.Add("Order", "Order");
                        bcc.ColumnMappings.Add("Eligible", "Eligible");
                        bcc.ColumnMappings.Add("State", "State");
                        bcc.ColumnMappings.Add("AssignedOn", "AssignedOn");
                        bcc.ColumnMappings.Add("MongoAssignedBy", "MongoAssignedBy");
                        bcc.ColumnMappings.Add("AssignedBy", "AssignedBy");
                        bcc.ColumnMappings.Add("MongoAssignedTo", "MongoAssignedTo");
                        bcc.ColumnMappings.Add("AssignedTo", "AssignedTo");
                        bcc.ColumnMappings.Add("Completed", "Completed");
                        bcc.ColumnMappings.Add("EligibilityEndDate", "EligibilityEndDate");
                        bcc.ColumnMappings.Add("Name", "Name");
                        bcc.ColumnMappings.Add("Description", "Description");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("StateUpdatedOn", "StateUpdatedOn");
                        bcc.ColumnMappings.Add("Enabled", "Enabled");
                        bcc.ColumnMappings.Add("MongoCompletedBy", "MongoCompletedBy");
                        bcc.ColumnMappings.Add("CompletedBy", "CompletedBy");
                        bcc.ColumnMappings.Add("DateCompleted", "DateCompleted");
                        bcc.ColumnMappings.Add("MongoNext", "MongoNext");
                        bcc.ColumnMappings.Add("Next", "Next");
                        bcc.ColumnMappings.Add("MongoPrevious", "MongoPrevious");
                        bcc.ColumnMappings.Add("Previous", "Previous");
                        bcc.ColumnMappings.Add("EligibilityRequirements", "EligibilityRequirements");
                        bcc.ColumnMappings.Add("EligibilityStartDate", "EligibilityStartDate");
                        bcc.ColumnMappings.Add("LastupdatedOn", "LastupdatedOn");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.DestinationTableName = "RPT_PatientProgramModule";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items =
                                (Object[])
                                    sortedColumns.GetType()
                                        .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                        .GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType()
                                .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column =
                                metadata.GetType()
                                    .GetField("column",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            var length =
                                metadata.GetType()
                                    .GetField("length",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientProgramModules():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
                    }
                }

                #region old
                //Parallel.ForEach(list, mod =>
                //    //foreach (Module mod in list)
                //    {
                //        try
                //        {
                //            ParameterCollection parms = new ParameterCollection();
                //            parms.Add(new Parameter("@PatientProgramId", patientProgramId, SqlDbType.Int, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoProgramId", mod.ProgramId == null ? string.Empty : mod.ProgramId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoAssignedBy", mod.AssignedBy == null ? string.Empty : mod.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AssignedOn", mod.AssignedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoAssignedToId", mod.AssignedTo == null ? string.Empty : mod.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AttributeEndDate", mod.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@AttributeStartDate", mod.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Completed", mod.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Description", mod.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //            parms.Add(new Parameter("@MongoPrevious", mod.Previous == null ? string.Empty : mod.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoNext", mod.Next == null ? string.Empty : mod.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //            parms.Add(new Parameter("@EligibilityStartDate", mod.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Name", mod.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Order", mod.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@SourceId", mod.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@State", mod.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Status", mod.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@Enabled", mod.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@StateUpdatedOn", mod.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoCompletedBy", mod.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@DateCompleted", mod.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@EligibilityRequirements", mod.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //            parms.Add(new Parameter("@EligibilityEndDate", mod.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //            parms.Add(new Parameter("@MongoId", mod.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //            //
                //            //parms.Add(new Parameter("@BackGround", (string.IsNullOrEmpty(prog.Background) ? string.Empty : prog.Background), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //            var patientProgramModuleId = SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientProgramModule", parms);

                //            if (patientProgramModuleId != null && mod.Spawn != null && mod.Spawn.Count > 0)
                //            {
                //                //LoadSpawnElement(ctr, patientProgramModuleId, mod.Id.ToString(), mod.Spawn);
                //                RegisterSpawnElement(mod.Spawn, mod.Id.ToString(), (int)patientProgramModuleId);
                //            }

                //            LoadPatientProgramActions(ctr, patientProgramModuleId, mod.Actions, prog);
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
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                throw new Exception("LoadPatientProgramModules()" + ex.Message + ex.StackTrace);
            }
        }

        private void LoadPatientProgramActions(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient actions.", IsError = false });

                var rSeries = new ReadPlanElementsSeries(_contract).ReadEActionSeries(actions, modules, programs);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 180;
                        bcc.ColumnMappings.Add("PatientProgramModuleId", "PatientProgramModuleId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("MongoModuleId", "MongoModuleId");
                        bcc.ColumnMappings.Add("SourceId", "SourceId");
                        bcc.ColumnMappings.Add("Order", "Order");
                        bcc.ColumnMappings.Add("Eligible", "Eligible");
                        bcc.ColumnMappings.Add("State", "State");
                        bcc.ColumnMappings.Add("StateUpdatedOn", "StateUpdatedOn");
                        bcc.ColumnMappings.Add("AssignedOn", "AssignedOn");
                        bcc.ColumnMappings.Add("MongoAssignedBy", "MongoAssignedBy");
                        bcc.ColumnMappings.Add("AssignedBy", "AssignedBy");
                        bcc.ColumnMappings.Add("MongoAssignedTo", "MongoAssignedTo");
                        bcc.ColumnMappings.Add("AssignedTo", "AssignedTo");
                        bcc.ColumnMappings.Add("MongoCompletedBy", "MongoCompletedBy");
                        bcc.ColumnMappings.Add("CompletedBy", "CompletedBy");
                        bcc.ColumnMappings.Add("Completed", "Completed");
                        bcc.ColumnMappings.Add("EligibilityEndDate", "EligibilityEndDate");
                        bcc.ColumnMappings.Add("Name", "Name");
                        bcc.ColumnMappings.Add("Description", "Description");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("AttributeEndDate", "AttributeEndDate");
                        bcc.ColumnMappings.Add("AttributeStartDate", "AttributeStartDate");
                        bcc.ColumnMappings.Add("Enabled", "Enabled");
                        bcc.ColumnMappings.Add("DateCompleted", "DateCompleted");
                        bcc.ColumnMappings.Add("MongoNext", "MongoNext");
                        bcc.ColumnMappings.Add("Next", "Next");
                        bcc.ColumnMappings.Add("MongoPrevious", "MongoPrevious");
                        bcc.ColumnMappings.Add("Previous", "Previous");
                        bcc.ColumnMappings.Add("EligibilityRequirements", "EligibilityRequirements");
                        bcc.ColumnMappings.Add("EligibilityStartDate", "EligibilityStartDate");
                        bcc.ColumnMappings.Add("version", "version");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.ColumnMappings.Add("Archived", "Archived");
                        bcc.ColumnMappings.Add("ArchivedDate", "ArchivedDate");
                        bcc.ColumnMappings.Add("MongoArchiveOriginId", "MongoArchiveOriginId");
                        bcc.DestinationTableName = "RPT_PatientProgramAction";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items =
                                (Object[])
                                    sortedColumns.GetType()
                                        .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                        .GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType()
                                .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column =
                                metadata.GetType()
                                    .GetField("column",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            var length =
                                metadata.GetType()
                                    .GetField("length",
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(metadata);
                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientProgramActions():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
                    }
                }

                #region old
                //Parallel.ForEach(list, act =>
                ////foreach (Action act in list)
                //{
                //    try
                //    {
                //        ParameterCollection parms = new ParameterCollection();
                //        parms.Add(new Parameter("@MongoId", act.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoModuleId", act.ModuleId == null ? string.Empty : act.ModuleId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@PatientProgramModuleId", patientProgramModuleId, SqlDbType.Int, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoAssignedBy", act.AssignedBy == null ? string.Empty : act.AssignedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@AssignedOn", act.AssignedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoAssignedToId", act.AssignedTo == null ? string.Empty : act.AssignedTo.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@AttributeEndDate", act.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@AttributeStartDate", act.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Completed", act.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Description", act.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@EligibilityStartDate", act.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Name", act.Name ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Order", act.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@SourceId", act.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@State", act.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Status", act.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Enabled", act.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@StateUpdatedOn", act.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoCompletedBy", act.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@DateCompleted", act.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@EligibilityRequirements", act.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@EligibilityEndDate", act.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoPrevious", act.Previous == null ? string.Empty : act.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoNext", act.Next == null ? string.Empty : act.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Archived", act.Archived.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@ArchivedDate", act.ArchivedDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoArchiveOriginId", act.ArchiveOriginId == null ? string.Empty : act.ArchiveOriginId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //        parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //        var patientProgramActionId = SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientProgramModuleAction", parms);

                //        if (patientProgramActionId != null && act.Spawn != null && act.Spawn.Count > 0)
                //        {
                //            //LoadSpawnElement(ctr, patientProgramActionId, act.Id.ToString(), act.Spawn);
                //            RegisterSpawnElement(act.Spawn, act.Id.ToString(), (int)patientProgramActionId);
                //        }

                //        LoadPatientProgramSteps(ctr, patientProgramActionId, act.Steps, prog);
                //    }
                //    catch (Exception ex)
                //    {
                //        OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                //    }
                //});
                #endregion
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramActions()", ex));
            }
        }

        private void LoadPatientProgramSteps(string ctr)
        {
            try
            {

                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient steps.", IsError = false });

                var rSeries = new ReadPlanElementsSeries(_contract).ReadEStepSeries(steps, modules, programs);

                using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
                using (var objRdr = ObjectReader.Create(rSeries))
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 580;

                        bcc.ColumnMappings.Add("MongoActionId", "MongoActionId");
                        bcc.ColumnMappings.Add("ActionId", "ActionId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("AttributeEndDate", "AttributeEndDate");
                        bcc.ColumnMappings.Add("AttributeStartDate", "AttributeStartDate");
                        bcc.ColumnMappings.Add("SourceId", "SourceId");
                        bcc.ColumnMappings.Add("Order", "Order");
                        bcc.ColumnMappings.Add("Eligible", "Eligible");
                        bcc.ColumnMappings.Add("State", "State");
                        bcc.ColumnMappings.Add("Completed", "Completed");
                        bcc.ColumnMappings.Add("EligibilityEndDate", "EligibilityEndDate");
                        bcc.ColumnMappings.Add("Header", "Header");
                        bcc.ColumnMappings.Add("SelectedResponseId", "SelectedResponseId");
                        bcc.ColumnMappings.Add("ControlType", "ControlType");
                        bcc.ColumnMappings.Add("SelectType", "SelectType");
                        bcc.ColumnMappings.Add("IncludeTime", "IncludeTime");
                        bcc.ColumnMappings.Add("Question", "Question");
                        bcc.ColumnMappings.Add("Title", "Title");
                        bcc.ColumnMappings.Add("Description", "Description");
                        bcc.ColumnMappings.Add("Notes", "Notes");
                        bcc.ColumnMappings.Add("Text", "Text");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("Response", "Response");
                        bcc.ColumnMappings.Add("StepTypeId", "StepTypeId");
                        bcc.ColumnMappings.Add("Enabled", "Enabled");
                        bcc.ColumnMappings.Add("StateUpdatedOn", "StateUpdatedOn");
                        bcc.ColumnMappings.Add("MongoCompletedBy", "MongoCompletedBy");
                        bcc.ColumnMappings.Add("CompletedBy", "CompletedBy");
                        bcc.ColumnMappings.Add("DateCompleted", "DateCompleted");
                        bcc.ColumnMappings.Add("MongoNext", "MongoNext");
                        bcc.ColumnMappings.Add("Next", "Next");
                        bcc.ColumnMappings.Add("Previous", "Previous");
                        bcc.ColumnMappings.Add("EligibilityRequirements", "EligibilityRequirements");
                        bcc.ColumnMappings.Add("EligibilityStartDate", "EligibilityStartDate");
                        bcc.ColumnMappings.Add("MongoPrevious", "MongoPrevious");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.ColumnMappings.Add("Delete", "Delete");

                        bcc.DestinationTableName = "RPT_PatientProgramStep";
                        bcc.WriteToServer(objRdr);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                        {
                            string pattern = @"\d+";
                            Match match = Regex.Match(ex.Message.ToString(), pattern);
                            var index = Convert.ToInt32(match.Value) - 1;

                            FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                            var sortedColumns = fi.GetValue(bcc);
                            var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                            FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                            var metadata = itemdata.GetValue(items[index]);

                            var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                            var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);

                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] LoadPatientProgramSteps():SqlBulkCopy process failure: " + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " +
                                    ex.InnerException,
                                IsError = true
                            });
                        }
                    }
                }

                #region old

                //Parallel.ForEach(list, step =>
                ////foreach (Step step in list)
                //{
                //    try
                //    {
                //        ParameterCollection parms = new ParameterCollection();
                //        parms.Add(new Parameter("@MongoId", step.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoActionId", step.ActionId == null ? string.Empty : step.ActionId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@ActionId", patientProgramActionId, SqlDbType.Int, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@StepTypeId", step.StepTypeId, SqlDbType.Int, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Header", step.Header ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 100));
                //        parms.Add(new Parameter("@SelectedResponseId", step.SelectedResponseId == null ? string.Empty : step.SelectedResponseId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@ControlType", step.ControlType.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@SelectType", step.SelectType.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@IncludeTime", step.IncludeTime.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Question", step.Question ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@Title", step.Title ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 100));
                //        parms.Add(new Parameter("@Description", step.Description ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@Notes", step.Notes ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@Text", step.Text ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@Status", step.Status.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoPrevious", step.Previous == null ? string.Empty : step.Previous.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoNext", step.Next == null ? string.Empty : step.Next.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        // inherited fields
                //        parms.Add(new Parameter("@AttributeEndDate", step.AttributeEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@AttributeStartDate", step.AttributeStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Completed", step.Completed.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@EligibilityStartDate", step.EligibilityStartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Eligible", "", SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Order", step.Order.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@SourceId", step.SourceId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@State", step.State.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Enabled", step.Enabled.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@StateUpdatedOn", step.StateUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoCompletedBy", step.CompletedBy ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@DateCompleted", step.DateCompleted ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@EligibilityRequirements", step.EligibilityRequirements ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, -1));
                //        parms.Add(new Parameter("@EligibilityEndDate", step.EligibilityEndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

                //        parms.Add(new Parameter("@Version", prog.Version.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoUpdatedBy", prog.UpdatedBy == null ? string.Empty : prog.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@LastUpdatedOn", prog.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@MongoRecordCreatedBy", prog.RecordCreatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@RecordCreatedOn", prog.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@TTLDate", prog.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                //        parms.Add(new Parameter("@Delete", prog.DeleteFlag.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));

                //        var StepId = SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientProgramModuleActionStep", parms);

                //        if (StepId != null && step.Spawn != null && step.Spawn.Count > 0)
                //        {
                //            //LoadSpawnElement(ctr, StepId, step.Id.ToString(), step.Spawn);
                //            RegisterSpawnElement(step.Spawn, step.Id.ToString(), (int)StepId);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        OnEtlEvent(new ETLEventArgs { Message = ex.Message + ": " + ex.StackTrace, IsError = true });
                //    }
                //});

                #endregion
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
                OnEtlEvent(new ETLEventArgs
                {
                    Message = "[" + _contract + "] Disabling indexes in PatientProgramResponses table.",
                    IsError = false
                });

                ParameterCollection p1 = new ParameterCollection();
                p1.Add(new Parameter("@tableName", "RPT_PatientProgramResponse", SqlDbType.VarChar, ParameterDirection.Input, 128));
                p1.Add(new Parameter("@indexOption", 0, SqlDbType.Bit, ParameterDirection.Input, 1));
                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_ToggleTableIndexes", p1);

                using (var pmctx = new ProgramMongoContext(ctr))
                {
                    var batchCounter = 0;
                    foreach (var batch in Utils.GetMongoCollectionBatch(pmctx.PatientProgramResponses.Collection, 100000))
                    {
                        batchCounter += batch.Count;
                        LoadPatientProgramResponse(batch);
                        try
                        {
                            OnEtlEvent(new ETLEventArgs
                            {
                                Message =
                                    "[" + _contract + "] Loading PatientProgramResponses in batch = " +
                                    batchCounter.ToString(),
                                IsError = false
                            });
                        }
                        catch
                        {
                        }
                    }
                }
                OnEtlEvent(new ETLEventArgs
                {
                    Message = "[" + _contract + "] Rebuilding indexes in PatientProgramResponses table.",
                    IsError = false
                });
                ParameterCollection p2 = new ParameterCollection();
                p2.Add(new Parameter("@tableName", "RPT_PatientProgramResponse", SqlDbType.VarChar, ParameterDirection.Input, 128));
                p2.Add(new Parameter("@indexOption", 1, SqlDbType.Bit, ParameterDirection.Input, 1));
                SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_ToggleTableIndexes", p2);
            }
            catch (Exception ex)
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
            }
        }

        private void LoadPatientProgramResponse(List<MEPatientProgramResponse> responses)
        {
            // get stepidlist
            var stepIdList = Utils.GetStepIdList(_contract);
            var rSeries = new ReadPlanElementsSeries(_contract).ReadEStepResponseSeries(responses);

            using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
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
                    if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                    {
                        string pattern = @"\d+";
                        Match match = Regex.Match(ex.Message.ToString(), pattern);
                        var index = Convert.ToInt32(match.Value) - 1;

                        FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        var sortedColumns = fi.GetValue(bcc);
                        var items =
                            (Object[])
                                sortedColumns.GetType()
                                    .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                    .GetValue(sortedColumns);

                        FieldInfo itemdata = items[index].GetType()
                            .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                        var metadata = itemdata.GetValue(items[index]);

                        var column =
                            metadata.GetType()
                                .GetField("column",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .GetValue(metadata);
                        var length =
                            metadata.GetType()
                                .GetField("length",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .GetValue(metadata);

                        OnEtlEvent(new ETLEventArgs
                        {
                            Message =
                                "[" + _contract + "] LoadPatientProgramResponses():SqlBulkCopy process failure: " + ex.Message +
                                String.Format("Column: {0} contains data with a length greater than: {1}", column,
                                    length) + " : " +
                                ex.InnerException,
                            IsError = true
                        });
                    }
                }
            }
        }

        //private void LoadPatientProgramResponses(string ctr)
        //{
        //    try
        //    {
        //        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading program responses.", IsError = false });

        //        List<MEPatientProgramResponse> responses;
        //        using (ProgramMongoContext pmctx = new ProgramMongoContext(ctr))
        //        {
        //            responses = Utils.GetMongoCollectionList(pmctx.PatientProgramResponses.Collection, 50000);
        //        }

        //        // get stepidlist
        //        var stepIdList = Utils.GetStepIdList(_contract);
        //        var rSeries = new ReadPlanElementsSeries(_contract).ReadEStepResponseSeries(responses);

        //        using (var bcc = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default))
        //        using (var objRdr = ObjectReader.Create(rSeries))
        //        {
        //            try
        //            {
        //                bcc.BulkCopyTimeout = 580;
        //                bcc.ColumnMappings.Add("MongoStepId", "MongoStepId");
        //                bcc.ColumnMappings.Add("StepId", "StepId");
        //                bcc.ColumnMappings.Add("MongoNextStepId", "MongoNextStepId");
        //                bcc.ColumnMappings.Add("NextStepId", "NextStepId");
        //                bcc.ColumnMappings.Add("MongoId", "MongoId");
        //                bcc.ColumnMappings.Add("MongoActionId", "MongoActionId");
        //                bcc.ColumnMappings.Add("Order", "Order");
        //                bcc.ColumnMappings.Add("Text", "Text");
        //                bcc.ColumnMappings.Add("Value", "Value");
        //                bcc.ColumnMappings.Add("Nominal", "Nominal");
        //                bcc.ColumnMappings.Add("Required", "Required");
        //                bcc.ColumnMappings.Add("Selected", "Selected");
        //                bcc.ColumnMappings.Add("Version", "Version");
        //                bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
        //                bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
        //                bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
        //                bcc.ColumnMappings.Add("Delete", "Delete");
        //                bcc.ColumnMappings.Add("MongoStepSourceId", "MongoStepSourceId");
        //                bcc.ColumnMappings.Add("StepSourceId", "StepSourceId");
        //                bcc.ColumnMappings.Add("ActionId", "ActionId");
        //                bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
        //                bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
        //                bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
        //                bcc.ColumnMappings.Add("TTLDate", "TTLDate");
        //                bcc.DestinationTableName = "RPT_PatientProgramResponse";
        //                bcc.WriteToServer(objRdr);
        //            }

        //            catch (Exception ex)
        //            {
        //                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
        //                {
        //                    string pattern = @"\d+";
        //                    Match match = Regex.Match(ex.Message.ToString(), pattern);
        //                    var index = Convert.ToInt32(match.Value) - 1;

        //                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings",
        //                        BindingFlags.NonPublic | BindingFlags.Instance);
        //                    var sortedColumns = fi.GetValue(bcc);
        //                    var items =
        //                        (Object[])
        //                            sortedColumns.GetType()
        //                                .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
        //                                .GetValue(sortedColumns);

        //                    FieldInfo itemdata = items[index].GetType()
        //                        .GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
        //                    var metadata = itemdata.GetValue(items[index]);

        //                    var column =
        //                        metadata.GetType()
        //                            .GetField("column",
        //                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        //                            .GetValue(metadata);
        //                    var length =
        //                        metadata.GetType()
        //                            .GetField("length",
        //                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        //                            .GetValue(metadata);

        //                    OnEtlEvent(new ETLEventArgs
        //                    {
        //                        Message =
        //                            "[" + _contract + "] LoadPatientProgramResponses():SqlBulkCopy process failure: " + ex.Message +
        //                            String.Format("Column: {0} contains data with a length greater than: {1}", column,
        //                                length) + " : " +
        //                            ex.InnerException,
        //                        IsError = true
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientProgramResponse()", ex));
        //    }
        //}

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
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                //throw ex; //SimpleLog.Log(new ArgumentException("RegisterSpawnElement()", ex));
            }
        }

        public void ProcessSpawnElements()
        {
            OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading SpawnElements.", IsError = false });
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

                    SQLDataService.Instance.ExecuteProcedure(_contract, true, "REPORT", "spPhy_RPT_SaveSpawnElement", parms);
                    //OnEtlEvent(new ETLEventArgs { Message = "Spawn Element : "+ entry.SqlId + " saved.", IsError = false });
                }
                catch (Exception ex)
                {
                    OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                }
            });
        }

        private void LoadPatientTasks(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient tasks.", IsError = false });

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
                        parms.Add(new Parameter("@Description", task.Description == null ? string.Empty : task.Description, SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        parms.Add(new Parameter("@StartDate", task.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(task.Status.ToString()) ? string.Empty : task.Status.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
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
                        parms.Add(new Parameter("@Details", (string.IsNullOrEmpty(task.Details) ? (object)DBNull.Value : task.Details), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        if (task.ExtraElements != null)
                            parms.Add(new Parameter("@ExtraElements", task.ExtraElements.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        else
                            parms.Add(new Parameter("@ExtraElements", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));

                        // ENG-763, 848, 849 new fields
                        parms.Add(new Parameter("@ClosedDate", task.ClosedDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@TemplateId", (string.IsNullOrEmpty(task.TemplateId.ToString()) ? string.Empty : task.TemplateId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientTask", parms);
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

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientTaskBarrier", parms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientTasks()", ex));
            }
        }

        private void LoadAllergies(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading allergies.", IsError = false });

                ConcurrentBag<MEAllergy> pts;
                using (AllergyMongoContext pctx = new AllergyMongoContext(ctr))
                {
                    pts = new ConcurrentBag<MEAllergy>(pctx.Allergies.Collection.FindAllAs<MEAllergy>().ToList());
                }

                //Parallel.ForEach(pts, pa =>
                foreach (MEAllergy a in pts.Where(t => !t.DeleteFlag))
                {
                    try
                    {
                        ParameterCollection parms = new ParameterCollection();

                        parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(a.Id.ToString()) ? string.Empty : a.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(a.Name) ? string.Empty : a.Name.ToUpper()), SqlDbType.VarChar, ParameterDirection.Input, 100));
                        parms.Add(new Parameter("@CodingSystem", (string.IsNullOrEmpty(a.CodingSystem.ToString()) ? string.Empty : a.CodingSystem.ToString()), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        parms.Add(new Parameter("@CodingSystemCode", (string.IsNullOrEmpty(a.CodingSystemCode) ? string.Empty : a.CodingSystemCode), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        parms.Add(new Parameter("@Version", a.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                        parms.Add(new Parameter("@MongoUpdatedBy", (string.IsNullOrEmpty(a.UpdatedBy.ToString()) ? string.Empty : a.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastUpdatedOn", a.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(a.RecordCreatedBy.ToString()) ? string.Empty : a.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedOn", a.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@TTLDate", a.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(a.DeleteFlag.ToString()) ? string.Empty : a.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SaveAllergy", parms);

                        if (a.TypeIds != null)
                        {
                            foreach (ObjectId rec in a.TypeIds)
                            {
                                parms.Clear();

                                parms.Add(new Parameter("@MongoAllergyId", (string.IsNullOrEmpty(a.Id.ToString()) ? string.Empty : a.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoTypeId", (string.IsNullOrEmpty(rec.ToString()) ? string.Empty : rec.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoUpdatedBy", a.UpdatedBy == null ? string.Empty : a.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", a.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(a.RecordCreatedBy.ToString()) ? string.Empty : a.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", a.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", a.Version, SqlDbType.Float, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SaveAllergyType", parms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }//);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientUsers()", ex));
            }
        }

        private void LoadPatientAllergies(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient allergies.", IsError = false });

                ConcurrentBag<MEPatientAllergy> pts;
                using (AllergyMongoContext pctx = new AllergyMongoContext(ctr))
                {
                    pts = new ConcurrentBag<MEPatientAllergy>(pctx.PatientAllergies.Collection.FindAllAs<MEPatientAllergy>().ToList());
                }

                //Parallel.ForEach(pts, pa =>
                foreach (MEPatientAllergy pa in pts.Where(t => !t.DeleteFlag))
                {
                    try
                    {
                        ParameterCollection parms = new ParameterCollection();

                        parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(pa.Id.ToString()) ? string.Empty : pa.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoAllergyId", (string.IsNullOrEmpty(pa.AllergyId.ToString()) ? string.Empty : pa.AllergyId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoPatientId", (string.IsNullOrEmpty(pa.PatientId.ToString()) ? string.Empty : pa.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoSeverityId", (string.IsNullOrEmpty(pa.SeverityId.ToString()) ? string.Empty : pa.SeverityId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@StatusId", (string.IsNullOrEmpty(pa.StatusId.ToString()) ? string.Empty : pa.StatusId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 100));
                        parms.Add(new Parameter("@SourceId", (string.IsNullOrEmpty(pa.SourceId.ToString()) ? string.Empty : pa.SourceId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@StartDate", pa.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@EndDate", pa.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Notes", pa.Notes ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        parms.Add(new Parameter("@SystemName", (string.IsNullOrEmpty(pa.SystemName) ? string.Empty : pa.SystemName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoUpdatedBy", (string.IsNullOrEmpty(pa.UpdatedBy.ToString()) ? string.Empty : pa.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastUpdatedOn", pa.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(pa.RecordCreatedBy.ToString()) ? string.Empty : pa.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedOn", pa.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Version", pa.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                        parms.Add(new Parameter("@TTLDate", pa.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(pa.DeleteFlag.ToString()) ? string.Empty : pa.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientAllergy", parms);

                        if (pa.ReactionIds != null)
                        {
                            foreach (ObjectId rec in pa.ReactionIds)
                            {
                                parms.Clear();
                                parms.Add(new Parameter("@MongoPatientAllergyId", (string.IsNullOrEmpty(pa.Id.ToString()) ? string.Empty : pa.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoReactionId", (string.IsNullOrEmpty(rec.ToString()) ? string.Empty : rec.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoUpdatedBy", pa.UpdatedBy == null ? string.Empty : pa.UpdatedBy.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@LastUpdatedOn", pa.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(pa.RecordCreatedBy.ToString()) ? string.Empty : pa.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@RecordCreatedOn", pa.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@Version", pa.Version, SqlDbType.Float, ParameterDirection.Input, 50));

                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientAllergyReaction", parms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }//);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientUsers()", ex));
            }
        }

        private void LoadPatientMedSups(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient MedSupp.", IsError = false });

                ConcurrentBag<MEPatientMedSupp> pts;
                using (MedicationMongoContext pctx = new MedicationMongoContext(ctr))
                {
                    pts = new ConcurrentBag<MEPatientMedSupp>(pctx.PatientMedSupps.Collection.FindAllAs<MEPatientMedSupp>().ToList());
                }

                //Parallel.ForEach(pts, pa =>
                foreach (MEPatientMedSupp pm in pts.Where(t => !t.DeleteFlag))
                {
                    try
                    {
                        ParameterCollection parms = new ParameterCollection();

                        parms.Add(new Parameter("@MongoId", (string.IsNullOrEmpty(pm.Id.ToString()) ? string.Empty : pm.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoFrequencyId", pm.FrequencyId == null ? (object)DBNull.Value : pm.FrequencyId.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoFamilyId", string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoPatientId", (string.IsNullOrEmpty(pm.PatientId.ToString()) ? string.Empty : pm.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Name", (string.IsNullOrEmpty(pm.Name) ? string.Empty : pm.Name), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@Category", (string.IsNullOrEmpty(pm.CategoryId.ToString()) ? string.Empty : pm.CategoryId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@MongoTypeId", (string.IsNullOrEmpty(pm.TypeId.ToString()) ? string.Empty : pm.TypeId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Status", (string.IsNullOrEmpty(pm.StatusId.ToString()) ? string.Empty : pm.StatusId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@Strength", (string.IsNullOrEmpty(pm.Strength) ? string.Empty : pm.Strength), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@Route", (string.IsNullOrEmpty(pm.Route) ? string.Empty : pm.Route), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@Form", (string.IsNullOrEmpty(pm.Form) ? string.Empty : pm.Form), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@FreqQuantity", (string.IsNullOrEmpty(pm.FreqQuantity) ? string.Empty : pm.FreqQuantity), SqlDbType.VarChar, ParameterDirection.Input, 200));
                        parms.Add(new Parameter("@MongoSourceId", (string.IsNullOrEmpty(pm.SourceId.ToString()) ? string.Empty : pm.SourceId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@StartDate", pm.StartDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@EndDate", pm.EndDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Reason", pm.Reason ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        parms.Add(new Parameter("@Notes", pm.Notes ?? string.Empty, SqlDbType.VarChar, ParameterDirection.Input, 5000));
                        parms.Add(new Parameter("@PrescribedBy", (string.IsNullOrEmpty(pm.PrescribedBy) ? string.Empty : pm.PrescribedBy), SqlDbType.VarChar, ParameterDirection.Input, 500));
                        parms.Add(new Parameter("@SystemName", (string.IsNullOrEmpty(pm.SystemName) ? string.Empty : pm.SystemName), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoUpdatedBy", (string.IsNullOrEmpty(pm.UpdatedBy.ToString()) ? string.Empty : pm.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@LastUpdatedOn", pm.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@MongoRecordCreatedBy", (string.IsNullOrEmpty(pm.RecordCreatedBy.ToString()) ? string.Empty : pm.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@RecordCreatedOn", pm.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Version", pm.Version, SqlDbType.Float, ParameterDirection.Input, 32));
                        parms.Add(new Parameter("@TTLDate", pm.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(pm.DeleteFlag.ToString()) ? string.Empty : pm.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientMedSupp", parms);

                        if (pm.NDCs != null)
                        {
                            foreach (var rec in pm.NDCs)
                            {
                                parms.Clear();
                                parms.Add(new Parameter("@MongoPatientMedSuppId", (string.IsNullOrEmpty(pm.Id.ToString()) ? string.Empty : pm.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@NDC", (string.IsNullOrEmpty(rec) ? string.Empty : rec), SqlDbType.VarChar, ParameterDirection.Input, 200));


                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_PatientMedSuppNDC", parms);
                            }
                        }

                        if (pm.PharmClasses != null)
                        {
                            foreach (var rec in pm.PharmClasses)
                            {
                                parms.Clear();
                                parms.Add(new Parameter("@MongoPatientMedSuppId", (string.IsNullOrEmpty(pm.Id.ToString()) ? string.Empty : pm.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                                parms.Add(new Parameter("@PharmClass", (string.IsNullOrEmpty(rec) ? string.Empty : rec), SqlDbType.VarChar, ParameterDirection.Input, 2000));


                                SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_PatientMedSuppPhClass", parms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }//);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientUsers()", ex));
            }
        }

        private void LoadPatientUsers(string ctr)
        {
            try
            {
                OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] Loading patient users.", IsError = false });

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

                        SQLDataService.Instance.ExecuteScalar(_contract, true, "REPORT", "spPhy_RPT_SavePatientUser", parms);
                    }
                    catch (Exception ex)
                    {
                        OnEtlEvent(new ETLEventArgs { Message = "[" + _contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientUsers()", ex));
            }
        }

    }
}

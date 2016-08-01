using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FastMember;
using MongoDB.Bson;
using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL.Templates
{
    public class PatientInfo : DocumentCollection
    {
        private void SaveSubcollection(IEnumerable<EPatient> dic)
        {
            OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Saving patient data.", IsError = false });
            var rSeries = dic;

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    try
                    {
                        bcc.BulkCopyTimeout = 580;
                        bcc.ColumnMappings.Add("MongoPatientSystemId", "MongoPatientSystemId");
                        bcc.ColumnMappings.Add("MongoId", "MongoId");
                        bcc.ColumnMappings.Add("FirstName", "FirstName");
                        bcc.ColumnMappings.Add("MiddleName", "MiddleName");
                        bcc.ColumnMappings.Add("LastName", "LastName");
                        bcc.ColumnMappings.Add("PreferredName", "PreferredName");
                        bcc.ColumnMappings.Add("Suffix", "Suffix");
                        bcc.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
                        bcc.ColumnMappings.Add("Gender", "Gender");
                        bcc.ColumnMappings.Add("Priority", "Priority");
                        bcc.ColumnMappings.Add("Background", "Background");
                        bcc.ColumnMappings.Add("Version", "Version");
                        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                        bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                        bcc.ColumnMappings.Add("Delete", "Delete");
                        bcc.ColumnMappings.Add("FSSN", "FSSN");
                        bcc.ColumnMappings.Add("LSSN", "LSSN");

                        bcc.ColumnMappings.Add("DataSource", "DataSource");
                        bcc.ColumnMappings.Add("MongoMaritalStatusId", "MongoMaritalStatusId");
                        bcc.ColumnMappings.Add("Protected", "Protected");
                        bcc.ColumnMappings.Add("Deceased", "Deceased");
                        bcc.ColumnMappings.Add("Status", "Status");
                        bcc.ColumnMappings.Add("MongoReasonId", "MongoReasonId");
                        bcc.ColumnMappings.Add("StatusDataSource", "StatusDataSource");
                        bcc.ColumnMappings.Add("ClinicalBackGround", "ClinicalBackGround");
                        bcc.ColumnMappings.Add("ExtraElements", "ExtraElements");

                        bcc.DestinationTableName = "RPT_Patient";
                        bcc.NotifyAfter = 5000;
                        bcc.SqlRowsCopied += bcc_SqlRowsCopied;
                    }
                    catch (Exception ex)
                    {
                        FormatError(ex, bcc);
                        throw new ArgumentException("SaveSubcollection() : column mappings failed: " + ex.Message);
                    }
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                    throw new ArgumentException("SaveSubcollection() : Save Patient Collection failed: " + ex.Message);
                }
            }
        }

        void bcc_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading " + e.RowsCopied, IsError = false });
        } 

        private void SaveSystemSubcollection(IEnumerable<EPatientSystem> dic)
        {
            OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Saving patient system data.", IsError = false });
            var rSeries = dic;

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoPatientId", "MongoPatientId");
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("Label", "Label");
                    //bcc.ColumnMappings.Add("SystemId", "SystemId");
                    bcc.ColumnMappings.Add("SystemName", "SystemName");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("Delete", "Delete");
                    bcc.ColumnMappings.Add("ExtraElements", "ExtraElements");
                    bcc.ColumnMappings.Add("Value", "Value");
                    bcc.ColumnMappings.Add("DataSource", "DataSource");
                    bcc.ColumnMappings.Add("Status", "Status");
                    bcc.ColumnMappings.Add("Primary", "Primary");
                    bcc.ColumnMappings.Add("SysId", "SysId");

                    bcc.DestinationTableName = "RPT_PatientSystem";
                    bcc.NotifyAfter = 5000;
                    bcc.SqlRowsCopied += bcc_SqlRowsCopied;
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                    throw new ArgumentException("SaveSystemSubcollection() : Save Patient system Collection failed: " + ex.Message);
                }
            }
        }

        public override void Execute()
        {
            try
            {
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading Patients.", IsError = false});

                ConcurrentBag<MEPatient> patients;
                using (PatientMongoContext pmctx = new PatientMongoContext(Contract))
                {
                    patients = new ConcurrentBag<MEPatient>(Utils.GetMongoCollectionList(pmctx.Patients.Collection, 50000));
                }
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Patient mongo collection loaded.", IsError = false });

                ConcurrentBag<MEPatientSystem> patientSystems = null;
                using (PatientSystemMongoContext psctx = new PatientSystemMongoContext(Contract))
                {
                    patientSystems = new ConcurrentBag<MEPatientSystem>(Utils.GetMongoCollectionList(psctx.PatientSystems.Collection, 50000));
                }
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Patient system mongo collection loaded.", IsError = false });

                Dictionary<ObjectId, string> dic = new Dictionary<ObjectId, string>();
                foreach (var s in patientSystems.Where(s => !dic.ContainsKey(s.PatientId)))
                {
                    dic.Add(s.PatientId, s.Id.ToString());
                }

                #region //set patients
                //ConcurrentBag<EPatient> patientMap = new ConcurrentBag<EPatient>();
                List<EPatient> patientMap = new List<EPatient>();
                var pList = patients.ToList();

                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Creating Patient bulk load set.", IsError = false });
                //Parallel.ForEach(pList, pt =>
                foreach (MEPatient pt in pList)
                {
                    try
                    {
                        string mongoPatientSystemId = dic.ContainsKey(pt.Id) ? dic[pt.Id] : null;

                        patientMap.Add(new EPatient
                        {
                            MongoId = pt.Id.ToString(),
                            FirstName = pt.FirstName,
                            MiddleName = pt.MiddleName,
                            LastName = pt.LastName,
                            PreferredName = pt.PreferredName,
                            Suffix = pt.Suffix,
                            DateOfBirth = String.IsNullOrEmpty(pt.DOB) ? null : pt.DOB,
                            Gender = pt.Gender,
                            Priority = pt.Priority.ToString(),
                            Version = Convert.ToInt32(pt.Version),
                            MongoUpdatedBy = pt.UpdatedBy == null ? string.Empty : pt.UpdatedBy.Value.ToString(),
                            LastUpdatedOn = pt.LastUpdatedOn ?? null,
                            MongoRecordCreatedBy = pt.RecordCreatedBy.ToString(),
                            RecordCreatedOn = ParseDate(pt.RecordCreatedOn),
                            Delete = pt.DeleteFlag.ToString(),
                            Background = pt.Background,
                            TTLDate = pt.TTLDate ?? null,
                            LSSN = pt.LastFourSSN, //== null ? null : (int?)Convert.ToInt32(pt.LastFourSSN),
                            FSSN = pt.FullSSN,
                            ClinicalBackGround = pt.ClinicalBackground,
                            DataSource = pt.DataSource,
                            MongoMaritalStatusId =
                            pt.MaritalStatusId == null ? string.Empty : pt.MaritalStatusId.Value.ToString(),
                            Protected = pt.Protected.ToString(),
                            Deceased = pt.Deceased.ToString(),
                            Status = pt.Status.ToString(),
                            MongoReasonId = pt.ReasonId == null ? string.Empty : pt.ReasonId.Value.ToString(),
                            StatusDataSource = pt.StatusDataSource,
                            MongoPatientSystemId = mongoPatientSystemId,
                            ExtraElements = null
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs
                        {
                            Message =
                                "[" + Contract + "] Error in Loading Patient information" + ex.Message + ": " +
                                ex.StackTrace,
                            IsError = true
                        });
                    }
                }//);
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Finished creating Patient bulk load set.", IsError = false });

                SaveSubcollection(patientMap);

                #endregion

                //set patientsystems
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Creating patient system bulk load set.", IsError = false });
                //ConcurrentBag<EPatientSystem> patientSystemMap = new ConcurrentBag<EPatientSystem>();
                List<EPatientSystem> patientSystemMap = new List<EPatientSystem>();
                List<MEPatientSystem> psList = patientSystems.ToList();
                foreach (MEPatientSystem ps in patientSystems)
                {
                    try
                    {
                        patientSystemMap.Add(new EPatientSystem
                        {
                            MongoPatientId = ps.PatientId.ToString(),
                            MongoId = ps.Id.ToString(),
                            Label = ps.DisplayLabel,
                            //SystemId = pt.SystemId.ToString(),
                            SystemName = ps.SystemName,
                            MongoUpdatedBy = ps.UpdatedBy == null ? string.Empty : ps.UpdatedBy.Value.ToString(),
                            LastUpdatedOn = ps.LastUpdatedOn ?? null,
                            MongoRecordCreatedBy = ps.RecordCreatedBy.ToString(),
                            RecordCreatedOn = ParseDate(ps.RecordCreatedOn).Value,
                            Version = Convert.ToInt32(ps.Version),
                            TTLDate = ps.TTLDate,
                            Delete = ps.DeleteFlag.ToString(),
                            Value = ps.Value,
                            DataSource = ps.DataSource,
                            Status = ps.Status.ToString(),
                            Primary = ps.Primary.ToString(),
                            SysId = ps.SystemId.ToString()
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs
                        {
                            Message =
                                "[" + Contract + "] Error in Loading Patient information" + ex.Message + ": " +
                                ex.StackTrace,
                            IsError = true
                        });
                    }
                }//);
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Finished creating Patient system bulk load set.", IsError = false });
                SaveSystemSubcollection(patientSystemMap);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime? ParseDate(DateTime dateTime)
        {
            var date = (DateTime)SqlDateTime.MinValue;
            return dateTime.Year.Equals(0001) ? date : dateTime;
        }


    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using FastMember;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.Data.ETL.BulkCopy;

namespace Phytel.Data.ETL.Templates
{
    public class PatientMedicationFrequency : DocumentCollection
    {
        public MELookup LookUp { get; set; }

        private void SaveSubcollection(ConcurrentBag<EMedicationFrequency> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("LookUpType", "LookUpType");
                    bcc.ColumnMappings.Add("Name", "Name");

                    bcc.DestinationTableName = "RPT_PatientMedFrequency";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SaveCustomSubcollection(ConcurrentBag<EPatientMedicationFrequency> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("Name", "Name");
                    bcc.ColumnMappings.Add("MongoPatientId","MongoPatientId");
                    bcc.ColumnMappings.Add("Version","Version");
                    bcc.ColumnMappings.Add("MongoUpdatedBy","MongoUpdatedBy");
                    bcc.ColumnMappings.Add("DeleteFlag","DeleteFlag");
                    bcc.ColumnMappings.Add("TTLDate","TTLDate");
                    bcc.ColumnMappings.Add("LastUpdatedOn","LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy","MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");

                    bcc.DestinationTableName = "RPT_CustomPatientMedFrequency";
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
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading Patient Medication Frequency.", IsError = false});
                ConcurrentBag<EMedicationFrequency> medFreq = new ConcurrentBag<EMedicationFrequency>();
                ConcurrentBag<EPatientMedicationFrequency> custMedFreq = new ConcurrentBag<EPatientMedicationFrequency>();
                var mType = "Frequency";

                Parallel.ForEach(LookUp.Data, l =>
                //foreach (LookUpBase l in LookUp.Data)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        var prb = (Frequency)l;
                        medFreq.Add(new EMedicationFrequency
                        {
                            LookUpType = mType,
                            MongoId = prb.DataId.ToString(),
                            Name = prb.Name,
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs{ Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                ConcurrentBag<MEPatientMedFrequency> meds;
                using (MedicationMongoContext smct = new MedicationMongoContext(Contract))
                {
                    meds = new ConcurrentBag<MEPatientMedFrequency>(smct.PatientMedFrequencies.Collection.FindAllAs<MEPatientMedFrequency>().ToList());
                }

                Parallel.ForEach(meds, med =>
                //foreach (MEPatientMedFrequency med in meds)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        custMedFreq.Add(new EPatientMedicationFrequency
                        {
                            Name = med.Name,
                            MongoPatientId = med.PatientId == null ? string.Empty : med.PatientId.ToString(),
                            Version = med.Version,
                            MongoUpdatedBy = med.UpdatedBy == null ? string.Empty : med.UpdatedBy.ToString(),
                            DeleteFlag = med.DeleteFlag,
                            MongoRecordCreatedBy = med.RecordCreatedBy == null ? string.Empty : med.RecordCreatedBy.ToString(),
                            RecordCreatedOn = med.RecordCreatedOn,
                            TTLDate = med.TTLDate,
                            MongoId = med.Id == null ? string.Empty : med.Id.ToString(),
                            LookUpType = mType
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(medFreq);
                SaveCustomSubcollection(custMedFreq);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using FastMember;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.Data.ETL.BulkCopy;

namespace Phytel.Data.ETL.Templates
{
    public class MedicationMap : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<EMedicationMap> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("FullName", "FullName");
                    bcc.ColumnMappings.Add("SubstanceName", "SubstanceName");
                    bcc.ColumnMappings.Add("Route", "Route");
                    bcc.ColumnMappings.Add("Form", "Form");
                    bcc.ColumnMappings.Add("Strength", "Strength");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("DeleteFlag", "DeleteFlag");
                    bcc.ColumnMappings.Add("Custom", "Custom");
                    bcc.ColumnMappings.Add("Verified", "Verified");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");


                    bcc.DestinationTableName = "RPT_MedicationMap";
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
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading MedicationMaps.", IsError = false});
                List<EMedicationMap> pharmClassMap = new List<EMedicationMap>();
                ConcurrentBag<EMedicationMap> eMeds = new ConcurrentBag<EMedicationMap>();

                ConcurrentBag<MEMedicationMapping> meds;
                using (MedicationMongoContext smct = new MedicationMongoContext(Contract))
                {
                    meds = new ConcurrentBag<MEMedicationMapping>(smct.MedicationMaps.Collection.FindAllAs<MEMedicationMapping>().ToList());
                }

                Parallel.ForEach(meds, med =>
                //foreach (MEMedicationMapping med in meds)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        eMeds.Add(new EMedicationMap
                        {
                            MongoId = med.Id.ToString(),
                            Form = med.Form,
                            FullName = med.FullName,
                            Route = med.Route,
                            Strength = med.Strength,
                            SubstanceName = med.SubstanceName,
                            Version = med.Version,
                            MongoUpdatedBy = med.UpdatedBy == null ? string.Empty : med.UpdatedBy.ToString(),
                            MongoRecordCreatedBy = med.RecordCreatedBy.ToString(),
                            RecordCreatedOn = med.RecordCreatedOn,
                            DeleteFlag = med.DeleteFlag,
                            Custom = med.Custom,
                            Verified = med.Verified,
                            TTLDate = med.TTLDate,
                            LastUpdatedOn = med.LastUpdatedOn
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs{ Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(eMeds);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using FastMember;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Data.ETL.BulkCopy;

namespace Phytel.Data.ETL.Templates
{
    public class System : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<ESystem> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("DisplayLabel", "DisplayLabel");
                    bcc.ColumnMappings.Add("Field", "Field");
                    bcc.ColumnMappings.Add("Name", "Name");
                    bcc.ColumnMappings.Add("Primary", "Primary");
                    bcc.ColumnMappings.Add("Status", "Status");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("DeleteFlag", "DeleteFlag");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");

                    bcc.DestinationTableName = "RPT_System";
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
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading Systems.", IsError = false});
                ConcurrentBag<ESystem> sSys = new ConcurrentBag<ESystem>();

                ConcurrentBag<MESystem> syss;
                using (PatientSystemMongoContext smct = new PatientSystemMongoContext(Contract))
                {
                    syss = new ConcurrentBag<MESystem>(smct.Systems.Collection.FindAllAs<MESystem>().ToList());
                }

                //Parallel.ForEach(syss, sys =>
                foreach (MESystem sys in syss)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        sSys.Add(new ESystem
                        {
                            MongoId = sys.Id.ToString(),
                            DisplayLabel = sys.DisplayLabel,
                            Field = sys.Field,
                            Name = sys.Name,
                            Primary = sys.Primary.ToString(),
                            Status = sys.Status.ToString(),
                            Version = sys.Version,
                            UpdatedBy = sys.UpdatedBy == null ? string.Empty : sys.UpdatedBy.ToString(),
                            RecordCreatedBy = sys.RecordCreatedBy.ToString(),
                            RecordCreatedOn = sys.RecordCreatedOn,
                            DeleteFlag = sys.DeleteFlag.ToString(),
                            TTLDate = sys.TTLDate,
                            LastUpdatedOn = sys.LastUpdatedOn
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs{ Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                }//);

                SaveSubcollection(sSys);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

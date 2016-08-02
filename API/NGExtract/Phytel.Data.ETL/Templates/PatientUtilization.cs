using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Data.ETL.BulkCopy;
using Phytel.API.DataDomain.PatientNote.Repo;
using System.Collections.Generic;

namespace Phytel.Data.ETL.Templates
{
    public class PatientUtilization : DocumentCollection
    {
        private void SavePatientUtilizationCollection(ConcurrentBag<EPatientUtilization> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("Id", "MongoId");
                    bcc.ColumnMappings.Add("PatientId", "MongoPatientId");
                    bcc.ColumnMappings.Add("NoteTypeId", "MongoNoteTypeId");
                    bcc.ColumnMappings.Add("Reason", "Reason");
                    bcc.ColumnMappings.Add("VisitTypeId", "MongoVisitTypeId");
                    bcc.ColumnMappings.Add("OtherVisitType", "OtherVisitType");
                    bcc.ColumnMappings.Add("AdmitDate", "AdmitDate");
                    bcc.ColumnMappings.Add("Admitted", "Admitted");
                    bcc.ColumnMappings.Add("DischargeDate", "DischargeDate");
                    bcc.ColumnMappings.Add("LocationId", "MongoLocationId");
                    bcc.ColumnMappings.Add("OtherLocation", "OtherLocation");
                    bcc.ColumnMappings.Add("DispositionId", "MongoDispositionId");
                    bcc.ColumnMappings.Add("OtherDisposition", "OtherDisposition");
                    bcc.ColumnMappings.Add("UtilizationSourceId", "MongoUtilizationSourceId");
                    bcc.ColumnMappings.Add("DataSource", "DataSource");
                    bcc.ColumnMappings.Add("UpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("DeleteFlag", "Delete");

                    bcc.DestinationTableName = "RPT_PatientUtilization";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SavePatientUtilizationProgramCollection(List<EPatientUtilizationProgram> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("Id", "MongoId");
                    bcc.ColumnMappings.Add("PatientUtilizationId", "MongoPatientUtilizationId");
                    bcc.ColumnMappings.Add("UpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("Version", "Version");

                    bcc.DestinationTableName = "RPT_PatientUtilizationProgram";
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
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading Patient Utilization.", IsError = false});
                ConcurrentBag<EPatientUtilization> ePU = new ConcurrentBag<EPatientUtilization>();
                List<EPatientUtilizationProgram> associatedPrograms = new List<EPatientUtilizationProgram>();
                ConcurrentBag<MEPatientUtilization> mePU;
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(Contract))
                {
                    mePU = new ConcurrentBag<MEPatientUtilization>(ctx.PatientUtilizations.Collection.FindAllAs<MEPatientUtilization>().ToList());
                }

                foreach (MEPatientUtilization u in mePU)
                {
                    try
                    {
                        ePU.Add(new EPatientUtilization
                        {
                            Id = u.Id.ToString(),
                            PatientId = u.PatientId.ToString(),
                            NoteTypeId = u.NoteType == null ? null : u.NoteType.ToString(),
                            Reason = u.Reason,
                            VisitTypeId = u.VisitType == null ? null : u.VisitType.ToString(),
                            OtherVisitType = u.OtherType,
                            AdmitDate = u.AdmitDate,
                            Admitted = u.Admitted.ToString(),
                            DischargeDate = u.DischargeDate,
                            LocationId = u.Location == null ? null : u.Location.ToString(),
                            OtherLocation = u.OtherLocation,
                            DispositionId = u.Disposition == null ? null : u.Disposition.ToString(),
                            OtherDisposition = u.OtherDisposition,
                            UtilizationSourceId = u.SourceId == null ? null : u.SourceId.ToString(),
                            DataSource = u.DataSource,
                            Version = u.Version,
                            UpdatedBy = u.UpdatedBy == null ? string.Empty : u.UpdatedBy.ToString(),
                            RecordCreatedBy = u.RecordCreatedBy.ToString(),
                            RecordCreatedOn = u.RecordCreatedOn,
                            DeleteFlag = u.DeleteFlag.ToString(),
                            LastUpdatedOn = u.LastUpdatedOn
                        });

                        if (u.ProgramIds != null)
                        {
                            foreach (var progId in u.ProgramIds)
                            {
                                associatedPrograms.Add(new EPatientUtilizationProgram
                                {
                                    Id = progId.ToString(),
                                    LastUpdatedOn = u.LastUpdatedOn,
                                    UpdatedBy = u.UpdatedBy == null ? string.Empty : u.UpdatedBy.ToString(),
                                    RecordCreatedBy = u.RecordCreatedBy.ToString(),
                                    PatientUtilizationId = u.Id.ToString(),
                                    RecordCreatedOn = u.RecordCreatedOn,
                                    Version = u.Version
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs{ Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }
                SavePatientUtilizationCollection(ePU);
                SavePatientUtilizationProgramCollection(associatedPrograms);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}

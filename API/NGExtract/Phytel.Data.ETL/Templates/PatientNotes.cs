using Phytel.API.DataDomain.PatientNote.DTO;
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
using Phytel.API.DataDomain.PatientNote;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.Data.ETL.Templates
{
    public class PatientNotes : DocumentCollection
    {
        private void SaveSubcollection(List<EPatientNoteProgram> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoId", "MongoId");
                    bcc.ColumnMappings.Add("PatientNoteMongoId", "MongoPatientNoteId");
                    bcc.ColumnMappings.Add("UpdatedBy", "MongoUpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("Version", "Version");

                    bcc.DestinationTableName = "RPT_PatientNoteProgram";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                    {
                        var pattern = @"\d+";
                        Match match = Regex.Match(ex.Message.ToString(), pattern);
                        var index = Convert.ToInt32(match.Value) - 1;

                        FieldInfo fi = typeof (SqlBulkCopy).GetField("_sortedColumnMappings",
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        var sortedColumns = fi.GetValue(bcc);
                        var items =(Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                        FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                        var metadata = itemdata.GetValue(items[index]);

                        var column =metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                        var length =metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);

                        OnDocColEvent(new ETLEventArgs
                        {
                            Message = "[" + Contract + "] PatientNotes():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " + ex.InnerException,
                            IsError = true
                        });
                    }
                }
            }
        }

        public override void Execute()
        {
            try
            {
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading Patient Notes.", IsError = false });
                List<EPatientNoteProgram> noteMap = new List<EPatientNoteProgram>();
                ConcurrentBag<MEPatientNote> notes;
                using (PatientNoteMongoContext pnctx = new PatientNoteMongoContext(Contract))
                {
                    notes = new ConcurrentBag<MEPatientNote>(pnctx.PatientNotes.Collection.FindAllAs<MEPatientNote>().ToList());
                }

                //Parallel.ForEach(notes, note =>
                foreach (MEPatientNote note in notes)//.Where(t => !t.DeleteFlag))
                {
                    try
                    {
                        ParameterCollection parms = new ParameterCollection();
                        parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@PatientMongoId", (string.IsNullOrEmpty(note.PatientId.ToString()) ? string.Empty : note.PatientId.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        parms.Add(new Parameter("@Text", (string.IsNullOrEmpty(note.Text) ? string.Empty : note.Text), SqlDbType.VarChar, ParameterDirection.Input, int.MaxValue));
                        parms.Add(new Parameter("@Type", note.Type.ToString() ?? (object)DBNull.Value, SqlDbType.VarChar, ParameterDirection.Input, 50));
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

                        SQLDataService.Instance.ExecuteScalar(Contract, true, "REPORT", "spPhy_RPT_SavePatientNote", parms);

                        if (note.ProgramIds != null)
                        {
                            foreach (var prg in note.ProgramIds)
                            {
                                noteMap.Add(new EPatientNoteProgram
                                {
                                    MongoId = prg.ToString(),
                                    LastUpdatedOn = note.LastUpdatedOn,
                                    UpdatedBy = note.UpdatedBy.ToString(),
                                    RecordCreatedBy = note.RecordCreatedBy.ToString(),
                                    PatientNoteMongoId = note.Id.ToString(),
                                    RecordCreatedOn = note.RecordCreatedOn,
                                    Version = Convert.ToInt32(note.Version)
                                });
                            }
                        }

                        #region
                        ////if (note.ProgramIds != null)
                        ////{
                        ////    foreach (ObjectId prg in note.ProgramIds)
                        ////    {
                        ////        parms.Clear();
                        ////        parms.Add(new Parameter("@MongoID", (string.IsNullOrEmpty(prg.ToString()) ? string.Empty : prg.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@PatientNoteMongoId", (string.IsNullOrEmpty(note.Id.ToString()) ? string.Empty : note.Id.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(note.UpdatedBy.ToString()) ? string.Empty : note.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@LastUpdatedOn", note.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(note.RecordCreatedBy.ToString()) ? string.Empty : note.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@RecordCreatedOn", note.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
                        ////        parms.Add(new Parameter("@Version", (string.IsNullOrEmpty(note.Version.ToString()) ? string.Empty : note.Version.ToString()), SqlDbType.Float, ParameterDirection.Input, 32));

                        ////        SQLDataService.Instance.ExecuteScalar(Contract, true, "REPORT", "spPhy_RPT_SavePatientNoteProgram", parms);
                        ////    }
                        ////}
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                }//);

                SaveSubcollection(noteMap);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientNotes()", ex));
            }
        }
    }
}

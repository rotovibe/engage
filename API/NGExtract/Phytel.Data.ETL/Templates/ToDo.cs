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
using Phytel.API.DataDomain.Scheduling;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL.Templates
{
    public class ToDo : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<EToDoProgram> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoToDoId", "MongoToDoId");
                    bcc.ColumnMappings.Add("MongoProgramId", "MongoProgramId");

                    bcc.DestinationTableName = "RPT_ToDoProgram";
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
                            Message = "[" + Contract + "] ToDo():SqlBulkCopy process failure: " + ex.Message + String.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " + ex.InnerException,
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
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading todos.", IsError = false });
                ConcurrentBag<EToDoProgram> todoMap = new ConcurrentBag<EToDoProgram>();
                ConcurrentBag<METoDo> todo;
                using (SchedulingMongoContext smct = new SchedulingMongoContext(Contract))
                {
                    todo = new ConcurrentBag<METoDo>(smct.ToDos.Collection.FindAllAs<METoDo>().ToList());
                }

                Parallel.ForEach(todo, td =>
                //foreach (METoDo td in todo)//.Where(t => !t.DeleteFlag ))
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
                                new Parameter("@StartTime", td.StartTime == null ? (object) DBNull.Value : td.StartTime.Value.TimeOfDay, SqlDbType.Time,ParameterDirection.Input, 50),
                                new Parameter("@Duration", td.Duration ?? (object) DBNull.Value, SqlDbType.Int,ParameterDirection.Input, 50),
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

                        SQLDataService.Instance.ExecuteScalar(Contract, true, "REPORT", "spPhy_RPT_SaveToDo", parms);

                        if (td.ProgramIds != null)
                        {
                            foreach (var prg in td.ProgramIds)
                            {
                                todoMap.Add(new EToDoProgram
                                {
                                    MongoToDoId = td.Id.ToString(),
                                    MongoProgramId = prg.ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }
                });

                SaveSubcollection(todoMap);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

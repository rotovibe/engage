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
using Phytel.API.DataDomain.Contact;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.Scheduling;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL.Templates
{
    public class Medication : DocumentCollection
    {
        //private void SaveSubcollection(List<ERecentUserList> dic)
        //{
        //    //var rSeries = dic.AsEnumerable();

        //    //using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
        //    //using (var objRdr = ObjectReader.Create(rSeries))
        //    //{
        //    //    try
        //    //    {
        //    //        bcc.BulkCopyTimeout = 580;
        //    //        bcc.ColumnMappings.Add("UserId", "UserId");
        //    //        bcc.ColumnMappings.Add("MongoUserId", "MongoUserId");
        //    //        bcc.ColumnMappings.Add("MongoId", "MongoId");
        //    //        bcc.ColumnMappings.Add("Version", "Version");
        //    //        bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");
        //    //        bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
        //    //        bcc.ColumnMappings.Add("MongoRecordCreatedBy", "MongoRecordCreatedBy");
        //    //        bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");

        //    //        bcc.DestinationTableName = "RPT_UserRecentList";
        //    //        bcc.WriteToServer(objRdr);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        FormatError(ex, bcc);
        //    //    }
        //    //}
        //}

        public override void Execute()
        {
            //string name = string.Empty;
            //try
            //{
            //    OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading users.", IsError = false });

            //    ConcurrentBag<ERecentUserList> recentList =  new ConcurrentBag<ERecentUserList>();
            //    ConcurrentBag<MEContact> contacts;
            //    using (ContactMongoContext cctx = new ContactMongoContext(Contract))
            //    {
            //        contacts = new ConcurrentBag<MEContact>(cctx.Contacts.Collection.FindAllAs<MEContact>().ToList());
            //    }

            //    Parallel.ForEach(contacts, contact =>
            //    //foreach (MEContact contact in contacts)//.Where(t => !t.DeleteFlag))
            //    {
            //        name = contact.LastName + ", " + contact.FirstName;
            //        if (contact.PatientId == null)
            //        {
            //            try
            //            {
            //                ParameterCollection parms = new ParameterCollection();
            //                parms.Add(new Parameter("@MongoID", contact.Id.ToString(), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@ResourceId", (string.IsNullOrEmpty(contact.ResourceId) ? string.Empty : contact.ResourceId), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@FirstName", (string.IsNullOrEmpty(contact.FirstName) ? string.Empty : contact.FirstName), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@LastName", (string.IsNullOrEmpty(contact.LastName) ? string.Empty : contact.LastName), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@PreferredName", (string.IsNullOrEmpty(contact.PreferredName) ? string.Empty : contact.PreferredName), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@Version", contact.Version, SqlDbType.Float, ParameterDirection.Input, 32));
            //                parms.Add(new Parameter("@UpdatedBy", (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@LastUpdatedOn", contact.LastUpdatedOn ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@RecordCreatedBy", (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@RecordCreatedOn", contact.RecordCreatedOn, SqlDbType.DateTime, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@Delete", (string.IsNullOrEmpty(contact.DeleteFlag.ToString()) ? string.Empty : contact.DeleteFlag.ToString()), SqlDbType.VarChar, ParameterDirection.Input, 50));
            //                parms.Add(new Parameter("@TTLDate", contact.TTLDate ?? (object)DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input, 50));

            //                SQLDataService.Instance.ExecuteProcedure(Contract, true, "REPORT", "spPhy_RPT_SaveUser", parms);

            //                if (contact.RecentList != null)
            //                {
            //                    foreach (var rec in contact.RecentList)
            //                    {
            //                        recentList.Add(new ERecentUserList{
            //                            MongoId = rec.ToString(), 
            //                            MongoUserId = (string.IsNullOrEmpty(contact.Id.ToString()) ? string.Empty : contact.Id.ToString()),
            //                            Version = Convert.ToInt32(contact.Version),
            //                            MongoUpdatedBy = (string.IsNullOrEmpty(contact.UpdatedBy.ToString()) ? string.Empty : contact.UpdatedBy.ToString()), 
            //                            LastUpdatedOn = contact.LastUpdatedOn,
            //                            MongoRecordCreatedBy =  (string.IsNullOrEmpty(contact.RecordCreatedBy.ToString()) ? string.Empty : contact.RecordCreatedBy.ToString()),
            //                            RecordCreatedOn = contact.RecordCreatedOn
            //                        });
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] name: " + name + ": " + ex.Message + ": " + ex.StackTrace, IsError = true });
            //            }
            //        }
            //    });

            //    SaveSubcollection(recentList.ToList());
            //}
            //catch (Exception ex)
            //{
            //    OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] LoadUsers():: name: " + name + ": " + ex.Message + ": " + ex.StackTrace, IsError = true });
            //}
        }
    }
}

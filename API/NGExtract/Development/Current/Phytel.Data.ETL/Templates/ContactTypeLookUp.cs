using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FastMember;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.MongoDB;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.Data.ETL.BulkCopy;

namespace Phytel.Data.ETL.Templates
{
    public class ContactTypeLookUp : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<EContactTypeLookUp> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("Id", "MongoId");
                    bcc.ColumnMappings.Add("Name", "Name");
                    bcc.ColumnMappings.Add("Role", "Role");
                    bcc.ColumnMappings.Add("ParentId", "ParentId");
                    bcc.ColumnMappings.Add("Group", "Group");
                    bcc.ColumnMappings.Add("Active", "Active");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("DeleteFlag", "DeleteFlag");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.DestinationTableName = "RPT_ContactTypeLookUp";
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
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading ContactTypeLookUp.", IsError = false });
                ConcurrentBag<EContactTypeLookUp> eContactTypeLookUps = new ConcurrentBag<EContactTypeLookUp>();

                ConcurrentBag<MEContactTypeLookup> cTypes;
                using (var ctx = new ContactTypeLookUpMongoContext(Contract))
                {
                    cTypes = new ConcurrentBag<MEContactTypeLookup>(ctx.ContactTypeLookUps.Collection.FindAllAs<MEContactTypeLookup>().ToList());
                }

                Parallel.ForEach(cTypes, c =>
                {
                    try
                    {
                        eContactTypeLookUps.Add(new EContactTypeLookUp
                        {
                            MongoId = c.Id.ToString(),
                            Name = c.Name,
                            Role  = c.Role,
                            ParentId = c.ParentId == null ? null : c.ParentId.ToString(),
                            Group = c.GroupId.ToString(),
                            Active = c.Active,
                            Version = c.Version,
                            DeleteFlag =  c.DeleteFlag,
                            TTLDate = c.TTLDate,
                            UpdatedBy = c.UpdatedBy == null ? null : c.UpdatedBy.ToString(),
                            LastUpdatedOn = c.LastUpdatedOn,
                            RecordCreatedBy = c.RecordCreatedBy.ToString(),
                            RecordCreatedOn = c.RecordCreatedOn,
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(eContactTypeLookUps);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}

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
    public class CareTeam : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<ECareTeam> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MongoCareTeamId", "MongoCareTeamId");
                    bcc.ColumnMappings.Add("MongoContactIdForPatient", "MongoContactIdForPatient");
                    bcc.ColumnMappings.Add("MongoCareMemberId", "MongoCareMemberId");
                    bcc.ColumnMappings.Add("MongoContactIdForCareMember", "MongoContactIdForCareMember");
                    bcc.ColumnMappings.Add("RoleId", "RoleId");
                    bcc.ColumnMappings.Add("CustomRoleName", "CustomRoleName");
                    bcc.ColumnMappings.Add("StartDate", "StartDate");
                    bcc.ColumnMappings.Add("EndDate", "EndDate");
                    bcc.ColumnMappings.Add("Core", "Core");
                    bcc.ColumnMappings.Add("Notes", "Notes");
                    bcc.ColumnMappings.Add("FrequencyId", "FrequencyId");
                    bcc.ColumnMappings.Add("Distance", "Distance");
                    bcc.ColumnMappings.Add("DistanceUnit", "DistanceUnit");
                    bcc.ColumnMappings.Add("DataSource", "DataSource");
                    bcc.ColumnMappings.Add("ExternalRecordId", "ExternalRecordId");
                    bcc.ColumnMappings.Add("Status", "Status");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("DeleteFlag", "DeleteFlag");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "RecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.DestinationTableName = "RPT_CareTeam";
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
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading CareTeam.", IsError = false });
                ConcurrentBag<ECareTeam> eCareTeams = new ConcurrentBag<ECareTeam>();

                ConcurrentBag<MEContactCareTeam> meContactCareTeams;
                using (var ctx = new ContactCareTeamMongoContext(Contract))
                {
                    meContactCareTeams = new ConcurrentBag<MEContactCareTeam>(ctx.CareTeam.Collection.FindAllAs<MEContactCareTeam>().ToList());
                }

                Parallel.ForEach(meContactCareTeams, ct =>
                {
                    try
                    {
                        if (ct.MeCareTeamMembers != null && ct.MeCareTeamMembers.Count > 0)
                        {
                            var careMembers = ct.MeCareTeamMembers;
                            foreach (var cm in careMembers)
                            {

                                eCareTeams.Add(new ECareTeam
                                {
                                    //Populating these fields from CareTeam
                                    MongoCareTeamId = ct.Id.ToString(),
                                    MongoContactIdForPatient = ct.ContactId.ToString(),
                                    Version = ct.Version,
                                    DeleteFlag = ct.DeleteFlag,
                                    TTLDate = ct.TTLDate,
                                    //Populating these fields from the CareTeam's CareMember. 
                                    MongoCareMemberId = cm.Id.ToString(),
                                    MongoContactIdForCareMember = cm.ContactId.ToString(),
                                    RoleId = cm.RoleId == null ? null : cm.RoleId.ToString(),
                                    CustomRoleName = cm.CustomRoleName,
                                    StartDate = cm.StartDate,
                                    EndDate = cm.EndDate,
                                    Core = cm.Core,
                                    Notes = cm.Notes,
                                    FrequencyId = cm.Frequency == null ? null : cm.Frequency.ToString(),
                                    Distance = cm.Distance,
                                    DistanceUnit = cm.DistanceUnit,
                                    DataSource = cm.DataSource,
                                    ExternalRecordId = cm.ExternalRecordId,
                                    Status = cm.Status.ToString(),
                                    UpdatedBy = cm.UpdatedBy == null ? null : cm.UpdatedBy.ToString(),
                                    LastUpdatedOn = cm.LastUpdatedOn,
                                    RecordCreatedBy = cm.RecordCreatedBy.ToString(),
                                    RecordCreatedOn = cm.RecordCreatedOn,
                                });
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(eCareTeams);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}

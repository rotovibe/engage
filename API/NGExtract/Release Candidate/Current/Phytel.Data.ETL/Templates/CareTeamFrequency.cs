using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FastMember;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Data.ETL.BulkCopy;

namespace Phytel.Data.ETL.Templates
{
    public class CareTeamFrequency : DocumentCollection
    {
        public MELookup LookUp { get; set; }

        private void SaveSubcollection(ConcurrentBag<ECareTeamFrequency> dic)
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
                    bcc.DestinationTableName = "RPT_CareTeamFrequency";
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
                OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] Loading Care Team Frequency.", IsError = false });
                ConcurrentBag<ECareTeamFrequency> careTeamFrequencies = new ConcurrentBag<ECareTeamFrequency>();
                Parallel.ForEach(LookUp.Data, l =>
                {
                    try
                    {
                        careTeamFrequencies.Add(new ECareTeamFrequency
                        {
                            MongoId = l.DataId.ToString(),
                            Name = l.Name,
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs { Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(careTeamFrequencies);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}

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
    public class PatientNoteType : DocumentCollection
    {
        public MELookup LookUp { get; set; }

        private void SaveSubcollection(ConcurrentBag<EPatientNoteType> dic)
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

                    bcc.DestinationTableName = "RPT_NoteTypeLookUp";
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
                ConcurrentBag<EPatientNoteType> medFreq = new ConcurrentBag<EPatientNoteType>();
                var mType = "NoteType";

                Parallel.ForEach(LookUp.Data, l =>
                //foreach (LookUpBase l in LookUp.Data)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        medFreq.Add(new EPatientNoteType
                        {
                            LookUpType = mType,
                            MongoId = l.DataId.ToString(),
                            Name = l.Name,
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs{ Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace, IsError = true });
                    }

                });

                SaveSubcollection(medFreq);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

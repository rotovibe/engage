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
using DataDomain.Medication.Repo;
using FastMember;
using MongoDB.Bson;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.PatientNote;
using Phytel.API.DataDomain.Scheduling;
using Phytel.Data.ETL.BulkCopy;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL.Templates
{
    public class Medications : DocumentCollection
    {
        private void SaveSubcollection(ConcurrentBag<EMedication> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("Id", "MongoId");
                    bcc.ColumnMappings.Add("ProductId", "ProductId");
                    bcc.ColumnMappings.Add("NDC", "NDC");
                    bcc.ColumnMappings.Add("FullName", "FullName");
                    bcc.ColumnMappings.Add("ProprietaryName", "ProprietaryName");
                    bcc.ColumnMappings.Add("ProprietaryNameSuffix", "ProprietaryNameSuffix");
                    bcc.ColumnMappings.Add("StartDate", "StartDate");
                    bcc.ColumnMappings.Add("EndDate", "EndDate");
                    bcc.ColumnMappings.Add("SubstanceName", "SubstanceName");
                    bcc.ColumnMappings.Add("Route", "Route");
                    bcc.ColumnMappings.Add("Form", "Form");
                    bcc.ColumnMappings.Add("FamilyId", "FamilyId");
                    bcc.ColumnMappings.Add("Unit", "Unit");
                    bcc.ColumnMappings.Add("Strength", "Strength");
                    bcc.ColumnMappings.Add("Version", "Version");
                    bcc.ColumnMappings.Add("DeleteFlag", "DeleteFlag");
                    bcc.ColumnMappings.Add("TTLDate", "TTLDate");
                    bcc.ColumnMappings.Add("LastUpdatedOn", "LastUpdatedOn");
                    bcc.ColumnMappings.Add("RecordCreatedBy", "MongoRecordCreatedBy");
                    bcc.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                    bcc.ColumnMappings.Add("MongoUpdatedBy", "MongoUpdatedBy");


                    bcc.DestinationTableName = "RPT_Medication";
                    bcc.WriteToServer(objRdr);
                }
                catch (Exception ex)
                {
                    FormatError(ex, bcc);
                }
            }
        }

        private void SavePharmClasses(ConcurrentBag<EPharmClass> dic)
        {
            var rSeries = dic.AsEnumerable();

            using (var bcc = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.Default))
            using (var objRdr = ObjectReader.Create(rSeries))
            {
                try
                {
                    bcc.BulkCopyTimeout = 580;
                    bcc.ColumnMappings.Add("MedMongoId", "MedMongoId");
                    bcc.ColumnMappings.Add("PharmClass", "PharmClass");

                    bcc.DestinationTableName = "RPT_MedPharmClass";
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
                OnDocColEvent(new ETLEventArgs {Message = "[" + Contract + "] Loading Medication.", IsError = false});
                ConcurrentBag<EPharmClass> pharmClassMap = new ConcurrentBag<EPharmClass>();
                ConcurrentBag<EMedication> eMeds = new ConcurrentBag<EMedication>();

                ConcurrentBag<MEMedication> meds;
                using (MedicationMongoContext smct = new MedicationMongoContext(Contract))
                {
                    meds = new ConcurrentBag<MEMedication>(smct.Medications.Collection.FindAllAs<MEMedication>().ToList());
                }

                Parallel.ForEach(meds, med =>
                    //foreach (MEMedication med in meds)//.Where(t => !t.DeleteFlag ))
                {
                    try
                    {
                        eMeds.Add(new EMedication
                        {
                            Id = med.Id.ToString(),
                            EndDate = med.EndDate,
                            FamilyId = med.FamilyId == null ? string.Empty : med.FamilyId.ToString(),
                            Form = med.Form,
                            FullName = med.FullName,
                            NDC = med.NDC,
                            ProductId = med.ProductId,
                            ProprietaryName = med.ProprietaryName,
                            ProprietaryNameSuffix = med.ProprietaryNameSuffix,
                            Route = med.Route,
                            StartDate = med.StartDate,
                            Strength = med.Strength,
                            SubstanceName = med.SubstanceName,
                            Unit = med.Unit,
                            Version = med.Version,
                            MongoUpdatedBy = med.UpdatedBy == null ? string.Empty : med.UpdatedBy.ToString(),
                            LastUpdatedOn = med.LastUpdatedOn,
                            RecordCreatedBy = med.RecordCreatedBy.ToString(),
                            RecordCreatedOn = med.RecordCreatedOn,
                            TTLDate = med.TTLDate,
                            DeleteFlag = med.DeleteFlag
                        });
                    }
                    catch (Exception ex)
                    {
                        OnDocColEvent(new ETLEventArgs
                        {
                            Message = "[" + Contract + "] " + ex.Message + ": " + ex.StackTrace,
                            IsError = true
                        });
                    }

                    if (med.PharmClass != null)
                    {
                        foreach (string pc in med.PharmClass)
                        {
                            if (!string.IsNullOrEmpty(pc))
                            {
                                pharmClassMap.Add(new EPharmClass
                                {
                                    MedMongoId = med.Id.ToString(),
                                    PharmClass = pc
                                });
                            }
                        }
                    }
                });

                SaveSubcollection(eMeds);
                SavePharmClasses(pharmClassMap);
            }
            catch (Exception ex)
            {
                throw ex; //SimpleLog.Log(new ArgumentException("LoadPatientPrograms()", ex));
            }
        }
    }
}

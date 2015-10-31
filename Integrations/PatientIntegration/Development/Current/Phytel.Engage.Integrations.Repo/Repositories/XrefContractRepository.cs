using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs.SQL;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class XrefContractRepository : IRepository
    {
        private readonly string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }

        public XrefContractRepository(string contract, ISQLConnectionProvider conProvider)
        {
            _contract = contract;
            ConnStr = conProvider;
        }

        public object SelectAll()
        {
            List<PatientXref> ptInfo = null;
            using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
            {
                var query = (from ce in ct.ContactEntities
                    join xf in ct.IntegrationPatientXrefs on new {PhytelPatientID = ce.ID} equals
                        new {PhytelPatientID = xf.PhytelPatientID}
                    where
                        ((from x in ct.IntegrationPatientXrefs
                            where
                                x.SendingApplication != "ENGAGE"
                            select new
                            {
                                x.PhytelPatientID
                            }).Distinct()).Contains(new {PhytelPatientID = ce.ID}) &&

                        ((from C3ProblemList in ct.C3ProblemList
                            where
                                C3ProblemList.PatientID == ce.ID &&
                                (C3ProblemList.ProblemDescription == "ACO CIGN" ||
                                 C3ProblemList.ProblemDescription == "ACO BLUE" ||
                                 C3ProblemList.ProblemDescription == "MGDMCARE" ||
                                 C3ProblemList.ProblemDescription.Contains("AVMED") ||
                                 C3ProblemList.ProblemDescription.Contains("ACO CMS") ||
                                 C3ProblemList.ProblemDescription.StartsWith("CCM "))
                            select new
                            {
                                C3ProblemList.PatientID
                            }).Distinct()).Contains(new {PatientID = ce.ID})
                    select new PatientXref
                    {
                        ID = xf.ID,
                        PhytelPatientID = (int?) xf.PhytelPatientID,
                        ExternalDisplayPatientId = xf.ExternalDisplayPatientId,
                        ExternalPatientID = xf.ExternalPatientID,
                        SendingApplication = xf.SendingApplication,
                        CreateDate = xf.CreateDate,
                        UpdateDate = xf.UpdateDate,
                        UpdatedBy = xf.UpdatedBy
                    });

                ptInfo = query.ToList();
            }

            return ptInfo;
        }

        public object Insert(object list)
        {
            LoggerDomainEvent.Raise(LogStatus.Create("3) Register integrationpatientxrefs for patients...", true));
            try
            {
                var xrefList = (List<EIntegrationPatientXref>)list;

                using (var bcc = new SqlBulkCopy(ConnStr.GetConnectionString(_contract), SqlBulkCopyOptions.Default))
                {
                    using (var objRdr = ObjectReader.Create(xrefList))
                    {
                        try
                        {
                            bcc.BulkCopyTimeout = 580;
                            bcc.ColumnMappings.Add("SendingApplication", "SendingApplication");
                            bcc.ColumnMappings.Add("ExternalPatientID", "ExternalPatientID");
                            bcc.ColumnMappings.Add("PhytelPatientID", "PhytelPatientID");
                            bcc.ColumnMappings.Add("CreateDate", "CreateDate");
                            bcc.ColumnMappings.Add("UpdateDate", "UpdateDate");
                            bcc.ColumnMappings.Add("UpdatedBy", "UpdatedBy");
                            bcc.ColumnMappings.Add("ExternalDisplayPatientId", "ExternalDisplayPatientId");

                            bcc.DestinationTableName = "IntegrationPatientXref";
                            bcc.WriteToServer(objRdr);
                            LoggerDomainEvent.Raise(LogStatus.Create("3) Success", true));
                        }
                        catch (Exception ex)
                        {
                            Utils.FormatError(ex, bcc);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("XrefContractRepository:Insert(): " + ex.Message);
            }
        }
    }
}

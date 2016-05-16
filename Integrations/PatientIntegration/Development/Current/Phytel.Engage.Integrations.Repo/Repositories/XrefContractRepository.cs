using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FastMember;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Bridge;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs.SQL;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class XrefContractRepository : IRepository
    {
        private readonly string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }
        public IQueryImplementation Implementor { get; set; }

        public XrefContractRepository(string contract, ISQLConnectionProvider conProvider, IQueryImplementation implementor)
        {
            _contract = contract;
            ConnStr = conProvider;
            Implementor = implementor;
        }

        public object SelectAll()
        {
            List<PatientXref> ptInfo = null;
            using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
            {
                var query = Implementor.GetPatientXrefQuery(ct);

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


        public object SelectAllGeneral()
        {
            throw new NotImplementedException();
        }
    }
}

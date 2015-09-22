using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Services.SQLServer;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class XrefContractRepository : IRepository
    {
        private string _contract;
        private string _connString;
        public ISQLConnectionProvider ConnStr { get; set; }

        public XrefContractRepository(string contract)
        {
            _contract = contract;
            ConnStr = new SQLConnectionProvider();
            _connString = ConnStr.GetConnectionString(contract);

            //_sqlConnection = new SqlConnection(_connString);
        }

        public object SelectAll()
        {
            List<PatientXref> ptInfo = null;
            using (var ct = new ContractEntities())
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

    }
}

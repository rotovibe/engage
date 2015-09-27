using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;
using Phytel.Services.SQLServer;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PatientsContractRepository : IRepository
    {
        private string _contract;
        private string _connString;
        public ISQLConnectionProvider ConnStr { get; set; }
        public SqlConnectionStringBuilder connBuilder;

        public PatientsContractRepository(string contract)
        {
            _contract = contract;
            ConnStr = new SQLConnectionProvider();
            _connString = @"data source=azurephyoutreach.cloudapp.net;initial catalog=ORLANDOHEALTH001;persist security info=True;user id=mbobadilla;password=Ju1cy-Fru1t;MultipleActiveResultSets=True;App=EntityFramework";//ConnStr.GetConnectionString(contract);
            connBuilder = new SqlConnectionStringBuilder(_connString);
        }

        public object SelectAll()
        {
            Dictionary<int, PatientInfo> ptInfo = null;
            using (var ct = new Phytel.Engage.Integrations.Repo.EF.ContractEntities())
            {
                var query = (from c3p in ct.C3Patient
                             join ce in ct.ContactEntities on new { PatientID = c3p.PatientID } equals new { PatientID = ce.ID }
                             join rp in ct.ReportPatients on c3p.PatientID equals rp.PatientID
                             join pl in ct.C3ProblemList on c3p.PatientID equals pl.PatientID
                             join kd in ct.KeyDatas
                                   on new { ce.ID, CategoryCode = "SSN" }
                               equals new { ID = kd.OwnerID, kd.CategoryCode } into kd_join
                             from kd in kd_join.DefaultIfEmpty()
                             where
                                 ((from x in ct.IntegrationPatientXrefs
                                   where
                                     x.SendingApplication != "ENGAGE"
                                   select new
                                   {
                                       x.PhytelPatientID
                                   }).Distinct()).Contains(new { PhytelPatientID = ce.ID }) &&

                                 ((from C3ProblemList in ct.C3ProblemList
                                   where
                                     C3ProblemList.PatientID == c3p.PatientID &&
                                     (C3ProblemList.ProblemDescription == "ACO CIGN" ||
                                     C3ProblemList.ProblemDescription == "ACO BLUE" ||
                                     C3ProblemList.ProblemDescription == "MGDMCARE" ||
                                     C3ProblemList.ProblemDescription.Contains("AVMED") ||
                                     C3ProblemList.ProblemDescription.StartsWith("CCM "))
                                   select new
                                   {
                                       C3ProblemList.PatientID
                                   }).Distinct()).Contains(new { PatientID = ce.ID })
                    select new PatientInfo
                    {
                        SubscriberId = rp.SubscriberID,
                        PatientId = (int?) c3p.PatientID,
                        FirstName = ce.FirstName,
                        MiddleInitial = ce.MiddleInitial,
                        LastName = ce.LastName,
                        Suffix = ce.Suffix,
                        Gender = ce.Gender,
                        BirthDate = ce.BirthDate,
                        Ssn = kd.KeyData1,
                        CareManagerId = rp.CareManagerID,
                        CareManagerName = rp.CareManagerName,
                        Phone = rp.Phone,
                        Priority = rp.Priority,
                        PCP = rp.ProviderName,
                        CreateDate = ce.CreateDate,
                        UpdateDate = ce.UpdateDate,
                        Status =
                            ((from pt in ct.C3Patient
                                where
                                    pt.PatientID == ce.ID
                                select new
                                {
                                    pt.PatientStatus
                                }).FirstOrDefault().PatientStatus)
                    }).Distinct()
                    .Take(5); // remove this!!

                ptInfo = query.ToDictionary(r => Convert.ToInt32(r.PatientId));
            }

            return ptInfo;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
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

        public PatientsContractRepository(string contract)
        {
            _contract = contract;
            ConnStr = new SQLConnectionProvider();
            _connString = ConnStr.GetConnectionString(contract);

            //_sqlConnection = new SqlConnection(_connString);
        }

        public object SelectAll()
        {
            Dictionary<int, PatientInfo> ptInfo = null;
            using (var ct = new ContractEntities())
            {
                #region // older query

                //var query = from ce in ct.ContactEntities
                //    join rp in ct.ReportPatients on new {ID = ce.ID} equals new {ID = rp.PatientID}
                //    join ge in ct.C3GroupEntity on rp.SubscriberID equals ge.EntityId
                //    join pl in ct.C3ProblemList on new {PatientID = ce.ID} equals new {PatientID = pl.PatientID}
                //    join kd in ct.KeyDatas
                //        on new {ce.ID, CategoryCode = "SSN"}
                //        equals new {ID = kd.OwnerID, kd.CategoryCode} into kd_join
                //    from kd in kd_join.DefaultIfEmpty()
                //    where
                //        (from C3Group in ct.C3Group
                //            where
                //                C3Group.Name.Contains("Physician") ||
                //                C3Group.Description.Contains("Physician")
                //            select new
                //            {
                //                C3Group.C3GroupId
                //            }).Contains(new {C3GroupId = ge.C3Group.C3GroupId}) &&

                //        (from x in ct.IntegrationPatientXrefs
                //            where x.SendingApplication != "ENGAGE"
                //            select new
                //            {
                //                x.PhytelPatientID
                //            }).Contains(new {PhytelPatientID = ce.ID}) &&
                //        ((new string[] {"ACO CIGN", "ACO BLUE", "MGDMCARE"}).Contains(pl.ProblemDescription) ||
                //         pl.ProblemDescription.Contains("AVMED") ||
                //         pl.ProblemDescription.StartsWith("CCM "))
                //    select new PatientInfo
                //    {
                //        GroupName = ge.C3Group.Name,
                //        GroupId = (int?) ge.C3Group.C3GroupId,
                //        SubscriberId = (int?) rp.SubscriberID,
                //        PatientId = (int?) ce.ID,
                //        FirstName = ce.FirstName,
                //        MiddleInitial = ce.MiddleInitial,
                //        LastName = ce.LastName,
                //        Suffix = ce.Suffix,
                //        Gender = ce.Gender,
                //        BirthDate = ce.BirthDate,
                //        SSN = kd.KeyData1,
                //        CreateDate = ce.CreateDate,
                //        UpdateDate = ce.UpdateDate,
                //        Status =
                //            ((from pt in ct.C3Patient
                //                where
                //                    pt.PatientID == ce.ID
                //                select new
                //                {
                //                    pt.PatientStatus
                //                }).FirstOrDefault().PatientStatus)
                //    };

                #endregion

                var query = (from ce in ct.ContactEntities
                    join rp in ct.ReportPatients on new {ID = ce.ID} equals new {ID = rp.PatientID}
                    join pl in ct.C3ProblemList on new {PatientID = ce.ID} equals new {PatientID = pl.PatientID}
                    join kd in ct.KeyDatas
                        on new {ce.ID, CategoryCode = "SSN"}
                        equals new {ID = kd.OwnerID, kd.CategoryCode} into kd_join
                    from kd in kd_join.DefaultIfEmpty()
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
                    select new PatientInfo
                    {
                        SubscriberId = rp.SubscriberID,
                        PatientId = (int?) ce.ID,
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
                    }).Distinct();

                ptInfo = query.ToDictionary(r => Convert.ToInt32(r.PatientId));
            }

            return ptInfo;
        }

    }
}

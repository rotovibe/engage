using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PatientsContractRepository : IRepository
    {
        private string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }

        public PatientsContractRepository(string contract, ISQLConnectionProvider conProvider)
        {
            _contract = contract;
            ConnStr = conProvider;

        }

        public object SelectAll()
        {
            try
            {
                Dictionary<int, PatientInfo> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = (from c3p in ct.C3Patient
                        join ce in ct.ContactEntities on new {PatientID = c3p.PatientID} equals new {PatientID = ce.ID}
                        join rp in ct.ReportPatients on c3p.PatientID equals rp.PatientID
                        join pl in ct.C3ProblemList on c3p.PatientID equals pl.PatientID
                        join kd in ct.KeyDatas
                            on new {ce.ID, CategoryCode = "SSN"}
                            equals new {ID = kd.OwnerID, kd.CategoryCode} into kd_join
                        from kd in kd_join.DefaultIfEmpty()
                        where
                            !(from IntegrationPatientXref in ct.IntegrationPatientXrefs where IntegrationPatientXref.SendingApplication == "ENGAGE" select new
                             {
                                 IntegrationPatientXref.PhytelPatientID
                             }).Contains(new { PhytelPatientID = ce.ID }) &&

                            ((from C3ProblemList in ct.C3ProblemList
                                where
                                    C3ProblemList.PatientID == c3p.PatientID &&
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
                            && (new int[]{0}).Contains(c3p.PatientStatusID)
                            //&& (new int[] { 478006,493302,509210,531607,536306,538123,542605,542612,548320,555121}).Contains(ce.ID) // this is for testing!!!!
                            //&& (new int[] { 28124, 38424, 509703, 495517, 497800, 577221, 433112, 563706, 567721, 607607 }).Contains(ce.ID) // testing!!!
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
                            FollowupDueDate = c3p.FollowupDueDate,
                            Status = 
                                ((from pt in ct.C3Patient
                                    where
                                        pt.PatientID == ce.ID
                                    select new
                                    {
                                        pt.PatientStatus
                                    }).FirstOrDefault().PatientStatus)
                        }).Distinct();
                    //.Take(1000); // remove this!!

                    ptInfo = query.ToDictionary(r => Convert.ToInt32(r.PatientId));
                }

                return ptInfo;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("PatientsContractRepository:SelectAll(): " + ex.Message, false) );
                throw;
            }
        }



        public object Insert(object list)
        {
            return null;
        }
    }
}

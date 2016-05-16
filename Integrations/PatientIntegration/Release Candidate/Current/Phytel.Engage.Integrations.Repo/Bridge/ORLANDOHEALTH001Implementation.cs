using System;
using System.Linq;
using System.Transactions;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Bridge
{
    public class ORLANDOHEALTH001Implementation : IQueryImplementation
    {
        public IQueryable<PatientInfo> GetPatientInfoQuery(ContractEntities ct)
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
                             !(from IntegrationPatientXref in ct.IntegrationPatientXrefs
                               where IntegrationPatientXref.SendingApplication == "ENGAGE"
                               select new
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
                               }).Distinct()).Contains(new { PatientID = ce.ID })
                             && (new int[] { 0 }).Contains(c3p.PatientStatusID)
                         //&& (new int[] { 354917 }).Contains(ce.ID) // this is for testing!!!!
                         select new PatientInfo
                         {
                             SubscriberId = rp.SubscriberID,
                             PatientId = ce.ID,
                             //PatientId = (int?)c3p.PatientID,
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

            return query;
        }

        public IQueryable<PCPPhone> GetPCPPhoneQuery(ContractEntities ct)
        {
            IQueryable<PCPPhone> query = null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                query = (from ce in ct.ContactEntities
                         join p in ct.C3Patient
                             on new { ce.ID, PatientStatus = "Active" }
                             equals new { ID = p.PatientID, p.PatientStatus }
                         join prh in ct.PatientRegistryHdrs on new { PatientID = p.PatientID } equals new { PatientID = prh.PATIENTID }
                         join ppp in ct.PatientPersonalPhysicians on prh.PatientRegistryHdrID equals ppp.PatientRegistryHdrID
                         join ce1 in ct.ContactEntities on new { ProviderID = ppp.ProviderID ?? 0 } equals new { ProviderID = ce1.ID }
                         join se in ct.SubscriberEntities on new { ProviderID = p.ProviderID ?? 0, CategoryCode = "DR" } equals new { ProviderID = se.SUBSCRIBERID, se.CategoryCode }
                         join ce2 in ct.ContactEntities on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = ce2.ID }
                         join pn in ct.Phones on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = pn.OwnerID } into pn_join
                         from pn in pn_join.DefaultIfEmpty()
                         join pl in ct.C3ProblemList on new { ID = ce.ID } equals new { ID = pl.PatientID } into pl_join
                         from pl in pl_join.DefaultIfEmpty()
                         join i in ct.IntegrationPatientXrefs on new { ID = ce.ID } equals new { ID = i.PhytelPatientID }
                         where
                             !(from IntegrationPatientXref in ct.IntegrationPatientXrefs
                               where
                                   IntegrationPatientXref.SendingApplication == "ENGAGE"
                               select new
                               {
                                   IntegrationPatientXref.PhytelPatientID
                               }).Contains(new { PhytelPatientID = i.PhytelPatientID }) &&
                             (pl.ProblemDescription == "ACO CIGN" ||
                              pl.ProblemDescription == "ACO BLUE" ||
                              pl.ProblemDescription == "MGDMCARE" ||
                              pl.ProblemDescription.Contains("AVMED") ||
                              pl.ProblemDescription.StartsWith("CCM ") ||
                              pl.ProblemDescription == "ACO CMS")
                         select new PCPPhone
                         {
                             PatientID = p.PatientID,
                             PCPId = ce1.ID,
                             PCP_Name = ce1.Name,
                             Facility = ce2.Name,
                             desc = ((from CommCategory in ct.CommCategories
                                      where CommCategory.CommCategoryCode == pn.CategoryCode
                                      select new { CommCategory.Description }).FirstOrDefault().Description),
                             Phone = (pn.DialString)
                         }).Distinct();
                //.Take(10);
            }

            return query;
        }

        public IQueryable<PatientXref> GetPatientXrefQuery(ContractEntities ct)
        {
            var query = (from ce in ct.ContactEntities
                         join xf in ct.IntegrationPatientXrefs on new { PhytelPatientID = ce.ID } equals
                             new { PhytelPatientID = xf.PhytelPatientID }
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
                               }).Distinct()).Contains(new { PatientID = ce.ID })
                         select new PatientXref
                         {
                             ID = xf.ID,
                             PhytelPatientID = (int?)xf.PhytelPatientID,
                             ExternalDisplayPatientId = xf.ExternalDisplayPatientId,
                             ExternalPatientID = xf.ExternalPatientID,
                             SendingApplication = xf.SendingApplication,
                             CreateDate = xf.CreateDate,
                             UpdateDate = xf.UpdateDate,
                             UpdatedBy = xf.UpdatedBy
                         });
            return query;
        }

        public IQueryable<PatientNote> GetPatientNotesQuery(ContractEntities ct)
        {
            var query = (from pn in ct.C3NotePatient
                         join na in ct.C3NoteAction on pn.ActionID equals na.ActionID
                         join nc in ct.C3NoteCategory on pn.CategoryId equals nc.CategoryId
                         select new PatientNote
                         {
                             NoteId = pn.NoteId,
                             Note = pn.Note,
                             ActionID = na.ActionID,
                             ActionName = na.ActionName,
                             CategoryId = nc.CategoryId,
                             CategoryName = nc.CategoryName,
                             PatientId = pn.PatientId,
                             Enabled = pn.Enabled.ToString(),
                             CreatedDate = pn.CreatedDate,
                             UpdatedDate = pn.UpdatedDate,
                             CreatedBy = pn.CreatedBy,
                             CreatedById = pn.CreatedById.ToString(),
                             DeletedBy = pn.DeletedBy,
                             DeletedDate = pn.DeletedDate,
                             DeletedStatus = pn.DeletedStatus.ToString()
                         });
            return query;
        }


        public IQueryable<PCPPhone> GetPCPPhoneQueryGeneral(ContractEntities ct)
        {
            IQueryable<PCPPhone> query = null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                query = (from ce in ct.ContactEntities
                         join p in ct.C3Patient
                             on new { ce.ID, PatientStatus = "Active" }
                             equals new { ID = p.PatientID, p.PatientStatus }
                         join prh in ct.PatientRegistryHdrs on new { PatientID = p.PatientID } equals new { PatientID = prh.PATIENTID }
                         join ppp in ct.PatientPersonalPhysicians on prh.PatientRegistryHdrID equals ppp.PatientRegistryHdrID
                         join ce1 in ct.ContactEntities on new { ProviderID = ppp.ProviderID ?? 0 } equals new { ProviderID = ce1.ID }
                         join se in ct.SubscriberEntities on new { ProviderID = p.ProviderID ?? 0, CategoryCode = "DR" } equals new { ProviderID = se.SUBSCRIBERID, se.CategoryCode }
                         join ce2 in ct.ContactEntities on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = ce2.ID }
                         join pn in ct.Phones on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = pn.OwnerID } into pn_join
                         from pn in pn_join.DefaultIfEmpty()
                         join pl in ct.C3ProblemList on new { ID = ce.ID } equals new { ID = pl.PatientID } into pl_join
                         from pl in pl_join.DefaultIfEmpty()
                         join i in ct.IntegrationPatientXrefs on new { ID = ce.ID } equals new { ID = i.PhytelPatientID }
                         where
                             (pl.ProblemDescription == "ACO CIGN" ||
                              pl.ProblemDescription == "ACO BLUE" ||
                              pl.ProblemDescription == "MGDMCARE" ||
                              pl.ProblemDescription.Contains("AVMED") ||
                              pl.ProblemDescription.StartsWith("CCM ") ||
                              pl.ProblemDescription == "ACO CMS")
                         select new PCPPhone
                         {
                             PatientID = p.PatientID,
                             PCPId = ce1.ID,
                             PCP_Name = ce1.Name,
                             Facility = ce2.Name,
                             desc = ((from CommCategory in ct.CommCategories
                                      where CommCategory.CommCategoryCode == pn.CategoryCode
                                      select new { CommCategory.Description }).FirstOrDefault().Description),
                             Phone = (pn.DialString)
                         }).Distinct();
                //.Take(10);
            }

            return query;
        }
    }
}

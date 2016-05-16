using System;
using System.Linq;
using System.Transactions;
using Phytel.Engage.Integrations.Repo.DTO;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Bridge
{
    public class HILLCREST001Implementation : IQueryImplementation
    {
        public IQueryable<PatientInfo> GetPatientInfoQuery(ContractEntities db)
        {
           IQueryable<PatientInfo> query = null;

            query = (from g in db.C3Group
                join ge in db.C3GroupEntity on new {g.C3GroupId, DeleteFlag = false}
                    equals new {ge.C3GroupId, ge.DeleteFlag}
                join et in db.C3GroupEntityType on new {ge.C3GroupEntityTypeId, Name = "Subscriber"}
                    equals new {et.C3GroupEntityTypeId, et.Name}
                join s in db.Subscribers on new {ge.EntityId, Enabled = 1, DeleteFlag = 0}
                    equals new {EntityId = s.SUBSCRIBERID, s.Enabled, s.DeleteFlag}
                join ss in db.SubscriberSolutions on s.SUBSCRIBERID 
                    equals ss.SUBSCRIBERID
                join p in db.C3Patient on new {SUBSCRIBERID = s.SUBSCRIBERID} 
                    equals new {SUBSCRIBERID = (int) p.ProviderID}
                join ce in db.ContactEntities on new {PatientID = p.PatientID} 
                    equals new {PatientID = ce.ID}
                join rp in db.ReportPatients on new {ID = ce.ID} 
                    equals new {ID = rp.PatientID}
                join kd in db.KeyDatas on new {ce.ID, CategoryCode = "SSN"}
                    equals new {ID = kd.OwnerID, kd.CategoryCode} into kd_join from kd in kd_join.DefaultIfEmpty()
                join c3p in db.C3Patient on new {ID = ce.ID} 
                     equals new {ID = c3p.PatientID}
                where
                    g.DeleteFlag == false &&
                    g.EnableAll == false &&
                    ss.DeleteFlag == 0 &&
                    ss.EffectiveDate <= DateTime.Now &&
                    (ss.TermDate == null ||
                     ss.TermDate >= DateTime.Now) &&
                    ss.SOLUTIONID == 55 &&
                    s.EffectiveDate <= DateTime.Now &&
                    (s.TermDate == null ||
                     s.TermDate >= DateTime.Now) &&
                    p.Priority == 3 &&
                    !((from x in db.IntegrationPatientXrefs
                        where
                            x.SendingApplication == "ENGAGE"
                        select new
                        {
                            x.PhytelPatientID
                        }).Distinct()).Contains(new {PhytelPatientID = ce.ID})
                select new PatientInfo
                {
                    SubscriberId = (int?) s.SUBSCRIBERID,
                    PatientId = (int?) p.PatientID,
                    FirstName = ce.FirstName,
                    LastName = ce.LastName,
                    MiddleInitial = ce.MiddleInitial,
                    Suffix = ce.Suffix,
                    Gender = ce.Gender,
                    BirthDate = ce.BirthDate,
                    Ssn = kd.KeyData1,
                    CareManagerId = rp.CareManagerID,
                    CareManagerName = rp.CareManagerName,
                    Phone = rp.Phone,
                    Priority = rp.Priority,
                    //rp.ProviderName,
                    CreateDate = ce.CreateDate,
                    UpdateDate = ce.UpdateDate,
                    FollowupDueDate = c3p.FollowupDueDate,
                    Status =
                        ((from pt in db.C3Patient
                            where
                                pt.PatientID == ce.ID
                            select new
                            {
                                pt.PatientStatus
                            }).FirstOrDefault().PatientStatus)
                }).Distinct();
            //.Take(3); // remove this!!

            return query;
        }

        public IQueryable<PCPPhone> GetPCPPhoneQueryGeneral(ContractEntities db)
        {
            IQueryable<PCPPhone> query = null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                query = (from ce in db.ContactEntities
                         join p in db.C3Patient on new { ID = ce.ID } equals new { ID = p.PatientID }
                         join prh in db.PatientRegistryHdrs on new { PatientID = p.PatientID } equals new { PatientID = prh.PATIENTID }
                         join ppp in db.PatientPersonalPhysicians on prh.PatientRegistryHdrID equals ppp.PatientRegistryHdrID
                         join ce1 in db.ContactEntities on new { ProviderID = (int)ppp.ProviderID } equals new { ProviderID = ce1.ID }
                         join se in db.SubscriberEntities
                               on new { ProviderID = (int)p.ProviderID, CategoryCode = "DR" }
                           equals new { ProviderID = se.SUBSCRIBERID, se.CategoryCode }
                         join ce2 in db.ContactEntities on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = ce2.ID }
                         join pn in db.Phones on new { FACILITYID = se.FACILITYID } equals new { FACILITYID = pn.OwnerID } into pn_join
                         from pn in pn_join.DefaultIfEmpty()
                         join cc in db.CommCategories on new { CommCategoryCode = pn.CategoryCode } equals new { CommCategoryCode = cc.CommCategoryCode }
                         join i in db.IntegrationPatientXrefs on new { ID = ce.ID } equals new { ID = i.PhytelPatientID } into i_join
                         from i in i_join.DefaultIfEmpty()
                         where
                             ((from g in db.C3Group
                               join ge in db.C3GroupEntity
                                     on new { g.C3GroupId, DeleteFlag = false}
                                 equals new { ge.C3GroupId, ge.DeleteFlag }
                               join et in db.C3GroupEntityType
                                     on new { ge.C3GroupEntityTypeId, Name = "Subscriber" }
                                 equals new { et.C3GroupEntityTypeId, et.Name }
                               join s in db.Subscribers
                                     on new { ge.EntityId, Enabled = 1, DeleteFlag = 0 }
                                 equals new { EntityId = s.SUBSCRIBERID, s.Enabled, s.DeleteFlag }
                               join ss in db.SubscriberSolutions
                                     on new { s.SUBSCRIBERID, DeleteFlag = 0, SOLUTIONID = 55 }
                                 equals new { ss.SUBSCRIBERID, ss.DeleteFlag, ss.SOLUTIONID }
                               join p0 in db.C3Patient on new { SUBSCRIBERID = s.SUBSCRIBERID } equals new { SUBSCRIBERID = (int)p0.ProviderID }
                               join ce12 in db.ContactEntities on new { PatientID = p0.PatientID } equals new { PatientID = ce12.ID }
                               where
                                 g.DeleteFlag == false &&
                                 s.EffectiveDate <= DateTime.Now &&
                                 (s.TermDate == null ||
                                 s.TermDate >= DateTime.Now) &&
                                 ss.EffectiveDate <= DateTime.Now &&
                                 (ss.TermDate == null ||
                                 ss.TermDate >= DateTime.Now) &&
                                 g.EnableAll == false
                               select new
                               {
                                   p0.PatientID
                               }).Distinct()).Contains(new { PatientID = i.PhytelPatientID })
                         select new PCPPhone
                         {
                             PatientID = ce.ID,
                             PCPId = (int)ppp.ProviderID,
                             PCP_Name = ce1.Name,
                             Facility = ce2.Name,
                             desc = ((from CommCategory in db.CommCategories
                                      where CommCategory.CommCategoryCode == pn.CategoryCode
                                      select new { CommCategory.Description }).FirstOrDefault().Description),
                             Phone = pn.DialString
                         }).Distinct();
            }

            return query;
        }

        public IQueryable<PCPPhone> GetPCPPhoneQuery(ContractEntities db)
        {
            IQueryable<PCPPhone> query = null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
            {
                query = (from ce in db.ContactEntities
                    join p in db.C3Patient on new {ID = ce.ID} equals new {ID = p.PatientID}
                    join prh in db.PatientRegistryHdrs on new {PatientID = p.PatientID} equals new {PatientID = prh.PATIENTID}
                    join ppp in db.PatientPersonalPhysicians on prh.PatientRegistryHdrID equals ppp.PatientRegistryHdrID
                    join ce1 in db.ContactEntities on new {ProviderID = (int) ppp.ProviderID} equals new {ProviderID = ce1.ID}
                    join se in db.SubscriberEntities
                        on new {ProviderID = (int) p.ProviderID, CategoryCode = "DR"}
                        equals new {ProviderID = se.SUBSCRIBERID, se.CategoryCode}
                    join ce2 in db.ContactEntities on new {FACILITYID = se.FACILITYID} equals new {FACILITYID = ce2.ID}
                    join pn in db.Phones on new {FACILITYID = se.FACILITYID} equals new {FACILITYID = pn.OwnerID} into pn_join
                    from pn in pn_join.DefaultIfEmpty()
                    join cc in db.CommCategories on new {CommCategoryCode = pn.CategoryCode} equals new {CommCategoryCode = cc.CommCategoryCode}
                    join i in db.IntegrationPatientXrefs on new {ID = ce.ID} equals new {ID = i.PhytelPatientID} into i_join
                    from i in i_join.DefaultIfEmpty()
                    where
                        ((from g in db.C3Group
                            join ge in db.C3GroupEntity
                                on new {g.C3GroupId, DeleteFlag = false}
                                equals new {ge.C3GroupId, ge.DeleteFlag}
                            join et in db.C3GroupEntityType
                                on new {ge.C3GroupEntityTypeId, Name = "Subscriber"}
                                equals new {et.C3GroupEntityTypeId, et.Name}
                            join s in db.Subscribers
                                on new {ge.EntityId, Enabled = 1, DeleteFlag = 0}
                                equals new {EntityId = s.SUBSCRIBERID, s.Enabled, s.DeleteFlag}
                            join ss in db.SubscriberSolutions
                                on new {s.SUBSCRIBERID, DeleteFlag = 0, SOLUTIONID = 55}
                                equals new {ss.SUBSCRIBERID, ss.DeleteFlag, ss.SOLUTIONID}
                            join p0 in db.C3Patient on new {SUBSCRIBERID = s.SUBSCRIBERID} equals new {SUBSCRIBERID = (int) p0.ProviderID}
                            join ce12 in db.ContactEntities on new {PatientID = p0.PatientID} equals new {PatientID = ce12.ID}
                            where
                                g.DeleteFlag == false &&
                                s.EffectiveDate <= DateTime.Now &&
                                (s.TermDate == null ||
                                 s.TermDate >= DateTime.Now) &&
                                ss.EffectiveDate <= DateTime.Now &&
                                (ss.TermDate == null ||
                                 ss.TermDate >= DateTime.Now) &&
                                g.EnableAll == false &&
                                p0.Priority == 3
                            select new
                            {
                                p0.PatientID
                            }).Distinct()).Contains(new {PatientID = i.PhytelPatientID})
                    select new PCPPhone
                    {
                        PatientID = ce.ID,
                        PCPId = (int) ppp.ProviderID,
                        PCP_Name = ce1.Name,
                        Facility = ce2.Name,
                        desc = ((from CommCategory in db.CommCategories
                            where CommCategory.CommCategoryCode == pn.CategoryCode
                            select new {CommCategory.Description}).FirstOrDefault().Description),
                        Phone = pn.DialString
                    }).Distinct();
            }

            return query;
        }

        public IQueryable<PatientXref> GetPatientXrefQuery(ContractEntities db)
        {
            IQueryable<PatientXref> query = null;

            query = (from g in db.C3Group
                join ge in db.C3GroupEntity
                    on new {g.C3GroupId, DeleteFlag = false}
                    equals new {ge.C3GroupId, ge.DeleteFlag}
                join et in db.C3GroupEntityType
                    on new {ge.C3GroupEntityTypeId, Name = "Subscriber"}
                    equals new {et.C3GroupEntityTypeId, et.Name}
                join s in db.Subscribers
                    on new {ge.EntityId, Enabled = 1, DeleteFlag = 0}
                    equals new {EntityId = s.SUBSCRIBERID, s.Enabled, s.DeleteFlag}
                join ss in db.SubscriberSolutions on s.SUBSCRIBERID equals ss.SUBSCRIBERID
                join p in db.C3Patient on new {SUBSCRIBERID = s.SUBSCRIBERID} equals new {SUBSCRIBERID = (int) p.ProviderID}
                join ce in db.ContactEntities on new {PatientID = p.PatientID} equals new {PatientID = ce.ID}
                join xf in db.IntegrationPatientXrefs on new {PhytelPatientID = ce.ID} equals new {PhytelPatientID = xf.PhytelPatientID}
                where
                    g.DeleteFlag == false &&
                    g.EnableAll == false &&
                    p.Priority == 3 &&
                    ss.DeleteFlag == 0 &&
                    ss.EffectiveDate <= DateTime.Now &&
                    (ss.TermDate == null ||
                     ss.TermDate >= DateTime.Now) &&
                    ss.SOLUTIONID == 55 &&
                    s.EffectiveDate <= DateTime.Now &&
                    (s.TermDate == null ||
                     s.TermDate >= DateTime.Now)
                    && !((from x in db.IntegrationPatientXrefs
                        where
                            x.SendingApplication == "ENGAGE"
                        select new
                        {
                            x.PhytelPatientID
                        }).Distinct()).Contains(new {PhytelPatientID = ce.ID})
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
                }).Distinct();

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PCPPhoneRepository : IRepository
    {
        private string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }

        public PCPPhoneRepository(string contract, ISQLConnectionProvider conProvider)
        {
            _contract = contract;
            ConnStr = conProvider;

        }

        public object SelectAll()
        {
            try
            {
                List<PCPPhone> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = (from ce in ct.ContactEntities
                        join c3p in ct.C3Patient on new {ID = ce.ID} equals new {ID = c3p.PatientID}
                        join se in ct.SubscriberEntities on new {ProviderID = c3p.ProviderID ?? default(int)} equals new {ProviderID = se.SUBSCRIBERID}
                        join pn in ct.Phones on new {FACILITYID = se.FACILITYID} equals new {FACILITYID = pn.OwnerID}
                        join rp in ct.ReportPatients on new {ID = ce.ID} equals new {ID = rp.PatientID}
                        join pl in ct.C3ProblemList on new {PatientID = ce.ID} equals new {PatientID = pl.PatientID}
                        join kd in ct.KeyDatas on new {ce.ID, CategoryCode = "SSN"} equals new {ID = kd.OwnerID, kd.CategoryCode} into kd_join
                        from kd in kd_join.DefaultIfEmpty()
                        where
                            ((from x in ct.IntegrationPatientXrefs where x.SendingApplication != "ENGAGE" select new { x.PhytelPatientID }).Distinct()).Contains(new {PhytelPatientID = ce.ID}) &&
                            ((from C3ProblemList in ct.C3ProblemList
                                where
                                    C3ProblemList.PatientID == ce.ID &&
                                    (C3ProblemList.ProblemDescription == "ACO CIGN" ||
                                     C3ProblemList.ProblemDescription == "ACO BLUE" ||
                                     C3ProblemList.ProblemDescription == "MGDMCARE" ||
                                     C3ProblemList.ProblemDescription.Contains("AVMED") ||
                                     C3ProblemList.ProblemDescription.Contains("ACO CMS") ||
                                     C3ProblemList.ProblemDescription.StartsWith("CCM "))
                                select new { C3ProblemList.PatientID}).Distinct()).Contains(new {PatientID = ce.ID}) &&
                            c3p.PatientStatusID == 0 &&
                            se.CategoryCode == "DR"
                        select new PCPPhone
                        {
                            PatientID = ce.ID,
                            PCPId = se.CONTACTENTITYID,
                            PCP_Name = ((from ContactEntities in ct.ContactEntities where ContactEntities.ID == se.CONTACTENTITYID select new{ Name = (ContactEntities.LastName + ", " + ContactEntities.FirstName) }).FirstOrDefault().Name),
                            Facility = ((from ContactEntities in ct.ContactEntities where ContactEntities.ID == se.FACILITYID select new {ContactEntities.Name}).FirstOrDefault().Name),
                            desc = ((from CommCategory in ct.CommCategories where CommCategory.CommCategoryCode == pn.CategoryCode select new {CommCategory.Description }).FirstOrDefault().Description),
                            Phone = ("(" + pn.AreaCode + ")" + pn.PhoneNumberString)
                        }).Distinct();

                    ptInfo = query.ToList();
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

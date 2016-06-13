using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using Phytel.API.Common.Audit;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactRepository : IRepository
    {
        object FindContactByPatientId(GetContactByPatientIdDataRequest request);
        IEnumerable<object> FindCareManagers();
        bool UpdateRecentList(PutRecentPatientRequest request, List<string> recentList);
        IEnumerable<object> GetContactsByContactIds(GetContactsByContactIdsDataRequest request);
        object FindContactByUserId(GetContactByUserIdDataRequest request);
        IAuditHelpers AuditHelpers { get; set; }
        object GetContactByPatientId(string patientId);
        IEnumerable<object> FindContactsWithAPatientInRecentList(string entityId);
        IEnumerable<object> Select(List<string> ids);
        IEnumerable<object> SearchContacts(SearchContactsDataRequest request);
        long GetSearchContactsCount(SearchContactsDataRequest request);
        bool SyncContact(SyncContactInfoDataRequest request);
        bool DereferencePatient(DereferencePatientDataRequest request);
        bool UnDereferencePatient(UndoDereferencePatientDataRequest request);

        IEnumerable<ContactData> GetContactsByPatientIds(List<string> patientIds);
    }
}

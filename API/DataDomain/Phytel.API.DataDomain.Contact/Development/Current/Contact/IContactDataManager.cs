using Phytel.API.DataDomain.Contact.DTO;
using System;
using System.Collections.Generic;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactDataManager
    {
        GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request);
        ContactData GetContactByPatientId(GetContactByPatientIdDataRequest request);
        ContactData GetContactByUserId(GetContactByUserIdDataRequest request);
        string InsertContact(InsertContactDataRequest request);
        GetContactsByContactIdsDataResponse GetContactsByContactId(GetContactsByContactIdsDataRequest request);
        List<HttpObjectResponse<ContactData>> InsertBatchContacts(InsertBatchContactDataRequest request);
        UpdateContactDataResponse UpdateContact(UpdateContactDataRequest request);
        PutRecentPatientResponse AddRecentPatient(PutRecentPatientRequest request);
        GetContactByContactIdDataResponse GetContactByContactId(GetContactByContactIdDataRequest request);
        DeleteContactByPatientIdDataResponse DeleteContactByPatientId(DeleteContactByPatientIdDataRequest request);
        UndoDeleteContactDataResponse UndoDeleteContact(UndoDeleteContactDataRequest request);
        SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request );
        SyncContactInfoDataResponse SyncContactInfo(SyncContactInfoDataRequest request);
        DereferencePatientDataResponse DereferencePatient(DereferencePatientDataRequest request);
        UndoDereferencePatientDataResponse UndoDereferencePatient(UndoDereferencePatientDataRequest request);
    }
}

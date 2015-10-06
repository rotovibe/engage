﻿using Phytel.API.DataDomain.Contact.DTO;
using System;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactDataManager
    {
        GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request);
        ContactData GetContactByPatientId(GetContactByPatientIdDataRequest request);
        ContactData GetContactByUserId(GetContactByUserIdDataRequest request);
        string InsertContact(PutContactDataRequest request);
        SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request);
        UpsertBatchContactDataResponse UpsertContacts(UpsertBatchContactDataRequest request);
        PutUpdateContactDataResponse UpdateContact(PutUpdateContactDataRequest request);
        PutRecentPatientResponse AddRecentPatient(PutRecentPatientRequest request);
        GetContactByContactIdDataResponse GetContactByContactId(GetContactByContactIdDataRequest request);
        DeleteContactByPatientIdDataResponse DeleteContactByPatientId(DeleteContactByPatientIdDataRequest request);
        UndoDeleteContactDataResponse UndoDeleteContact(UndoDeleteContactDataRequest request);
    }
}

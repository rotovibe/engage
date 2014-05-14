﻿using Phytel.API.DataDomain.Contact.DTO;
using System;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactDataManager
    {
        GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request);
        ContactData GetContactByPatientId(GetContactDataRequest request);
        ContactData GetContactByUserId(GetContactByUserIdDataRequest request);
        PutContactDataResponse InsertContact(PutContactDataRequest request);
        SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request);
        PutUpdateContactDataResponse UpdateContact(PutUpdateContactDataRequest request);
        PutRecentPatientResponse AddRecentPatient(PutRecentPatientRequest request);
        ContactData GetContactByContactId(GetContactByContactIdDataRequest request);
    }
}

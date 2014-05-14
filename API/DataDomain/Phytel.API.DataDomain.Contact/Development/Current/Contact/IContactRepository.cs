using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Contact
{
    public interface IContactRepository : IRepository
    {
        object FindContactByPatientId(GetContactByPatientIdDataRequest request);
        IEnumerable<object> FindCareManagers();
        bool UpdateRecentList(PutRecentPatientRequest request, List<string> recentList);
        IEnumerable<object> SearchContacts(SearchContactsDataRequest request);
        object FindContactByUserId(GetContactByUserIdDataRequest request);
    }
}

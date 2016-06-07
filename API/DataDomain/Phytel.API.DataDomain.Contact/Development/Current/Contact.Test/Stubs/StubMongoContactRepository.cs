using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contact.Test.Stubs
{
    public class StubMongoContactRepository : IContactRepository
    {
        public IEnumerable<object> FindCareManagers()
        {
            throw new NotImplementedException();
        }

        public object FindContactByPatientId(DTO.GetContactByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindContactByUserId(DTO.GetContactByUserIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetContactsByContactIds(DTO.GetContactsByContactIdsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRecentList(DTO.PutRecentPatientRequest request, List<string> recentList)
        {
            bool result = true;
            return result;
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            ContactData mC = new ContactData
            {
                UserId = "666656789012345678906666"
            };
            return mC;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public string UserId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public Common.Audit.IAuditHelpers AuditHelpers
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object GetContactByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindContactsWithAPatientInRecentList(string entityId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Select(List<string> ids)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> SearchContacts(SearchContactsDataRequest request)
        {
            throw new NotImplementedException();
        }


        public long GetSearchContactsCount(SearchContactsDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool SyncContact(SyncContactInfoDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool DereferencePatient(DereferencePatientDataRequest request)
        {
            throw new NotImplementedException();
        }


        public bool UnDereferencePatient(UndoDereferencePatientDataRequest request)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ContactData> GetContactsByPatientIds(List<string> patientIds)
        {
            throw new NotImplementedException();
        }
    }
}

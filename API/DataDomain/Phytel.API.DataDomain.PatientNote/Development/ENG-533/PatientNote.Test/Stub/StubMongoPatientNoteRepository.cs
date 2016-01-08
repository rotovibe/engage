using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.Repo;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.Test.Stub
{
    public class StubMongoPatientUtilizationRepository : IMongoPatientNoteRepository
    {
        public IEnumerable<object> FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindNotesWithAProgramId(string entityId)
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

        public object Insert(object newEntity)
        {
            return "123456rtyufghdnfh46f6788";
        }

        public object InsertAll(List<object> entities)
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
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(APIExpression expression)
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

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Select(List<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}

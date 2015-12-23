using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Scheduling;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.DataDomain.ToDo.Test.Stubs
{
    class StubMongoScheduleRepository : ISchedulingRepository
    {
        string userId = "testuser";

        public StubMongoScheduleRepository(string contract)
        {
        }
        
        public GetToDosDataResponse FindToDos(object request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindToDosWithAProgramId(string entityId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID, bool includeDeletedToDo)
        {
            throw new NotImplementedException();
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
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

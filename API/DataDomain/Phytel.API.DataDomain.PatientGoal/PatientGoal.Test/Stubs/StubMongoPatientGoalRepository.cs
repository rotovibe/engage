using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Test.Stubs
{
    public class StubMongoPatientGoalRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private string _userId;
        public StubMongoPatientGoalRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }
        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindGoalsWithAProgramId(string entityId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Search(object request, List<string> patientGoalIds)
        {
            throw new NotImplementedException();
        }

        public object FindByTemplateId(string patientId, string entityID)
        {
            var goalData = new PatientGoalData
            {
                Id = "5329ce4ad6a4850ebc4a7f07",
                PatientId = "5325db9cd6a4850adcbba9ca",
                FocusAreaIds = new List<string> {},
                Name = "test",
                SourceId = "52fa57c9d433231dd0775011",
                ProgramIds = new List<string> {"5325db9cd6a4850adcbba9ca"},
                TypeId = 0,
                StatusId = 1
            };
            return goalData;
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
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
    }
}

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.PatientSystem.Repo;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    class StubPatientSystemRepository : IMongoPatientSystemRepository
    {
        public IEnumerable<object> FindByPatientId(string patientId)
        {
            List<PatientSystemData> dataList = null;
            try
            {
                dataList = new List<PatientSystemData>();
                dataList.Add(new PatientSystemData
                    {
                        Id = "52fa6270d433231dd0775022",
                        PatientId = "5325da76d6a4850adcbba656",
                        SystemId = "121212",
                        DisplayLabel = "ID",
                        SystemName = "Lamar",
                        DeleteFlag = false
                    });
                dataList.Add(new PatientSystemData
                {
                    Id = "532b4748d6a4851308856a31",
                    PatientId = "5325da76d6a4850adcbba656",
                    SystemId = "78956",
                    DisplayLabel = "ID",
                    SystemName = "Maryland",
                    DeleteFlag = false
                });
                return dataList;
            }
            catch (Exception) { throw; }
        }

        string userId = "testuser";

        public StubPatientSystemRepository(string contract)
        {

        }

        public object Insert(object newEntity)
        {
            PutPatientSystemDataRequest request = newEntity as PutPatientSystemDataRequest;
            string patientSystemId = null;
            MEPatientSystem patientSystem = new MEPatientSystem(this.UserId)
            {
                PatientID = ObjectId.Parse(request.PatientID),
                SystemID = request.SystemID,
                DisplayLabel = request.DisplayLabel,
                SystemName = request.SystemName,
                TTLDate = null,
                DeleteFlag = false
            };
            patientSystemId = patientSystem.Id.ToString();
            return patientSystemId;
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
            return true;
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
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


        public IEnumerable<object> Find(object entity)
        {
            throw new NotImplementedException();
        }
    }
}

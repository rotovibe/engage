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
                        Primary = true,
                        StatusId = 1,
                        SystemId = "559e8c70d4332320bc076f4d",
                        DataSource = "Engage",
                        Value = "455679TYU"
                    });
                dataList.Add(new PatientSystemData
                {
                    Id = "532b4748d6a4851308856a31",
                    PatientId = "5325da76d6a4850adcbba656",
                    Primary = false,
                    StatusId = 2,
                    SystemId = "559e8c70d4332320bc076f4d",
                    DataSource = "Engage",
                    Value = "AC567"
                });
                return dataList;
            }
            catch (Exception) { throw; }
        }

        string userId = "testuser";

        public StubPatientSystemRepository(string contract)
        {

        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeletePatientSystemByPatientIdDataRequest request = (DeletePatientSystemByPatientIdDataRequest)entity;
            if (string.IsNullOrEmpty(request.Id))
            { 
                //Update the patientsystem object having _id as request.Id
            }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            PatientSystemData data = null;
            if(!string.IsNullOrEmpty(entityID))
            {

                
            }
            return data;
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


        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            throw new NotImplementedException();
        }


        public List<PatientSystemData> Select(List<string> Ids)
        {
            throw new NotImplementedException();
        }
    }
}

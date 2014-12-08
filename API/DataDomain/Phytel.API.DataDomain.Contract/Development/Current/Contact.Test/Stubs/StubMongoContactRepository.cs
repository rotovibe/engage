using MongoDB.Bson;
using Phytel.API.DataDomain.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contract.Test.Stubs
{
    public class StubMongoContractRepository : IContractRepository
    {
        public IEnumerable<object> FindCareManagers()
        {
            throw new NotImplementedException();
        }

        public object FindContractByPatientId(DTO.GetContractByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindContractByUserId(DTO.GetContractByUserIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SearchContracts(DTO.SearchContractsDataRequest request)
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
            ContractData mC = new ContractData
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

        public object GetContractByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindContractsWithAPatientInRecentList(string entityId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}

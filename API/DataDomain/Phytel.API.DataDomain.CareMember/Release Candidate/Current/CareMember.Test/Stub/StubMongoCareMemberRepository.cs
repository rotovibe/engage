using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.DataDomain.CareMember.Test
{
    public class StubMongoCareMemberRepository : ICareMemberRepository
    {
        public IEnumerable<object> FindByPatientId(string entityId)
        {
            List<CareMemberData> careMembersDataList = new List<CareMemberData>();
            careMembersDataList.Add(new CareMemberData { ContactId = "5325c81f072ef705080d347e", Id = "53271896d6a4850adc518b1f", PatientId = "5325db1ad6a4850adcbba83a", Primary = true, TypeId = "530cd571d433231ed4ba969b" });
            careMembersDataList.Add(new CareMemberData { ContactId = "5325c821072ef705080d3488", Id = "5327282ed6a4850adc518b21", PatientId = "5325db1ad6a4850adcbba83a", Primary = false, TypeId = "530cd571d433231ed4ba969b" });
            careMembersDataList.Add(new CareMemberData { ContactId = "5325c81f072ef705080d347e", Id = "53272f00d6a4850adc518b23", PatientId = "5325db1ad6a4850adcbba83a", Primary = true, TypeId = "530cd576d433231ed4ba969c" });
            return careMembersDataList;
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
           return new CareMemberData { ContactId = "5325c81f072ef705080d347e", Id = "53271896d6a4850adc518b1f", PatientId = "5325db1ad6a4850adcbba83a", Primary = true, TypeId = "530cd571d433231ed4ba969b" };
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

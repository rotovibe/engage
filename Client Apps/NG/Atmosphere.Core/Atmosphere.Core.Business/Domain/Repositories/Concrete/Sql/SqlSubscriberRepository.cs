using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Data;
using C3.Domain.Repositories.Abstract;

namespace C3.Domain.Repositories.Concrete.Sql
{
    public class SqlSubscriberRepository : ISubscriberRepository
    {
        public List<Data.Subscriber> GetSubscribersByGroups(int contractId, List<Data.Group> groups)
        {
            throw new NotImplementedException();
        }

        public List<Data.Subscriber> GetSubscribersByUserAndProduct(int contractId, Guid userid, int groupType)
        {
            throw new NotImplementedException();
        }


        public List<Data.Subscriber> GetSubscribersByUser(int contractId, Guid userid)
        {
            throw new NotImplementedException();
        }

        //public List<Data.Subscriber> GetInsightSubscribersByGroups(int contractId, List<Data.Group> groups)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

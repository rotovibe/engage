using System;
using System.Collections.Generic;
using System.Linq;
using C3.Data;

namespace C3.Domain.Repositories.Abstract
{
    public interface ISubscriberRepository
    {
        List<Subscriber> GetSubscribersByGroups(int contractId, List<Group> groups);
        List<Subscriber> GetSubscribersByUserAndProduct(int contractId, Guid userId,int groupType);
        List<Subscriber> GetSubscribersByUser(int contractId, Guid userId);
        //List<Subscriber> GetInsightSubscribersByGroups(int contractId, List<Group> groups);
    }
}

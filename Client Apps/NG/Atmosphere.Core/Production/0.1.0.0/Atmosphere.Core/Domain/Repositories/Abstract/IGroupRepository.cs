using System.Collections.Generic;
using System.Linq;
using C3.Data;
using System;

namespace C3.Domain.Repositories.Abstract
{
    public interface IGroupRepository
    {
        List<Group> GroupsByContract(int contractId, User user);
        
        List<Group> GetGroupsByUserAndProduct(int contractId, Guid userId , int groupType);

        //List<Group> GetInsightGroupsByUser(int contractId, Guid userId);

        List<Group> GetAllGroupsSubscribersByUserProduct(int contractId, Guid userId, int productId);

        PQRSSubmissionSummaryFilter GetPQRSFilters(int contractId, List<Group> groups);

        List<Group> GetPQRSGroupsSubscribersByUserProduct(int contractId, Guid userId, string year);
    
        
    }
}

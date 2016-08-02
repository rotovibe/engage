using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Text;
using C3.Data;
using C3.Domain.Repositories.Abstract;

namespace C3.Domain.Repositories.Concrete.Sql
{
    public class SqlGroupRepository : IGroupRepository
    {
        #region IGroupRepository Members

        public List<global::C3.Data.Group> GroupsByContract(int contractId, User user)
        {
            //for the purpose of this mock data store, ignore User for now.
            var context = new SqlComparisonDataContext();
            List<Data.Group> groups = (from cg in context.ContractGroups
                                       from g in context.Groups
                                       where cg.GroupId == g.GroupId
                                             && cg.ContractId == contractId
                                       select new Data.Group
                                                  {
                                                      CreateDate = null,
                                                      EnableAll = false,
                                                      GroupId = g.GroupId,
                                                      Name = g.Name
                                                  }).ToList();
            return groups;
        }

        public List<Data.Group> GetGroupsByUserAndProduct(int contractId, Guid userid,int groupType)
        {
            throw new NotImplementedException();
        }

        //public List<Data.Group> GetInsightGroupsByUser(int contractId, Guid userId)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Data.Group> GetAllGroupsSubscribersByUserProduct(int contractId, Guid userId, int productId)
        {
            throw new NotImplementedException();
        }

        // ####### removed from the interface and moved to PQRS
        //// public PQRSSubmissionSummaryFilter GetPQRSFilters(int contractId, List<Data.Group> groups)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //// public List<Data.Group> GetPQRSGroupsSubscribersByUserProduct(int contractId, Guid userId, string year)
        //// {
        ////     throw new NotImplementedException();
        //// }

         #endregion
    }
}

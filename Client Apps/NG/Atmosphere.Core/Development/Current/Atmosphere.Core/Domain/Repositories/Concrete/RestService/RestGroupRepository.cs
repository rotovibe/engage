using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using C3.Domain.Repositories.Abstract;
using C3.Data;


namespace C3.Domain.Repositories.Concrete.RestService
{
    public class RestGroupRepository : BaseRepository, IGroupRepository
    {
        public List<Group> GroupsByContract(int contractId, User user)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}", ContractsServiceBaseAddress, contractId.ToString(), GroupsServiceBaseAddress));
            List<Group> groups = RequestRESTData<List<Group>>(queryString, uri);

            return groups;
        }

        public List<Group> GetGroupsByUserAndProduct(int contractId, Guid userId, int groupType)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}", ContractsServiceBaseAddress, contractId.ToString(), GroupsServiceBaseAddress, userId, groupType));
            List<Group> groups = RequestRESTData<List<Group>>(queryString, uri);

            return groups;
        }

        //public List<Group> GetInsightGroupsByUser(int contractId, Guid userId)
        //{
        //    HttpQueryString queryString = GetDefaultQueryString();
        //    Uri uri = new Uri(string.Format("{0}/{1}/{2}/{3}", ContractsServiceBaseAddress, contractId.ToString(), InsightGroupsServiceBaseAddress, userId));
        //    List<Group> groups = RequestRESTData<List<Group>>(queryString, uri);

        //    return groups;
        //}
        public List<Group> GetAllGroupsSubscribersByUserProduct(int contractId, Guid userId, int productId)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}", ContractsServiceBaseAddress, contractId.ToString(), GroupsSubscribersServiceBaseAddress, userId, productId));
            List<Group> groups = RequestRESTData<List<Group>>(queryString, uri);

            return groups;
        }

        


        public DataTable SchedulesByUserAndGroups(Guid userId, int currentContractId, string groups)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}/{3}",GroupsServiceBaseAddress, "Schedules", currentContractId.ToString(), userId.ToString()));
            DataTable schedules = RequestRESTData<DataTable>(queryString, uri);

            StringBuilder sb = new StringBuilder();

            sb.Append("C3GroupId IN (");
            string groupCsv = string.Join(",", groups.ToArray());
            sb.Append(groupCsv);
            sb.Append(")");
            schedules.DefaultView.RowFilter = sb.ToString();
            return schedules.DefaultView.ToTable();
        }

        public PQRSSubmissionSummaryFilter GetPQRSFilters(int contractId, List<Group> groups)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}", ContractsServiceBaseAddress, contractId.ToString(), PQRSGroupsServiceBaseAddress));
            PQRSSubmissionSummaryFilter filter = PostRESTData<PQRSSubmissionSummaryFilter>(queryString, uri, groups);

            return filter;
        }

        public List<Group> GetPQRSGroupsSubscribersByUserProduct(int contractId, Guid userId, string year)
        {  
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}", ContractsServiceBaseAddress, contractId.ToString(), PQRSGroupsSubscribersServiceBaseAddress, userId, year));
            List<Group> groups = RequestRESTData<List<Group>>(queryString, uri);

            return groups;
        }
    }
}

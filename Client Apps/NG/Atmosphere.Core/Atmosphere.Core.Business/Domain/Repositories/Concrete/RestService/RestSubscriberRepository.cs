using System;
using System.Collections.Generic;
using C3.Data;
using Microsoft.Http;
using C3.Domain.Repositories.Abstract;
using Atmosphere.Core.Data.Services;

namespace C3.Domain.Repositories.Concrete.RestService
{
    public class RestSubscriberRepository : BaseRepository, ISubscriberRepository
    {
        #region ISubscriberRepository Members

        public List<Subscriber> GetSubscribersByGroups(int contractId, List<Group> groups)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = GetServiceRequestUri(ServiceUriFormats.GroupsSubscribers, ServiceUriFormats.Application, new object[] { contractId });

            List<Subscriber> subscribers = PostRESTData<List<Subscriber>>(queryString, uri, groups);
            return subscribers;
        }


        public List<Subscriber> GetSubscribersByUserAndProduct(int contractId, Guid userid, int groupType)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = GetServiceRequestUri(ServiceUriFormats.UserSubscribersPerProduct, ServiceUriFormats.Application, new object[] { contractId, userid, groupType });

            List<Subscriber> subscribers = RequestRESTData<List<Subscriber>>(queryString, uri);
            return subscribers;
        }

        public List<Subscriber> GetSubscribersByUser(int contractId, Guid userid)
        {
            HttpQueryString queryString = GetDefaultQueryString();
            Uri uri = GetServiceRequestUri(ServiceUriFormats.UserSubscribers, ServiceUriFormats.Application, new object[] { contractId, userid });

            List<Subscriber> subscribers = RequestRESTData<List<Subscriber>>(queryString, uri);
            return subscribers;
        }

        //public List<Subscriber> GetInsightSubscribersByGroups(int ContractId, List<Group> groups)
        //{
        //    HttpQueryString queryString = GetDefaultQueryString();
        //    Uri uri = new Uri(string.Format("{0}/{1}/{2}", ContractsServiceBaseAddress, ContractId.ToString(), SubscribersInsightServiceBaseAddress));
        //    List<Subscriber> subscribers = PostRESTData<List<Subscriber>>(queryString, uri, groups);

        //    return subscribers;
        //}


        #endregion
    }
}

using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Phytel.Framework.SQL.Data;
using C3.Data;
using C3.Data.Enum;
using C3.Business.Interfaces;

namespace C3.Business
{
    public class SubscriberService : ServiceBase, ISubscriberService
    {
        private static volatile SubscriberService _instance;
        private static object _syncRoot = new Object();

        //FF: I made this public for Dependency Injection Implementation.
        public SubscriberService()
        {

        }

        public static SubscriberService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if ( _instance == null)
                            _instance = new SubscriberService();
                    }
                }
                return _instance;
            }
        }

        public DataTable GetSubscribersByUser(Contract contract, Guid userId, GroupEntityTypes type)
        {
            return CachedQuery(contract.ConnectionString, contract.Database, StoredProcedure.GetSubscribersByUser, new CacheAccessor("C3Cache", string.Format("Subscribers{0}", userId.ToString())), userId, (int)type);
        }

        public List<Subscriber> GetSubscribersByUserAndType(Contract contract, Guid userId, GroupEntityTypes type)
        {
            return CachedQueryAll<Subscriber>(contract.ConnectionString, contract.Database, StoredProcedure.GetSubscribersByUser, Subscriber.Build, new CacheAccessor("C3Cache", string.Format("Subscribers{0}", userId.ToString())), userId, (int)type);
        }

        public DataTable GetSubscribersByGroup(Contract contract, string groupXML)
        {
            return Query(contract.ConnectionString, contract.Database, StoredProcedure.GetSubscribersByGroup, groupXML);
        }

        public List<Subscriber> GetSubscribersByGroups(Contract contract, string groupXML)
        {
            return QueryAll<Subscriber>(contract.ConnectionString, contract.Database, StoredProcedure.GetSubscribersByGroup, Subscriber.Build, groupXML);
        }

        public Subscriber GetPQRSSubscriberBySubmissionId(Contract contract, int pqrsSubmissionId)
        {
            return Query(contract.ConnectionString, contract.Database, StoredProcedure.GetPQRSSubscriberBySubmissionId, Subscriber.BuildPQRSsubscriber, pqrsSubmissionId);            
        }

        //This is used to pass subscribers to the stored procedures.
        public string GetSubscribersXML(List<Subscriber> susbcribers)
        {
            string _subscribers = "";
            StringBuilder subscriberXML = new StringBuilder();

            //Beginning of XML 
            subscriberXML.Append("<subscribers>");

            List<int> ids = new List<int>();
            foreach (Subscriber subscriber in susbcribers)
            {
                if (!ids.Contains(subscriber.SubscriberId))
                {
                    subscriberXML.AppendFormat("<subscriber>{0}</subscriber>", subscriber.SubscriberId);
                }
                ids.Add(subscriber.SubscriberId);
            }

            subscriberXML.Append("</subscribers>");
            _subscribers = subscriberXML.ToString();
            return _subscribers;
        }
    }
}

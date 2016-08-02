using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Framework.SQL.Data;
using C3.Data;

namespace C3.Business
{
    public class ApplicationMessageService : SqlDataAccessor
    {
        private static volatile ApplicationMessageService _svc = null;
        private static object syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public ApplicationMessageService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public static ApplicationMessageService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new ApplicationMessageService();
                    }
                }

                return _svc;
            }
        }

        public List<ApplicationMessage> GetAll()
        {
            return CachedQueryAll<ApplicationMessage>(null, _dbConnName, StoredProcedure.GetAllApplicationMessages, ApplicationMessage.Build, new CacheAccessor("C3Cache", "ApplicationMessages"));
        }

        public ApplicationMessage GetMessage(string appCode)
        {
            List<ApplicationMessage> messages = GetAll();

            ApplicationMessage message = messages.Find(delegate(ApplicationMessage a) { return a.Code == appCode; });

            if (message == null)
                message = new ApplicationMessage();

            return message;
        }
    }
}

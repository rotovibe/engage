using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Framework.SQL.Data;
using C3.Data;

namespace C3.Business
{
    public class ApplicationSettingService : SqlDataAccessor
    {
        private static volatile ApplicationSettingService _svc = null;
        private static object syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public ApplicationSettingService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public static ApplicationSettingService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new ApplicationSettingService();
                    }
                }

                return _svc;
            }
        }

        public List<ApplicationSetting> GetAll()
        {
            return CachedQueryAll<ApplicationSetting>(null, _dbConnName, StoredProcedure.GetAllApplicationSettings, ApplicationSetting.Build, new CacheAccessor("C3Cache", "ApplicationSettings"));
        }

        public ApplicationSetting GetSetting(string key)
        {
            List<ApplicationSetting> values = GetAll();

            ApplicationSetting value = values.Find(delegate(ApplicationSetting a) { return a.Key == key; });

            return value;
        }
    }
}

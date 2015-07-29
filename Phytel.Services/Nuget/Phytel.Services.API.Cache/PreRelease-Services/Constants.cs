using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.API.Cache
{
    public static class Constants
    {
        public const string ApplicationScopePrefix = "api";

        public static class AppSettingDefaultValues
        {
            public const string ConnectionName = "PhytelCache";
        }

        public static class AppSettingsKeys
        {
            public const string ConnectionName = "Phytel.Services.API.Cache.Mongo.ConnectionName";
        }
    }
}

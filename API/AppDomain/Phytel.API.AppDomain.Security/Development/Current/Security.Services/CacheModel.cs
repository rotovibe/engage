using System.Web;

namespace Phytel.API.AppDomain.Security.Service
{
    public static class CacheModel
    {
        public static string CachedToken
        {
            get
            {
                return (string)HttpRuntime.Cache["ApplicationToken"];
            }

            private set { }
        }

        /// <summary>
        /// Returns a token from cache.If one does not exists or expired, it will create a new one.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="apikey"></param>
        /// <returns></returns>
        public static string GetValidToken(string username, string password, string apikey)
        {
            string _token = string.Empty;

            // check to see if it's expired
            if (HttpRuntime.Cache["ApplicationToken"] != null)
            {
                string _cachedToken = HttpRuntime.Cache["ApplicationToken"] as string;
                if (IsTokenExpired(_cachedToken))
                {
                    _token = GetNewToken(username, password, apikey);
                }
                else
                {
                    CachedToken = _cachedToken;
                }
            }
            else  // create a new one
            {
                // get new token
                _token = GetNewToken(username, password, apikey);
                // register it to cache
                CachedToken = _token;
            }
            return _token;
        }

        public static bool IsTokenExpired(string tempToken)
        {
            bool result = false;
            result = SecurityManager.IsTokenExpired(tempToken);
            return result;
        }

        public static string GetNewToken(string username, string password, string apikey)
        {
            string stringKey = username + "," + password + "," + apikey;
            string token = RSAClass.Encrypt(stringKey);

            // register with the apisession datastore
            return token;
        }
    }
}
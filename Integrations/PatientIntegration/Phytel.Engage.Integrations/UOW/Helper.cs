using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Phytel.Engage.Integrations.UOW
{
    public static class Helper
    {
        public static string BuildURL(string baseURL, string userId)
        {
            string returnURL = baseURL;
            if (returnURL.Contains("?"))
                return string.Format("{0}&UserId={1}", returnURL, userId);
            else
                return string.Format("{0}?UserId={1}", returnURL, userId);
        }

    }

}

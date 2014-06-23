using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;
using System.Web;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        public void LogException(Exception ex)
        {
            string _aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
            Common.Helper.LogException(int.Parse(_aseProcessID), ex);
        }

    }
}

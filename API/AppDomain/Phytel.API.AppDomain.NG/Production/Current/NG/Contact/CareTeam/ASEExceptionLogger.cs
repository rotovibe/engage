using System;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public class ASEExceptionLogger : ILogger
    {
       
        public void Log(Exception ex)
        {
            string _aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
            Common.Helper.LogException(int.Parse(_aseProcessID), ex);
        }
    }
}

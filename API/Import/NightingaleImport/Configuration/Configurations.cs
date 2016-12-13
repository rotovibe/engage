using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightingaleImport.Configuration
{
    public class Configurations
    {
        public static double version
        {
            get { return double.Parse(ConfigurationManager.AppSettings.Get("version")); }
        }

        public static string context
        {
            get { return ConfigurationManager.AppSettings.Get("context"); }
        }

        public static string contractNumber
        {
            get { return ConfigurationManager.AppSettings.Get("contractNumber"); }
        }
        public static string enhancedFeauresContracts
        {
            get { return ConfigurationManager.AppSettings.Get("EnhancedFeauresContracts"); }
        }
    }
}

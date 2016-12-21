using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightingaleImport.Configuration
{
    public class ImportToolConfigurations
    {
        public static double version
        {
            get { return double.Parse(ConfigurationManager.AppSettings.Get("version")); }
        }

        public static string context
        {
            get { return ConfigurationManager.AppSettings.Get("context"); }
        }

        public static string contractNumber { get; set; }
        public static List<string> contracts
        {
            get
            {
                var c = ConfigurationManager.AppSettings.Get("ContractsList");
                var res =c.Split(',').ToList();
                res.Add(" ");
                return res.OrderBy(q=>q).ToList();
            }
        }

        public string enhancedFeaturesContracts
        {
            get { return ConfigurationManager.AppSettings.Get("EnhancedFeaturesContract"); }
        }
    }
}

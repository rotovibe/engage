using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Configurations
{
    public class ApplicableContractProvider : IApplicableContractProvider
    {
        public List<string> Contracts { get; set; }

        public bool Exists(string name)
        {
            var result = false;
            var contract = Contracts.Find(r => r == name);
            if (!string.IsNullOrEmpty(contract))
                result = true;

            return result;
        }

        public ApplicableContractProvider()
        {
            // load contracts
            var names = ConfigurationManager.AppSettings["Contracts"];
            var sNames = names.Split(';');
            var contracts = sNames.ToList();

            Contracts = contracts;
        }
    }
}

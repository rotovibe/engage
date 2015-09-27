using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Phytel.Engage.Integrations.Configurations
{
    public class ApplicableContractProvider : IApplicableContractProvider
    {
        public bool Exists(string name)
        {
            var result = false;

            var contract = GetContracts().Find(r => r == name);
            if (!string.IsNullOrEmpty(contract))
                result = true;

            return result;
        }

        private static List<string> GetContracts()
        {
            // load contracts
            var names = ConfigurationManager.AppSettings["Contracts"];
            var sNames = names.Split(';');
            var contracts = sNames.ToList();

            return contracts;
        }
    }
}

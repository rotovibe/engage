using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.Engage.Integrations.DTO;

namespace Phytel.Engage.Integrations.Configurations
{
    public class ApplicableContractProvider : IApplicableContractProvider
    {
        public bool Exists(string name)
        {
            try
            {
                var result = false;

                var contract = GetContracts().Find(r => r == name);
                if (!string.IsNullOrEmpty(contract))
                    result = true;

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ApplicableContractProvider:Exists()" + ex.Message);
            }
        }

        private static List<string> GetContracts()
        {
            try
            {
                // load contracts
                var names = ProcConstants.Contracts;
                var sNames = names.Split(';');
                var contracts = sNames.ToList();

                return contracts;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ApplicableContractProvider:GetContracts()" + ex.Message);
            }
        }
    }
}

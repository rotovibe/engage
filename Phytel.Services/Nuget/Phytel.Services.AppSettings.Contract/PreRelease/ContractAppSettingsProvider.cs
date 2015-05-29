using Phytel.API.DataDomain.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.AppSettings.Contract
{
    public class ContractAppSettingsProvider : AppSettingsProvider
    {
        protected readonly IContractClient _contractClient;
        protected readonly string _contractNumber;
        protected readonly string _context;

        public ContractAppSettingsProvider(IContractClient contractClient, string contractNumber, string context)
            :base()
        {
            _contractClient = contractClient;
            _contractNumber = contractNumber;
            _context = context ?? "Unknown";
        }

        protected override string OnGetValueFromSource(string key)
        {
            string rvalue = null;

            try
            {
                if(_contractClient != null && !string.IsNullOrEmpty(_contractNumber))
                {
                    ContractProperty contractProperty = _contractClient.GetContractProperty(_contractNumber, key, _context);
                    if (contractProperty != null)
                    {
                        rvalue = contractProperty.Value;
                    }
                }                
            }
            catch
            {
                //TODO: Log exception
            }

            return rvalue;
        }
    }
}

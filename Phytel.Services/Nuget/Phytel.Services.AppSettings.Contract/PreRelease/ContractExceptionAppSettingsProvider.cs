using Phytel.API.DataDomain.Contract.DTO;

namespace Phytel.Services.AppSettings.Contract
{
    public class ContractExceptionAppSettingsProvider : AppSettingsProvider
    {
        protected readonly string _context;
        protected readonly IContractClient _contractClient;
        protected readonly string _contractNumber;

        public ContractExceptionAppSettingsProvider(IContractClient contractClient, string contractNumber, string context)
            : base()
        {
            _contractClient = contractClient;
            _contractNumber = contractNumber;
            _context = context ?? "Unknown";
        }

        protected override string OnGetValueFromSource(string key)
        {
            string rvalue = null;

            ContractProperty contractProperty = _contractClient.GetContractProperty(_contractNumber, key, _context);
            if (contractProperty != null)
            {
                rvalue = contractProperty.Value;
            }

            return rvalue;
        }
    }
}
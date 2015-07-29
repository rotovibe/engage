using Phytel.API.DataDomain.Contract.DTO;

namespace Phytel.Services.AppSettings.Contract
{
    public class ContractAppSettingsProvider : ContractExceptionAppSettingsProvider
    {
        public ContractAppSettingsProvider(IContractClient contractClient, string contractNumber, string context)
            : base(contractClient, contractNumber, context)
        {
        }

        protected override string OnGetValueFromSource(string key)
        {
            string rvalue = null;

            try
            {
                if (_contractClient != null && !string.IsNullOrEmpty(_contractNumber))
                {
                    rvalue = base.OnGetValueFromSource(key);
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
using Phytel.Services.API.DTO;

namespace Phytel.Services.API.Provider
{
    public class ContractNumberProvider : IContractNumberProvider
    {
        public string Get(object requestDto)
        {
            string rvalue = string.Empty;
            if (requestDto is IContractRequest)
            {
                IContractRequest contractRequest = requestDto as IContractRequest;
                rvalue = Get(contractRequest);
            }

            return rvalue;
        }

        public string Get(IContractRequest contractRequest)
        {
            string rvalue = string.Empty;

            if (contractRequest != null)
            {
                rvalue = contractRequest.ContractNumber;
            }

            return rvalue;
        }
    }
}
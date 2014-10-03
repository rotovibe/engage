using Phytel.Services.ServiceStack.DTO;

namespace Phytel.Services.ServiceStack.Provider
{
    public interface IContractNumberProvider
    {
        string Get(IContractRequest contractRequest);

        string Get(object requestDto);
    }
}
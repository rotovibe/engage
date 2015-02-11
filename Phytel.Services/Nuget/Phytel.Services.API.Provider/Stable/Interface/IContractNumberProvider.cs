using Phytel.Services.API.DTO;

namespace Phytel.Services.API.Provider
{
    public interface IContractNumberProvider
    {
        string Get(IContractRequest contractRequest);

        string Get(object requestDto);
    }
}
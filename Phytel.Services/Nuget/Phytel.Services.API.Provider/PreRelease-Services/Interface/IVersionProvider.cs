using Phytel.Services.API.DTO;

namespace Phytel.Services.API.Provider
{
    public interface IVersionProvider
    {
        double Get(IVersionRequest versionRequest);

        double Get(object requestDto);
    }
}
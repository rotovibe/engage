using Phytel.Services.ServiceStack.DTO;

namespace Phytel.Services.ServiceStack.Provider
{
    public interface IVersionProvider
    {
        double Get(IVersionRequest versionRequest);

        double Get(object requestDto);
    }
}
using Phytel.Services.ServiceStack.DTO;

namespace Phytel.Services.ServiceStack.Provider
{
    public class VersionProvider : IVersionProvider
    {
        public double Get(object requestDto)
        {
            double rvalue = 1;
            if (requestDto is IVersionRequest)
            {
                IVersionRequest versionRequest = requestDto as IVersionRequest;
                rvalue = Get(versionRequest);
            }

            return rvalue;
        }

        public double Get(IVersionRequest versionRequest)
        {
            double rvalue = 1;

            if (versionRequest != null)
            {
                rvalue = versionRequest.Version;
            }

            return rvalue;
        }
    }
}
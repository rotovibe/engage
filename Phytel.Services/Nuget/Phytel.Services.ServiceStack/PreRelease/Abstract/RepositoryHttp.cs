using ServiceStack.ServiceClient.Web;

namespace Phytel.Services.ServiceStack
{
    public class RepositoryHttp : IRepositoryHttp
    {
        protected readonly JsonServiceClient _client;
        public const int DefaultTimeoutInSeconds = 60;

        public RepositoryHttp(string baseUri, int timeoutInSeconds = DefaultTimeoutInSeconds)
        {
            _client = new JsonServiceClient(baseUri);
            _client.Timeout = new System.TimeSpan(0, 0, timeoutInSeconds);
        }

        public TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams)
            where TResponse : class, new()
        {
            TResponse rvalue = new TResponse();

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            rvalue = _client.Get<TResponse>(relativeOrAbsoluteUrl);

            return rvalue;
        }

        public TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams)
        {
            return Post<TResponse>(null, relativeUrlFormat, relativeUrlParams);
        }

        public TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Delete<TResponse>(relativeOrAbsoluteUrl);
        }
    }
}
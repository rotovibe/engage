using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Specialized;

namespace Phytel.Services.ServiceStack.Client
{
    public class RepositoryHttp : IRepositoryHttp
    {
        public const int DefaultTimeoutInSeconds = 60;
        protected readonly JsonServiceClient _client;

        public RepositoryHttp(string baseUri, int timeoutInSeconds = DefaultTimeoutInSeconds)
        {
            _client = new JsonServiceClient(baseUri);
            _client.Timeout = new System.TimeSpan(0, 0, timeoutInSeconds);
        }

        public TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Delete<TResponse>(relativeOrAbsoluteUrl);
        }

        public TResponse Delete<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);

            return _client.Delete<TResponse>(request);
        }

        public TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams)
            where TResponse : class, new()
        {
            TResponse rvalue = new TResponse();

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            rvalue = _client.Get<TResponse>(relativeOrAbsoluteUrl);

            return rvalue;
        }

        public TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, params string[] relativeUrlParams)
            where TResponse : class, new()
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            TResponse rvalue = new TResponse();

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            rvalue = _client.Get<TResponse>(relativeOrAbsoluteUrl);

            return rvalue;
        }

        public TResponse Get<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);

            return _client.Get<TResponse>(request);
        }

        public TResponse Patch<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);

            return _client.Patch<TResponse>(request);
        }

        public TResponse Patch<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Patch<TResponse>(relativeOrAbsoluteUrl, request);
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

        public TResponse Post<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);

            return _client.Post<TResponse>(request);
        }

        public TResponse Put<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);

            return _client.Put<TResponse>(request);
        }

        public TResponse Put<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Put<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Put<TResponse>(relativeOrAbsoluteUrl, request);
        }

        protected virtual IReturn<TResponse> OnExecuteConvertToIReturn<TResponse>(object requestDto)
        {
            IReturn<TResponse> rvalue = null;

            if (requestDto is IReturn<TResponse>)
            {
                rvalue = requestDto as IReturn<TResponse>;
            }

            if (rvalue == null)
            {
                throw new ArgumentException("Provided request dto was not of type " + typeof(IReturn<TResponse>).Name);
            }

            return rvalue;
        }

        public TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }
    }
}
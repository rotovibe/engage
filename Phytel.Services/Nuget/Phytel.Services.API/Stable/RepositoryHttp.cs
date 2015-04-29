﻿using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Specialized;

namespace Phytel.Services.API
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

        public void Delete(object requestDto)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Delete(request);
        }

        public void DeleteAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.DeleteAsync<TResponse>(request, onSuccess, onError);
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

        public void GetAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.GetAsync<TResponse>(request, onSuccess, onError);
        }

        public TResponse Patch<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            return _client.Patch<TResponse>(request);
        }

        public void PatchAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.PatchAsync<TResponse>(request, onSuccess, onError);
        }

        public void Patch(object requestDto)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Patch(request);
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

        public void Post(object requestDto)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Post(request);
        }

        public TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public void PostAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.PostAsync<TResponse>(request, onSuccess, onError);
        }

        public TResponse Put<TResponse>(object requestDto)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            return _client.Put<TResponse>(request);
        }

        public void PutAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.PutAsync<TResponse>(request, onSuccess, onError);
        }

        public void Put(object requestDto)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Put(request);
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

        public TResponse Delete<TResponse>(string relativeUrlFormat, TimeSpan timeout, params object[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Delete<TResponse>(relativeOrAbsoluteUrl);
        }

        public TResponse Delete<TResponse>(object requestDto, TimeSpan timeout)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Delete<TResponse>(request);
        }

        public void Delete(object requestDto, TimeSpan timeout)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Delete(request);
        }

        public TResponse Get<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams) 
            where TResponse : class, new()
        {
            TResponse rvalue = new TResponse();

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            rvalue = _client.Get<TResponse>(relativeOrAbsoluteUrl);

            return rvalue;
        }

        public TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams) 
            where TResponse : class, new()
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            TResponse rvalue = new TResponse();

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            rvalue = _client.Get<TResponse>(relativeOrAbsoluteUrl);

            return rvalue;
        }

        public TResponse Get<TResponse>(object requestDto, TimeSpan timeout)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Get<TResponse>(request);
        }

        public TResponse Patch<TResponse>(object requestDto, TimeSpan timeout)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Patch<TResponse>(request);
        }

        public void Patch(object requestDto, TimeSpan timeout)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Patch(request);
        }

        public TResponse Patch<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Patch<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Post<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            return Post<TResponse>(null, relativeUrlFormat, timeout, relativeUrlParams);
        }

        public void Post(object requestDto, TimeSpan timeout)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Post(request);
        }

        public TResponse Post<TResponse>(object requestDto, TimeSpan timeout)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Post<TResponse>(request);
        }

        public TResponse Post<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Put<TResponse>(object requestDto, TimeSpan timeout)
        {
            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Put<TResponse>(request);
        }

        public void Put(object requestDto, TimeSpan timeout)
        {
            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Put(request);
        }

        public TResponse Put<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Put<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Put<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Get<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Get<TResponse>(request);
        }

        public TResponse Patch<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Patch<TResponse>(request);
        }

        public void Patch(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Patch(request);
        }

        public TResponse Patch<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Patch<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Post<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams)
        {
            return Post<TResponse>(null, relativeUrlFormat, headers, timeout, relativeUrlParams);
        }

        public void Post(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Post(request);
        }

        public TResponse Post<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Post<TResponse>(request);
        }

        public TResponse Post<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
            return _client.Post<TResponse>(relativeOrAbsoluteUrl, request);
        }

        public TResponse Put<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturn<TResponse> request = OnExecuteConvertToIReturn<TResponse>(requestDto);
            _client.Timeout = timeout;
            return _client.Put<TResponse>(request);
        }

        public void Put(object requestDto, NameValueCollection headers, TimeSpan timeout)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            IReturnVoid request = OnExecuteConvertToIReturnVoid(requestDto);
            _client.Timeout = timeout;
            _client.Put(request);
        }

        public TResponse Put<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams)
        {
            _client.LocalHttpWebRequestFilter = x => x.Headers.Add(headers);

            string relativeOrAbsoluteUrl = string.Format(relativeUrlFormat, relativeUrlParams);
            _client.Timeout = timeout;
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

        protected virtual IReturnVoid OnExecuteConvertToIReturnVoid(object requestDto)
        {
            IReturnVoid rvalue = null;

            if (requestDto is IReturnVoid)
            {
                rvalue = requestDto as IReturnVoid;
            }

            if (rvalue == null)
            {
                throw new ArgumentException("Provided request dto was not of type " + typeof(IReturnVoid).Name);
            }

            return rvalue;
        }
    }
}
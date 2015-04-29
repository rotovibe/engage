using System;
using System.Collections.Specialized;

namespace Phytel.Services.API
{
    public interface IRepositoryHttp
    {
        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto);

        void Delete(object requestDto);

        void DeleteAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto);

        void GetAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Patch<TResponse>(object requestDto);

        void Patch(object requestDto);

        void PatchAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams);

        void Post(object requestDto);

        void PostAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Post<TResponse>(object requestDto);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object requestDto);

        void Put(object requestDto);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);

        void PutAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Delete<TResponse>(string relativeUrlFormat, TimeSpan timeout, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto, TimeSpan timeout);

        void Delete(object requestDto, TimeSpan timeout);

        TResponse Get<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto, TimeSpan timeout);

        TResponse Patch<TResponse>(object requestDto, TimeSpan timeout);

        void Patch(object requestDto, TimeSpan timeout);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Post<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        void Post(object requestDto, TimeSpan timeout);

        TResponse Post<TResponse>(object requestDto, TimeSpan timeout);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object requestDto, TimeSpan timeout);

        void Put(object requestDto, TimeSpan timeout);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Get<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Patch<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        void Patch(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Post<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        void Post(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Post<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        void Put(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);
    }
}
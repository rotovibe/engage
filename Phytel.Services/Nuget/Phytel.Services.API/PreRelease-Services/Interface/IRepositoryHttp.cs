using System;
using System.Collections.Specialized;

namespace Phytel.Services.API
{
    public interface IRepositoryHttp
    {
        #region Delete Methods
        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto);

        void Delete(object requestDto);

        void DeleteAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Delete<TResponse>(string relativeUrlFormat, TimeSpan timeout, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto, TimeSpan timeout);

        void Delete(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Delete<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        #endregion

        #region Get Methods
        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto);

        void GetAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Get<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto, TimeSpan timeout);

        TResponse Get<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        #endregion

        #region Patch Methods
        TResponse Patch<TResponse>(object requestDto);

        void Patch(object requestDto);

        void PatchAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Patch<TResponse>(object requestDto, TimeSpan timeout);

        void Patch(object requestDto, TimeSpan timeout);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Patch<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        void Patch(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        #endregion

        #region Post Methods
        TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams);

        void Post(object requestDto);

        void PostAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Post<TResponse>(object requestDto);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        void Post(object requestDto, TimeSpan timeout);

        TResponse Post<TResponse>(object requestDto, TimeSpan timeout);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Post<TResponse>(string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        void Post(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Post<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);

        #endregion

        #region Put Methods
        TResponse Put<TResponse>(object requestDto);

        void Put(object requestDto);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);

        void PutAsync<TResponse>(object requestDto, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);

        TResponse Put<TResponse>(object requestDto, TimeSpan timeout);

        void Put(object requestDto, TimeSpan timeout);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, TimeSpan timeout, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object requestDto, NameValueCollection headers, TimeSpan timeout);

        void Put(object requestDto, NameValueCollection headers, TimeSpan timeout);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, NameValueCollection headers, TimeSpan timeout, params string[] relativeUrlParams);
        #endregion
    }
}
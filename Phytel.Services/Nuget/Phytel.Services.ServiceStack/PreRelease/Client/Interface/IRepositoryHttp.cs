using System;
using System.Collections.Specialized;

namespace Phytel.Services.ServiceStack.Client
{
    public interface IRepositoryHttp
    {
        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto);

        void Delete(object requestDto);

        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(string relativeUrlFormat, NameValueCollection headers, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto);

        TResponse Patch<TResponse>(object requestDto);

        void Patch(object requestDto);

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
    }
}
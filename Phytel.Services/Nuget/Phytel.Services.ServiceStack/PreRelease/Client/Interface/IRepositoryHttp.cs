using System.Collections.Specialized;

namespace Phytel.Services.ServiceStack.Client
{
    public interface IRepositoryHttp
    {
        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);

        TResponse Delete<TResponse>(object requestDto);

        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Get<TResponse>(object requestDto);

        TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Patch<TResponse>(object requestDto);

        TResponse Patch<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object requestDto);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Put<TResponse>(object requestDto);

        TResponse Put<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);
    }
}
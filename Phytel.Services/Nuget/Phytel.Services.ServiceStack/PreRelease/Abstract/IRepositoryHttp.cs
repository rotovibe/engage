namespace Phytel.Services.ServiceStack
{
    public interface IRepositoryHttp
    {
        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);
    }
}
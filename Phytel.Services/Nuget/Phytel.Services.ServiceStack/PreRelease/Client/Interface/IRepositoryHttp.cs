﻿using System.Collections.Specialized;

namespace Phytel.Services.ServiceStack.Client
{
    public interface IRepositoryHttp
    {
        TResponse Delete<TResponse>(string relativeUrlFormat, params object[] relativeUrlParams);

        TResponse Get<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams) where TResponse : class, new();

        TResponse Post<TResponse>(string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, string relativeUrlFormat, params string[] relativeUrlParams);

        TResponse Post<TResponse>(object request, NameValueCollection headers, string relativeUrlFormat, params string[] relativeUrlParams);
    }
}
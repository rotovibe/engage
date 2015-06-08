using ServiceStack.ServiceClient.Web;
using System;
using System.Net;
using System.Threading;

namespace Phytel.Services.API
{
    public static class RepositoryHttpExtend
    {
        public static TResponse Delete<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry(repositoryHttp.Delete<TResponse>, request, retries, retryInterval);
        }

        public static TResponse DoWithRetry<TResponse>(Func<object, TResponse> func, object request, int retries, int retryInterval)
        {
            int retryCounter = 1;

            while (retryCounter <= retries)
            {
                try
                {
                    return func(request);
                }
                catch (WebServiceException ex)
                {
                    //If this is a client error (bad url, unauthorized, etc) do not retry
                    if (ex.StatusCode >= (int)HttpStatusCode.BadRequest && ex.StatusCode < (int)HttpStatusCode.InternalServerError)
                    {
                        throw ex;
                    }
                    else if (retryCounter == retries)
                    {
                        throw ex;
                    }

                    retryCounter++;
                    Thread.Sleep(retryInterval);
                }
                catch (Exception ex)
                {
                    if (retryCounter == retries)
                    {
                        throw ex;
                    }

                    retryCounter++;
                    Thread.Sleep(retryInterval);
                }
            }

            throw new ApplicationException("RetryException");
        }

        public static TResponse Get<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry(repositoryHttp.Get<TResponse>, request, retries, retryInterval);
        }

        public static TResponse Patch<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry(repositoryHttp.Patch<TResponse>, request, retries, retryInterval);
        }

        public static TResponse Post<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry(repositoryHttp.Post<TResponse>, request, retries, retryInterval);
        }

        public static TResponse Put<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry(repositoryHttp.Put<TResponse>, request, retries, retryInterval);
        }
    }
}
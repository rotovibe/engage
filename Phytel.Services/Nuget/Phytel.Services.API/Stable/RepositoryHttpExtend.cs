using ServiceStack.ServiceClient.Web;
using System;
using System.Net;
using System.Threading;

namespace Phytel.Services.API
{
    [Serializable]
    public static class RepositoryHttpExtend
    {
        public static TResponse Delete<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry<TResponse>(repositoryHttp.Delete<TResponse>, request, retries, retryInterval);
        }

        public static void Delete(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            DoWithRetry(repositoryHttp.Delete, request, retries, retryInterval);
        }

        public static void DoWithRetry(Action<object> func, object request, int retries, int retryInterval)
        {
            int retryCounter = 1;

            while (retryCounter <= retries)
            {
                try
                {
                    func(request);
                    retryCounter += retries;
                    break;
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
        }
        
        public static TResponse DoWithRetry<TResponse>(Func<object, TResponse> func, object request, int retries, int retryInterval)
        {
            int retryCounter = 1;
            TResponse rvalue = default(TResponse);

            while (retryCounter <= retries)
            {
                try
                {
                    rvalue = func(request);
                    retryCounter += retries;
                    break;
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

            return rvalue;
        }

        public static TResponse Get<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry<TResponse>(repositoryHttp.Get<TResponse>, request, retries, retryInterval);
        }

        public static TResponse Patch<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry<TResponse>(repositoryHttp.Patch<TResponse>, request, retries, retryInterval);
        }

        public static void Patch(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            DoWithRetry(repositoryHttp.Patch, request, retries, retryInterval);
        }

        public static TResponse Post<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry<TResponse>(repositoryHttp.Post<TResponse>, request, retries, retryInterval);
        }

        public static void Post(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            DoWithRetry(repositoryHttp.Post, request, retries, retryInterval);
        }

        public static TResponse Put<TResponse>(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            return DoWithRetry<TResponse>(repositoryHttp.Put<TResponse>, request, retries, retryInterval);
        }

        public static void Put(this IRepositoryHttp repositoryHttp, object request, int retries, int retryInterval)
        {
            DoWithRetry(repositoryHttp.Put, request, retries, retryInterval);
        }
    }
}
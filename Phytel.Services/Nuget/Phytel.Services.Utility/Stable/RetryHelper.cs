using System;
using System.Threading;

namespace Phytel.Services.Utility
{
    public static class RetryHelper
    {
        public const int RETRIES = 2;
        public const int RETRYDELAY = 150;

        public static void DoWithRetry(Action action, int retries, int delayBetweenRetries = 0)
        {
            int retryCounter = 1;

            while (retryCounter <= retries)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                    if (retryCounter == retries)
                    {
                        throw;
                    }

                    retryCounter++;
                    Thread.Sleep(delayBetweenRetries);
                }
            }
        }

        public static T DoWithRetry<T>(Func<T> func, int retries, int delayBetweenRetries = 0)
        {
            int retryCounter = 1;

            while (retryCounter <= retries)
            {
                try
                {
                    return func();
                }
                catch (Exception)
                {
                    if (retryCounter == retries)
                    {
                        throw;
                    }

                    retryCounter++;
                    Thread.Sleep(delayBetweenRetries);
                }
            }

            throw new ApplicationException("RetryException");
        }

        public static TResult DoWithRetry<T, TResult>(Func<T, TResult> func, T methodParam, int retries, int delayBetweenRetries = 0)
        {
            int retryCounter = 1;

            while (retryCounter <= retries)
            {
                try
                {
                    return func(methodParam);
                }
                catch (Exception)
                {
                    if (retryCounter == retries)
                    {
                        throw;
                    }

                    retryCounter++;
                    Thread.Sleep(delayBetweenRetries);
                }
            }

            throw new ApplicationException("RetryException");
        }        
    }
}

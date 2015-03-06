using ServiceStack.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubJsonRestClient : IRestClient
    {
        public TResponse CustomMethod<TResponse>(string httpVerb, ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public void CustomMethod(string httpVerb, ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Delete<TResponse>(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        public void Delete(ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Delete<TResponse>(ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public TResponse Get<TResponse>(string relativeOrAbsoluteUrl)
        {
            var instance = Activator.CreateInstance(typeof(TResponse));
            return (TResponse)instance;
        }

        public void Get(ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Get<TResponse>(ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public System.Net.HttpWebResponse Head(string relativeOrAbsoluteUrl)
        {
            throw new NotImplementedException();
        }

        public System.Net.HttpWebResponse Head(ServiceStack.ServiceHost.IReturn request)
        {
            throw new NotImplementedException();
        }

        public TResponse Patch<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public void Patch(ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Patch<TResponse>(ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public void Post(ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Post<TResponse>(ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }

        public TResponse PostFile<TResponse>(string relativeOrAbsoluteUrl, System.IO.FileInfo fileToUpload, string mimeType)
        {
            throw new NotImplementedException();
        }

        public TResponse Put<TResponse>(string relativeOrAbsoluteUrl, object request)
        {
            throw new NotImplementedException();
        }

        public void Put(ServiceStack.ServiceHost.IReturnVoid request)
        {
            throw new NotImplementedException();
        }

        public TResponse Put<TResponse>(ServiceStack.ServiceHost.IReturn<TResponse> request)
        {
            throw new NotImplementedException();
        }
    }
}

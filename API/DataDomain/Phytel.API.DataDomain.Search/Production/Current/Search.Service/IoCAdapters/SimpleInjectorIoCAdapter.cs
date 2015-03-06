using ServiceStack.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phytel.API.DataDomain.Search.Service.IoCAdapters
{
    public class SimpleInjectorIoCAdapter : IContainerAdapter
    {
        private readonly Container container;

        public SimpleInjectorIoCAdapter(Container container)
        {
            this.container = container;
        }

        public T Resolve<T>()
        {
            return (T)this.container.GetInstance(typeof(T));
        }

        public T TryResolve<T>()
        {
            IServiceProvider provider = this.container;
            object service = provider.GetService(typeof(T));
            return service != null ? (T)service : default(T);
        }
    }
}
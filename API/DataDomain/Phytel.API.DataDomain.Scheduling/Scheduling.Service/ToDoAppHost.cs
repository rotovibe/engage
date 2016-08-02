using System.Reflection;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.ToDo.Service
{
    public class ToDoAppHost : AppHostBase
    {
        public ToDoAppHost() : base("Phytel ToDo Data Domain Services", Assembly.GetExecutingAssembly()) { }

        public override void Configure(Funq.Container container)
        {
            //register any dependencies your services use, e.g:
            //container.Register<ICacheClient>(new MemoryCacheClient());
            Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

            DependencyConfig.Config(container);
        }
    }
}
using Phytel.API.DataDomain.ToDo.MongoDB.Repository;
using Phytel.API.DataDomain.ToDo.Service.ContextProxy;

namespace Phytel.API.DataDomain.ToDo.Service
{
    public static class DependencyConfig
    {
        public static void Config(Funq.Container container)
        {
            container.RegisterAutoWiredAs<ToDoDataManager, IToDoDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.Register<IHostContextProxy>(new HostContextProxy());

            container.Register<string>(RepositoryContainer.NamedUnitOfWorkToDo, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            RepositoryContainer.Configure(container);
        }
    }
}
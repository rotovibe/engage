using Phytel.API.DataDomain.Medication.MongoDB.Repository;
using Phytel.API.DataDomain.Medication.Service.ContextProxy;

namespace Phytel.API.DataDomain.Medication.Service
{
    public static class DependencyConfig
    {
        public static void Config(Funq.Container container)
        {
            container.RegisterAutoWiredAs<MedicationDataManager, IMedicationDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.Register<IHostContextProxy>(new HostContextProxy());

            container.Register<string>(RepositoryContainer.NamedUnitOfWorkMedication, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            RepositoryContainer.Configure(container);
        }
    }
}
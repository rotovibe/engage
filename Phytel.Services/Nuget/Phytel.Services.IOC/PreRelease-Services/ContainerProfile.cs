namespace Phytel.Services.IOC
{
    public abstract class ContainerProfile : IContainerProfile
    {
        public abstract ContainerBuilder GetBuilder(Funq.Container container = null);

        public virtual Funq.Container GetContainer(Funq.Container container = null)
        {
            ContainerBuilder containerBuilder = GetBuilder(container);
            return containerBuilder.Build();
        }
    }
}
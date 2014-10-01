namespace Phytel.Services.IOC
{
    public interface IContainerProfile
    {
        ContainerBuilder GetBuilder(Funq.Container container = null);

        Funq.Container GetContainer(Funq.Container container = null);
    }
}
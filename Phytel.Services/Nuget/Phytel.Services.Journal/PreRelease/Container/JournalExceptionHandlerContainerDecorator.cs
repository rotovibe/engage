using AutoMapper;
using Phytel.Services.IOC;
using Phytel.Services.Journal.Dispatch;

namespace Phytel.Services.Journal
{
    public class JournalExceptionHandlerContainerDecorator : ContainerDecorator
    {
        public JournalExceptionHandlerContainerDecorator(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<IJournalExceptionHandler>(c =>
                new JournalExceptionHandler(
                    c.Resolve<IJournalDispatcher>()
                    )
                );
            container.Register<IJournalServiceExceptionHandler>(c =>
                new JournalServiceExceptionHandler(
                    c.Resolve<IJournalDispatcher>(),
                    c.Resolve<IMappingEngine>()
                ));
        }
    }
}
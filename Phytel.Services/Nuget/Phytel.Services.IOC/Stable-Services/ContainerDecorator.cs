using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.IOC
{
    public abstract class ContainerDecorator : ContainerBuilder
    {
        protected ContainerBuilder _containerBuilder;

        public ContainerDecorator(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public override Funq.Container Build()
        {
            Funq.Container container = _containerBuilder.Build();

            OnBuild(container);

            return container;
        }

        protected abstract void OnBuild(Funq.Container container);
    }
}

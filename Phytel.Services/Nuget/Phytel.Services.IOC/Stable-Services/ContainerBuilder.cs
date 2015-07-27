using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.IOC
{
    public abstract class ContainerBuilder
    {
        protected readonly Funq.Container _container;

        public ContainerBuilder(Funq.Container container = null)
        {
            if (container == null)
            {
                container = new Funq.Container();
            }

            _container = container;
        }

        public abstract Funq.Container Build();
    }
}

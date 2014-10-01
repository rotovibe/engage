using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.IOC
{
    public static class ContainerBuilderExtend
    {
        public static ContainerBuilder DecorateWith<T>(this ContainerBuilder containerBuilder, params object[] constructorArgs)
            where T : ContainerDecorator
        {
            List<object> args = new List<object>();
            args.Add(containerBuilder);
            foreach (object constructorArg in constructorArgs)
            {
                if (constructorArg.GetType().IsSubclassOf(typeof(ContainerBuilder)))
                {
                }
                else
                {
                    args.Add(constructorArg);
                }
            }

            return System.Activator.CreateInstance(typeof(T), args.ToArray()) as T;
        }

        public static ContainerBuilder DecorateWith(this ContainerBuilder containerBuilder, Func<ContainerBuilder, ContainerDecorator> factory)
        {
            return factory(containerBuilder);
        }
    }
}

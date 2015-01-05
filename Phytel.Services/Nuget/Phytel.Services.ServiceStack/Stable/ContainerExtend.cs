using Funq;
using System;

namespace Phytel.Services.ServiceStack
{
    public static class ContainerExtend
    {
        public static void RegisterAutoWiredAsIfNotAlready<T, TAs>(this Container container, ReuseScope scope = ReuseScope.Default)
            where T : TAs
        {
            if (container.TryResolve<T>() == null)
            {
                container.RegisterAutoWiredAs<T, TAs>().ReusedWithin(scope);
            }
        }

        public static void RegisterIfNotAlready<T>(this Container container, Func<Container, T> factory, ReuseScope scope = ReuseScope.Default)
        {
            if (container.TryResolve<T>() == null)
            {
                container.Register<T>(factory).ReusedWithin(scope);
            }
        }
    }
}
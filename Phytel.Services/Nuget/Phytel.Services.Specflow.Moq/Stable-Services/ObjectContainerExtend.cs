using BoDi;
using Moq;

namespace Phytel.Services.Specflow.Moq
{
    public static class ObjectContainerExtend
    {
        public static void RegisterMock<T>(this IObjectContainer objectContainer)
            where T : class
        {
            objectContainer.RegisterInstanceAs<Mock<T>>(new Mock<T>());
            objectContainer.RegisterInstanceAs<T>(objectContainer.Resolve<Mock<T>>().Object);
        }
    }
}
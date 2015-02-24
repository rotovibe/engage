using System.Threading.Tasks;
namespace Phytel.Services.Dispatch
{
    public interface IDispatcher
    {
        void Dispatch(object message);

        Task DispatchAsync(object message);
    }
}
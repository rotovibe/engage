namespace Phytel.Services.Dispatch
{
    public interface IDispatcher
    {
        void Dispatch(object message);
    }
}
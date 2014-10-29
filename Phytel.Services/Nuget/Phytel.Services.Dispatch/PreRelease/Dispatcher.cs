using Phytel.Services.Serializer;

namespace Phytel.Services.Dispatch
{
    public abstract class Dispatcher : IDispatcher
    {
        protected readonly ISerializer _serializer;

        public Dispatcher(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Dispatch(object message)
        {
            string messageAsString = _serializer.Serialize(message, message.GetType());
            OnDispatch(messageAsString);
        }

        protected abstract void OnDispatch(string message);
    }
}
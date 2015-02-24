using Phytel.Services.Serializer;
using System.Threading;
using System.Threading.Tasks;

namespace Phytel.Services.Dispatch
{
    public abstract class Dispatcher : IDispatcher
    {
        protected readonly ISerializer _serializer;
        public const int MaxDispatchingThreads = 20;

        protected int _dispatchingThreads;

        public Dispatcher(ISerializer serializer)
        {
            _serializer = serializer;
            _dispatchingThreads = 0;
        }

        public void Dispatch(object message)
        {
            string messageAsString = _serializer.Serialize(message, message.GetType());
            OnDispatch(messageAsString);
        }


        public Task DispatchAsync(object message)
        {
            return Task.Run(() => 
            {
                bool dispatched = false;

                while(!dispatched)
                {
                    if(_dispatchingThreads < MaxDispatchingThreads)
                    {
                        string messageAsString = _serializer.Serialize(message, message.GetType());
                        OnDispatch(messageAsString);
                        _dispatchingThreads++;
                        dispatched = true;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }                
            })
            .ContinueWith(task => 
            {
                _dispatchingThreads--;
            });
        }

        protected abstract void OnDispatch(string message);
    }
}
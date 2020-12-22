using System;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Contracts.Events
{
    public class PublisherSingleton
    {
        private static Lazy<PublisherSingleton> instance = new Lazy<PublisherSingleton>(() => new PublisherSingleton());

        private event Func<object, ReviewCreatedEventArgs, Task> _reviewCreatedEvent;

        public event Func<object, ReviewCreatedEventArgs, Task> ReviewCreatedEvent
        {
            add
            {
                if (_reviewCreatedEvent != null)
                {
                    lock (_reviewCreatedEvent)
                    {

                        foreach (var del in _reviewCreatedEvent.GetInvocationList())
                        {
                            if (del.Method.Name == value.Method.Name && del.Method.Module.Name == value.Method.Module.Name)
                            {
                                _reviewCreatedEvent -= del as Func<object, ReviewCreatedEventArgs, Task>;
                                break;
                            }
                        }
                    }
                }
                _reviewCreatedEvent += value;

            }
            remove
            {
                if (_reviewCreatedEvent != null)
                {
                    lock (_reviewCreatedEvent)
                    {
                        foreach (var del in _reviewCreatedEvent.GetInvocationList())
                        {
                            if (del.Method.Name == value.Method.Name && del.Method.Module.Name == value.Method.Module.Name)
                            {
                                _reviewCreatedEvent -= value;
                            }
                        }
                    }
                }
            }
        }

        private PublisherSingleton() { }

        public static PublisherSingleton Instance { get => instance.Value; }

        public void Send(ReviewCreatedEventArgs eventArgs)
        {
            _reviewCreatedEvent?.Invoke(this, eventArgs);
        }
    }
}

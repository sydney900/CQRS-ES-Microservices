using CQRS.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CQRS.Domain
{
    public class InProcessBus : ICommandSender, IEventPublisher
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

        public void RegisterHandler<T>(Action<T> handler) where T : IMessage
        {
            List<Action<IMessage>> handlers;

            if(!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }

        public void Send(ICommand command)
        {
            List<Action<IMessage>> handlers;

            if (_routes.TryGetValue(command.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    var handler1 = handler;
                    ThreadPool.QueueUserWorkItem(x => handler1(command));
                }                
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

        public void Publish(IEvent @event)
        {
            List<Action<IMessage>> handlers;

            if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;

            foreach(var handler in handlers)
            {
                var handler1 = handler;
                ThreadPool.QueueUserWorkItem(x => handler1(@event));
            }
        }
    }
}

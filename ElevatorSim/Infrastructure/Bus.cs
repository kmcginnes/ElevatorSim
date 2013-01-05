using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using Ninject;
using Ninject.Syntax;

namespace ElevatorSim.Infrastructure
{
    public class FakeBus : ICommandSender, IEventPublisher
    {
        private readonly IResolutionRoot _resolutionRoot;
        private readonly Dictionary<Type, List<Action<Message>>> _routes = new Dictionary<Type, List<Action<Message>>>();

        public FakeBus(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        //public void RegisterHandler<T>(Action<T> handler) where T : Message
        //{
        //    List<Action<Message>> handlers;
        //    if (!_routes.TryGetValue(typeof(T), out handlers))
        //    {
        //        handlers = new List<Action<Message>>();
        //        _routes.Add(typeof(T), handlers);
        //    }
        //    handlers.Add(DelegateAdjuster.CastArgument<Message, T>(x => handler(x)));
        //}

        public void Send<T>(T command) where T : Command
        {
            var handlers = GetHandlers(command);
            if (handlers.Count() != 1)
                throw new InvalidOperationException("cannot send to more than one handler");
            if(!handlers.Any())
                throw new InvalidOperationException("no handler registered");
            var handler = handlers.First();
            handler.AsDynamic().Handle(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var handlers = GetHandlers(@event);
            foreach (var handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                var handler1 = handler;
                //ThreadPool.QueueUserWorkItem(x => handler1.AsDynamic().Handle(@event));
                handler1.AsDynamic().Handle(@event);
            }
        }

        private IEnumerable<object> GetHandlers(Message message)
        {
            var handlerType = typeof (Handles<>).MakeGenericType(message.GetType());
            var handlers = _resolutionRoot.GetAll(handlerType);
            return handlers;
        }
    }

    public interface Handles<T>
    {
        void Handle(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : Command;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }

    public interface Message
    {
        Guid Id { get; }
    }
    public interface Command : Message
    {
    }
}
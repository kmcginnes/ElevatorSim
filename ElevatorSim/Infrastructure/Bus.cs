using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Syntax;

namespace ElevatorSim.Infrastructure
{
    public static class Bus
    {
        private static ICommandSender _commandSender;
        private static IEventPublisher _eventPublisher;

        static Bus()
        {
            var nullBus = new NullBus();
            _commandSender = nullBus;
            _eventPublisher = nullBus;
        }

        public static void SetCommandSender(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public static void SetEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public static void Send<T>(T command) where T : ICommand
        {
            _commandSender.Send(command);
        }

        public static void Publish<T>(T @event) where T : IEvent
        {
            _eventPublisher.Publish(@event);
        }
    }

    public class NullBus : ICommandSender, IEventPublisher
    {
        public void Send<T>(T command) where T : ICommand
        {
            this.Log().Error("No command sender set. Null command sender being used.");
        }
        public void Publish<T>(T @event) where T : IEvent
        {
            this.Log().Error("No event publisher set. Null event publisher being used.");
        }
    }

    public class InMemoryBus : ICommandSender, IEventPublisher
    {
        private readonly IResolutionRoot _resolutionRoot;
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

        public InMemoryBus(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public void Send<T>(T command) where T : ICommand
        {
            var handlers = GetHandlers(command);
            if (handlers.Count() > 1)
                throw new InvalidOperationException("cannot send to more than one handler");
            if (handlers.Count() < 1)
                throw new InvalidOperationException("no handler registered");
            var handler = handlers.First();
            handler.AsDynamic().When(command);
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            var handlers = GetHandlers(@event);
            foreach (var handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                var handler1 = handler;
                //ThreadPool.QueueUserWorkItem(x => handler1.AsDynamic().Handle(@event));
                handler1.AsDynamic().When(@event);
            }
        }

        private IEnumerable<object> GetHandlers(IMessage message)
        {
            var handlerType = typeof (IHandle<>).MakeGenericType(message.GetType());
            var handlers = _resolutionRoot.GetAll(handlerType);
            return handlers;
        }
    }

    public interface IHandle<T>
    {
        void When(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : ICommand;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : IEvent;
    }
}
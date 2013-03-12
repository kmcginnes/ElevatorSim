using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ElevatorSim.Infrastructure
{
    public interface IMessage {}
    public interface ICommand : IMessage {}
    public interface IEvent : IMessage {}
    public interface ICommand<out TIdentity> : ICommand where TIdentity : IIdentity
    {
        TIdentity Id { get; }
    }
    public interface IEvent<out TIdentity> : IEvent where TIdentity : IIdentity
    {
        TIdentity Id { get; }
    }

    public interface IAggregateRoot {}

    public interface IAggregateState
    {
        void Apply(IEvent theEvent);
    }

    public abstract class AggregateRoot<TState> : IAggregateRoot 
        where TState : IAggregateState
    {
        protected readonly TState State;
        public List<IEvent> EventsThatHappened = new List<IEvent>();

        public AggregateRoot(TState state)
        {
            State = state;
        }

        protected void ApplyChange(IEvent theEvent)
        {
            EventsThatHappened.Add(theEvent);
            State.Apply(theEvent);
            Bus.Publish(theEvent);
        }

        protected EnsureSubject<T> Ensure<T>(T subject)
        {
            return new EnsureSubject<T>(subject);
        }
    }

    public class EnsureSubject<T>
    {
        public T Subject { get; private set; }

        public EnsureSubject(T subject)
        {
            Subject = subject;
        }
    }

    public class EnsurePredicate<T>
    {
        public EnsureSubject<T> Subject { get; private set; }
        public Func<bool> Predicate { get; private set; }

        public EnsurePredicate(EnsureSubject<T> subject, Func<bool> predicate)
        {
            Subject = subject;
            Predicate = predicate;
        }

        public void WithDomainError(string name, string format, params object[] args)
        {
            if (!Predicate())
            {
                throw DomainError.Named(name, format, args);
            }
        }
    }

    public static class EnsureExtensions
    {
        public static EnsurePredicate<T> IsNotNull<T>(this EnsureSubject<T> that) where T : class
        {
            return new EnsurePredicate<T>(that, () => that.Subject != null);
        }
        public static EnsurePredicate<T> IsNull<T>(this EnsureSubject<T> that) where T : class
        {
            return new EnsurePredicate<T>(that, () => that.Subject == null);
        }
        public static EnsurePredicate<int> IsNot(this EnsureSubject<int> that, int compareToValue)
        {
            return new EnsurePredicate<int>(that, () => that.Subject != compareToValue);
        }
        public static EnsurePredicate<string> IsNotNullOrWhitespace(this EnsureSubject<string> that)
        {
            return new EnsurePredicate<string>(that, () => !string.IsNullOrWhiteSpace(that.Subject));
        }
        public static EnsurePredicate<IEnumerable<T>> DoesNotContain<T>(this EnsureSubject<IEnumerable<T>> that, T element)
        {
            return new EnsurePredicate<IEnumerable<T>>(that, () => !that.Subject.Contains(element));
        }
        public static EnsurePredicate<IReadOnlyDictionary<TKey, TValue>> DoesNotContainKey<TKey, TValue>(this EnsureSubject<IReadOnlyDictionary<TKey, TValue>> that, TKey key)
        {
            return new EnsurePredicate<IReadOnlyDictionary<TKey, TValue>>(that, () => !that.Subject.ContainsKey(key));
        }
    }

    /// <summary>
    /// Special exception that is thrown by application services
    /// when something goes wrong in an expected way. This exception
    /// bears human-readable code (the name property acting as sort of a 'key') which is used to verify it
    /// in the tests.
    /// An Exception name is a hard-coded identifier that is still human readable but is not likely to change.
    /// </summary>
    [Serializable]
    public class DomainError : Exception
    {
        public DomainError(string message) : base(message) { }

        /// <summary>
        /// Creates domain error exception with a string name that is easily identifiable in the tests
        /// </summary>
        /// <param name="name">The name to be used to identify this exception in tests.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static DomainError Named(string name, string format, params object[] args)
        {
            return new DomainError(string.Format(format, args))
            {
                Name = name
            };
        }

        public string Name { get; private set; }

        public DomainError(string message, Exception inner) : base(message, inner) { }

        protected DomainError(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }
}

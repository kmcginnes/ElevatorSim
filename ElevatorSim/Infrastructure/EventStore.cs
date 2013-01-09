using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ElevatorSim.Infrastructure
{
    public interface IApplicationService
    {
        void Execute(object command);
    }

    public interface IEventStore
    {
        EventStream LoadEventStream(string id);
        void AppendEventsToStream(string id, long expectedVersion, ICollection<IEvent> events);
    }

    public sealed class EventStream
    {
        public long StreamVersion;
        public List<IEvent> Events = new List<IEvent>();
    }

    public sealed class InMemoryStore : IEventStore
    {
        readonly ConcurrentDictionary<string, IList<IEvent>> _store = new ConcurrentDictionary<string, IList<IEvent>>();

        public EventStream LoadEventStream(string id)
        {
            var stream = _store.GetOrAdd(id, new IEvent[0]).ToList();

            return new EventStream()
            {
                Events = stream,
                StreamVersion = stream.Count
            };
        }

        public void AppendEventsToStream(string id, long expectedVersion, ICollection<IEvent> events)
        {
            foreach (var @event in events)
            {
                this.Log().Info("{0}", @event);
            }
            _store.AddOrUpdate(id, events.ToList(), (s, list) => list.Concat(events).ToList());
        }
    }
}
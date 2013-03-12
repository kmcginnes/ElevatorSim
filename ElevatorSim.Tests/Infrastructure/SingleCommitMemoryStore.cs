using System;
using System.Collections.Generic;
using System.Linq;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Tests
{
    sealed class SingleCommitMemoryStore : IEventStore
    {
        public readonly IList<Tuple<string, IEvent>> Store = new List<Tuple<string, IEvent>>();
        public IEvent[] Appended = null;
        public void Preload(string id, IEvent e)
        {
            Store.Add(Tuple.Create(id, e));
        }

        EventStream IEventStore.LoadEventStream(string id)
        {
            var events = Store.Where(i => id.Equals((string) i.Item1)).Select(i => i.Item2).ToList();
            return new EventStream
                {
                    Events = events,
                    StreamVersion = events.Count
                };
        }

        void IEventStore.AppendEventsToStream(string id, long expectedVersion, ICollection<IEvent> events)
        {
            if (Appended != null)
                throw new InvalidOperationException("Only one commit it allowed");
            Appended = events.ToArray();
        }
    }
}
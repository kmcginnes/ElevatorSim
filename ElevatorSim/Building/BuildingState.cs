using System.Collections.Generic;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Building
{
    public class BuildingState : IAggregateState
    {
        public BuildingId Id { get; private set; }
        public IReadOnlyDictionary<int, string> Floors { get { return _floors; } } 

        private readonly Dictionary<int, string> _floors = new Dictionary<int, string>();

        public BuildingState(IEnumerable<IEvent> loadedEvents)
        {
            foreach (var loadedEvent in loadedEvents)
            {
                Apply(loadedEvent);
            }
        }

        private void When(BuildingOpened e)
        {
            Id = e.Id;
        }

        private void When(FloorBuilt e)
        {
            _floors[e.Level] = e.Name;
        }

        public void Apply(IEvent thisEventTypeHappened)
        {
            ((dynamic)this).When((dynamic)thisEventTypeHappened);
        }
    }
}
using System.Collections.Generic;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorState : IAggregateState
    {
        private FloorId _id;
        private int _level;
        private string _name;

        public FloorId Id
        {
            get { return _id; }
        }

        public FloorState(IEnumerable<IEvent> loadedEvents)
        {
            foreach (var loadedEvent in loadedEvents)
            {
                Apply(loadedEvent);
            }
        }

        private void When(FloorBuilt e)
        {
            _id = e.Id;
            _level = e.Level;
            _name = e.Name;
        }

        public void Apply(IEvent thisEventTypeHappened)
        {
            ((dynamic)this).When((dynamic)thisEventTypeHappened);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorApplicationService : IApplicationService, IHandle<Messages>
    {
        private readonly IEventStore _eventStore;

        public FloorApplicationService(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _eventStore = eventStore;
        }

        public void Execute(object command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        void Update(IFloorCommand command, Action<FloorAggregate> commandExecutor)
        {
            var key = command.Id.ToString();
            var eventStream = _eventStore.LoadEventStream(key);

            var aggregateState = new FloorState(eventStream.Events);
            var aggregate = new FloorAggregate(aggregateState);

            commandExecutor(aggregate);

            _eventStore.AppendEventsToStream(key, eventStream.StreamVersion, aggregate.EventsThatHappened);
        }

        // Now let's use the Update helper method above to wire command messages to actual aggregate methods
        public void When(Messages cmd)
        {
            Update(cmd, ar => ar.Build(cmd.Id, cmd.Level, cmd.Name));
        }
    }
}

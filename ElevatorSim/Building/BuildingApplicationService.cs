using System;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Building
{
    public class BuildingApplicationService : IApplicationService, IHandle<OpenBuilding>, IHandle<BuildFloor>
    {
        private readonly IEventStore _eventStore;

        public BuildingApplicationService(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _eventStore = eventStore;
        }

        public void Execute(object command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        void Update(IBuildingCommand command, Action<BuildingAggregate> commandExecutor)
        {
            var key = command.Id.ToString();
            var eventStream = _eventStore.LoadEventStream(key);

            var aggregateState = new BuildingState(eventStream.Events);
            var aggregate = new BuildingAggregate(aggregateState);

            commandExecutor(aggregate);

            _eventStore.AppendEventsToStream(key, eventStream.StreamVersion, aggregate.EventsThatHappened);
        }

        public void When(OpenBuilding cmd)
        {
            Update(cmd, ar => ar.Open(cmd.Id));
        }

        // Now let's use the Update helper method above to wire command messages to actual aggregate methods
        public void When(BuildFloor cmd)
        {
            Update(cmd, ar => ar.BuildFloor(cmd.Id, cmd.Level, cmd.Name));
        }

        public void When(PushFloorUpButton cmd)
        {
            //Update(cmd, ar => ar.PushUpButton());
        }

        public void When(FloorPushDownButton cmd)
        {
            //Update(cmd, ar => ar.PushDownButton());
        }
    }
}

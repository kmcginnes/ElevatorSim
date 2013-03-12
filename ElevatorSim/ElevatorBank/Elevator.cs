using System;
using System.Collections.Generic;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.ElevatorBank
{
    public class ElevatorBankApplicationService : IApplicationService,
        IHandle<AddElevator>
    {
        private readonly IEventStore _eventStore;

        public ElevatorBankApplicationService(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _eventStore = eventStore;
        }

        public void Execute(object command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        void Update(ICommand<ElevatorBankId> command, Action<ElevatorBankAggregate> commandExecutor)
        {
            var key = command.Id.ToString();
            var eventStream = _eventStore.LoadEventStream(key);

            var aggregateState = new ElevatorBankState(eventStream.Events);
            var aggregate = new ElevatorBankAggregate(aggregateState);

            commandExecutor(aggregate);

            _eventStore.AppendEventsToStream(key, eventStream.StreamVersion, aggregate.EventsThatHappened);
        }

        public void When(AddElevator message)
        {
            Update(message, ar => ar.Add(message.Id, message.Chute));
        }
    }

    public class ElevatorBankAggregate : AggregateRoot<ElevatorBankState>
    {
        public ElevatorBankAggregate(ElevatorBankState state) : base(state) {}

        public void Add(ElevatorBankId id, int chute)
        {
            Ensure(State.Elevators).DoesNotContainKey(chute).WithDomainError("elevator-already-exists", "Elevator already exists");
            ApplyChange(new ElevatorAdded(id, chute));
        }
    }

    public class ElevatorBankState : IAggregateState
    {
        private readonly Dictionary<int, Elevator> _elevators = new Dictionary<int, Elevator>();

        public IReadOnlyDictionary<int, Elevator> Elevators
        {
            get { return _elevators; }
        }

        public ElevatorBankState(IEnumerable<IEvent> loadedEvents)
        {
            foreach (var loadedEvent in loadedEvents)
            {
                Apply(loadedEvent);
            }
        }

        public void Apply(IEvent thisEventTypeHappened)
        {
            ((dynamic)this).When((dynamic)thisEventTypeHappened);
        }

        private void When(ElevatorAdded cmd)
        {
            _elevators[cmd.Chute] = new Elevator();
        }
    }

    public class Elevator
    {
    }

    public class AddElevator : IElevatorBankCommand
    {
        public ElevatorBankId Id { get; private set; }
        public int Chute { get; private set; }

        public AddElevator(ElevatorBankId id, int chute)
        {
            Id = id;
            Chute = chute;
        }
    }
    public class ElevatorAdded : IElevatorBankEvent
    {
        public ElevatorBankId Id { get; private set; }
        public int Chute { get; private set; }

        public ElevatorAdded(ElevatorBankId id, int chute)
        {
            Id = id;
            Chute = chute;
        }
    }

    public class ElevatorBankId : AbstractIdentity<long>
    {
        public override long Id { get; protected set; }
        public override string GetTag()
        {
            return "Elevator";
        }
    }
    public interface IElevatorBankCommand : ICommand<ElevatorBankId> {}
    public interface IElevatorBankEvent : IEvent<ElevatorBankId> {}
}

using ElevatorSim.Infrastructure;

namespace ElevatorSim.Building
{
    public class BuildingAggregate : AggregateRoot<BuildingState>
    {
        public BuildingAggregate(BuildingState state) : base(state) {}

        public void Open(BuildingId id)
        {
            Ensure(State.Id).IsNull().WithDomainError("building-already-created", "Building was already created");

            ApplyChange(new BuildingOpened(id));
        }

        public void BuildFloor(BuildingId id, int level, string name)
        {
            ThrowExceptionIfBuildingNotOpened();
            Ensure(State.Floors).DoesNotContainKey(level).WithDomainError("floor-already-exists", "Floor already exists");
            Ensure(level).IsNot(0).WithDomainError("level-cannot-be-zero", "Level can not be zero");
            Ensure(name).IsNotNullOrWhitespace().WithDomainError("empty-name", "Name can not be empty");

            ApplyChange(new FloorBuilt(id, level, name));
        }

        private void ThrowExceptionIfBuildingNotOpened()
        {
            Ensure(State.Id).IsNotNull().WithDomainError("building-not-open", "Building is not open");
        }
    }
}

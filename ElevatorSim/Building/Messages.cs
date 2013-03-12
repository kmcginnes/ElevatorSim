using ElevatorSim.Infrastructure;

namespace ElevatorSim.Building
{
    public class OpenBuilding : IBuildingCommand
    {
        public BuildingId Id { get; private set; }

        public OpenBuilding(BuildingId id)
        {
            Id = id;
        }
    }
    public class BuildingOpened : IBuildingEvent
    {
        public BuildingId Id { get; private set; }

        public BuildingOpened(BuildingId id)
        {
            Id = id;
        }
    }

    public class BuildFloor : IBuildingCommand
    {
        public BuildingId Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }

        public BuildFloor(BuildingId id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }
    public class FloorBuilt : IBuildingEvent
    {
        public BuildingId Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int Version { get; set; }

        public FloorBuilt(BuildingId id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }

    public class PushFloorUpButton : IBuildingCommand
    {
        public BuildingId Id { get; private set; }

        public PushFloorUpButton(BuildingId id)
        {
            Id = id;
        }
    }
    public class FloorUpButtonPushed : IBuildingEvent
    {
        public BuildingId Id { get; private set; }

        public FloorUpButtonPushed(BuildingId id)
        {
            Id = id;
        }
    }

    public class FloorPushDownButton : IBuildingCommand
    {
        public BuildingId Id { get; private set; }

        public FloorPushDownButton(BuildingId id)
        {
            Id = id;
        }
    }
    public class FloorDownButtonPushed : IBuildingEvent
    {
        public BuildingId Id { get; private set; }

        public FloorDownButtonPushed(BuildingId id)
        {
            Id = id;
        }
    }

    public class BuildingId : AbstractIdentity<long>
    {
        public override long Id { get; protected set; }
        public override string GetTag()
        {
            return "Building";
        }

        public BuildingId(long id)
        {
            Id = id;
        }
    }
    public interface IBuildingCommand : ICommand<BuildingId> {}
    public interface IBuildingEvent : IEvent<BuildingId> {}
}
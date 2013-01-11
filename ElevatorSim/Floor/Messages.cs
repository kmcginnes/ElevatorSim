using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class BuildFloor : IFloorCommand
    {
        public FloorId Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }

        public BuildFloor(FloorId id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }
    public class FloorBuilt : IFloorEvent
    {
        public FloorId Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int Version { get; set; }

        public FloorBuilt(FloorId id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }

    public class PushFloorUpButton : IFloorCommand
    {
        public FloorId Id { get; private set; }

        public PushFloorUpButton(FloorId id)
        {
            Id = id;
        }
    }
    public class FloorUpButtonPushed : IFloorEvent
    {
        public FloorId Id { get; private set; }

        public FloorUpButtonPushed(FloorId id)
        {
            Id = id;
        }
    }

    public class FloorPushDownButton : IFloorCommand
    {
        public FloorId Id { get; private set; }

        public FloorPushDownButton(FloorId id)
        {
            Id = id;
        }
    }
    public class FloorDownButtonPushed : IFloorEvent
    {
        public FloorId Id { get; private set; }

        public FloorDownButtonPushed(FloorId id)
        {
            Id = id;
        }
    }

    public class FloorId : AbstractIdentity<long>
    {
        public override long Id { get; protected set; }
        public override string GetTag()
        {
            return "Floor";
        }

        public FloorId(long id)
        {
            Id = id;
        }
    }
    public interface IFloorCommand : ICommand
    {
        FloorId Id { get; }
    }
    public interface IFloorEvent : IEvent
    {
        FloorId Id { get; }
    }
}
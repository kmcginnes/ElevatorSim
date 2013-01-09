using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class Messages : IFloorCommand
    {
        public FloorId Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }

        public Messages(FloorId id, int level, string name)
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

    public interface IFloorCommand : ICommand
    {
        FloorId Id { get; }
    }

    public interface IFloorEvent : IEvent
    {
        FloorId Id { get; }
    }
}
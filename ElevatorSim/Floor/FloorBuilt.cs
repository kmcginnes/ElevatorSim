using System;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorBuilt : Event
    {
        public Guid Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int Version { get; set; }

        public FloorBuilt(Guid id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }
}
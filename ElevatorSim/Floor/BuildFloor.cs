using System;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class BuildFloor : Command
    {
        public Guid Id { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }

        public BuildFloor(Guid id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }
}
using System;

namespace ElevatorSim.Floor
{
    public class FloorDto
    {
        public Guid Id;
        public int Level;
        public string Name;

        public FloorDto(Guid id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }
}
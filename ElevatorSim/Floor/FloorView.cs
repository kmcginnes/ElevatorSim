using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorDto
    {
        public FloorId Id;
        public int Level;
        public string Name;

        public FloorDto(FloorId id, int level, string name)
        {
            Id = id;
            Level = level;
            Name = name;
        }
    }

    public class FloorView : IHandle<FloorBuilt>
    {
        public void When(FloorBuilt message)
        {
            Database.Add(new FloorDto(message.Id, message.Level, message.Name));
        }
    }
}
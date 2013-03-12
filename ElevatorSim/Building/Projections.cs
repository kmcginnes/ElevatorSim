using ElevatorSim.Infrastructure;

namespace ElevatorSim.Building
{
    public class FloorDto
    {
        public BuildingId BuildingId;
        public int Level;
        public string Name;

        public FloorDto(BuildingId buildingId, int level, string name)
        {
            BuildingId = buildingId;
            Level = level;
            Name = name;
        }
    }

    public class FloorProjection : IHandle<FloorBuilt>
    {
        public void When(FloorBuilt message)
        {
            Database.Add(new FloorDto(message.Id, message.Level, message.Name));
        }
    }
}
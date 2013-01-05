using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorView : Handles<FloorBuilt>
    {
        public void Handle(FloorBuilt message)
        {
            Database.Add(new FloorDto(message.Id, message.Level, message.Name));
        }
    }
}
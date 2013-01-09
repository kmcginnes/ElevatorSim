using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
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
}
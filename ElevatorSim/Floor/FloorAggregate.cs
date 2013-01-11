using System;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorAggregate : AggregateRoot<FloorState>
    {
        public FloorAggregate(FloorState state) : base(state) {}

        public void Build(FloorId id, int level, string name)
        {
            if(id == null)
                throw new ArgumentNullException("id", "Id can not be empty");
            if(level == 0)
                throw new ArgumentOutOfRangeException("level", "Level can not be zero");
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "Name can not be null or empty");
            ApplyChange(new FloorBuilt(id, level, name));
        }

        public void PushUpButton()
        {
            throw new NotImplementedException();
        }

        public void PushDownButton()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Floor
{
    public class FloorAggregate : AggregateRoot
    {
        private Guid _id;
        private int _level;
        private string _name;

        public override Guid Id
        {
            get { return _id; }
        }

        private void Apply(FloorBuilt e)
        {
            _id = e.Id;
            _level = e.Level;
            _name = e.Name;
        }

        public void Build(Guid id, int level, string name)
        {
            if(id == Guid.Empty)
                throw new ArgumentNullException("id", "Id can not be empty");
            if(level == 0)
                throw new ArgumentOutOfRangeException("level", "Level can not be zero");
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "Name can not be null or empty");
            ApplyChange(new FloorBuilt(id, level, name));
        }
    }
}

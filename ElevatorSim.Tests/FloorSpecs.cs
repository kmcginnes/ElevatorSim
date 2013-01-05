using System;
using ElevatorSim.Floor;
using Xunit;

namespace ElevatorSim.Tests
{
    public class FloorSpecs : SpecsFor<Floor.FloorAggregate>
    {
        [Fact]
        public void build_floor()
        {
            Given();
            var id = Guid.NewGuid();
            When(new BuildFloor(id, 1, "Lobby"));
            Then(new FloorBuilt(id, 1, "Lobby"));
        }
    }
}

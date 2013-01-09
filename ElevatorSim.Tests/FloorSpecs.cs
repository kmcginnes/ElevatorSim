using System;
using ElevatorSim.Floor;
using Xunit;

namespace ElevatorSim.Tests
{
    public class FloorSpecs : floor_application_service_spec
    {
        FloorId Id = new FloorId(11);

        [Fact]
        public void build_floor()
        {
            Given();
            When(new Messages(Id, 1, "Lobby"));
            Expect(new FloorBuilt(Id, 1, "Lobby"));
        }
    }
}

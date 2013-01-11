using System;
using ElevatorSim.Floor;
using Xunit;

namespace ElevatorSim.Tests
{
    public class FloorSpecs : floor_application_service_spec
    {
        FloorId Id = new FloorId(11);

        [Fact]
        public void build()
        {
            Given();
            When(new BuildFloor(Id, 1, "Lobby"));
            Expect(new FloorBuilt(Id, 1, "Lobby"));
        }

        //[Fact]
        //public void push_up_button_on_lobby()
        //{
        //    Given(new FloorBuilt(Id, 1, "Lobby"));
        //    When(new PushFloorUpButton(Id));
        //    Expect(new FloorUpButtonPushed(Id));
        //}

        //[Fact]
        //public void push_down_button_on_lobby_without_basement()
        //{
        //    Given(new FloorBuilt(Id, 1, "Lobby"));
        //    When(new FloorPushDownButton(Id));
        //    Expect(new FloorDownButtonPushed(Id));
        //}

        //[Fact]
        //public void push_down_button_on_second_floor()
        //{
        //    var secondFloorId = new FloorId(Id.Id + 1);
        //    Given(new FloorBuilt(Id, 1, "Lobby"), new FloorBuilt(secondFloorId, 2, "Floor 2"));
        //    When(new FloorPushDownButton(Id));
        //    Expect(new FloorDownButtonPushed(Id));
        //}
    }
}

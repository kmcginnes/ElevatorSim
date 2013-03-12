using ElevatorSim.Building;
using Xunit;

namespace ElevatorSim.Tests
{
    public class building_specs : building_application_service_spec
    {
        BuildingId Id = new BuildingId(11);

        [Fact]
        public void open_building()
        {
            Given();
            When(new OpenBuilding(Id));
            Expect(new BuildingOpened(Id));
        }

        [Fact]
        public void build_floor()
        {
            Given(new BuildingOpened(Id));
            When(new BuildFloor(Id, 1, "Lobby"));
            Expect(new FloorBuilt(Id, 1, "Lobby"));
        }

        [Fact]
        public void build_floor_before_opening()
        {
            Given();
            When(new BuildFloor(Id, 1, "Lobby"));
            ExpectError("building-not-open");
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
        //    var secondFloorId = new BuildingId(Id.Id + 1);
        //    Given(new FloorBuilt(Id, 1, "Lobby"), new FloorBuilt(secondFloorId, 2, "Floor 2"));
        //    When(new FloorPushDownButton(Id));
        //    Expect(new FloorDownButtonPushed(Id));
        //}
    }
}

using System.Linq;
using System.Runtime.CompilerServices;
using ElevatorSim.Building;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Tests
{
    public abstract class building_application_service_spec : application_service_spec
    {
        protected override void SetupServices() {}

        protected override IEvent[] ExecuteCommand(IEvent[] given, ICommand cmd)
        {
            var store = new SingleCommitMemoryStore();
            foreach (var e in given.OfType<IBuildingEvent>())
            {
                store.Preload(e.Id.ToString(), e);
            }
            new BuildingApplicationService(store).Execute(cmd);
            return store.Appended ?? new IEvent[0];
        }

        protected void When(IBuildingCommand when, [CallerMemberName] string testName = null)
        {
            WhenMessage(when, testName);
        }
        protected void Given(params IBuildingEvent[] given)
        {
            GivenMessages(given);
        }
        protected void GivenSetup(params SpecSetupEvent[] setup)
        {
            GivenMessages(setup);
        }
        protected void Expect(params IBuildingEvent[] given)
        {
            ExpectMessages(given);
        }
    }
}
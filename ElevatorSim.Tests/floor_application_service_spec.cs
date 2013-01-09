using System.Linq;
using System.Runtime.CompilerServices;
using ElevatorSim.Floor;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Tests
{
    /// <summary>
    /// Base class that acts as execution environment (container) for our application 
    /// service. It will be responsible for wiring in test version of services to
    /// factory and executing commands
    /// </summary>
    public abstract class floor_application_service_spec : application_service_spec
    {
        protected override void SetupServices()
        {
        }

        protected override IEvent[] ExecuteCommand(IEvent[] given, ICommand cmd)
        {
            var store = new SingleCommitMemoryStore();
            foreach (var e in given.OfType<IFloorEvent>())
            {
                store.Preload(e.Id.ToString(), e);
            }
            new FloorApplicationService(store).Execute(cmd);
            return store.Appended ?? new IEvent[0];
        }


        protected void When(IFloorCommand when, [CallerMemberName] string testName = null)
        {
            WhenMessage(when, testName);
        }
        protected void Given(params IFloorEvent[] given)
        {
            GivenMessages(given);
        }
        protected void GivenSetup(params SpecSetupEvent[] setup)
        {
            GivenMessages(setup);
        }
        protected void Expect(params IFloorEvent[] given)
        {
            ExpectMessages(given);
        }
    }
}
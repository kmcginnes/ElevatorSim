using System.Linq;
using System.Runtime.CompilerServices;
using ElevatorSim.ElevatorBank;
using ElevatorSim.Infrastructure;

namespace ElevatorSim.Tests
{
    public abstract class elevator_bank_application_service_spec : application_service_spec
    {
        protected override void SetupServices() { }

        protected override IEvent[] ExecuteCommand(IEvent[] given, ICommand cmd)
        {
            var store = new SingleCommitMemoryStore();
            foreach (var e in given.OfType<IElevatorBankEvent>())
            {
                store.Preload(e.Id.ToString(), e);
            }
            new ElevatorBankApplicationService(store).Execute(cmd);
            return store.Appended ?? new IEvent[0];
        }

        protected void When(IElevatorBankCommand when, [CallerMemberName] string testName = null)
        {
            WhenMessage(when, testName);
        }
        protected void Given(params IElevatorBankEvent[] given)
        {
            GivenMessages(given);
        }
        protected void GivenSetup(params SpecSetupEvent[] setup)
        {
            GivenMessages(setup);
        }
        protected void Expect(params IElevatorBankEvent[] given)
        {
            ExpectMessages(given);
        }
    }
}
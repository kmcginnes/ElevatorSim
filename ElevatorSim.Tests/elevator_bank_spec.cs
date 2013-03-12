using System;
using ElevatorSim.ElevatorBank;
using ElevatorSim.Infrastructure;
using Xunit;

namespace ElevatorSim.Tests
{
    public class elevator_bank_spec : elevator_bank_application_service_spec
    {
        readonly ElevatorBankId Id = new ElevatorBankId();

        [Fact]
        public void add_elevator()
        {
            Given();
            When(new AddElevator(Id, 0));
            Expect(new ElevatorAdded(Id, 0));
        }
    }
}
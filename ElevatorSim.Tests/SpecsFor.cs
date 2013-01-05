using System.Collections.Generic;
using System.Linq;
using ElevatorSim.Infrastructure;
using FluentAssertions;
using Xunit;

namespace ElevatorSim.Tests
{
    public class SpecsFor<T> where T : AggregateRoot, new()
    {
        private object _when;
        public T SUT { get; private set; }

        public SpecsFor()
        {
            SUT = new T();
        }

        protected void Given(params Event[] events)
        {
            foreach (var @event in events)
            {
                SUT.AsDynamic().Apply(@event);
            }
        }

        protected void When(Command command)
        {
            _when = command;
        }

        protected void Then(Event @event)
        {
            var commandType = _when.GetType();
            var commandName = commandType.Name;
            var commandProperties = commandType.GetProperties();

            var methodName = commandName.Replace(SUT.GetType().Name, "");
            var methodInfo = SUT.GetType().GetMethod(methodName);
            var parameterInfos = methodInfo.GetParameters().OrderBy(pi => pi.Position);
            var parameters = new List<object>();
            foreach (var parameterInfo in parameterInfos)
            {
                var property = commandProperties.SingleOrDefault(
                    p => p.Name.ToLower() == parameterInfo.Name.ToLower());

                parameters.Add(property.GetValue(_when));
            }
            methodInfo.Invoke(SUT, parameters.ToArray());

            var result = SUT.GetUncommittedChanges().SingleOrDefault();
            if (result == null)
            {
                var message = string.Format("Couldn't find {0} in the uncommitted changes", @event.GetType().Name);
                this.Log().Error(message);
                Assert.True(false, message);
            }

            var eventTypeProperties = @event.GetType().GetProperties();
            foreach (var propertyInfo in eventTypeProperties)
            {
                var resultValue = propertyInfo.GetValue(result);
                var expectedValue = propertyInfo.GetValue(@event);
                resultValue.Should().Be(expectedValue);
            }
        }
    }
}
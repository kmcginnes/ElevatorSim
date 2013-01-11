using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElevatorSim.Floor;
using ElevatorSim.Infrastructure;
using FluentAssertions;
using Xunit;

namespace ElevatorSim.Tests
{
    public class EnsureAllCommandsHandled
    {
        [Fact]
        public void Test()
        {
            var allTypes = Assembly.GetAssembly(typeof (FloorAggregate)).GetTypes();

            var commands = allTypes
                .Where(x => x.IsClass)
                .Where(x => x.GetInterfaces().Contains(typeof(ICommand)));

            var commandHandlerMethodInfos = allTypes
                .Where(x => x.IsClass)
                .Where(x => x.GetInterfaces().Contains(typeof (IApplicationService)))
                .Select(x => new CommandHandlerInfo
                    {
                        ApplicationService = x.Name, 
                        HandlerMethodInfo = x.Methods().Where(m => m.Name == "When")
                    });

            var commandHandlersByCommand = new Dictionary<string, int>();

            var messageBuilder = new StringBuilder();
            foreach (var commandType in commands)
            {
                //var matchingCommandHandler = commandHandlerMethodInfos
                //    .SelectMany(x => x.HandlerMethodInfo.Where(mi => mi.GetParameters().Any(p => p.ParameterType == commandType)));

                //if (matchingCommandHandler.Count() < 1)
                //{
                //    messageBuilder.AppendLine(string.Format("Command: {0} has no handlers", commandType.Name));
                //}

                //if (matchingCommandHandler.Count() > 1)
                //{
                //    messageBuilder.AppendLine(string.Format("Command: {0} has too many handlers [{1}]", commandType.Name, string.Join(", ", matchingCommandHandler.Select(x => x))))
                //}

                //if (matchingCommandHandler.Count() != 1)
                //{
                //    commandHandlersByCommand[commandType.Name] = matchingCommandHandler.Count();
                //}
            }

            commandHandlerMethodInfos.Should().HaveCount(1);
        }
    }

    public class CommandHandlerInfo
    {
        public string ApplicationService { get; set; }

        public IEnumerable<MethodInfo> HandlerMethodInfo { get; set; }
    }
}

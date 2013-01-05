using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using ElevatorSim.Floor;
using ElevatorSim.Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;

namespace ElevatorSim
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IKernel _kernel;

        protected override void Configure()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            _kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindToSelf());
            _kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());

            _kernel.Rebind<FakeBus>().ToSelf().InSingletonScope();
            _kernel.Rebind<ICommandSender, IEventPublisher>().To<FakeBus>().InSingletonScope();

            _kernel.Bind<Handles<BuildFloor>>().To<GenericHandler<Floor.FloorAggregate, BuildFloor>>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _kernel.Get(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }
    }

    public class GenericHandler<TAggregate, TCommand> : Handles<TCommand>
        where TAggregate : IAggregateRoot, new()
        where TCommand : Command
    {
        private readonly IRepository<TAggregate> _repository;

        public GenericHandler(IRepository<TAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(TCommand command)
        {
            var aggregate = _repository.GetById(command.Id);

            var commandType = command.GetType();
            var commandName = commandType.Name;
            var commandProperties = commandType.GetProperties();

            var aggregateType = aggregate.GetType();

            var methodName = commandName.Replace(aggregateType.Name, "");
            var methodInfo = aggregateType.GetMethod(methodName);
            var parameterInfos = methodInfo.GetParameters().OrderBy(pi => pi.Position);
            
            var parameters = new List<object>();
            foreach (var parameterInfo in parameterInfos)
            {
                var property = commandProperties.SingleOrDefault(
                    p => p.Name.ToLower() == parameterInfo.Name.ToLower());

                parameters.Add(property.GetValue(command));
            }
            methodInfo.Invoke(aggregate, parameters.ToArray());

            _repository.Save(aggregate, aggregate.Version + 1);
        }
    }
}

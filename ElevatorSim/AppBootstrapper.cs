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

            _kernel.Rebind<InMemoryBus>().ToSelf().InSingletonScope();
            _kernel.Rebind<ICommandSender, IEventPublisher>().To<InMemoryBus>().InSingletonScope();

            Bus.SetCommandSender(_kernel.Get<ICommandSender>());
            Bus.SetEventPublisher(_kernel.Get<IEventPublisher>());
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
}

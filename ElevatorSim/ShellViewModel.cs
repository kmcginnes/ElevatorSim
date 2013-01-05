using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using ElevatorSim.Floor;
using ElevatorSim.Infrastructure;

namespace ElevatorSim
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ICommandSender _bus;
        public IEnumerable<FloorListItemViewModel> Floors
        {
            get
            {
                return Database.Get<FloorDto>()
                    .Select(x => new FloorListItemViewModel
                        {
                            Level = x.Level,
                            Name = x.Name
                        });
            }
        }

        public ShellViewModel(ICommandSender bus)
        {
            _bus = bus;
        }

        public void BuildFloor()
        {
            var countOfFloors = Floors.Count();
            var floorLevel = countOfFloors + 1;
            var floorName = "Lobby";
            if (countOfFloors > 0)
            {
                floorName = string.Format("FloorAggregate {0}", floorLevel);
            }
            _bus.Send(new BuildFloor(Guid.NewGuid(), floorLevel, floorName));
            NotifyOfPropertyChange(() => Floors);
        }
    }

    public class FloorListItemViewModel
    {
        public string Name { get; set; }
        public int Level { get; set; }
    }
}

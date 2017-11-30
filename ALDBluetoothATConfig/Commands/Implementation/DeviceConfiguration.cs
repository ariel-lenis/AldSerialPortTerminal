using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALDBluetoothATConfig.Commands.Implementation
{
    [Serializable]
    class DeviceConfiguration
    {
        public DeviceConfiguration()
        {
            this.Devices = new Dictionary<string, List<string>>();
            this.Commands = new List<DeviceCommand>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, List<string>> Devices { get; set; }
        public string SequenceAfterEveryCommand { get; set; }
        public List<DeviceCommand> Commands { get; set; }
    }
}

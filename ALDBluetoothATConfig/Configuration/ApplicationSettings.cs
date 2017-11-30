using ALDSerialPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALDBluetoothATConfig.Configuration
{
    public class ApplicationSettings
    {
        public string LastUsedPort { get; set; }
        public string DefaultPortSetting { get; set; }
        public Dictionary<string, SerialConfiguration> Configurations { get; set; }

        public ApplicationSettings()
        {
            this.Configurations = new Dictionary<string, SerialConfiguration>();
        }
    }
}

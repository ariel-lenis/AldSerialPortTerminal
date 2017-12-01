using Ald.SerialPort.Configuration;
using System;
using System.Collections.Generic;

namespace Ald.SerialTerminal.Main.Configuration
{
    [Serializable]
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

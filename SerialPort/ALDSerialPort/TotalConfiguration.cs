using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALDSerialPort
{
    [Serializable()]
    public class TotalConfiguration
    {
        public string COM { get; set; }
        public SerialConfiguration Configuration { get; set; }
        public TotalConfiguration(string COM, SerialConfiguration Configuration)
        {
            this.COM = COM;
            this.Configuration = Configuration;
        }
    }
}

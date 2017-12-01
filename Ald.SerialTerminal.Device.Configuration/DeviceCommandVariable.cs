using System;
using System.Collections.Generic;
using R = System.Text.RegularExpressions;

namespace Ald.SerialTerminal.Device.Configuration
{
    [Serializable]
    public class DeviceCommandVariable
    {
        public string Variable { get; set; }
        public string Regex { get; set; }
        public Dictionary<string, string> Map { get; set; }

        public DeviceCommandVariable()
        {
            this.Map = new Dictionary<string, string>();
        }

        public bool Validate(string value)
        {
            if (this.Map.Count > 0)
            {
                return this.Map.ContainsKey(value);
            }

            string regex = this.Regex ?? ".+";

            return R.Regex.IsMatch(value, regex);
        }
    }
}

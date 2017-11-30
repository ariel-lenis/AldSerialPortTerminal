using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ALDBluetoothATConfig.Commands.Implementation
{
    [Serializable]
    public class DeviceCommand
    {
        public string Command { get; set; }
        public string Description { get; set; }
        public string ShortLabel { get; set; }

        public List<DeviceCommandVariable> Variables { get; set; }

        public DeviceCommand()
        {
            this.Variables = new List<DeviceCommandVariable>();
        }

        public bool ValidateVariables(Dictionary<string, string> values)
        {
            var dictionary = this.Variables.ToDictionary(x => x.Variable, x=>x);

            foreach(var item in values)
            {
                if (!dictionary.ContainsKey(item.Key))
                {
                    throw new Exception(string.Format("Invalid Variables {0}", item.Key));
                }

                if (!dictionary[item.Key].Validate(item.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public string CreateCommand(Dictionary<string, string> values)
        {
            string regex = @"(\{\{[A-Z0-9a-z\-_]+\}\})";

            if (!this.ValidateVariables(values))
            {
                throw new Exception("Invalid values for variables.");
            }

            string command = Regex.Replace(this.Command, regex, delegate (Match m)
            {
                string key = m.Value.Substring(2, m.Value.Length - 4);
                return values.ContainsKey(key)?values[key]:string.Empty;
            });

            return command;
        }
    }
}

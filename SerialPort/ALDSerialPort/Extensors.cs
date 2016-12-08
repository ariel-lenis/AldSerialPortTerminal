using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace ALDSerialPort
{
    public static class Extensors
    {
        public static bool ContainsProperty(this ManagementObject managementObject, string propertyName)
        {
            foreach (var property in managementObject.Properties)
                if (property.Name == propertyName)
                    return true;

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALDBluetoothATConfig.Commands.Implementation
{
    public delegate void DOnCommandAction(object sender, string command);

    public interface IDeviceCommand
    {
        void SetCommandInformation(DeviceCommand commandInformation);
        void SetOnCommandActionEvent(DOnCommandAction onCommandAction);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALDBluetoothATConfig.Commands.Implementation
{
    public delegate void DOnCommandVariableChanged(object sender, bool isValid, string newValue);

    public interface ICommandVariable
    {
        void SetVariableInformation(DeviceCommandVariable variable);
        void SetOnCommandVariableChangedEvent(DOnCommandVariableChanged listener);
        bool Validate();
        string VariableValue { get; }

        DeviceCommandVariable Variable { get; }
    }
}

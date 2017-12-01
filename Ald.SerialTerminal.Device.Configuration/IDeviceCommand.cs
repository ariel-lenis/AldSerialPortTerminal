namespace Ald.SerialTerminal.Device.Configuration
{
    public delegate void DOnCommandAction(object sender, string command);

    public interface IDeviceCommand
    {
        void SetCommandInformation(DeviceCommand commandInformation);
        void SetOnCommandActionEvent(DOnCommandAction onCommandAction);
    }
}

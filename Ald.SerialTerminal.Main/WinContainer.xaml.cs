using Ald.SerialPort.Configuration;
using System.Windows;

namespace Ald.SerialTerminal.Main
{
    /// <summary>
    /// Interaction logic for WinContainer.xaml
    /// </summary>
    public partial class WinContainer : Window
    {
        ALDSerialPort serial;
        public WinContainer(ALDSerialPort serial)
        {
            InitializeComponent();
            this.serial = serial;
            this.usrTabCommands.InitializeControl(serial);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.serial.ClosePort();
        }
    }
}

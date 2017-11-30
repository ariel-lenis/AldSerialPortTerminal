using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ALDBluetoothATConfig
{
    /// <summary>
    /// Interaction logic for WinContainer.xaml
    /// </summary>
    public partial class WinContainer : Window
    {
        ALDSerialPort.ALDSerialPort serial;
        public WinContainer(ALDSerialPort.ALDSerialPort serial)
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

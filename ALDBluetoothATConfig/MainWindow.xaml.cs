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
using System.Windows.Navigation;
using System.Windows.Shapes;
using A = ALDSerialPort;
namespace ALDBluetoothATConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdatePorts();        
        }

        private void UpdatePorts()
        {
            var res = ALDSerialPort.ALDSerialPort.EnumeratePorts();
            cbCOMPorts.ItemsSource = null;
            cbCOMPorts.ItemsSource = res;
            if (res.Count > 0) cbCOMPorts.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            UpdatePorts();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Type type;
            /*
            type= typeof(A.ALDSerialPort.SerialConfiguration.EBitsPerSecond);
            cbBitsPerSecond.ItemsSource = type.GetEnumValues();
            cbBitsPerSecond.SelectedItem = ALDSerialPort.ALDSerialPort.SerialConfiguration.EBitsPerSecond.V9600;
            */
            cbBitsPerSecond.Items.Add(1200);
            cbBitsPerSecond.Items.Add(2400);
            cbBitsPerSecond.Items.Add(4800);
            cbBitsPerSecond.Items.Add(9600);
            cbBitsPerSecond.Items.Add(19200);
            cbBitsPerSecond.Items.Add(38400);
            cbBitsPerSecond.Items.Add(57600);
            cbBitsPerSecond.Items.Add(230400);
            cbBitsPerSecond.SelectedItem = 9600;
            
            type = typeof(A.ALDSerialPort.SerialConfiguration.EDataBits);
            cbDataBits.ItemsSource = type.GetEnumValues();
            cbDataBits.SelectedItem = ALDSerialPort.ALDSerialPort.SerialConfiguration.EDataBits.V8;
            



            type = typeof(System.IO.Ports.StopBits);           
            cbStopBits.ItemsSource = type.GetEnumValues();
            cbStopBits.SelectedItem = System.IO.Ports.StopBits.One;

            type = typeof(System.IO.Ports.Parity);
            cbParity.ItemsSource = type.GetEnumValues();
            cbParity.SelectedItem = System.IO.Ports.Parity.None; 
        }

        private void btnStartConfiguration_Click(object sender, RoutedEventArgs e)
        {
            A.ALDSerialPort.PreviousInformation prev = cbCOMPorts.SelectedItem as A.ALDSerialPort.PreviousInformation;
            A.ALDSerialPort.SerialConfiguration sconf = new A.ALDSerialPort.SerialConfiguration();
            A.ALDSerialPort.TotalConfiguration configuration = new A.ALDSerialPort.TotalConfiguration(prev.Port, sconf);
            configuration.Configuration.ConfigBitsPerSecond = (int)cbBitsPerSecond.SelectedItem;
            configuration.Configuration.ConfigDataBits = (A.ALDSerialPort.SerialConfiguration.EDataBits)this.cbDataBits.SelectedItem;
            configuration.Configuration.ConfigParity = (System.IO.Ports.Parity)this.cbParity.SelectedItem;
            configuration.Configuration.ConfigStopBits = (System.IO.Ports.StopBits)this.cbStopBits.SelectedItem;

            A.ALDSerialPort serial = new A.ALDSerialPort(configuration);
            try
            {
                serial.OpenPort();
                WPFComandosAT w = new WPFComandosAT(serial);
                w.ShowDialog();
            }
            catch
            {
                MessageBox.Show("No se puede abrir este puerto.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void cbBitsPerSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}

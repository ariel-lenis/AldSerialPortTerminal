using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace ALDBluetoothATConfig
{
	/// <summary>
	/// Interaction logic for WPFTestearComunicacion.xaml
	/// </summary>
	public partial class WPFComandosAT : Window
	{
        Stopwatch sw;
        Timer timer;
        string snew;
        string sold;
        public enum HC06Baudrate
        {
            v1200bps = 1,
            v2400bps,
            v4800bps,
            v9600bps,
            v19200bps,
            v38400bps,
            v57600bps,
            v115200bps,
            v230400bps
        }

        private ALDSerialPort.ALDSerialPort serial;

		public WPFComandosAT()
		{
			this.InitializeComponent();

            var type = typeof(HC06Baudrate);
            cbBaudrate.ItemsSource = type.GetEnumValues();
            cbBaudrate.SelectedItem = HC06Baudrate.v9600bps;
            sw = new Stopwatch();
            sw.Start();
            snew = sold = "";
            timer = new Timer(500);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
		}

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate {
                this.IsEnabled = true;
            }));
            timer.Stop();
        }

        public WPFComandosAT(ALDSerialPort.ALDSerialPort serial):this()
        {            
            this.serial = serial;
            serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);
            serial.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(serial_ErrorReceived);
        }

        void serial_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show(e.EventType.ToString());
        }

        void serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string res = serial.Serial.ReadExisting();
            if (sw.ElapsedMilliseconds > 500)
            {
                if(snew!="") sold = snew + "\n" + sold;
                snew = "[" + DateTime.Now.TimeOfDay.ToString() + "] " + res;
                sw.Restart();
            }
            else
                snew += res;
            this.Dispatcher.Invoke(new Action(delegate
            {
                if (snew != "")
                    txtResults.Text = snew + "\n" + sold;
                else
                    txtResults.Text = sold;
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            serial.ClosePort();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTryComunication_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            timer.Start();
            SendData("AT");
        }

        private void btnGetFirmwareVersion_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+VERSION");
            
        }

        private void btnBaudrateChange_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Estas cambiando el modo de comunicacion por lo tanto deberias volver a abrir el programa con la nueva configuracion para que todo este OK.");
            this.IsEnabled = false;
            timer.Start();

            

            HC06Baudrate res = (HC06Baudrate)cbBaudrate.SelectedItem;
            SendData("AT+BAUD"+(int)res);

        }

        private void btnNameChange_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+NAME" + txtName.Text);
        }

        private void btnPINChange_Click(object sender, RoutedEventArgs e)
        {
            int val;
            if (!int.TryParse(txtPIN.Text, out val))
            {
                MessageBox.Show("El pin tiene que ser de 4 numeros");
                return;
            }
            
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+PIN" + txtPIN.Text);
        }

        private void btnParity_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Estas cambiando el modo de comunicacion por lo tanto deberias volver a abrir el programa con la nueva configuracion para que todo este OK.");
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+P" + cbParity.Text[0]);
        }

        void SendData(string data)
        {
            if (this.ckCrLF.IsChecked == true)
                data += "\r\n";

            serial.SendData(data);
        }

        private void txtInputConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendData(txtInputConsole.Text);
                txtInputConsole.Text = "";
            }
        }

        private void txtInputConsole_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
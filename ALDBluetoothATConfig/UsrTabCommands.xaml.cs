using ALDBluetoothATConfig.Commands.Implementation;
using ALDBluetoothATConfig.Commands.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ALDBluetoothATConfig
{
    /// <summary>
    /// Interaction logic for UsrTabCommands.xaml
    /// </summary>
    public partial class UsrTabCommands : UserControl
    {

        Stopwatch sw;
        Timer timer;
        string strNew;
        string strOld;

        List<DeviceConfiguration> configurations = new List<DeviceConfiguration>();

        public UsrTabCommands()
        {
            InitializeComponent();
            this.IsEnabled = true;

            this.InitializeDevicesList();

            
            sw = new Stopwatch();
            sw.Start();
            strNew = strOld = "";
            timer = new Timer(500);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            

            this.consoleMain.OnCommandDispatch += ConsoleMain_OnCommandDispatch;
        }

        private void InitializeDevicesList()
        {
            string currentFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Devices");

            foreach (var jsonFile in Directory.GetFiles(currentFolder, "*.json"))
            {
                string content = File.ReadAllText(jsonFile);
                var newConfiguration = JsonConvert.DeserializeObject<DeviceConfiguration>(content);
                this.configurations.Add(newConfiguration);
            }

            this.cbDevices.ItemsSource = this.configurations;
            this.cbDevices.DisplayMemberPath = "Name";

            this.cbDevices.SelectedIndex = 0;
        }

        private void ConsoleMain_OnCommandDispatch(object who, string command)
        {
            this.SendData(command);
        }

        private ALDSerialPort.ALDSerialPort serial;

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate {
                this.IsEnabled = true;
            }));
            timer.Stop();
        }

        public void InitializeControl(ALDSerialPort.ALDSerialPort serial)
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
            System.Threading.Thread.Sleep(100);
            string strNew = serial.Serial.ReadExisting();

            Debug.WriteLine(strNew);

            //if (sw.ElapsedMilliseconds > 500)
            //{
            //if(strNew!="") strOld = strNew + "\n" + strOld;
            //strNew = "\n[" + DateTime.Now.TimeOfDay.ToString() + "] " + strNew;

            this.Dispatcher.Invoke(new Action(delegate
            {
                this.consoleMain.AppendResponse("\n" + strNew);
            }));

            sw.Restart();
            //}
            //else
            //    strNew += res;
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
            //MessageBox.Show("Estas cambiando el modo de comunicacion por lo tanto deberias volver a abrir el programa con la nueva configuracion para que todo este OK.");
            //this.IsEnabled = false;
            //timer.Start();



            //HC06Baudrate res = (HC06Baudrate)cbBaudrate.SelectedItem;
            //SendData("AT+BAUD" + (int)res);

        }

        private void btnNameChange_Click(object sender, RoutedEventArgs e)
        {
            /*
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+NAME" + txtName.Text);
            */
        }

        private void btnPINChange_Click(object sender, RoutedEventArgs e)
        {
            /*
            int val;
            if (!int.TryParse(txtPIN.Text, out val))
            {
                MessageBox.Show("El pin tiene que ser de 4 numeros");
                return;
            }

            this.IsEnabled = false;
            timer.Start();

            SendData("AT+PIN" + txtPIN.Text);
            */
        }

        private void btnParity_Click(object sender, RoutedEventArgs e)
        {
            /*
            MessageBox.Show("Estas cambiando el modo de comunicacion por lo tanto deberias volver a abrir el programa con la nueva configuracion para que todo este OK.");
            this.IsEnabled = false;
            timer.Start();

            SendData("AT+P" + cbParity.Text[0]);
            */
        }

        void SendData(string data)
        {
            
            if (this.ckCrLF.IsChecked == true)
                data += "\r\n";

            serial.SendData(data);
            sw.Restart();
            
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbDevices.SelectedIndex < 0) return;
            DeviceConfiguration configuration = this.cbDevices.SelectedValue as DeviceConfiguration;

            this.LoadConfiguration(configuration);
        }

        private void LoadConfiguration(DeviceConfiguration configuration)
        {
            this.stackCommands.Children.Clear();

            foreach (var command in configuration.Commands)
            {
                var newItem = new UsrCommandAction();
                newItem.SetOnCommandActionEvent(OnCommandAction);

                newItem.SetCommandInformation(command);

                newItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                newItem.Width = double.NaN;
                newItem.Height = double.NaN;
                newItem.Margin = new Thickness(0, 2, 0, 2);

                this.stackCommands.Children.Add(newItem);
            }
        }

        private void OnCommandAction(object sender, string command)
        {
            this.consoleMain.SendCommand(command);
        }
    }
}

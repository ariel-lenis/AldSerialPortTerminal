using Ald.SerialTerminal.Main.Internationalization;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ald.SerialPort.Configuration;

namespace Ald.SerialTerminal.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Configuration.SettingsManager settingsManager;

        public MainWindow()
        {
            InitializeComponent();
            
            this.settingsManager = new Configuration.SettingsManager(Constants.FileName);
            this.settingsManager.LoadFromFile();
        }

        private void UpdatePorts()
        {
            string lastUsedPort = this.settingsManager.CurrentSettings.LastUsedPort;

            var res = ALDSerialPort.EnumeratePorts();
            var ports = res.ToDictionary(x => x.Port);

            cbCOMPorts.ItemsSource = null;
            cbCOMPorts.ItemsSource = res;

            if (lastUsedPort!=null && ports.ContainsKey(lastUsedPort))
                cbCOMPorts.SelectedItem = ports[lastUsedPort];
            else
            {
                if (res.Count > 0) cbCOMPorts.SelectedIndex = 0;
            }
            
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            UpdatePorts();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Type type;

            this.cbLanguage.ItemsSource = MyLanguages.Current.AllLanguages;
            this.cbLanguage.Text = "English";           

            /*
            
            type= typeof(A.ALDSerialPort.SerialConfiguration.EBitsPerSecond);
            cbBitsPerSecond.ItemsSource = type.GetEnumValues();
            cbBitsPerSecond.SelectedItem = ALDSerialPort.ALDSerialPort.SerialConfiguration.EBitsPerSecond.V9600;
            */

            this.cbBitsPerSecond.ItemsSource = SerialConfiguration.BitsPerSecondArray;
            cbBitsPerSecond.SelectedValue = 115200;
            
            type = typeof(SerialConfiguration.EDataBits);
            cbDataBits.ItemsSource = type.GetEnumValues();
            cbDataBits.SelectedItem = SerialConfiguration.EDataBits.V8;
            

            type = typeof(System.IO.Ports.StopBits);
            cbStopBits.ItemsSource = type.GetEnumValues();
            cbStopBits.SelectedItem = System.IO.Ports.StopBits.One;

            type = typeof(System.IO.Ports.Parity);
            cbParity.ItemsSource = type.GetEnumValues();
            cbParity.SelectedItem = System.IO.Ports.Parity.None;


            this.cbProfile.ItemsSource = this.settingsManager.CurrentSettings.Configurations.Keys.ToList();

            if (this.settingsManager.CurrentSettings.DefaultPortSetting != null && this.settingsManager.CurrentSettings.Configurations.ContainsKey(this.settingsManager.CurrentSettings.DefaultPortSetting))
                this.cbProfile.SelectedItem = this.settingsManager.CurrentSettings.DefaultPortSetting;

            this.UpdatePorts();
        }

        private void btnStartConfiguration_Click(object sender, RoutedEventArgs e)
        {
            string profile = this.cbProfile.Text.Trim();

            SerialPreviousInformation portInformation = cbCOMPorts.SelectedItem as SerialPreviousInformation;
            SerialConfiguration serialConfiguration = new SerialConfiguration();

            serialConfiguration.ConfigBitsPerSecond = (int)cbBitsPerSecond.SelectedValue;
            serialConfiguration.ConfigDataBits = (SerialConfiguration.EDataBits)this.cbDataBits.SelectedItem;
            serialConfiguration.ConfigParity = (System.IO.Ports.Parity)this.cbParity.SelectedItem;
            serialConfiguration.ConfigStopBits = (System.IO.Ports.StopBits)this.cbStopBits.SelectedItem;
            

            if (profile.Length > 0)
            {
                if (!this.settingsManager.CurrentSettings.Configurations.ContainsKey(profile))
                    this.settingsManager.CurrentSettings.Configurations.Add(profile, serialConfiguration);
                else
                    this.settingsManager.CurrentSettings.Configurations[profile] = serialConfiguration;
            }

            this.settingsManager.CurrentSettings.LastUsedPort = portInformation.Port;
            this.settingsManager.CurrentSettings.DefaultPortSetting = profile;
            this.settingsManager.SaveToFile();

            TotalConfiguration configuration = new TotalConfiguration(portInformation.Port, serialConfiguration);

            ALDSerialPort serial = new ALDSerialPort(configuration);

            try
            {
                serial.OpenPort();

                WinContainer winContainer = new WinContainer(serial);

                this.Hide();
                winContainer.ShowDialog();
                this.Show();
            }
            catch
            {
                MessageBox.Show("Cannot open port.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void cbBitsPerSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbLanguage.SelectedIndex != -1)
                MyLanguages.Current.CurrentLanguage = this.cbLanguage.SelectedItem.ToString() ;
        }

        private void cbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cbProfile.SelectedItem == null)
                return;

            string profile = this.cbProfile.SelectedItem.ToString();
            var serialConfiguration = this.settingsManager.CurrentSettings.Configurations[profile];

            this.cbBitsPerSecond.SelectedValue = serialConfiguration.ConfigBitsPerSecond;
            this.cbDataBits.SelectedItem = serialConfiguration.ConfigDataBits;
            this.cbParity.SelectedItem = serialConfiguration.ConfigParity;
            this.cbStopBits.SelectedItem = serialConfiguration.ConfigStopBits;
        }
    }
}

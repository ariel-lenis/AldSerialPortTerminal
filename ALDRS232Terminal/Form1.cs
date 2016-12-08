using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ALDRS232Terminal
{
    public partial class Form1 : Form
    {
        ALDBluetoothLibrary.ALDBluetoothRadio btRadio;

        public Form1()
        {
            InitializeComponent();
            btRadio = ALDBluetoothLibrary.ALDBluetoothRadio.AllRatios().First();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var devices = btRadio.AllDevices(true, true, true, false, false, 0);
            listBox1.DataSource = null;
            listBox1.DataSource = devices;            
            listBox1.DisplayMember = "Name";            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ALDBluetoothLibrary.ALDBluetoothDevice device = listBox1.SelectedItem as ALDBluetoothLibrary.ALDBluetoothDevice;
            label1.Text = device.inf.Address.ToString();
            if (device == null) return;
            var res = device.GetInstalledServices().Select(x => ALDBluetoothLibrary.ALDBluetoothUUIDS.GetNameFromGuid(x) + " " + x).ToList();
            listBox2.DataSource = null;
            listBox2.DataSource = res;
        }
    }
}
